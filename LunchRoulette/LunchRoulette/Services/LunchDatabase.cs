﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using SQLite;

namespace LunchRoulette.Services
{
    public class LunchDatabase
    {
        private SQLiteAsyncConnection database;

        public LunchDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //database.DropTableAsync<Lunch>().Wait();
            database.CreateTableAsync<Lunch>().Wait();
        }

        public Task<List<Lunch>> GetItemsAsync()
        {
            var table = database.Table<Lunch>();
            if(table != null)
                return database.Table<Lunch>().ToListAsync();
            return null;
        }

        public Task<List<Lunch>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Lunch>("SELECT * FROM [Lunch] WHERE [Done] = 0");
        }

        public Task<Lunch> GetItemAsync(int id)
        {
            return database.Table<Lunch>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Lunch item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Lunch item)
        {
            return database.DeleteAsync(item);
        }
    }
}
