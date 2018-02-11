using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant;
using OopRestaurant.Models;

namespace OopRestaurant.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuItems
        public ActionResult Index()
        {
            // az include-ban álló lambda kifejezéssel töltöm be a más táblákból jövő adatokat
            return View(db.MenuItems.Include(x => x.Category).ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: MenuItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            //Hogy be tudjuk állítani alenyílót, megadjuk az akuális category értékből az azonosítót
            //Mivel a kategória egy másik táblában van ezért az Entity framework nem tölti be ehhez
            //be kell tölteni az adatokat onnan is (db.Entry sor)
            db.Entry(menuItem).Reference(x => x.Category).Load();
            menuItem.CategoryId = menuItem.Category.Id;

            //A választható kategóriák listájához szükség van egy selectList-re
            //amelynek konstruktorában lekérjuk a lista elemeit szolgáltató Categories táblát, megadjuk, postban elküldendő értéket tartalmazó mezőt(Id) és amegjelenítendő értéket tartalmazó mezőt(Name)
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x=>x.Name).ToList(),"Id","Name");
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)  //be kell engedni a lenyíló által kiválasztott CategoryId azonosítót is
        {
            if (ModelState.IsValid)
            {
                //megkeressük az ürlapon érkezett CategoryId-hez tartozó kategóriát
                var category =db.Categories.Find(menuItem.CategoryId);

                //A formról jövő adatokat bemutatjuk az adatbázisnal
                db.MenuItems.Attach(menuItem);
                
                //Az adatbázissal kapcsolatos dolgok eléréséhez kell az entry
                var entry = db.Entry(menuItem);
                
                //az entry-t felhasználva betöltjük a category tábla adatait a menuItem.Category property-be
                entry.Reference(x => x.Category).Load();

                //majd felülírjuk azzal ami a Form-on jön
                menuItem.Category = category;
                entry.State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
