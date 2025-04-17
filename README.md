# BankApp Webbapplikation

Detta är en ASP.NET Core Razor Pages-applikation för att hantera kunder, konton och transaktioner i en bankmiljö. Projektet är uppdelat i flera lager med arkitektur enligt best practices.

## Länk till Azure

https://aurorabank-gjdmc9f6eefvdza3.swedencentral-01.azurewebsites.net

##  Databas

Den här applikationen använder en befintlig databas (Database First).

👉 **[Ladda ner databasen här (BankAppData.bak)](https://aspcodeprod.blob.core.windows.net/school-dev/BankAppDatav2%20(1).bak)**

## Tekniker och ramverk

- ASP.NET Core Razor Pages
- Entity Framework Core (Database First)
- ASP.NET Core Identity
- AutoMapper
- Bootstrap (SB Admin 2-template)
- JavaScript (AJAX för transaktionspaginering)
- SQL Server
- Azure App Service (driftsättning)

## Projektstruktur

- **BankAppProject** – Huvudprojekt med Razor Pages, ViewModels, Profiles och frontend
- **Service** – Klassbibliotek för affärslogik, services och interfaces
- **DataAccessLayer** – Klassbibliotek för databaskoppling, DTOs, modeller, repositories, context
- **BankAppTransactionMonitor** – Console App som analyserar misstänkta transaktioner per land

## Roller & Behörighet

- **Admin** – Kan administrera systemanvändare
- **Cashier** – Kan administrera kunder och konton
- Kunder är inte inloggade användare

### Seedade användare:
| E-post                       | Lösenord | Roll    |
|------------------------------|----------|---------|
| admin@aurorabank.com         | Abc123#  | Admin   |
| cashier@aurorabank.com       | Abc123#  | Cashier |

## Funktionalitet

- Skapa, ändra och sök kunder
- Visa kundbild med alla konton & totalt saldo
- Transaktionsbild för konto – laddar 20 i taget via AJAX
- Insättning, uttag och överföring mellan konton (via transaktioner)
- Startsida med statistik per land – publik
- Top 10-kunder per land (responsecachad i 1 min)
- Console-app som kan köras och som loggar misstänkta transaktioner

## Starta lokalt

1. Klona repo
2. Kontrollera att connection string i `appsettings.json` pekar mot rätt instans av SQL Server.  
   Exempel:  
   `"Server=localhost;Database=BankAppData;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"`
3. Återställ databasen från `.bak`-filen till `BankAppData` med hjälp av SSMS
4. Starta projektet – databasen uppdateras automatiskt med `Migrate()`
5. Logga in med en av de seedade användarna



