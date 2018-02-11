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

            /// a lambda kifejez�s mondja meg ,hogy az addorupdate mi alapj�n ellen�rizze, hogy hozz�adni kell a t�bl�hoz, vagy friss�teni
            /// (en�lk�l minden update-database n�l be�lleszten� a megadott �rt�kekekt �j sorokba mivel alapesetben az Id alapj�n ellen�riz, amivel ez a konstruktor nem foglalkozik)
            context.Categories.AddOrUpdate(
                x => x.Name,
                new Category( name : "Pizz�k"),
                new Category( name : "Italok"),
                new Category( name : "Desszertek")
            );
            context.SaveChanges();

            /// mivel a k�vetkez� l�p�sben sz�ks�g lesz a l�trej�tt Categ�ri�k teljes modelj�re (Id-vel egy�tt)
            /// ez�rt lek�rdezz�k a .Single met�dus egy eredm�nyelemet ad vissza akkor is ha t�bb is el�fordul a lambda kifejez�sben megadott mez�re vonatkoz� felt�tel alapj�n.
            Category pizzaCategory = context.Categories.Single(x => x.Name == "Pizz�k");
            Category drinkCategory = context.Categories.Single(x => x.Name == "Italok");

            /// Felt�ltj�k a MenuItems t�bl�t is n�h�ny adattal felhaszn�lva az el�z� l�p�sben lek�rdezett kateg�ri�kat
            context.MenuItems.AddOrUpdate(
                x => x.Name,
                new MenuItem(name : "Margarita",description : "mozzarella, pizzasz�sz", price: 200, category : pizzaCategory),
                new MenuItem(name: "Hawai", description: "sonka, anan�sz, mozzarella, pizzasz�sz", price: 300, category: pizzaCategory),
                new MenuItem(name: "Cola", description: "3 dl", price: 120, category: drinkCategory),
                new MenuItem(name: "�sv�nyv�z", description: "3 dl", price: 100, category: drinkCategory)
            );
            context.SaveChanges();
        }
    }
}
