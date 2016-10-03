using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseService databaseService = new DatabaseService(new SqlDatabaseFactory());

            //databaseService.TableItemGroup.CreateItemGroup("MejeriAss", 2);

            //databaseService.TableItemGroup.DeleteItemGroup(4);

            ItemGroup itemgroup = databaseService.TableItemGroup.GetItemGroup(2);

            Console.ReadKey();

        }
    }
}
