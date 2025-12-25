using System.Diagnostics;
using FinalProject.Domain;
using FinalProject.Data;
using FinalProject.Services;
using FinalProject.Controllers;

IProductRepository repository = new FileProductRepository("C:\\Users\\Asus\\OneDrive\\Desktop\\FinalProject\\FinalProject\\Dataset\\products.txt");
// Use absolute path (doesn't work in other cases for some reason)
var service = new InventoryService(repository);
var controller = new ProductController(service);
string searchUsage = ("- search example: " +
                      "\n>>> :search name [Press Enter]" +
                      "\nEnter name of product:" +
                      "\n>>> :Vase");

Console.WriteLine("Tableware and Household Goods Warehouse\nv 1.4\nWrite 'help' to receive list of commands.");
while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Enter command\n>>> :");
    Console.ResetColor();
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
                              "exit => exit the program\n" +
                              "search [search category(e.g name, id ..)]*enter* then input [value]\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(searchUsage);
            Console.ResetColor();
            break;
        
        case "info":
            Console.WriteLine("Developer:Ilya Serbin\n" +
                              "Application Name: Tableware and Household Goods Warehouse\n" +
                              "Version: 1.4\n" +
                              "Dev Contact info:\n" +
                              "- Phone Number: +12345678\n" +
                              "- Email: ilya_serbin@student.itpu.uz");
            break;
        
        case "list":
            Console.WriteLine($"List of products:");
            var allProducts = controller.GetAllProducts();
            DisplayResults(allProducts);
            break;
        
        case "search":
            if (parts.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong number of parameters!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(searchUsage);
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter valid command.\nType 'help' for more information.");
                    Console.ResetColor();
                    continue;
            }

            break;

        case "exit":
                return;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong command, please try again.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Type 'help' for a list of commands.");
            Console.ResetColor();
            break;
    }
}

void DisplayResults(List<Product> products)
{
    if (products.Count == 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("No products found matching those criteria.");
        Console.ResetColor();
        return;
    }
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n{0,-5} {1,-25} {2,-15} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Stock");
    Console.WriteLine(new string('-', 70));
    Console.ResetColor();

    foreach (var p in products)
    {
        Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10:C} {4,-10}", 
            p.Id, p.Name, p.Category, p.Price, p.Quantity);
    }
}

