using Microsoft.Extensions.Configuration;

namespace Support.DataConductor.ServerTests.TestHelpers
{
    public static class ConfigurationFns
    {
        public static IConfigurationRoot GetIConfigurationRoot(this string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

    }
}
