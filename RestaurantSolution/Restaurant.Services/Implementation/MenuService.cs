using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Services.Implementation
{
    
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repo;
        private readonly RestaurantContext _context;

       
        public MenuService(IMenuRepository repo, RestaurantContext context)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        public IEnumerable<Menu> GetAllMenus()
        {
            var menus = _repo.GetAll().ToList();
            // Încărcăm și preparatele asociate fiecărui meniu
            foreach (var menu in menus)
            {
                menu.MenuPreparate = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menu.MenuID)
                    .Include(mp => mp.Preparat)
                    .ToList();
            }
            return menus;
        }

        public IEnumerable<Menu> GetAllMenusWithCategories()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MenuService: Apel GetAllMenusWithCategories");
                var menus = _repo.GetAllWithCategories();
                System.Diagnostics.Debug.WriteLine($"MenuService: {menus.Count()} meniuri încărcate cu categorii");
                return menus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Eroare la GetAllMenusWithCategories - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw; // Re-aruncăm excepția pentru a fi gestionată la nivel superior
            }
        }

        public IEnumerable<Menu> GetMenusByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID trebuie să fie pozitiv", nameof(categoryId));

            var menus = _repo.GetByCategory(categoryId).ToList();
            // Încărcăm și preparatele asociate fiecărui meniu
            foreach (var menu in menus)
            {
                menu.MenuPreparate = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menu.MenuID)
                    .Include(mp => mp.Preparat)
                    .ToList();
            }
            return menus;
        }

        
        public Menu GetMenuById(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID trebuie să fie pozitiv", nameof(menuId));

            var menu = _repo.GetById(menuId);
            if (menu != null)
            {
                menu.MenuPreparate = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menu.MenuID)
                    .Include(mp => mp.Preparat)
                    .ToList();
            }
            return menu;
        }

       
        public int CreateMenu(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

            // Creăm meniul în baza de date
            var menu = new Menu
            {
                Name = name,
                CategoryID = categoryId
            };

            _context.Menus.Add(menu);
            _context.SaveChanges();

            return menu.MenuID;
        }

       
        public void UpdateMenu(int menuId, string name, int categoryId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

            _repo.Update(menuId, name, categoryId);
        }

        
        public void DeleteMenu(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));

            // Mai întâi ștergem toate legăturile cu preparatele
            RemoveAllPreparateFromMenu(menuId);

            // Apoi ștergem meniul
            _repo.Delete(menuId);
        }

       
        public void AddPreparatToMenu(int menuId, int preparatId, int quantityMenuPortie)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID invalid", nameof(preparatId));
            if (quantityMenuPortie <= 0)
                throw new ArgumentException("Cantitatea trebuie să fie pozitivă", nameof(quantityMenuPortie));

            // Verificăm dacă legătura există deja
            var existing = _context.MenuPreparate
                .FirstOrDefault(mp => mp.MenuID == menuId && mp.PreparatID == preparatId);

            if (existing != null)
            {
                // Dacă există, doar actualizăm cantitatea
                existing.QuantityMenuPortie = quantityMenuPortie;
            }
            else
            {
                // Dacă nu există, creăm o nouă înregistrare
                var menuPreparat = new MenuPreparat
                {
                    MenuID = menuId,
                    PreparatID = preparatId,
                    QuantityMenuPortie = quantityMenuPortie
                };

                _context.MenuPreparate.Add(menuPreparat);
            }

            _context.SaveChanges();
        }

        
        public void RemovePreparatFromMenu(int menuId, int preparatId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID invalid", nameof(preparatId));

            var menuPreparat = _context.MenuPreparate
                .FirstOrDefault(mp => mp.MenuID == menuId && mp.PreparatID == preparatId);

            if (menuPreparat != null)
            {
                _context.MenuPreparate.Remove(menuPreparat);
                _context.SaveChanges();
            }
        }

       
        public void RemoveAllPreparateFromMenu(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));

            var menuPreparate = _context.MenuPreparate
                .Where(mp => mp.MenuID == menuId)
                .ToList();

            if (menuPreparate.Any())
            {
                _context.MenuPreparate.RemoveRange(menuPreparate);
                _context.SaveChanges();
            }
        }

       
        public decimal CalculateMenuPrice(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));

            // Citim procentajul de discount direct din configurare
            decimal discountPercentage = 10; // Valoare implicită

            string configDiscount = ConfigurationManager.AppSettings["MenuDiscountPercentage"];
            if (!string.IsNullOrEmpty(configDiscount) && decimal.TryParse(configDiscount, out decimal discountValue))
            {
                discountPercentage = discountValue;
                System.Diagnostics.Debug.WriteLine($"Discount-ul citit din configurare: {discountValue}%");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Se folosește discount-ul implicit: {discountPercentage}%");
            }

            // Obținem toate preparatele din meniu și calculăm prețul total
            var menuPreparate = _context.MenuPreparate
                .Where(mp => mp.MenuID == menuId)
                .Include(mp => mp.Preparat)
                .ToList();

            if (!menuPreparate.Any())
                return 0;

            // Calculăm suma prețurilor preparatelor
            decimal totalPrice = 0;
            foreach (var mp in menuPreparate)
            {
                if (mp.Preparat != null)
                {
                    totalPrice += mp.Preparat.Price;
                }
            }

            // Aplicăm reducerea
            decimal discount = totalPrice * (discountPercentage / 100);
            decimal finalPrice = totalPrice - discount;

            return Math.Round(finalPrice, 2);
        }

       
        public decimal CalculateMenuPrice(int menuId, decimal discountPercentage)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Procentajul de reducere trebuie să fie între 0 și 100", nameof(discountPercentage));

            // Obținem toate preparatele din meniu și calculăm prețul total
            var menuPreparate = _context.MenuPreparate
                .Where(mp => mp.MenuID == menuId)
                .Include(mp => mp.Preparat)
                .ToList();

            if (!menuPreparate.Any())
                return 0;

            // Calculăm suma prețurilor preparatelor
            decimal totalPrice = 0;
            foreach (var mp in menuPreparate)
            {
                if (mp.Preparat != null)
                {
                    totalPrice += mp.Preparat.Price;
                }
            }

            // Aplicăm reducerea
            decimal discount = totalPrice * (discountPercentage / 100);
            decimal finalPrice = totalPrice - discount;

            return Math.Round(finalPrice, 2);
        }
    }
}