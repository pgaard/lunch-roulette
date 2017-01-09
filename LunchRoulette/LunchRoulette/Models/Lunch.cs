using SQLite;

namespace LunchRoulette.Models
{
    using System;

    public class Lunch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }    
        public string GoogleId { get; set; }
        public DateTime Date { get; set; }
    }
}
