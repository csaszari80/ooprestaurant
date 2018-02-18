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
        [Authorize(Roles = "Headwaiter,Admin")] //csak a főpincér vagy az admin csoport tagjai használhatják a controlt [lehet [Authorize Users="felhsználók emailcímei", de ezt nem használjuk]
        public ActionResult Create()
        {
            var menuItem = new MenuItem();

            //A választható kategóriák listájához szükség van egy selectList-re
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Headwaiter,Admin")] //csak a főpincér vagy az admin csoport tagjai használhatják a controlt
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {
            

            //megkeressük az ürlapon érkezett CategoryId-hez tartozó kategóriát
            var category = db.Categories.Find(menuItem.CategoryId);

            //Ha nem találja meg a kategóriát akkor vissza kell küldeni
            if (category == null)
            {
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            //A formról jövő adatokat bemutatjuk az adatbázisnal
            db.MenuItems.Attach(menuItem);

            //Mivel ez egy újelem lesz ezért erre nincs szükség
            //var entry = db.Entry(menuItem);
            //entry.Reference(x => x.Category).Load();

            //majd felülírjuk azzal ami a Form-on jön
            menuItem.Category = category;
            
            // Az adatok validációját töröljük (eddig nem volt valid mivel a category null volt ( a formról csak a csategorID-jön)
            ModelState.Clear();
            // Újra validáljuk
            TryValidateModel(menuItem);

         

            if (ModelState.IsValid)
            {

                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //A választható kategóriák listájához szükség van egy selectList-re ez itt azért kell hogyha nem jó az űrlap akkor is legyen lenyíló
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }
        /// <summary>
        /// A paraméterben megadott MenuItem típusú objektum AssignableCategories property-jét tölti fel az adatbázisból lelérdezett értékekkel.
        /// </summary>
        /// <param name="menuItem"></param>
        private void LoadAssignableCategories(MenuItem menuItem)
        {
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name");
        }

        // GET: MenuItems/Edit/5
        [Authorize]
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
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)  //be kell engedni a lenyíló által kiválasztott CategoryId azonosítót is
        {
            //megkeressük az ürlapon érkezett CategoryId-hez tartozó kategóriát
            var category = db.Categories.Find(menuItem.CategoryId);

            //Ha nem találja meg a kategóriát akkor vissza kell küldeni
            if (category == null)
            {
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            //A formról jövő adatokat bemutatjuk az adatbázisnal
            db.MenuItems.Attach(menuItem);

            //Az adatbázissal kapcsolatos dolgok eléréséhez kell az entry
            var entry = db.Entry(menuItem);

            //az entry-t felhasználva betöltjük a category tábla adatait a menuItem.Category property-be
            entry.Reference(x => x.Category).Load();

            //majd felülírjuk azzal ami a Form-on jön
            menuItem.Category = category;
            
            // Az adatok validációját töröljük (eddig nem volt valid mivel a category null volt ( a formról csak a csategorID-jön)
            ModelState.Clear();
            // Újra validáljuk
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
                
                entry.State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize]
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
        [Authorize]
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
