using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

public class ProductsModel : PageModel
{
    // Display a list of products
    public List<Product> Products { get; set; }

    public void OnGet()
    {
        //Create a new products list and then start Sql Connection the Northwind database.
        Products = new List<Product>();
        string connectionString = "Server=localhost;Database=Northwind;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            //Open Sql Connection and execute a query to retrieve ProductName, 
            // CategoryName and UnitPrice from Products joined with Categories

            connection.Open();
            string sql = @"SELECT p.ProductName, c.CategoryName, p.UnitPrice
                           FROM Products p
                           JOIN Categories c ON p.CategoryID = c.CategoryID";
            //Execute the query and read the outcome into Products list.
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map columns to the Product class properties

                        Products.Add(new Product
                        {
                            ProductName = reader.GetString(0),
                            CategoryName = reader.GetString(1),
                            UnitPrice = reader.GetDecimal(2)
                        });
                    }
                }
            }
        }
    }
}
// Product class 
public class Product
{
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public decimal UnitPrice { get; set; }
}