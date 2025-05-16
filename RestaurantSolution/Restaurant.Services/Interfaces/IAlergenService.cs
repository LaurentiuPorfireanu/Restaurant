using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IAlergenService
    {
        IEnumerable<Alergen> GetAllAlergens();
        Alergen GetAlergenById(int id);
        void CreateAlergen(string name);
        void UpdateAlergen(int id, string name);
        void DeleteAlergen(int id);
    }
}
