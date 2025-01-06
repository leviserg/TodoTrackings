Infrastructure project:
    - Microsoft.EntityFrameworkCore;
	- Microsoft.EntityFrameworkCore.SqlServer;
	- Microsoft.EntityFrameworkCore.Design;
Presentation project:
    - Microsoft.EntityFrameworkCore.Tools;

$ add migration > dotnet ef migrations add InitialMigration --project Infrastructure --startup-project Presentation
$ apply migr.   > dotnet ef database update --project Infrastructure --startup-project Presentation
$ drop db.      > dotnet ef database drop --project Infrastructure --startup-project Presentation
$ remove migr.  > dotnet ef migrations remove --project Infrastructure --startup-project Presentation