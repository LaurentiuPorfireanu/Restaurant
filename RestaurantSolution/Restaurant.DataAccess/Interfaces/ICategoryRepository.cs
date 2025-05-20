using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void Insert(string name);
        void Update(int id, string name);

        void Delete(int id);
    }
}
