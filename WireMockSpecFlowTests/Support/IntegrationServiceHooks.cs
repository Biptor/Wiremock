using BoDi;
using Microsoft.Extensions.Configuration;

namespace WireMockSpecFlowTests.Support
{
    [Binding]
    public class IntegrationServiceHooks
    {
        [BeforeTestRun]
        public static void RegisterSettings(IObjectContainer objectContainer)
        {
            if (!objectContainer.IsRegistered<IConfiguration>())
            {
                objectContainer.RegisterInstanceAs<IConfiguration>(
                    new ConfigurationBuilder()
                        .AddJsonFile(
                            "appsettings.integration.json", optional: false, reloadOnChange: true)
                        .Build());
            }
        }
    }
}
