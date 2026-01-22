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
            if (parts.Length != 5) continue;

            // Inside FileProductRepository.cs
            products.Add(new Tableware.Builder()
                .SetId(int.Parse(parts[0]))
                .SetName(parts[1])
                .SetCategory(parts[2]) // Correctly parse category from file
                .SetPrice(decimal.Parse(parts[3]))
                .SetQuantity(int.Parse(parts[4]))
                .SetMaterial("Ceramic") 
                .Build());
        }

        return products;
    }

}
