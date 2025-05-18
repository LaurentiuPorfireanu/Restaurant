// Restaurant.Services/Interfaces/IPreparatService.cs
using Restaurant.Domain.Entities;
using System.Collections.Generic;

namespace Restaurant.Services.Interfaces
{
    public interface IPreparatService
    {
        IEnumerable<Preparat> GetPreparateByCategory(int categoryId);

        int CreatePreparat(
            string name,
            decimal price,
            int quantityPortie,
            int quantityTotal,
            int categoryId);

        void UpdatePreparat(
            int preparatId,
            string name,
            decimal price,
            int quantityPortie,
            int quantityTotal,
            int categoryId);

        void DeletePreparat(int preparatId);

        void AddPreparatImage(int preparatId, string imagePath);

        void AddPreparatAlergen(int preparatId, int alergenId);

        void RemovePreparatAlergen(int preparatId, int alergenId);
    }
}