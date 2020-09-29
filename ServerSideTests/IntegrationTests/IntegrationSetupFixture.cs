using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ServerSide;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace ServerSideTests.IntegrationTests
{

    [SetUpFixture]
    public class IntegrationSetupFixture
    {
        protected TestServer? server;
        internal static HttpClient Client = null!;
        internal static string AssemblyPath = null!;
        internal static string TestOutputPath = null!;
        internal static string FlowLogsConnectionString = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            server = new TestServer(new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            Client = server.CreateClient();

        }

        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
