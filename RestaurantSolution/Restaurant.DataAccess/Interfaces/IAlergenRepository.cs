using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IAlergenRepository
    {
        IEnumerable<Alergen> GetAll();
        Alergen GetById(int id);
        void Insert(string name);
        void Update(int id, string name);
        void Delete(int id);
    }
}
