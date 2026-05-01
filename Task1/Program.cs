using Microsoft.Data.SqlClient;

namespace Task1;

interface IStorage
{
    void Save(string data);
    string Retrieve(int id);
}

class DataBaseStorage : IStorage
{
    private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=storagedb";

    public void Save(string data)
    {
        using var connection = new SqlConnection(ConnectionString);

        const string dataParameterName = "@data";
        const string query = $"""
            INSERT INTO dbo.stored_strings (data)
            VALUES ({dataParameterName});
        """;
        using var sqlCommand = new SqlCommand(query, connection);
        sqlCommand.Parameters.AddWithValue(dataParameterName, data);

        connection.Open();
        sqlCommand.ExecuteNonQuery();
    }

    public string Retrieve(int id)
    {
        using var connection = new SqlConnection(ConnectionString);

        const string idParameterName = "@id";
        const string query = $"""
            SELECT data FROM dbo.stored_strings
            WHERE id = ({idParameterName})
        """;
        using var sqlCommand = new SqlCommand(query, connection);
        sqlCommand.Parameters.AddWithValue(idParameterName, id);
        
        connection.Open();
        var result = sqlCommand.ExecuteScalar();

        return result?.ToString() ?? "";
    }
}

class Program
{
    public static void Main()
    {
        IStorage storage = new DataBaseStorage();

        storage.Save("String One");
        storage.Save("String Two");

        Console.WriteLine("The first string is " + storage.Retrieve(1));
        Console.WriteLine("The second string is " + storage.Retrieve(2));

        Console.WriteLine("The app have executed!");
    }
}
