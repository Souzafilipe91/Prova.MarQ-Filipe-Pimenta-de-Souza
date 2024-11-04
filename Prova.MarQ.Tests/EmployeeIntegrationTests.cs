using Xunit;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Prova.MarQ.API; // Namespace da sua API principal
using Prova.MarQ.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class EmployeeIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public EmployeeIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove o contexto de banco de dados real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProvaMarqDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adiciona um contexto de banco de dados em memória para testes
                services.AddDbContext<ProvaMarqDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task RegisterPoint_ShouldReturnSuccess()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProvaMarqDbContext>();

            // Cria um funcionário para o teste com PIN específico
            var employee = new Employee
            {
                Name = "Teste",
                Documento = "123456789",
                PIN = "1234",
                IsDeleted = false
            };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }

        // Envia o PIN para o endpoint register-point
        var pin = "1234"; // PIN do funcionário criado acima
        var content = new StringContent(JsonConvert.SerializeObject(pin), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Employee/register-point", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Ponto registrado com sucesso", responseString);
    }
}
