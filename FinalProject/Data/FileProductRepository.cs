namespace FinalProject.Data;
using Domain;

public class FileProductRepository(string path) : IProductRepository
{
    public List<Product> GetAll()
    {
        var products = new List<Product>();

        if (!File.Exists(path)) return products;

        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split(';');
            if (parts.Length < 5) continue;

            products.Add(new Product(
                int.Parse(parts[0]),
                parts[1],
                parts[2],
                decimal.Parse(parts[3]),
                int.Parse(parts[4])
            ));
        }
        return products;
    }
}
