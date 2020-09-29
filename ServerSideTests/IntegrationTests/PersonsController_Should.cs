using FluentAssertions;
using NUnit.Framework;
using ServerSide.DTOs;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ServerSideTests.IntegrationTests
{
    [TestFixture()]
    public class PersonsController_Should
    {
        [Test()]
        public async Task GetCustomers()
        {
            var result = await IntegrationSetupFixture.Client.GetFromJsonAsync<PersonDTO[]>("api/persons");
            result.Should().BeOfType<PersonDTO[]>();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(4);
        }
    }
}