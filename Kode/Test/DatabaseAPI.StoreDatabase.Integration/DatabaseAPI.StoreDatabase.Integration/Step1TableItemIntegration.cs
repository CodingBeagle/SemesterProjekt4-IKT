using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.Factories;
using NUnit.Framework;

namespace DatabaseAPI.StoreDatabase.Integration
{
    [TestFixture]
    public class Step1TableItemIntegration
    {
        private IDatabaseService _db;

        [SetUp]
        public void Setup()
        {   
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
        }


    }
}
