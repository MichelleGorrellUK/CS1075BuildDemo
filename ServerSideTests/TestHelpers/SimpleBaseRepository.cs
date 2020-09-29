using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Support.DataConductor.ServerTests.TestHelpers
{
    public class SimpleBaseRepository
    {
        protected readonly string connectionString;

        public SimpleBaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<T> Query<T>(string sqlFunction)
        {
            try
            {
                using IDbConnection dataConnection = new SqlConnection(connectionString);
                dataConnection.Open();
                return dataConnection.Query<T>(sqlFunction);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                Debugger.Break();
#endif
                throw;
            }
        }
    }
}
