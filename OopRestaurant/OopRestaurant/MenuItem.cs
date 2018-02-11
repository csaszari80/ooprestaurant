using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant
{
    /// <summary>
    /// Az étlapon szereplő tételek adatait tartalmazó osztály
    /// </summary>
    public class MenuItem
    {
        public MenuItem(string name, string description, int price, Category category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }
        public MenuItem()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Category Category { get; set; }
    }
}