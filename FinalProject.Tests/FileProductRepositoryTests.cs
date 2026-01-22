using FinalProject.Controllers;
using NUnit.Framework;
using FinalProject.DataAccess;
using FinalProject.Domain;
using FinalProject.Services;

namespace FinalProject.Tests;

[TestFixture]
public class TablewareCsvSourceTests
{
    private string _testFilePath = null!;

    [SetUp]
    public void SetUp()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"test_products_{Guid.NewGuid()}.txt");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public void Parse_WithValidLine_ReturnsCorrectTableware()
    {
        // Arrange
        var line = "101;Silver Spoon;Tableware;15.50;120";
        File.WriteAllText(_testFilePath, line);
        var source = new TablewareCsvSource(_testFilePath);

        // Act
        var product = source.First(); 

        // Assert
        Assert.That(product.Id, Is.EqualTo(101));
        Assert.That(product.Name, Is.EqualTo("Silver Spoon"));
        Assert.That(product.Category, Is.EqualTo("Tableware"));
        Assert.That(product.Price, Is.EqualTo(15.50m));
        Assert.That(product.Quantity, Is.EqualTo(120));
    }

    [Test]
    public void GetEnumerator_WithInvalidFormat_ThrowsDaoException()
    {
        // Arrange
        var content = "101;Silver Spoon;Tableware";
        File.WriteAllText(_testFilePath, content);
        var source = new TablewareCsvSource(_testFilePath);

        // Act / Assert
        Assert.Throws<DaoException>(() => {
            var list = source.ToList();
        });
    }

    [Test]
    public void ProductDao_FindAll_ReturnsAllItemsFromSource()
    {
        // Arrange
        var content = "101;Spoon;Tableware;1.0;10\n" +
                            "102;Fork;Tableware;2.0;20";
        File.WriteAllText(_testFilePath, content);
        var source = new TablewareCsvSource(_testFilePath);
        var dao = new ProductDao<Tableware>(source);

        // Act
        var results = dao.FindAll();

        // Assert
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results[0].Id, Is.EqualTo(101));
    }

    [Test]
    public void Factory_RegisterAndCreate_ReturnsWorkingDao()
    {
        // Arrange
        var factory = ProductDaoFactory.Instance;
        var source = new TablewareCsvSource(_testFilePath);
        factory.RegisterSource<Tableware>(source);

        // Act
        var dao = factory.CreateProductDao<Tableware>();

        // Assert
        Assert.That(dao, Is.Not.Null);
        Assert.That(dao, Is.InstanceOf<IProductDao<Tableware>>());
    }
    
    [Test]
    public void SearchByPrice_WithFuzzyMatch_ReturnsItemsWithinOneUnitRange()
    {
        // Arrange
        // Searching range from 9.0 to 11.0
        var content = "101;Lower Bound;Tableware;9.00;10\n" +   // Should match
                            "102;Exact Match;Tableware;10.00;10\n" +  // Should match
                            "103;Upper Bound;Tableware;11.00;10\n" +  // Should match
                            "104;Too Low;Tableware;8.99;10\n" +       // Should NOT match
                            "105;Too High;Tableware;11.01;10";        // Should NOT match
    
        File.WriteAllText(_testFilePath, content);

        var source = new TablewareCsvSource(_testFilePath);
        var factory = ProductDaoFactory.Instance;
        factory.RegisterSource<Tableware>(source);
        var dao = factory.CreateProductDao<Tableware>();
        var service = new InventoryService(factory);
        var controller = new ProductController(service);

        // Act
        var results = controller.SearchByPrice(10.0m);

        // Assert
        Assert.That(results.Count, Is.EqualTo(3), "Should find exactly 3 items within the 9.0-11.0 range");
    
        // Verify specific items are present
        var names = results.Select(p => p.Name).ToList();
        Assert.That(names, Contains.Item("Lower Bound"));
        Assert.That(names, Contains.Item("Exact Match"));
        Assert.That(names, Contains.Item("Upper Bound"));
        Assert.That(names, Does.Not.Contain("Too Low"));
        Assert.That(names, Does.Not.Contain("Too High"));
    }
}