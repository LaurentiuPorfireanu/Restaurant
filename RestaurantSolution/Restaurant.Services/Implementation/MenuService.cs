using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
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
            try
            {
                System.Diagnostics.Debug.WriteLine("MenuService: GetAllMenus called");
                var menus = _repo.GetAll().ToList();
                System.Diagnostics.Debug.WriteLine($"MenuService: Retrieved {menus.Count} menus");

                return menus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in GetAllMenus - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw; 
            }
        }

        public IEnumerable<Menu> GetAllMenusWithCategories()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MenuService: GetAllMenusWithCategories called");
                var menus = _repo.GetAllWithCategories().ToList();
                System.Diagnostics.Debug.WriteLine($"MenuService: Retrieved {menus.Count} menus with categories");

                return menus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in GetAllMenusWithCategories - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw; 
            }
        }

        public IEnumerable<Menu> GetMenusByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: GetMenusByCategory called with categoryId={categoryId}");
                var menus = _repo.GetByCategory(categoryId).ToList();
                System.Diagnostics.Debug.WriteLine($"MenuService: Retrieved {menus.Count} menus for category {categoryId}");

                return menus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in GetMenusByCategory - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public Menu GetMenuById(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: GetMenuById called with menuId={menuId}");
                var menu = _repo.GetById(menuId);

                if (menu == null)
                {
                    System.Diagnostics.Debug.WriteLine($"MenuService: No menu found with ID {menuId}");
                    return null;
                }

                System.Diagnostics.Debug.WriteLine($"MenuService: Retrieved menu '{menu.Name}' with ID {menuId}");
                return menu;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in GetMenuById - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public int CreateMenu(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu name is required", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: CreateMenu called with name='{name}', categoryId={categoryId}");

                // Create menu using repository
                int menuId = _repo.Insert(name, categoryId);

                System.Diagnostics.Debug.WriteLine($"MenuService: Created new menu with ID {menuId}");
                return menuId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in CreateMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void UpdateMenu(int menuId, string name, int categoryId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu name is required", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: UpdateMenu called with menuId={menuId}, name='{name}', categoryId={categoryId}");

                _repo.Update(menuId, name, categoryId);

                System.Diagnostics.Debug.WriteLine($"MenuService: Updated menu with ID {menuId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in UpdateMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void DeleteMenu(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: DeleteMenu called with menuId={menuId}");

                // First, remove all menu items
                RemoveAllPreparateFromMenu(menuId);

                // Then delete the menu
                _repo.Delete(menuId);

                System.Diagnostics.Debug.WriteLine($"MenuService: Deleted menu with ID {menuId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in DeleteMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void AddPreparatToMenu(int menuId, int preparatId, int quantityMenuPortie)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID must be positive", nameof(preparatId));
            if (quantityMenuPortie <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantityMenuPortie));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: AddPreparatToMenu called with menuId={menuId}, preparatId={preparatId}, quantity={quantityMenuPortie}");

                // Use a transaction to ensure atomicity
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Check if the item already exists in the menu
                        var existingItem = _context.MenuPreparate
                            .FirstOrDefault(mp => mp.MenuID == menuId && mp.PreparatID == preparatId);

                        if (existingItem != null)
                        {
                            // Update existing item
                            System.Diagnostics.Debug.WriteLine($"MenuService: Updating existing menu item with new quantity {quantityMenuPortie}");
                            existingItem.QuantityMenuPortie = quantityMenuPortie;
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Add new item through repository
                            System.Diagnostics.Debug.WriteLine($"MenuService: Adding new menu item with quantity {quantityMenuPortie}");
                            _repo.AddPreparat(menuId, preparatId, quantityMenuPortie);
                        }

                        transaction.Commit();
                        System.Diagnostics.Debug.WriteLine($"MenuService: Successfully added/updated preparat {preparatId} to menu {menuId}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"MenuService: Transaction rolled back due to error: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in AddPreparatToMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void RemovePreparatFromMenu(int menuId, int preparatId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID must be positive", nameof(preparatId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: RemovePreparatFromMenu called with menuId={menuId}, preparatId={preparatId}");

                _repo.RemovePreparat(menuId, preparatId);

                System.Diagnostics.Debug.WriteLine($"MenuService: Removed preparat {preparatId} from menu {menuId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in RemovePreparatFromMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void RemoveAllPreparateFromMenu(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: RemoveAllPreparateFromMenu called with menuId={menuId}");

                _repo.RemoveAllPreparate(menuId);

                System.Diagnostics.Debug.WriteLine($"MenuService: Removed all preparate from menu {menuId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in RemoveAllPreparateFromMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public decimal CalculateMenuPrice(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: CalculateMenuPrice called with menuId={menuId}");

                // Get discount percentage from configuration
                decimal discountPercentage = 10; // Default value

                string configDiscount = ConfigurationManager.AppSettings["MenuDiscountPercentage"];
                if (!string.IsNullOrEmpty(configDiscount) && decimal.TryParse(configDiscount, out decimal discountValue))
                {
                    discountPercentage = discountValue;
                    System.Diagnostics.Debug.WriteLine($"MenuService: Using discount from configuration: {discountValue}%");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"MenuService: Using default discount: {discountPercentage}%");
                }

                // Get menu items
                var menuItems = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menuId)
                    .Include(mp => mp.Preparat)
                    .ToList();

                if (!menuItems.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"MenuService: No items found for menu {menuId}, returning 0");
                    return 0;
                }

                // Calculate total price
                decimal totalPrice = 0;
                foreach (var item in menuItems)
                {
                    if (item.Preparat != null)
                    {
                        totalPrice += item.Preparat.Price;
                    }
                }

                // Apply discount
                decimal finalPrice = Math.Round(totalPrice * (1 - discountPercentage / 100), 2);

                System.Diagnostics.Debug.WriteLine($"MenuService: Calculated price for menu {menuId}: {finalPrice} Lei (Original: {totalPrice} Lei, Discount: {discountPercentage}%)");
                return finalPrice;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in CalculateMenuPrice - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public decimal CalculateMenuPrice(int menuId, decimal discountPercentage)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: CalculateMenuPrice called with menuId={menuId}, discountPercentage={discountPercentage}");

                // Get menu items
                var menuItems = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menuId)
                    .Include(mp => mp.Preparat)
                    .ToList();

                if (!menuItems.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"MenuService: No items found for menu {menuId}, returning 0");
                    return 0;
                }

                // Calculate total price
                decimal totalPrice = 0;
                foreach (var item in menuItems)
                {
                    if (item.Preparat != null)
                    {
                        totalPrice += item.Preparat.Price;
                    }
                }

                // Apply discount
                decimal finalPrice = Math.Round(totalPrice * (1 - discountPercentage / 100), 2);

                System.Diagnostics.Debug.WriteLine($"MenuService: Calculated price for menu {menuId}: {finalPrice} Lei (Original: {totalPrice} Lei, Discount: {discountPercentage}%)");
                return finalPrice;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuService: Error in CalculateMenuPrice with discount - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}