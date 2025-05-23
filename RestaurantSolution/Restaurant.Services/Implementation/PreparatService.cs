﻿using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using System.Linq;

namespace Restaurant.Services.Implementations
{
    public class PreparatService : IPreparatService
    {
        private readonly IPreparatRepository _repo;
        private readonly RestaurantContext _context; 

        public PreparatService(IPreparatRepository repo, RestaurantContext context)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Preparat> GetPreparateByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            return _repo.GetByCategory(categoryId);
        }

        public int CreatePreparat(
            string name,
            decimal price,
            int quantityPortie,
            int quantityTotal,
            int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (price < 0)
                throw new ArgumentException("Prețul trebuie să fie >= 0", nameof(price));
            if (quantityPortie <= 0)
                throw new ArgumentException("Cantitate portie invalidă", nameof(quantityPortie));
            if (quantityTotal < 0)
                throw new ArgumentException("Cantitate totală invalidă", nameof(quantityTotal));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

          
            var preparat = new Preparat
            {
                Name = name,
                Price = price,
                QuantityPortie = quantityPortie,
                QuantityTotal = quantityTotal,
                CategoryID = categoryId
            };

            _context.Preparate.Add(preparat);
            _context.SaveChanges();

            return preparat.PreparatID; 
        }

        public void UpdatePreparat(
            int preparatId,
            string name,
            decimal price,
            int quantityPortie,
            int quantityTotal,
            int categoryId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (price < 0)
                throw new ArgumentException("Prețul trebuie să fie >= 0", nameof(price));
            if (quantityPortie <= 0)
                throw new ArgumentException("Cantitate portie invalidă", nameof(quantityPortie));
            if (quantityTotal < 0)
                throw new ArgumentException("Cantitate totală invalidă", nameof(quantityTotal));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

            _repo.Update(preparatId, name, price, quantityPortie, quantityTotal, categoryId);
        }

        public void DeletePreparat(int preparatId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));

            try
            {
                _repo.Delete(preparatId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in PreparatService.DeletePreparat: {ex.Message}");
                throw new Exception("Nu se poate șterge preparatul. Este posibil să existe comenzi active care conțin acest preparat.", ex);
            }
        }

        public void AddPreparatImage(int preparatId, string imagePath)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("Cale imagine invalidă", nameof(imagePath));

            
            if (!imagePath.Contains("Resources"))
                throw new ArgumentException("Imaginile trebuie să fie salvate în directorul Resources", nameof(imagePath));

            var foto = new PreparatFoto
            {
                PreparatID = preparatId,
                ImagePath = imagePath
            };

            _context.PreparatFotos.Add(foto);
            _context.SaveChanges();
        }

        public void AddPreparatAlergen(int preparatId, int alergenId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));
            if (alergenId <= 0)
                throw new ArgumentException("ID alergen invalid", nameof(alergenId));

            var existing = _context.PreparatAlergens
                .FirstOrDefault(pa => pa.PreparatID == preparatId && pa.AlergenID == alergenId);

            if (existing == null)
            {
                
                System.Diagnostics.Debug.WriteLine($"Adăugare alergen {alergenId} la preparatul {preparatId}");

                var preparatAlergen = new PreparatAlergen
                {
                    PreparatID = preparatId,
                    AlergenID = alergenId
                };

                _context.PreparatAlergens.Add(preparatAlergen);
                _context.SaveChanges();

              
                var added = _context.PreparatAlergens
                    .FirstOrDefault(pa => pa.PreparatID == preparatId && pa.AlergenID == alergenId);

                System.Diagnostics.Debug.WriteLine($"Verificare: asocierea există acum în BD: {added != null}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Asocierea dintre preparatul {preparatId} și alergenul {alergenId} există deja");
            }
        }

        public void RemovePreparatAlergen(int preparatId, int alergenId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));
            if (alergenId <= 0)
                throw new ArgumentException("ID alergen invalid", nameof(alergenId));

            var preparatAlergen = _context.PreparatAlergens
                .FirstOrDefault(pa => pa.PreparatID == preparatId && pa.AlergenID == alergenId);

            if (preparatAlergen != null)
            {
                _context.PreparatAlergens.Remove(preparatAlergen);
                _context.SaveChanges();
            }
        }
    }
}