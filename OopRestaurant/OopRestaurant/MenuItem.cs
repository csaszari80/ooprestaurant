using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary>
        /// A lenyíló lista kiválasztott elemének azonosítója
        /// Az annotáció ebben az esetben azt a célt szolgálja , hogy ez a mezőt az EntityFramework codeFirst ne akarja betenni az adatbázisba a következő migrációnál
        /// és ne is hiányolja az adatbázis módosítását futtatáskor
        /// </summary>
        [NotMapped]
        public int CategoryId { get; set; }

        /// <summary>
        /// A lenyílólista tartalma a megjelenítendő Id Name párok
        /// Az annotáció ebben az esetben azt a célt szolgálja , hogy ez a mezőt az EntityFramework codeFirst ne akarja betenni az adatbázisba a következő migrációnál
        /// és ne is hiányolja az adatbázis módosítását futtatáskor
        /// </summary>
        [NotMapped]
        public SelectList AssignableCategories { get; set; }
    }
}