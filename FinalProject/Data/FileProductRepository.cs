namespace FinalProject.Data;
using Domain;

public class FileProductRepository(string path) : IProductRepository
{
    public List<Product> GetAll()
    {
        var products = new List<Product>();

        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split(';');
            if (parts.Length < 5) continue;

            products.Add(new Product(
                int.Parse(parts[0].Trim()),
                parts[1].Trim(),
                parts[2].Trim(),
                decimal.Parse(parts[3].Trim()),
                int.Parse(parts[4].Trim())
            ));
        }

        return products;
    }

}
