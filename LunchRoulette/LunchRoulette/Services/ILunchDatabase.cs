using System.Collections.Generic;
using System.Threading.Tasks;
using LunchRoulette.Models;

namespace LunchRoulette.Services
{
    public interface ILunchDatabase
    {
        void DropTable();
        Task<List<Lunch>> GetItemsAsync();
        Task<List<Lunch>> GetItemsNotDoneAsync();
        Task<Lunch> GetItemAsync(int id);
        Task<int> SaveItemAsync(Lunch item);
        Task<int> DeleteItemAsync(Lunch item);
    }
}