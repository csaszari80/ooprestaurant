namespace OopRestaurant.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OopRestaurant.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OopRestaurant.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            /// a lambda kifejezés mondja meg ,hogy az addorupdate mi alapján ellenõrizze, hogy hozzáadni kell a táblához, vagy frissíteni
            /// (enélkül minden update-database nél beíllesztené a megadott értékekekt új sorokba mivel alapesetben az Id alapján ellenõriz, amivel ez a konstruktor nem foglalkozik)
            context.Categories.AddOrUpdate(
                x => x.Name,
                new Category( name : "Pizzák"),
                new Category( name : "Italok"),
                new Category( name : "Desszertek")
            );
            context.SaveChanges();

            /// mivel a következõ lépésben szükség lesz a létrejött Categóriák teljes modeljére (Id-vel együtt)
            /// ezért lekérdezzük a .Single metódus egy eredményelemet ad vissza akkor is ha több is elõfordul a lambda kifejezésben megadott mezõre vonatkozó feltétel alapján.
            Category pizzaCategory = context.Categories.Single(x => x.Name == "Pizzák");
            Category drinkCategory = context.Categories.Single(x => x.Name == "Italok");

            /// Feltöltjük a MenuItems táblát is néhány adattal felhasználva az elõzõ lépésben lekérdezett kategóriákat
            context.MenuItems.AddOrUpdate(
                x => x.Name,
                new MenuItem(name : "Margarita",description : "mozzarella, pizzaszósz", price: 200, category : pizzaCategory),
                new MenuItem(name: "Hawai", description: "sonka, ananász, mozzarella, pizzaszósz", price: 300, category: pizzaCategory),
                new MenuItem(name: "Cola", description: "3 dl", price: 120, category: drinkCategory),
                new MenuItem(name: "Ásványvíz", description: "3 dl", price: 100, category: drinkCategory)
            );
            context.SaveChanges();
        }
    }
}
