using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IPreparatService
    {
        IEnumerable<Preparat> GetPreparateByCategory(int categoryId);
        void CreatePreparat(
            string name,
            decimal pret,
            int QuantityPortie,
            int QuantityTotal,
            int categoryId);
        void UpdatePreparat(
            int preparatId,
            string name,
            decimal pret,
            int QuantityPortie,
            int QuantityTotal,
            int categoryId);
        void DeletePreparat(int preparatId);
    }
}
