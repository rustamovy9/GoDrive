enter to this path : GoDrive\src\backend
command for migrations: dotnet ef migrations add "name" -p .\Infrastructure\ -s .\MobileApp\ --output-dir DataAccess/Migrations
command for database update : dotnet ef database update -p .\Infrastructure\ -s .\MobileApp\ 