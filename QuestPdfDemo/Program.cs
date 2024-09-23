using QuestPdfDemo.models;
using QuestPdfDemo.Report;
using System.ComponentModel.DataAnnotations;

var Orientation = "Portrait"; // or "Landscape"
var Language = "Ar"; // or "Ar" 
PageHeaderViewModel PageHeader =new PageHeaderViewModel()
            {ministryName = "Al Rayan Ministry",ReportTitle = "Dynamic Report Title"
                    ,ReportSubTitle = "bla bla bla ", ministryImg = "favicon.ico",EmployeeName = "Ahmed Saad Helmy"};
var headers = new List<Header>
{
    new Header("ID","رقم الهوية" , x => ((Product)x).ID),
    new Header("Price","السعر", x => ((Product)x).Price, 2),
    new Header("Product Name","اسم السلعة", x => ((Product)x).ProductName, 3),
};
var products = new List<Product>
    {
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Productفففففففففففففففففففففففففففففففففففففففففففففففففففففففففففففف A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Producبببt B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m },
        new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
        new Product { ID = 2, ProductName = "Product B", Price = 149.50m },
        new Product { ID = 3, ProductName = "Product C", Price = 200.00m }
    };
var reportOptions = new ReportOptions<Product>(Orientation,Language,PageHeader,headers, products);
ReportGeneration report = new ReportGeneration();
report.GeneratePdf(reportOptions);
public class Product
{
    public int ID { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}