using FinalProject.Domain;
using FinalProject.Data;
using FinalProject.Services;
using FinalProject.Controllers;
using FinalProject.ConsoleView;

IProductRepository repository = new FileProductRepository("C:\\Users\\Asus\\OneDrive\\Desktop\\FinalProject\\FinalProject\\Dataset\\products.txt");
// Use absolute path (doesn't work in other cases for some reason)
var service = new InventoryService(repository);
var controller = new ProductController(service);

UserView.Main(controller);

