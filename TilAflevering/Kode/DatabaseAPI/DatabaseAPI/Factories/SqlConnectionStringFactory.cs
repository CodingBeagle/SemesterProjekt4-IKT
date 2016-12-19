namespace DatabaseAPI.Factories
{
    class SqlConnectionStringFactory : IConnectionStringFactory
    {
        public string CreateConnectionString()
        {
            return
                "Server=tcp:storedatabase.database.windows.net,1433;Initial Catalog=StoreDatabase;Persist Security Info=False;User ID=Rieder;Password=Poelse$69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
