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
            "Tableware and Household Goods Warehouse\nv 1.5 01/22/2026\nWrite 'help' to receive list of commands.");
        
        // Local function for displaying results - moved before while loop
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
                                      "info => information about the app:  Developer Name, App version, Developer's Contact info ..etc.\n" +
                                      "list => unsorted list of all inventory.\n" +
                                      "list [id/name/category/price/quantity] [asc/desc] => list of all products sorted by parameter\n" +
                                      "- example:\n" +
                                      ">>> :list id desc\n" +
                                      "search [category/name/id/price] => searches items based on filters" +
                                      "\nexit => exit the program\n");
                    Console.WriteLine(searchUsage);
                    break;

                case "info":
                    Console.WriteLine("Developer:Ilya Serbin\n" +
                                      "Application Name: Tableware and Household Goods Warehouse\n" +
                                      "Version: 1.5\n" +
                                      "Dev Contact info:\n" +
                                      "- Phone Number: +12345678\n" +
                                      "- Email: ilya_serbin@student.itpu.uz");
                    break;

                case "list":
                    if (parts.Length == 2 || parts.Length > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong number of parameters! Type 'help' for a list of commands.");
                        break;
                    }
                    
                    if (parts.Length == 1)
                    {
                        try
                        {
                            var allProductsUnsorted = controller.GetAllProducts();
                            DisplayResults(allProductsUnsorted);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Error loading products: {ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    }
                    
                    string? sortField = parts[1]?.ToLower();
                    if (string.IsNullOrWhiteSpace(sortField))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sort field cannot be empty. Type 'help' for a list of commands.");
                        Console.ResetColor();
                        break;
                    }
                    
                    string? sortOrder = parts[2]?.ToLower();
                    if (string.IsNullOrWhiteSpace(sortOrder))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sort order cannot be empty. Type 'help' for a list of commands.");
                        Console.ResetColor();
                        break;
                    }
                    
                    bool ascending;
                    switch (sortOrder)
                    {
                        case "desc":
                            ascending = false;
                            try
                            {
                                var allProducts = controller.GetAllSorted(sortField, ascending);
                                DisplayResults(allProducts);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error loading sorted products: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;
                        case "asc":
                            ascending = true;
                            try
                            {
                                var allProducts = controller.GetAllSorted(sortField, ascending);
                                DisplayResults(allProducts);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error loading sorted products: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong parameter! Type 'help' for a list of commands.");
                            Console.ResetColor();
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
                        Console.ResetColor();
                        break;
                    }

                    var searchTerm = parts[1].ToLower();
                    switch (searchTerm)
                    {
                        case "name":
                            Console.WriteLine("Enter name of product:");
                            string? productName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(productName))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Product name cannot be empty.");
                                Console.ResetColor();
                                break;
                            }
                            
                            try
                            {
                                var nameSearchResult = controller.SearchByName(productName);
                                DisplayResults(nameSearchResult);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error searching by name: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;

                        case "category":
                            Console.WriteLine("Enter category:");
                            string? categoryName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(categoryName))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Category cannot be empty.");
                                Console.ResetColor();
                                break;
                            }

                            try
                            {
                                var categorySearchResult = controller.SearchByCategory(categoryName.Trim());
                                DisplayResults(categorySearchResult);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error searching by category: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;

                        case "price":
                            Console.WriteLine("Enter price:");
                            string? priceInput = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(priceInput))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Price cannot be empty.");
                                Console.ResetColor();
                                break;
                            }
                            
                            if (!decimal.TryParse(priceInput, out decimal price))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid price format. Please enter a valid number.");
                                Console.ResetColor();
                                break;
                            }
                            
                            if (price < 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Price cannot be negative.");
                                Console.ResetColor();
                                break;
                            }
                            
                            try
                            {
                                var priceSearchResult = controller.SearchByPrice(price);
                                DisplayResults(priceSearchResult);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error searching by price: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;

                        case "id":
                            Console.WriteLine("Enter product ID:");
                            string? productId = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(productId))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Product ID cannot be empty.");
                                Console.ResetColor();
                                break;
                            }
                            
                            if (!int.TryParse(productId, out int searchId))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid ID format. Please enter a valid integer.");
                                Console.ResetColor();
                                break;
                            }
                            
                            if (searchId <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Product ID must be a positive number.");
                                Console.ResetColor();
                                break;
                            }
                            
                            try
                            {
                                var idSearchResult = controller.SearchById(searchId);
                                DisplayResults(idSearchResult);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error searching by ID: {ex.Message}");
                                Console.ResetColor();
                            }
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Enter valid command.\nType 'help' for more information.");
                            Console.ResetColor();
                            break;
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
    }
}