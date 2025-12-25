using NUnit.Framework;
using FinalProject.Data;

namespace FinalProject.Tests;

[TestFixture]
public class FileProductRepositoryTests
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
    public void GetAll_WithValidProducts_ReturnsAllProducts()
    {
        var content = "101;Silver Spoon;Tableware;15.50;120\n" +
                     "102;Ceramic Plate;Tableware;25.00;80\n" +
                     "103;Crystal Vase;Household;45.99;30";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);
        
        var products = repository.GetAll();
        
        Assert.That(products, Is.Not.Null);
        Assert.That(products.Count, Is.EqualTo(3));
        
        Assert.That(products[0].Id, Is.EqualTo(101));
        Assert.That(products[0].Name, Is.EqualTo("Silver Spoon"));
        Assert.That(products[0].Category, Is.EqualTo("Tableware"));
        Assert.That(products[0].Price, Is.EqualTo(15.50m));
        Assert.That(products[0].Quantity, Is.EqualTo(120));

        Assert.That(products[1].Id, Is.EqualTo(102));
        Assert.That(products[1].Name, Is.EqualTo("Ceramic Plate"));
        Assert.That(products[1].Category, Is.EqualTo("Tableware"));
        Assert.That(products[1].Price, Is.EqualTo(25.00m));
        Assert.That(products[1].Quantity, Is.EqualTo(80));

        Assert.That(products[2].Id, Is.EqualTo(103));
        Assert.That(products[2].Name, Is.EqualTo("Crystal Vase"));
        Assert.That(products[2].Category, Is.EqualTo("Household"));
        Assert.That(products[2].Price, Is.EqualTo(45.99m));
        Assert.That(products[2].Quantity, Is.EqualTo(30));
    }

    [Test]
    public void GetAll_WithWhitespace_TrimsCorrectly()
    {
        var content = "  101  ;  Silver Spoon  ;  Tableware  ;  15.50  ;  120  ";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);
        
        var products = repository.GetAll();
        
        Assert.That(products.Count, Is.EqualTo(1));
        Assert.That(products[0].Id, Is.EqualTo(101));
        Assert.That(products[0].Name, Is.EqualTo("Silver Spoon"));
        Assert.That(products[0].Category, Is.EqualTo("Tableware"));
        Assert.That(products[0].Price, Is.EqualTo(15.50m));
        Assert.That(products[0].Quantity, Is.EqualTo(120));
    }

    [Test]
    public void GetAll_WithInvalidLines_SkipsInvalidLines()
    {
        var content = "101;Silver Spoon;Tableware;15.50;120\n" +
                     "102;Ceramic Plate;Tableware;25.00\n" + // Parts < 5
                     "103;Crystal Vase;Household;45.99;30;Extra\n" + // Parts != 5
                     "104;Cotton Towel;Household;12.00;200\n" +
                     "105\n" + // id only
                     "106;Glass Bowl;Tableware;10.25;60";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);
        
        var products = repository.GetAll();
        
        Assert.That(products.Count, Is.EqualTo(3));
        Assert.That(products[0].Id, Is.EqualTo(101));
        Assert.That(products[1].Id, Is.EqualTo(104));
        Assert.That(products[2].Id, Is.EqualTo(106));
    }

    [Test]
    public void GetAll_WithEmptyFile_ReturnsEmptyList()
    {
        File.WriteAllText(_testFilePath, string.Empty);
        var repository = new FileProductRepository(_testFilePath);
        
        var products = repository.GetAll();
        
        Assert.That(products, Is.Not.Null);
        Assert.That(products.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetAll_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"somefile.txt");
        var repository = new FileProductRepository(nonExistentPath);


        Assert.Throws<FileNotFoundException>(() => repository.GetAll());
    }

    [Test]
    public void GetAll_WithInvalidNumberFormat_ThrowsException()
    {
        var content = "abc;Silver Spoon;Tableware;15.50;120\n" + // Invalid ID
                     "102;Ceramic Plate;Tableware;invalid;80\n" + // Invalid price
                     "103;Crystal Vase;Household;45.99;abc"; // Invalid quantity
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);

        Assert.Throws<FormatException>(() => repository.GetAll());
    }

    [Test]
    public void GetAll_WithEmptyLines_SkipsEmptyLines()
    {
        var content = "101;Silver Spoon;Tableware;15.50;120\n" +
                     "\n" +
                     "102;Ceramic Plate;Tableware;25.00;80\n" +
                     "\n" +
                     "103;Crystal Vase;Household;45.99;30";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);


        var products = repository.GetAll();
        
        Assert.That(products.Count, Is.EqualTo(3));
        Assert.That(products[0].Id, Is.EqualTo(101));
        Assert.That(products[1].Id, Is.EqualTo(102));
        Assert.That(products[2].Id, Is.EqualTo(103));
    }

    [Test]
    public void GetAll_WithAscendingOrder_ReturnsProductsInAscendingOrder()
    {
        var content = "101;Silver Spoon;Tableware;15.50;120\n" +
                     "102;Ceramic Plate;Tableware;25.00;80\n" +
                     "103;Crystal Vase;Household;45.99;30\n" +
                     "104;Cotton Towel;Household;12.00;200\n" +
                     "105;Stainless Knife;Tableware;18.75;150";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);

        var products = repository.GetAll();

        Assert.That(products.Count, Is.EqualTo(5));
        
        Assert.That(products[0].Id, Is.EqualTo(101));
        Assert.That(products[1].Id, Is.EqualTo(102));
        Assert.That(products[2].Id, Is.EqualTo(103));
        Assert.That(products[3].Id, Is.EqualTo(104));
        Assert.That(products[4].Id, Is.EqualTo(105));
        
        for (int i = 0; i < products.Count - 1; i++)
        {
            Assert.That(products[i].Id, Is.LessThan(products[i + 1].Id));
        }
    }

    [Test]
    public void GetAll_WithDescendingOrder_ReturnsProductsInDescendingOrder()
    {
        var content = "105;Stainless Knife;Tableware;18.75;150\n" +
                     "104;Cotton Towel;Household;12.00;200\n" +
                     "103;Crystal Vase;Household;45.99;30\n" +
                     "102;Ceramic Plate;Tableware;25.00;80\n" +
                     "101;Silver Spoon;Tableware;15.50;120";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);

        var products = repository.GetAll();

        Assert.That(products.Count, Is.EqualTo(5));
        
        Assert.That(products[0].Id, Is.EqualTo(105));
        Assert.That(products[1].Id, Is.EqualTo(104));
        Assert.That(products[2].Id, Is.EqualTo(103));
        Assert.That(products[3].Id, Is.EqualTo(102));
        Assert.That(products[4].Id, Is.EqualTo(101));
        
        for (int i = 0; i < products.Count - 1; i++)
        {
            Assert.That(products[i].Id, Is.GreaterThan(products[i + 1].Id));
        }
    }

    [Test]
    public void GetAll_WithUnsortedOrder_ReturnsProductsInFileOrder()
    {
        var content = "103;Crystal Vase;Household;45.99;30\n" +
                     "101;Silver Spoon;Tableware;15.50;120\n" +
                     "105;Stainless Knife;Tableware;18.75;150\n" +
                     "102;Ceramic Plate;Tableware;25.00;80\n" +
                     "104;Cotton Towel;Household;12.00;200";
        File.WriteAllText(_testFilePath, content);
        var repository = new FileProductRepository(_testFilePath);

        var products = repository.GetAll();

        Assert.That(products.Count, Is.EqualTo(5));
        
        Assert.That(products[0].Id, Is.EqualTo(103));
        Assert.That(products[1].Id, Is.EqualTo(101));
        Assert.That(products[2].Id, Is.EqualTo(105));
        Assert.That(products[3].Id, Is.EqualTo(102));
        Assert.That(products[4].Id, Is.EqualTo(104));
    }
}