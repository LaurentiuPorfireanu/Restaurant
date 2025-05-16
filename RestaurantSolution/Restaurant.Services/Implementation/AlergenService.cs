using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementations
{
    public class AlergenService : IAlergenService
    {
        private readonly IAlergenRepository _repo;

        public AlergenService(IAlergenRepository repo)
            => _repo = repo;

        public IEnumerable<Alergen> GetAllAlergens()
            => _repo.GetAll();

        public Alergen GetAlergenById(int id)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            return _repo.GetById(id);
        }

        public void CreateAlergen(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nume obligatoriu", nameof(name));
            _repo.Insert(name);
        }

        public void UpdateAlergen(int id, string name)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nume obligatoriu", nameof(name));
            _repo.Update(id, name);
        }

        public void DeleteAlergen(int id)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            _repo.Delete(id);
        }
    }
}
