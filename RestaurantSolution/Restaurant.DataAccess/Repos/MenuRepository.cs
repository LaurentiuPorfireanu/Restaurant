using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.Repos
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantContext _context;

        public MenuRepository(RestaurantContext context)
        {
            _context = context;
        }

        public IEnumerable<Menu> GetAll()
        {
            return _context.Menus.FromSqlRaw("EXEC spGetAllMenus").AsNoTracking().ToList();
        }

        public IEnumerable<Menu> GetByCategory(int categoryId)
        {
            return _context.Menus
                       .FromSqlRaw("EXEC spGetMenusByCategory @p0", categoryId)
                       .AsNoTracking()
                       .ToList();
        }


        public Menu GetById(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));

            Menu menu = null;

            // Obținem conexiunea
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC spGetMenuById @MenuID";

            // Adăugăm parametrul
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@MenuID";
            parameter.Value = menuId;
            command.Parameters.Add(parameter);

            // Deschidem conexiunea dacă nu este deja deschisă
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        menu = new Menu
                        {
                            MenuID = reader.GetInt32(reader.GetOrdinal("MenuID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID"))
                        };
                    }
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            // Dacă am găsit meniul, încărcăm categoria și preparatele
            if (menu != null)
            {
                // Încărcăm categoria
                menu.Category = _context.Categories.Find(menu.CategoryID);

                // Încărcăm preparatele
                menu.MenuPreparate = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menuId)
                    .Include(mp => mp.Preparat)
                    .ToList();
            }

            return menu;
        }

        public IEnumerable<Menu> GetAllWithCategories()
        {
            var menuList = new List<Menu>();

            // Obținem conexiunea
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC spGetAllMenusWithCategories";

            // Deschidem conexiunea dacă nu este deja deschisă
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var menuId = reader.GetInt32(reader.GetOrdinal("MenuID"));
                        var categoryId = reader.GetInt32(reader.GetOrdinal("CategoryID"));

                        var menu = new Menu
                        {
                            MenuID = menuId,
                            Name = reader.GetString(reader.GetOrdinal("MenuName")),
                            CategoryID = categoryId,
                            Category = new Category
                            {
                                CategoryId = categoryId,
                                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                            }
                        };

                        menuList.Add(menu);
                    }
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            // Pentru fiecare meniu, încărcăm preparatele aferente
            foreach (var menu in menuList)
            {
                // Încărcăm preparatele - folosim o abordare separată pentru a evita erori de compunere
                var menuPreparatList = new List<MenuPreparat>();

                var mpCommand = connection.CreateCommand();
                mpCommand.CommandText = "SELECT mp.MenuID, mp.PreparatID, mp.QuantityMenuPortie, " +
                                       "p.Name AS PreparatName, p.Price, p.QuantityPortie, p.QuantityTotal, p.CategoryID " +
                                       "FROM MenuPreparat mp " +
                                       "JOIN Preparat p ON mp.PreparatID = p.PreparatID " +
                                       "WHERE mp.MenuID = @MenuID";

                var mpParameter = mpCommand.CreateParameter();
                mpParameter.ParameterName = "@MenuID";
                mpParameter.Value = menu.MenuID;
                mpCommand.Parameters.Add(mpParameter);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                try
                {
                    using (var mpReader = mpCommand.ExecuteReader())
                    {
                        while (mpReader.Read())
                        {
                            var preparatId = mpReader.GetInt32(mpReader.GetOrdinal("PreparatID"));

                            var menuPreparat = new MenuPreparat
                            {
                                MenuID = menu.MenuID,
                                PreparatID = preparatId,
                                Menu = menu,
                                QuantityMenuPortie = mpReader.GetInt32(mpReader.GetOrdinal("QuantityMenuPortie")),
                                Preparat = new Preparat
                                {
                                    PreparatID = preparatId,
                                    Name = mpReader.GetString(mpReader.GetOrdinal("PreparatName")),
                                    Price = mpReader.GetDecimal(mpReader.GetOrdinal("Price")),
                                    QuantityPortie = mpReader.GetInt32(mpReader.GetOrdinal("QuantityPortie")),
                                    QuantityTotal = mpReader.GetInt32(mpReader.GetOrdinal("QuantityTotal")),
                                    CategoryID = mpReader.GetInt32(mpReader.GetOrdinal("CategoryID"))
                                }
                            };

                            menuPreparatList.Add(menuPreparat);
                        }
                    }
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }

                menu.MenuPreparate = menuPreparatList;
            }

            return menuList;
        }
        public int Insert(string name, int categoryId)
        {
            // Utilizăm ADO.NET direct pentru a obține valoarea returnată
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "EXEC spInsertMenu @Name, @CategoryID; SELECT SCOPE_IDENTITY();";

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@Name";
            nameParam.Value = name;
            command.Parameters.Add(nameParam);

            var categoryParam = command.CreateParameter();
            categoryParam.ParameterName = "@CategoryID";
            categoryParam.Value = categoryId;
            command.Parameters.Add(categoryParam);

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public void Update(int menuId, string name, int categoryId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateMenu @MenuID = {0}, @Name = {1}, @CategoryID = {2}",
                menuId, name, categoryId);
        }


        public void Delete(int menuId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteMenu @MenuID = {0}",
                menuId);
        }


        public void AddPreparat(int menuId, int preparatId, int quantityMenuPortie)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertMenuPreparat @MenuID = {0}, @PreparatID = {1}, @QuantityMenuPortie = {2}",
                menuId, preparatId, quantityMenuPortie);
        }

        public void RemovePreparat(int menuId, int preparatId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteMenuPreparat @MenuID = {0}, @PreparatID = {1}",
                menuId, preparatId);
        }

        public void RemoveAllPreparate(int menuId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteAllMenuPreparate @MenuID = {0}",
                menuId);
        }

        public IEnumerable<MenuPreparat> GetMenuPreparate(int menuId)
        {
            return _context.MenuPreparate
                .FromSqlRaw("EXEC spGetMenuPreparate @MenuID = {0}", menuId)
                .AsNoTracking()
                .Include(mp => mp.Preparat)
                .ToList();
        }

    }
}
