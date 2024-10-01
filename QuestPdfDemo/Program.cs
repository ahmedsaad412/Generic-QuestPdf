using QuestPdfDemo.models;
using QuestPdfDemo.Report;
using System.ComponentModel.DataAnnotations;

var Orientation = "Portrait"; // or "Landscape"
var Language = "En"; // or "Ar" 
PageHeaderViewModel PageHeader =new PageHeaderViewModel()
            {ministryName = "Al Rayan Ministry",ReportTitle = "Dynamic Report Title"
                    ,ReportSubTitle = "bla bla bla ", ministryImg = "favicon.ico",EmployeeName = "Ahmed Saad Helmy"};
var headers = new List<Header>
{
    new Header("ID", "رقم الهوية", x => ((Product)x).ID, 1, type: HeaderType.Primitive),
    new Header("Product Name", "اسم السلعة", x => ((Product)x).ProductName, 2, type: HeaderType.Primitive),
    new Header("Price", "السعر", x => ((Product)x).Price, 4, width: 2, type: HeaderType.Primitive),
    new Header("Colors", "الالوان", x => ((Product)x).Colors, 3, width: 2, type: HeaderType.ListOfPrimitives),
    new Header("Types", "الأنواع",
        x => ((Product)x).Types,
        5,
        width: 2,
        childHeaders: new List<Header>
        {
            new Header("My Id", "رقم الهوية", y => ((MyType)y).Id, type: HeaderType.Primitive),
            new Header("MY Value", "القيمة", y => ((MyType)y).Value, type: HeaderType.Primitive)
        },
        type: HeaderType.ComplexType)
};

var products = new List<Product>
{

    new Product { ID = 1, ProductName = "Product A", Price = 99.99m ,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Red", "Blue", "Green", "Yellow" }    } ,
    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
   // new Header("Typees ","الأنواع ", x => ((Product)x).Typees,6  ),
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 1, ProductName = "Product A", Price = 99.99m ,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Red", "Blue", "Green", "Yellow" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 1, ProductName = "Product A", Price = 99.99m ,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Red", "Blue", "Green", "Yellow" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow","blallall"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Green", "Yellow"}} ,
    //new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Black", "White" }} ,
    //new Product { ID = 4, ProductName = "Product D", Price = 249.99m,Typees = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }},Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}, Colors = new List<string> { "Pink", "Purple" } }
};
#region without color 
//var products = new List<Product>
//{
//    new Product { ID = 1, ProductName = "Product A", Price = 99.99m },
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m},
//    new Product { ID = 4, ProductName = "Product D", Price = 249.99m},
//};
////with types only
//var products = new List<Product>
//{
//    new Product { ID = 1, ProductName = "Product A", Price = 99.99m ,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" },new MyType { Id = 2, Value = "Type2" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//    new Product { ID = 4, ProductName = "Product D", Price = 249.99m,Types = new List<MyType>{new MyType { Id = 1, Value = "Type1" },new MyType { Id = 2, Value = "Type2" }}},
//};
//var products = new List<Product>
//{
//    new Product { ID = 1, ProductName = "Product A", Price = 99.99m , Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow","blallall","blallall"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow","blallall","blallall"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow","blallall","blallall"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow","blallall","blallall"}},
//    new Product { ID = 2, ProductName = "Product B", Price = 149.50m, Colors = new List<string> { "Green", "Yellow","blallall","blallall"}},
//    new Product { ID = 3, ProductName = "Product C", Price = 200.00m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//    new Product { ID = 4, ProductName = "Product D", Price = 249.99m, Colors = new List<string> { "Green", "Yellow","blallall"}},
//};
#endregion
var reportOptions = new ReportOptions<Product>(Orientation,Language,PageHeader,headers, products);
ReportGeneration report = new ReportGeneration();
report.GeneratePdf(reportOptions);
public class Product
{
    public int ID { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public List<MyType> Typees{ get; set; }
    public List<MyType> Types{ get; set; }
    public List<string> Colors { get; set; }
}
public class MyType
{
    public int Id { get; set; }
    public string Value { get; set; }
}
