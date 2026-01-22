namespace FinalProject.DataAccess;
using Domain;

public class TablewareCsvSource(string filePath) : AbstractCsvSource<Tableware>(filePath)
{
    protected override Tableware ParseArgs(string[] args)
    {
        try
        {
            if (args.Length < 5) throw new DaoException($"Invalid line format in: {FilePath()}");
        
            if (!int.TryParse(args[0].Trim(), out int id))
                throw new DaoException($"Invalid ID format in: {FilePath()}. Value: '{args[0]}'");
            
            if (id <= 0)
                throw new DaoException($"ID must be positive in: {FilePath()}. Value: {id}");

            string name = args[1].Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new DaoException($"Name cannot be empty in: {FilePath()}");

            string category = args[2].Trim();
            if (string.IsNullOrWhiteSpace(category))
                throw new DaoException($"Category cannot be empty in: {FilePath()}");

            if (!decimal.TryParse(args[3].Trim(), out decimal price))
                throw new DaoException($"Invalid price format in: {FilePath()}. Value: '{args[3]}'");
            
            if (price < 0)
                throw new DaoException($"Price cannot be negative in: {FilePath()}. Value: {price}");

            if (!int.TryParse(args[4].Trim(), out int quantity))
                throw new DaoException($"Invalid quantity format in: {FilePath()}. Value: '{args[4]}'");
            
            if (quantity < 0)
                throw new DaoException($"Quantity cannot be negative in: {FilePath()}. Value: {quantity}");

            return new Tableware.Builder()
                .SetId(int.Parse(args[0].Trim()))
                .SetName(args[1].Trim())
                .SetCategory(args[2].Trim())
                .SetPrice(decimal.Parse(args[3].Trim()))
                .SetQuantity(int.Parse(args[4].Trim()))
                .Build();
        }
        catch (DaoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DaoException($"Error parsing line in: {FilePath()}", ex);
        }
    }

    public override object Clone() => new TablewareCsvSource(FilePath());
}