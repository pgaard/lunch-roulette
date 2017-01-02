using System.Collections.Generic;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Xamarin.Forms;

namespace LunchRoulette.Services
{    
    public class LunchService
    {
        static LunchDatabase database;

        public static LunchDatabase Database
        {
            get
            {
                if (database == null)
                {
                    var fileHelper = DependencyService.Get<IFileHelper>();
                    var file = fileHelper.GetLocalFilePath("LunchSQLite.db3");
                    if (file != null)
                    {
                        database = new LunchDatabase(file);
                    }
                }
                return database;
            }
        } 
    
        public async Task<List<Lunch>> GetAll()
        {
            return await Database.GetItemsAsync();
        }

        public async Task<int> Add(Lunch lunch)
        {
            var id = await Database.SaveItemAsync(lunch);
            lunch.Id = id;
            return id;
        }

        public async Task Delete(Lunch lunch)
        {
            await Database.DeleteItemAsync(lunch);
        }
    }
}
