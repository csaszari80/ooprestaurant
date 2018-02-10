﻿# Jegyzetek az étterem alkalmazáshoz
Segítség  [Asccii grafikákhoz](www.asciidraw.com/#draw)

# Codefirst Migration
Szükséges hozzá:
 - Entity framework: A projekt template tartalmazza az entity framework nuget csomagot ezért azt nem kell telepíteni.
 - codeFirst Migration engedélyezése: Tools | Nuget Package Manager | Package Manager Console | Projekt kiválasztása | "Enable-Migrations" parancs kiadása. Ezzel létrejön egy Migrations könyvtár és benne egy Configuration.cs állomány
 - Ezután: "Add-Migration 'InitialCreate'" parancs létrehozza az első lépést (ha még nincs adatbázis, ha van ez automatikusan létrejön) InitialCreate néven.
 - Ezután: "Update-Database" létrehozza az adatbázist az InitialCreate alapján, a beállított helyen. Alapértelmezetten ez a localdb
Innentől kezdve, ha módosítunk a modellen akkor egy új migrációs lépés létrehozásával és futtatásával ("Update-Database") az adatbázist is frissíthetjük. A bejátszott migrációs lépések a dbo._MigrationHistory táblában tárolódnak. 

## Saját adat adatbázisba tétele
- Létre kell hozni egy saját osztályt ami az adatokat tartalmazza pl Category
- Az osztályt fel kell venni DbSet típusú property-ként az ApplicationDbContext osztályba (itt az IdentityModels.cs-ben van)
- ki kell adni az "Add-Migration 'elnevezés'" parancsot
- ki kell adni az "Update-Database parancsot"


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