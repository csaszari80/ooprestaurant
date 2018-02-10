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