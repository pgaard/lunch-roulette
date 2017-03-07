using SQLite;

namespace LunchRoulette.Models
{
    using System;

    public class Lunch
    {
        //[PrimaryKey, AutoIncrement]
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }    
        public string GoogleId { get; set; }
        public double Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
