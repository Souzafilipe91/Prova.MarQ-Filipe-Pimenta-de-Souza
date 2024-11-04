using Xunit;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class EmployeeTests
{
    private ProvaMarqDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ProvaMarqDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new ProvaMarqDbContext(options);
    }

    [Fact]
    public async Task CreateEmployee_ShouldAddEmployeeToDatabase()
    {
        // Arrange
        var dbContext = GetDbContext();
        var employee = new Employee
        {
            Name = "Teste",
            Documento = "123456789",
            PIN = "1234"
        };

        // Act
        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();

        // Assert
        var savedEmployee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Documento == "123456789");
        Assert.NotNull(savedEmployee);
        Assert.Equal("1234", savedEmployee.PIN);
    }

    [Fact]
    public async Task SoftDeleteEmployee_ShouldMarkEmployeeAsDeleted()
    {
        // Arrange
        var dbContext = GetDbContext();
        var employee = new Employee
        {
            Name = "Teste",
            Documento = "123456789",
            PIN = "1234"
        };
        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();

        // Act
        employee.IsDeleted = true;
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedEmployee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Documento == "123456789");
        Assert.True(deletedEmployee.IsDeleted);
    }
}
