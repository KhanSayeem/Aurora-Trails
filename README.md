# Aurora Trails

Aurora Trails is a modern ASP.NET Core MVC platform that connects tourists with vetted travel agencies. It wraps the existing booking, feedback, and package management logic in a sleek Tailwind UI so both sides can collaborate smoothly.

---

## 1. Prerequisites

| Requirement                                                                                              | Notes                                                               |
| -------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------- |
| :white_check_mark: [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)                             | Required to build and run the MVC application.                      |
| :white_check_mark: [Node.js 18+](https://nodejs.org/en/download/)                                        | Needed to compile Tailwind CSS assets.                              |
| :white_check_mark: [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | Local database engine used by the app (choose the Express edition). |
| :white_check_mark: [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup)                   | Helpful for inspecting data and verifying the seeded content.       |
| :white_check_mark: Git                                                                                   | To clone and manage the repository.                                 |

### Connecting SSMS to SQL Express

1. Launch SSMS after installation.
2. The **Connect to Server** dialog appears.
3. Set **Server name** to `<YourComputerName>/SQLEXPRESS` (find the computer name via `System Information`).
4. Check the **Trust Certificate..** box
5. Leave **Authentication** as Windows Authentication, then click **Connect**.

---

## 2. Project Setup

```bash
# clone the repository
git clone https://github.com/KhanSayeem/Aurora-Trails.git
cd Aurora-Trails/TourismApp

# install the front-end toolchain (Tailwind + PostCSS)
npm install
```

> :information_source: The solution ships with a SQLite fallback (`app.db`) but is configured to work with SQL Server Express by default. The seed routine will hydrate whichever provider is configured in `appsettings.json`.

---

## 3. Configure the Database

Aurora Trails ships with a ready-to-use connection string pointing at SQL Server Express:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TourismDb;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

If your SQL Express instance has a different name, adjust the `Server=` value. Afterwards run the EF migrations:

```bash
dotnet ef database update
```

---

## 4. Build the Tailwind styles

```bash
npm run build:css
```

For live development you can keep Tailwind in watch mode:

```bash
npm run dev:css
```

---

## 5. Seed & Run the App

Seeding happens automatically on startup (see `Data/SeedData.cs`). Just run:

```bash
# run the web app
dotnet run
```

On first launch Aurora Trails creates:

- Roles: `Tourist`, `Agency`
- Agency profile with curated mock tour packages & bookings
- Tourist profile linked to the sample data

The site hosts at `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).

---

## 6. Demo Credentials

| Role    | Email              | Password    |
| ------- | ------------------ | ----------- |
| Agency  | `agency@demo.com`  | `Passw0rd!` |
| Tourist | `tourist@demo.com` | `Passw0rd!` |

Tourists can browse tours, create bookings, and leave feedback. Agency users manage packages, bookings, and review analytics via the dashboard.

---

## 7. Useful Commands

```bash
# apply migrations again if needed
dotnet ef database update

# publish for production
dotnet publish -c Release -o ./publish

# run Tailwind in watch mode
npm run dev:css
```

---

## 8. Troubleshooting

- **Database connection errors** → Recheck the SQL Server name (<ComputerName>/SQLEXPRESS) and confirm the SQL Server (SQLEXPRESS) service is running.
- **Missing styles** → Ensure
  pm run build:css completed successfully and wwwroot/dist/app.css exists.
- **Seed data absent** → Delete pp.db (if using SQLite) or drop the TourismDb database in SQL Server, then run dotnet run again to trigger the initializer.

Happy exploring! :earth_africa:

