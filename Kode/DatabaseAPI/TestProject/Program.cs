﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.TableItem;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;


namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlTableItem test = new SqlTableItem();
            
            test.CreateItem("Rieder Pølse", 2);
            List<Item> searchList = test.SearchItems("Rieder");

            foreach (var item in searchList)
            {
                Console.WriteLine(item.Name);
            }

            DatabaseService databaseService = new DatabaseService(new SqlDatabaseFactory());

            //databaseService.TableItemGroup.CreateItemGroup("MejeriAss", 2);

            //databaseService.TableItemGroup.DeleteItemGroup(4);

            ItemGroup itemgroup = databaseService.TableItemGroup.GetItemGroup(2);

            Console.ReadKey();
        }
    }
}
