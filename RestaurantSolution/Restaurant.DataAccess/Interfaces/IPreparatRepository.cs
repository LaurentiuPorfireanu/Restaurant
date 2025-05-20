using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IPreparatRepository
    {
        IEnumerable<Preparat> GetByCategory(int categoryId);

        void Insert(
            string Name,
            decimal Price,
            int QuantityPortie,
            int QuantityTotal,
            int CategoryID);


        void Update(
            int PreparatID,
            string Name,
            decimal Price,
            int QuantityPortie,
            int QuantityTotal,
            int CategoryID);



        void Delete(int PreparatID);
    }
}
