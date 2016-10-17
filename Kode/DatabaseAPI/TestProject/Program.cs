using System;
using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI;
using DatabaseAPI.Factories;


namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseService test = new DatabaseService(new SqlStoreDatabaseFactory());

            /*
            test.TableItem.DeleteItem(1);
            List<Item> searchList = test.TableItem.SearchItems("Rieder");
            
            foreach (var item in searchList)
            {
                Console.WriteLine(item.Name);
            }


            DatabaseService databaseService = new DatabaseService(new SqlStoreDatabaseFactory());

            databaseService.TableItemGroup.CreateItemGroup("Kød");

            //databaseService.TableItemGroup.DeleteItemGroup(4);

            ItemGroup itemgroup = databaseService.TableItemGroup.GetItemGroup(2);

            var stupidList = databaseService.TableItemGroup.GetAllItemGroups();
            foreach (var item in stupidList)
            {
                Console.WriteLine(item.ItemGroupName);
            }

            */

            //test.TableFloorplan.UploadFloorplan("TestFloorplan", 1, 1, "beagle.jpg");

            //test.TableFloorplan.GetFloorplan(1);

            Console.ReadKey();
        }
    }
}
