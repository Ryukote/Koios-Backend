# Dokumentacija

## .NET Core Web Api projekt

Backend je napravljen u .NET Core, verziji 2.2
Solution se zove **KoiosOffers** te se sastoji od dva projekta.
Prvi projekt je Web API, a drugi projekt je xUnit test projekt.

Za početak, potrebno je preuzeti i instalirati [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)

Nakon toga treba otvoriti solution, gdje prvo treba:

- restorati NuGet pakete
- buildati solution
- pokrenuti testove iz projekta **KoiosOffersTests** (testovi koji ne prođu treba ručno pokrenuti testove)

Za kraj, potrebno je u Web API projektu u **appsettings.json** staviti ispravan connection string, te u Visual Studio u **Package Manager Console** upisati sljedeće dvije naredbe:

- **Add-Migration** (gdje će zatražiti proizvoljan naziv migracije)
- **Update-Database**

## Testna skripta za insertanje dummy podataka

Potrebno je otvoriti SSMS, otvoriti novi file, te označiti u dropdownu bazu **Koios** u kojoj treba copy paste cijelog sadržaja **sqlinsert.txt** (nalazi se u glavnom direktoriju) i pokrenuti ga. Potrebno je neko vrijeme da se izvrši svaki insert.

Ako nešto bude krivo, potrebno je obrisati sve i reseedati tablice (točno ovim redoslijedom):

- **delete from OfferArticle**
- **delete from Article**
- **delete from Offer**
- **DBCC CHECKIDENT ('Offer', RESEED, 0)**
- **DBCC CHECKIDENT ('Article', RESEED, 0)**
- **DBCC CHECKIDENT ('OfferArticle', RESEED, 0)**

## ReactJS Frontend

Za UI je odabran ReactJS.
Za pokretanje view aplikacije potrebno je imati instalirani [NodeJS](https://nodejs.org/en/download/). Treba otvoriti u **CMD**-u direktorij **{root}\KoiosOffersUI\koiosui** i upisati naredbu **npm i**, te nakon toga pokrenuti aplikaciju sa naredbom **npm start**.

Kako bi aplikacija radila, Web API treba biti upaljen (za potrebe zadatka, na istom računalu).
