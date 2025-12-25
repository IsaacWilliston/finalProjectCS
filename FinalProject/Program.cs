using System.Diagnostics;
using FinalProject.Domain;
using FinalProject.Data;
using FinalProject.Services;
using FinalProject.Controllers;

IProductRepository repository = new FileProductRepository("C:\\Users\\Asus\\OneDrive\\Desktop\\FinalProject\\FinalProject\\Dataset\\products.txt");
// Use absolute path
var service = new InventoryService(repository);
var controller = new ProductController(service);

Console.WriteLine("Tableware and Household Goods Warehouse\nv 1.1\nWrite help to receive list of commands.");
while (true)
{
    Console.WriteLine("Enter command\n>>> :");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput)) continue;
    
    var parts = userInput.Split(' ');
    
    string command = parts[0].ToLower();

    switch (command)
    {
        case "help":
            Console.WriteLine("List of commands available:\n" +
                              "info => information about the app:  Developer Full Name, App version, Developer's Contact information ..etc.\n" +
                              "list => list of all products\n" +
                              "search [search category(e.g name, id ..)] *enter* then input [value]=> searches the inventory\n" +
                              "exit => exit the program\n");
            break;
        
        case "list":
            Console.WriteLine($"List of products:");
            var allProducts = controller.GetAllProducts();
            DisplayResults(allProducts);
            break;
        
        case "search":
            if (parts.Length < 2)
            {
                Console.WriteLine("Wrong number of parameters\nUsage example: search [category/name/id... ect]");
                break;
            }
            
            var searchTerm = parts[1].ToLower();
            switch (searchTerm)
            {
                case "name":
                    Console.WriteLine("Enter name of product:");
                    string? productName = Console.ReadLine();
                    Debug.Assert(productName != null, nameof(productName) + " != null");
                    var nameSearchResult = controller.SearchByName(productName);
                    DisplayResults(nameSearchResult);
                    break;
                
                case "category":
                    Console.WriteLine("Enter category:");
                    string? categoryName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(categoryName))
                    {
                        Console.WriteLine("Category cannot be empty.");
                        break;
                    }
                    var categorySearchResult = controller.SearchByCategory(categoryName.Trim());
                    DisplayResults(categorySearchResult);
                    break;
                
                case "price":
                    Console.WriteLine("Enter price:");
                    decimal price = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                    var priceSearchResult = controller.SearchByPrice(price);
                    DisplayResults(priceSearchResult);
                    break;
                
                case "id":
                    Console.WriteLine("Enter product ID:");
                    string? productId = Console.ReadLine();
                    var searchId = Convert.ToInt32(productId);
                    var idSearchResult = controller.SearchById(searchId);
                    DisplayResults(idSearchResult);
                    break;
                default:
                    Console.WriteLine("Enter valid command: search [category/name/id/price]");
                    continue;
            }

            break;

        case "exit":
                return;
    }
}

void DisplayResults(List<Product> products)
{
    if (products.Count == 0)
    {
        Console.WriteLine("No products found matching those criteria.");
        return;
    }

    Console.WriteLine("\n{0,-5} {1,-25} {2,-15} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Stock");
    Console.WriteLine(new string('-', 70));

    foreach (var p in products)
    {
        Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10:C} {4,-10}", 
            p.Id, p.Name, p.Category, p.Price, p.Quantity);
    }
    Console.WriteLine();
}

