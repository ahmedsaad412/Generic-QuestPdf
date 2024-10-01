using QuestPdfDemo.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Linq.Expressions;
using QuestPdfDemo.Styles;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;
namespace QuestPdfDemo.Report
{
    public class ReportGeneration
    {


        public void GeneratePdf<T> (ReportOptions<T> _options)
        {
            Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        // Set orientation
                        page.Size(_options.Orientation == "Landscape" ? PageSizes.A4.Landscape() : PageSizes.A4.Portrait());
                        if (_options.Language == "AR")
                            page.ContentFromRightToLeft();
                        page.Margin(50);

                        page.Header()
                            .Element(c => ComposeHeader(c, _options.PageHeader));

                        page.Content()
                            .Element(c => ComposeBody(c, _options.TableHeaders, _options.TableData, _options.Language));
                        ;

                        page.Footer()
                            .Element(ComposeFooter);
                    });
            }).ShowInPreviewer();

        }

        private void ComposeHeader (IContainer container, PageHeaderViewModel header)
        {

            container.Row(row =>
            {

                row.RelativeItem()
                  .PaddingTop(20)
                  .Height(90)
                 .Text(header.ReportTitle)
                 .FontSize(26).FontColor(Colors.Orange.Accent4).SemiBold();
                row.RelativeItem().PaddingTop(20).Height(90).Column(column =>

                {
                    column.Item().Text(header.ReportSubTitle).Style(Typography.Title).AlignCenter();

                    column.Item().Text(header.EmployeeName).AlignCenter();
                });
                row.RelativeItem().Height(80).Column(column =>
                {

                    column.Item().AlignCenter().Height(40).Width(40).Image(header.ministryImg ?? "favicon.ico");
                    column.Item().AlignCenter().Text(header.ministryName)
                     .FontColor(Colors.Orange.Accent4).SemiBold();
                    column.Item().AlignCenter().Text(Placeholders.Label());
                });
            });
        }


        private void ComposeBody<T> (IContainer container, List<Header> headers, List<T> data ,string language)
        {
            List<Header> headerList = ReorderHeadersByOrder(headers);
            container.PaddingBottom(1).Extend().Table(table =>
            {
                var rowData = data.FirstOrDefault();
                var listOfPropertyNames = GetNonPrimitiveListPropertyNames(rowData);
                var PropertiesNames = GetPropertyNamesArray(listOfPropertyNames);
                var listLength = (uint)listOfPropertyNames.Count;

                table.ColumnsDefinition(columns =>
                {
                    foreach (var header in headerList)
                    {
                        var name = language == "Ar" ? header.arName : header.enName;
                        if (listOfPropertyNames.ContainsKey(name.Trim()))
                        {
                            columns.RelativeColumn((float)header.Width);
                            columns.RelativeColumn();
                        }
                        else
                        {
                            columns.RelativeColumn((float)header.Width);
                        }
                    }
                });

                table.Header(headerRow =>
                {
                    
                    if (listLength == 0)
                    {
                        foreach (var header in headerList)
                        {
                            var name = language == "Ar" ? header.arName : header.enName;
                            headerRow.Cell().Element(headerBlock).Text(name);
                        }

                    }
                    else
                    {
                        foreach (var header in headerList)
                        {

                            var name = language == "Ar" ? header.arName : header.enName;


                            if (listOfPropertyNames.ContainsKey(name.Trim()))
                            {
                               
                                var propertyNames = listOfPropertyNames[name.Trim()];
                                headerRow.Cell().ColumnSpan((uint)propertyNames.Count).Element(headerBlock).Text(name);
                               
                            }
                            else
                            { 
                             headerRow.Cell().RowSpan(2).Element(headerBlock).Text(name);
                            }
                           
                        }

                        foreach (var propName in PropertiesNames)
                        {
                            headerRow.Cell().Element(SecondryHeaderBlock).Text(propName);
                        }
                    }
                });
  
                foreach (var row in data)
                {
                    var maxListCount = (uint)GetMaxListCount(row);
                    foreach (var header in headerList)
                    {
                        var value = header.Accessor(row); 
                        if (value is IEnumerable<object> list)
                        {
                            var firstItem = list.FirstOrDefault();
                            if (firstItem != null && !IsPrimitive(firstItem.GetType()))
                            {
                                var myProperities = 0;
                                foreach (var complexItem in list)
                                {
                                    var properties = complexItem.GetType().GetProperties();
                                    myProperities = properties.Length;
                                    foreach (var property in properties)
                                    {
                                        var propertyValue = property.GetValue(complexItem)?.ToString() ?? string.Empty;
                                        table.Cell().Element(mergedBlock).Text(propertyValue);
                                    }
                                }
                                var x = Math.Abs(maxListCount - list.Count());
                                for (var i = 0 ; i < x ; i++)
                                {
                                    for (int j = 0 ; j < myProperities ; j++)
                                    {
                                        table.Cell().Element(mergedBlock).Text("");

                                    }
                                }
                            }
                            else
                            {
                                /// not need
                               // var joinedValues = ValuesList(list);
                                var joinedvaluesLength =(uint) list.Count();
                                table.Cell().RowSpan(maxListCount).Row(p => p.RelativeColumn().Column(column =>
                                {
                                    foreach (var value in list)
                                    {
                                        column.Item().Element(mergedBlock).AlignCenter().Text(value);
                                    }
                                    var x = Math.Abs(maxListCount - joinedvaluesLength);
                                    for (var i = 0 ; i < x ; i++)
                                    {
                                        column.Item().Element(mergedBlock).Text("");
                                    }
                                }));
                            }
                        }
                        else
                        {
                            if (maxListCount > 0)
                            {
                                table.Cell().RowSpan(maxListCount).Element(Block).Text(value?.ToString() ?? string.Empty);
                            }
                            else
                            {
                                table.Cell().Element(Block).Text(value?.ToString() ?? string.Empty);
                            }
                        }
                    }
                }

            });
        }
        #region get max list count   
        #region v1  
        //public static int GetMaxListCount (object obj)
        //{
        //    int maxCount = 0;

        //    // Get all properties of the object
        //    var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (var property in properties)
        //    {
        //        // Check if the property is a list (but not a string)
        //        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
        //        {
        //            // Get the value of the property
        //            var value = property.GetValue(obj);

        //            if (value is IEnumerable list)
        //            {
        //                // Get the count of items in the list
        //                int count = list.Cast<object>().Count();
        //                // Track the maximum count
        //                maxCount = Math.Max(maxCount, count);
        //            }
        //        }
        //    }

        //    return maxCount;
        //}
        #endregion
        #region v2  
        //public static int GetMaxListCount (object obj)
        //{
        //    // Ensure the object is not null
        //    if (obj == null)
        //        return 0;


        //    return obj.GetType()
        //              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //              .Where(prop => typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
        //              .Select(prop => prop.GetValue(obj) as IEnumerable)
        //              .Where(list => list != null)  // Filter out null lists
        //              .Select(list => list.Cast<object>().Count())  // Get the count of each list
        //              .DefaultIfEmpty(0)  // If no lists are found, return 0
        //              .Max();  // Get the maximum count
        //}  
        #endregion
        #endregion
        public static int GetMaxListCount (object obj)
        {
            if (obj == null)
                return 0;
            return obj.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      .Select(prop => prop.GetValue(obj) as IEnumerable)   
                      .Where(list => list != null && !(list is string))    
                      .Select(list => list.Cast<object>().Count())          
                      .DefaultIfEmpty(0)                                   
                      .Max();                                               
        }

        public List<Header> ReorderHeadersByOrder (List<Header> headers)
        { 
            var orderedHeaders = headers.Where(h => h.Order.HasValue).OrderBy(h => h.Order).ToList();
            var unorderedHeaders = headers.Where(h => !h.Order.HasValue).ToList();
            var result = new List<Header>();
            int unorderedIndex = 0;

            for (int i = 0 ; i < headers.Count ; i++)
            {
                if (orderedHeaders.Any(h => h.Order == result.Count + 1))
                {
                    var nextOrdered = orderedHeaders.First(h => h.Order == result.Count + 1);
                    result.Add(nextOrdered);
                }
                else if (unorderedIndex < unorderedHeaders.Count)
                {
                    result.Add(unorderedHeaders[unorderedIndex]);
                    unorderedIndex++;
                }
            }

            return result;
        }
        private bool IsPrimitive (Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }
        private List<object> ConvertToList<T> (IEnumerable<object> list) =>
             list.Cast<T>().Select(item => (object)item).ToList();

        public List<object> ValuesList (IEnumerable<object> list)
        {
            if (list == null || !list.Any())
                return new List<object>();

            var firstItem = list.First();

            return firstItem switch
            {
                string _ => ConvertToList<string>(list),
                int _ => ConvertToList<int>(list),
                float _ => ConvertToList<float>(list),
                double _ => ConvertToList<double>(list),
                decimal _ => ConvertToList<decimal>(list),
                char _ => ConvertToList<char>(list),
                _ => list.ToList() // Fallback for mixed types
            };
        }
        IContainer Block (IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Blue.Accent4)
                .Background(Colors.Grey.Lighten3)
                
                .ShowEntire()
                
                .AlignMiddle()
                .AlignCenter();
        }
        IContainer mergedBlock (IContainer container)
        {
            return container

               .Border(1).BorderColor(Colors.Blue.Accent4)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                 .PaddingVertical(1)
                 .BorderColor(Colors.Blue.Accent4)
                .ShowEntire()

                 ;
        }
        IContainer headerBlock (IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Green.Lighten3)
                .PaddingBottom(1)
                .ShowOnce()
                .AlignCenter()
                .AlignMiddle();
        }
        IContainer SecondryHeaderBlock (IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Green.Lighten3)
                .PaddingBottom(1)
                .ShowOnce()
                .AlignCenter()
                .AlignMiddle();
        }
        private void ComposeFooter (IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(x => x.FontSize(16));
                text.Span("Page Number ");
                text.CurrentPageNumber();
                text.Span(" of ").FontColor(Colors.Orange.Accent4).Underline();
                text.TotalPages();
            });
        }
        private Dictionary<string, List<string>> GetNonPrimitiveListPropertyNames (object obj)
        {
            var result = new Dictionary<string, List<string>>();

            // Get all public properties of the object
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Check if the property is a list (IEnumerable) but not a string
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    // Get the value of the property (the list)
                    var value = property.GetValue(obj);

                    if (value is IEnumerable list)
                    {
                        // Get the type of elements in the list
                        var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault();

                        if (elementType != null && !elementType.IsPrimitive && elementType != typeof(string))
                        {
                            // Create a list to store property names of the list items
                            var propertyNames = new List<string>();

                            // Get properties of the first item in the list (if there is any)
                            var firstItem = list.Cast<object>().FirstOrDefault();
                            if (firstItem != null)
                            {
                                var elementProperties = elementType.GetProperties();

                                foreach (var prop in elementProperties)
                                {
                                    propertyNames.Add(prop.Name); // Add the property names
                                }
                            }

                            // Add the list type and its properties to the result dictionary
                            result.Add(property.Name, propertyNames);
                        }
                    }
                }
            }

            return result;
        }
        private string[] GetPropertyNamesArray (Dictionary<string, List<string>> dictionary)
        {
            // Create a list to store all property names
            var allPropertyNames = new List<string>();

            // Loop through the dictionary to get only the values (list of strings)
            foreach (var entry in dictionary)
            {
                // Add each property name in the list to the result
                allPropertyNames.AddRange(entry.Value);
            }

            // Convert the list of property names to an array and return it
            return allPropertyNames.ToArray();
        }

    }
}
