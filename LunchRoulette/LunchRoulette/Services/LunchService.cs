using System.Collections.Generic;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

namespace LunchRoulette.Services
{    
    public class LunchService
    {
        static ILunchDatabase database;

        public static ILunchDatabase Database { get; private set; }

        public LunchService()
        {
            Database = App.Container.Resolve<ILunchDatabase>();
        }

        public async Task<List<Lunch>> GetAll()
        {
            return await Database.GetItemsAsync();
        }

        public void DeleteAll()
        {
            database.DropTable();
        }

        public async Task<int> Add(Lunch lunch)
        {
            var id = await Database.SaveItemAsync(lunch);
            return id;
        }

        public async Task Delete(Lunch lunch)
        {
            await Database.DeleteItemAsync(lunch);
        }
    }
}
