using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Pocos.DataConductorEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using static Support.DataConductor.ServerTests.TestHelpers.IOFn;

namespace Support.DataConductor.ServerTests.TestHelpers
{

#pragma warning disable CS0168 // Variable is declared but never used
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
    public class DataConductorRepository : SimpleBaseRepository
    {
        private const string CreateTestDbDDL = @"CREATE DATABASE [TestDb]";

        private const string CreateTables = @"
CREATE TABLE [dbo].[Contexts](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [Context] [varchar](64) NOT NULL,
    [Explanation] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Contexts_1] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[Settings](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [Context] [int] NOT NULL,
    [SettingName] [varchar](64) NOT NULL,
    [Value] [varchar](max) NULL,
    [system_type_id] [tinyint] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[DataConnection](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [ConnectionName] [varchar](32) NOT NULL,
    [Explanation] [varchar](128) NULL,
    [ConnectionType] [int] NOT NULL,
    [ConnectionString] [varchar](512) NOT NULL,
 CONSTRAINT [PK_DataConnections] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ConnectionStrings] UNIQUE NONCLUSTERED 
(
    [ConnectionString] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

use master";

        private const string DropTestDbDDL = @"

IF EXISTS(SELECT * FROM sys.databases WHERE name = 'TestDb')
BEGIN
    ALTER DATABASE [TestDb] set single_user with rollback immediate;
    DROP DATABASE [TestDb]
END";

        public static readonly List<Settings> settings = new List<Settings>();
        public static readonly List<DataConnection> connections = new List<DataConnection>();
        public static readonly List<Contexts> contexts = new List<Contexts>();
        public static string TestOutputFolder = null!;
        private readonly List<string> utilities;

        private Action<IDbConnection, IDbTransaction?> GetExecuteNonQueryAction(string commandText) => (cnxn, trnx) =>
            {
                var command = cnxn.CreateCommand();
                command.CommandText = commandText;

                command.Transaction = trnx;
                command.ExecuteNonQuery();
            };


        public DataConductorRepository(string connectionString, string outputFolder, List<string> utilities)
            : base(connectionString)
        {
            TestOutputFolder = outputFolder;
            this.utilities = utilities;
            RunInSqlConnection(GetExecuteNonQueryAction(DropTestDbDDL), InitialCatalogue.master);
            RunInSqlConnection(GetExecuteNonQueryAction(CreateTestDbDDL), InitialCatalogue.master);
            RunInSqlConnection(GetExecuteNonQueryAction(CreateTables), InitialCatalogue.asConfigured);
            SetupSeedData();
        }

        private void RunInSqlConnection(Action<IDbConnection, IDbTransaction?> dataAction, InitialCatalogue initialCatalogue)
        {
            try
            {
                var catalogue = initialCatalogue == InitialCatalogue.master ? connectionString.Replace("TestDb", "master") : connectionString;
                using IDbConnection dataConnection = new SqlConnection(catalogue);
                dataConnection.Open();
                dataAction(dataConnection, null);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                throw;
            }
        }

        private void SetupSeedData()
        {
            try
            {
                SetTestOutputFolders(FolderAction.Create);
                connections.Add(new DataConnection { ConnectionName = "CatalogueApi", ConnectionString = "https://bpgflowcatalogueapi.azurewebsites.net/api", ConnectionType = 2, Explanation = "Master Catalogue API holding flow catalogue matadata for all Utilities" });
                contexts.Add(new Contexts { Context = "Outgoing DataFlow target folder", Explanation = "The file system location where newly created Dataflows are saved for sending." });

                RunInSqlConnection((cnxn, tranx) =>
                {
                    cnxn.Insert(settings);
                    cnxn.Insert(connections);
                    cnxn.Insert(contexts);
                }, InitialCatalogue.asConfigured);
            }
            catch (Exception)
            {
#if DEBUG
                Debugger.Break();
#endif
                throw;
            }
        }

        private void SetTestOutputFolders(FolderAction folderAction)
        {
            utilities.ForEach(u =>
            {
                var directory = Path.Combine(TestOutputFolder, u);
                directory.ManageTestOutputFolders(folderAction);
                if (folderAction == FolderAction.Create)
                {
                    settings.Add(new Settings { Context = 1, SettingName = u, Value = Path.Combine(TestOutputFolder, u), system_type_id = 167 });
                }
            });
        }

        public void DropTestDb()
        {
            try
            {
                SetTestOutputFolders(FolderAction.Delete);
                RunInSqlConnection(GetExecuteNonQueryAction(DropTestDbDDL), InitialCatalogue.master);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                throw;
            }

        }
    }

    public enum InitialCatalogue { master, asConfigured }
#pragma warning restore CS0168 // Variable is declared but never used
}
