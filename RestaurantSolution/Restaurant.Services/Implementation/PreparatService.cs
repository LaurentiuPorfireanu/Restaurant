using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementations
{
    public class PreparatService : IPreparatService
    {
        private readonly IPreparatRepository _repo;

        public PreparatService(IPreparatRepository repo)
            => _repo = repo;

        public IEnumerable<Preparat> GetPreparateByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            return _repo.GetByCategory(categoryId);
        }

        public void CreatePreparat(
            string name,
            decimal pret,
            int QuantityPortie,
            int QuantityTotal,
            int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (pret < 0)
                throw new ArgumentException("Prețul trebuie să fie >= 0", nameof(pret));
            if (QuantityPortie <= 0)
                throw new ArgumentException("Cantitate portie invalidă", nameof(QuantityPortie));
            if (QuantityTotal < 0)
                throw new ArgumentException("Cantitate totală invalidă", nameof(QuantityTotal));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

            _repo.Insert(name, pret, QuantityPortie, QuantityTotal, categoryId);
        }

        public void UpdatePreparat(
            int preparatId,
            string name,
            decimal pret,
            int QuantityPortie,
            int QuantityTotal,
            int categoryId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (pret < 0)
                throw new ArgumentException("Prețul trebuie să fie >= 0", nameof(pret));
            if (QuantityPortie <= 0)
                throw new ArgumentException("Cantitate portie invalidă", nameof(QuantityPortie));
            if (QuantityTotal < 0)
                throw new ArgumentException("Cantitate totală invalidă", nameof(QuantityTotal));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));

            _repo.Update(preparatId, name, pret, QuantityPortie, QuantityTotal, categoryId);
        }

        public void DeletePreparat(int preparatId)
        {
            if (preparatId <= 0)
                throw new ArgumentException("ID preparat invalid", nameof(preparatId));

            _repo.Delete(preparatId);
        }
    }
}
