using System.Diagnostics;
using FinalProject.Domain;
using FinalProject.Controllers;

namespace FinalProject.ConsoleView;

static class UserView
{
    public static void Main(ProductController controller)
    {
        string searchUsage = ("- search example: " +
                              "\n>>> :search name [Press Enter]" +
                              "\nEnter name of product:" +
                              "\n>>> :Vase");

        Console.WriteLine(
            "Tableware and Household Goods Warehouse\nv 1.5 12/25/2025\nWrite 'help' to receive list of commands.");
        while (true)
        {
            Console.ResetColor();
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
                                      "exit => exit the program\n" +
                                      "list [id/price/quantity] [asc/desc] => list of all products sorted by parameter\n" +
                                      "- example:\n" +
                                      ">>> :list id desc\n" +
                                      "search [search category(e.g name, id ..)]*enter* then input [value]\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(searchUsage);
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
                    if (parts.Length == 2 || parts.Length > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong number of parameters!");
                        break;
                    }
                    
                    if (parts.Length == 1)
                    {
                        var allProductsUnsorted = controller.GetAllProducts();
                        DisplayResults(allProductsUnsorted);
                        break;
                    }
                    
                    string? sortField = parts[1].ToLower();
                    Debug.Assert(sortField != null); 
                    
                    string? sortOrder = parts[2].ToLower();
                    Debug.Assert(sortOrder != null);
                    
                    bool ascending;
                    switch (sortOrder)
                    {
                        case "desc":
                            ascending = false;
                            var allProducts = controller.GetAllSorted(sortField, ascending);
                            DisplayResults(allProducts);
                            break;
                        case "asc":
                            ascending = true;
                            allProducts = controller.GetAllSorted(sortField, ascending);
                            DisplayResults(allProducts);
                            break;
                    }
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
    }
}
    
    
