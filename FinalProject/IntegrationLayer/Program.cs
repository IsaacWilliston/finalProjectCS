using FinalProject.Domain;
using FinalProject.DataAccess;
using FinalProject.Services;
using FinalProject.Controllers;
using FinalProject.ConsoleView;

var factory = ProductDaoFactory.Instance;
string filePath = "Dataset/products.txt";

try
{
    if (File.Exists(filePath))
    {
        factory.RegisterSource<Product>(new TablewareCsvSource(filePath));
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: File not found at '{filePath}'");
        Console.WriteLine("Please ensure the products.txt file exists in the Dataset folder.");
        Console.ResetColor();
        return;
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error initializing application: {ex.Message}");
    Console.ResetColor();
    return;
}

try
{
    var service = new InventoryService(factory);
    
    var controller = new ProductController(service);
    UserView.Main(controller);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Fatal error: {ex.Message}");
    Console.ResetColor();
}
