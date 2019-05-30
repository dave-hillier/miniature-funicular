#docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-CU14-ubuntu
cd Issues
dotnet ef migrations add InitialCreate
cd -
cd Properties
dotnet ef migrations add InitialCreate
cd -
cd Tasks
dotnet ef migrations add InitialCreate
cd -
cd Users
dotnet ef migrations add InitialCreate
cd -

