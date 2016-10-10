﻿using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableStoreSection;

namespace DatabaseAPI.Factories
{
    public class SqlStoreDatabaseFactory : IStoreDatabaseFactory
    {
        public ITableItem CreateTableItem()
        {
            return new SqlTableItem(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableItemGroup CreateTableItemGroup()
        {
            return new SqlTableItemGroup(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableStoreSection CreateTableStoreSection()
        {
            return new SqlTableStoreSection(new SqlConnectionStringFactory().CreateConnectionString());
        }
    }
}