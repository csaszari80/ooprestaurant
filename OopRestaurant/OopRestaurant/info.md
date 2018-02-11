# Jegyzetek az étterem alkalmazáshoz
Segítség  [Asccii grafikákhoz](www.asciidraw.com/#draw)

## Lenyíló lista több tábla adatainak összefűzéséhez:
- A modelben kell két mező egy a kiválasztott elem azonosítójához egyet a választható elemek listájának.
  - A probléma: A model változtatását az adatbázis változásnak is kellene követni (migrációval két új mező jönne létre a táblában) azonban ebben az esetben mi ezt nem akarjuk
  - A megoldás: [NotMapped] annotáció
- A GET Controlleren kitöltjük ezt a két mezőt. 
  - Az első a kiválasztott elemmel együtt jön az adatbázisból (ott már ki van töltve). Fontos, hogy mivel ez egy másik táblával kapcsolt adat ezért azt be kell tölteni.
  - A másodikhoz le kell kérnünk a kapcsolódó tábla tartalmát egy speciális típusba
- A view-ban ezzel a két paramétert felhasználva megjelenítem a dropdownList-et.
- Ahhoz, hogy a view-ból kiválasztott adat feldolgozásra kerüljön szükséges a POST controllert is megdolgozni
  - Az első dolog, hogy egyáltalán átvegye az adatot, mivel a template alpján létrehozott controllereknél bind-olva van, hogy mit vehet át azt ki kell egészíteni.
  - A következő problémát az okozza, hogy: A kategóriával kapcsolatban formról egy int jön és a menuItems táblában is int típusú oszlop van, a menuItem modeljében egy Category típusú adat van ezek viszont a Categories táblában tárolódnak. A controller a menuItem modelljével dolgozik annak módosításait menti el az adatbázisba(viszont csak egy int-et kap és azt is kell kiadnia).
    - Megoldás: 
    ```
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
    ```


# Codefirst Migration
Szükséges hozzá:
 - Entity framework: A projekt template tartalmazza az entity framework nuget csomagot ezért azt nem kell telepíteni.
 - codeFirst Migration engedélyezése: Tools | Nuget Package Manager | Package Manager Console | Projekt kiválasztása | **Enable-Migrations** parancs kiadása. Ezzel létrejön egy Migrations könyvtár és benne egy Configuration.cs állomány
 - Ezután: **Add-Migration 'InitialCreate'** parancs létrehozza az első lépést (ha még nincs adatbázis, ha van ez automatikusan létrejön) InitialCreate néven.
 - Ezután: **Update-Database** létrehozza az adatbázist az InitialCreate alapján, a beállított helyen. Alapértelmezetten ez a localdb
Innentől kezdve, ha módosítunk a modellen akkor egy új migrációs lépés létrehozásával és futtatásával (**Update-Database**) az adatbázist is frissíthetjük. A bejátszott migrációs lépések a *dbo._MigrationHistory* táblában tárolódnak. 

## Saját adat adatbázisba tétele
- Létre kell hozni egy saját osztályt ami az adatokat tartalmazza pl Category
- Az osztályt fel kell venni DbSet típusú property-ként az ApplicationDbContext osztályba (itt az IdentityModels.cs-ben van)
- ki kell adni az "Add-Migration 'migráció elnevezés'" parancsot
- ki kell adni az "Update-Database parancsot"

## Adatbázis helyének kijelölése
- Az adatbázis kapcsolatok a web.config connectionstrings szekciójában vannak megadva (több is lehet de alapértelmezetten egy van ez a defaultconnection)
- Az adatbázis fájlok a github szikronizálásnál nem kerülnek át az adatbázis a localdb-n jön létre automatikusan vagy kézzel egy **update-database** után.
- Ezt szerkesztve van lehetőség másik adatbázis szerverhez kapcsolódáshoz: [Connection Stringek szerkesztéséhez puska](https://connectionstrings.com)

## Kezdeti adatfeltöltés:
- \migrations\configuration.cs a Seed függvény-ben lehet az iniciális adatfeltöltést végrehajtani
- ez minden **update-database** kiadásakor lefut


## Hasznos parancsok
- **update-database -TargetMigration 0**: adatbázis visszaállítása az összes migráció előtti állapotba
- **update-database -TargetMigration Migrációnév**: adatbázis megadott migráció utáni állaptra való visszaállítása
- **update-database -Script**: Egy sql scriptet generál amely a módosításokat tartalmazza (az sql serveren megfuttatva ugyanazt a hatást érjuk el mintha a -script nélkül adtuk volna ki a parancsot)



# Az étterem projekt leírása(specifikáció)
## Képernyőképek
Nem készítünk ilyet mert az MVC template-ek elkészítik nekünk a képernyőket. Amit kapunk azt fogjuk használni 

## Szereplők:
### Érdeklődő
### Étlap
#### Példa étlap:
----
  - Pizzák:
    - Margarita (mozzarella, pizzaszósz)		200 Ft
	- Hawai(sonka, ananász, mozzarella, pizzaszósz)		300 Ft

  - Italok: 
    - Ásványvíz(3dl) 100 Ft
	- Cola(3dl) 120 Ft
----
## Forgatókönyvek
### Érdeklődő eldönti, hogy akar-e nálunk enni.
Érdeklődő megnézi az étlapot, hogy mit és mennyiért lehet enni