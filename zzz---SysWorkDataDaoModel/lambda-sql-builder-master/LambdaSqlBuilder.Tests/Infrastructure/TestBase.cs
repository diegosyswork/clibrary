using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LambdaSqlBuilder.Tests.Infrastructure
{
    public abstract class TestBase
    {
        protected SqlConnection Connection;

        [SetUp]
        public void Init()
        {
            Bootstrap.Initialize();

            Connection = new SqlConnection(Bootstrap.CONNECTION_STRING);
            Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Connection.Close();
        }
    }
}
