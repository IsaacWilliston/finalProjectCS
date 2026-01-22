using FinalProject.Domain;
using FinalProject.DataAccess;
using FinalProject.Services;
using FinalProject.Controllers;
using FinalProject.ConsoleView;

var factory = ProductDaoFactory.Instance;
string filePath = "Dataset/products.txt";

if (File.Exists(filePath))
{
    factory.RegisterSource<Product>(new TablewareCsvSource(filePath));
}
else
{
    throw new FileNotFoundException("File not found.", filePath);
}


var service = new InventoryService(factory);

var controller = new ProductController(service);
UserView.Main(controller);
