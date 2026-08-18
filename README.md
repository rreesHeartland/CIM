# Heartland Calibrated Instruments Manager

An ASP.NET Core 8 MVC + Entity Framework Core 8 + Microsoft SQL Server web application.
It is a full C# conversion of the original **Heartland Calibrated Instruments Manager**
Microsoft PowerApp, used to manage the removal, installation and verification of
calibration instruments across plant areas, organised by calibration cycle (Spring / Fall).

## Features

- **Home** – activity menu (Calibrations, Browse & Update Pictures, Instrument Records, Admin).
  The Calibrations tile is disabled until a cycle is confirmed.
- **Calibrations** – select area (with per-area progress bars), select instruments due for
  calibration, and step each instrument through the **Remove → Install → Verify** workflow.
- **Admin** – Area Management (set/reset cycle, open/close areas), batch update of next
  calibration dates, and reset of the calibration cycle.
- **Instrument Records** – look up instruments by area, tag search or dropdown.
- **Image Browser / Updater** – view and replace close-up and wide-angle instrument photos.
- Technician name captured on first use and stored in session for audit logging.
- Every workflow / photo action is written to the **InstrumentLog** table.

## Tech Stack

- ASP.NET Core 8 MVC
- Entity Framework Core 8 (SQL Server provider)
- Bootstrap 5 + Font Awesome 6
- jQuery (AJAX confirmation on instrument actions)
- File-system image storage under `wwwroot/uploads/{instrumentId}/`

## Project Structure

```
HeartlandCIM.sln
src/HeartlandCIM.Web/
  Controllers/       Home, Account, Calibrations, Admin, Instruments, Images
  Models/            CalibrationInstrument, AreaStatus, InstrumentLog
  ViewModels/        View-specific DTOs
  Services/          ICycleService, IInstrumentService, IAreaService, ILogService (+ impls)
  Data/              ApplicationDbContext, Migrations/
  Views/             Razor views for every page + shared layout
  wwwroot/           css/site.css, js/site.js, uploads/
  appsettings.json   Connection string (placeholder)
database/
  schema.sql         Standalone CREATE TABLE script
  seed.sql           Sample AreaStatus + CalibrationInstruments data
```

## Setup

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Microsoft SQL Server (Express, Developer, LocalDB or full)

### 2. Restore packages
```bash
dotnet restore
```

### 3. Configure the connection string
Edit `src/HeartlandCIM.Web/appsettings.json` and set `ConnectionStrings:DefaultConnection`,
for example:
```json
"DefaultConnection": "Server=localhost;Database=HeartlandCIM;Integrated Security=True;TrustServerCertificate=True;"
```
(Use `User Id=...;Password=...;` instead of `Integrated Security=True` for SQL authentication.)

### 4. Create the database schema

**Option A – EF Core migrations (recommended):**
```bash
cd src/HeartlandCIM.Web
dotnet tool install --global dotnet-ef        # if not already installed
dotnet ef database update
```

**Option B – standalone SQL scripts:**
```sql
CREATE DATABASE HeartlandCIM;
GO
-- then run database/schema.sql, followed by database/seed.sql
```

> If you used EF migrations and also want sample data, run `database/seed.sql` afterwards.

### 5. Run
```bash
cd src/HeartlandCIM.Web
dotnet run
```
Then open the URL shown in the console (e.g. `https://localhost:5001`).
On first use you will be prompted to enter your technician name.

## Business Rules

- **Calibration cycle:** Spring cutoff = **Apr 30**, Fall cutoff = **Oct 31** of the cycle year.
  Only instruments with `Next_Cal_Date <= cutoff` appear in the calibration workflow.
- **Status (computed, never stored):**
  - `Verified`  – `Feedback_Verified_Time` set
  - `Installed` – `Install_Time` set, not yet verified
  - `Removed`   – `Removed_Time` set, not yet installed
  - `Not Started` – none set
- **Next calibration date:** CCP → Last Cal Date + 6 months; CP → Last Cal Date + 12 months.
- **Access requirement colouring:** Scissor Lift / Crawl under = red bold; Ladder = italic;
  Ground Level = plain.
- **Drain tanks:** instruments with a `DrainTanks` value show a warning modal before removal.

## Colour Scheme (matches the PowerApp)

| Purpose        | Hex       |
|----------------|-----------|
| Primary navy   | `#00126B` |
| Accent blue    | `#0086D0` |
| Green / success| `#09AC7D` |
| Red / remove   | `#B80000` |
| Yellow / warn  | `#FFBF00` |
