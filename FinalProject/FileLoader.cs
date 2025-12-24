namespace FinalProject;

class FileLoader
{
    public static List<Product> LoadProducts(string path)
    {
        var products = new List<Product>();

        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split(';');
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
