using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IBASExeptionDBService
    {
        public Task AddAsync(DBBASExeption exeption);
    }
}