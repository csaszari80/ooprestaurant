using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant
{
    /// <summary>
    /// Az ételek kategóriáját tartalmazó osztály
    /// </summary>
    public class Category
    {
          
        /// <summary>
        /// A Category osztály konstruktora
        /// Figyelem: ha létrehozzuk akkor szükséges létrehozni paraméter nélküli konstruktort is, különben a fordító hibát jelez 
        ///     (a paraméter nélküli konstruktort alapesetben a fordító létrehozza futtatáskor de onnantól kezdve, hogy mi létrehozunk egyet a fordító onnantól kezdve nem generál)
        /// </summary>
        /// <param name="name"> a name paraméter tertalmazza a kategória nevét</param>
        public Category(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Ez a paraméter nélküli konstr
        /// </summary>
        public Category()
        {  
        }

        /// <summary>
        /// Az adatbázisba íráshoz kell egy primary key property a CodeFirst névkonvenció alapján ennek neve "Key" vagy "Id"
        /// Ezt felismeri és az adatbázisban ennek megfelelően hozza létre aaz adattáblákat
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Az ételkategória neve
        /// </summary>
        public string Name{ get; set; }
    }
}