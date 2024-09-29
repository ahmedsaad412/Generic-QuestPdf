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
        //public List<Header> ReorderHeadersByOrder (List<Header> headers)
        //{
        //    return headers.OrderBy(header => header.Order).ToList();
        //}
        //private List<string> GetNonPrimitivePropertyNames (object obj)
        //{
        //    var result = new List<string>();

        //    // Check if the object is non-primitive (not a value type or string)
        //    if (obj != null && !obj.GetType().IsPrimitive && obj.GetType() != typeof(string))
        //    {
        //        // Get all public properties of the object
        //        var properties = obj.GetType().GetProperties();

        //        foreach (var property in properties)
        //        {
        //            // Add only the name of the property to the result list
        //            result.Add(property.Name);
        //        }
        //    }

        //    return result;
        //}
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

        private void ComposeBody<T> (IContainer container, List<Header> headers, List<T> data ,string language)
        {
            List<Header> headerList = ReorderHeadersByOrder(headers);
            container.PaddingBottom(1).Extend().Table(table =>
            {
                var rowData = data.FirstOrDefault();
                var listOfPropertyNames = GetNonPrimitiveListPropertyNames(rowData);
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
                             

                            //var propertyNames = listOfPropertyNames[name.Trim()];
                            //foreach (var propName in propertyNames)
                            //{
                            //   columns.RelativeColumn();
                            //}
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
                                foreach (var propName in propertyNames)
                                {

                                    headerRow.Cell().Element(SecondryHeaderBlock).Text(propName);
                                    
                                }
                            }
                            else
                            { 
                             headerRow.Cell().RowSpan(2).Element(headerBlock).Text(name);
                            }
                        }
                    }
                });
               // Iterate through the data rows
                foreach (var row in data)
                {
                    var maxListCount = (uint)GetMaxListCount(row);
                    foreach (var header in headerList)
                    {
                        var value = header.Accessor(row); // Use delegate to get the property value
                        
                        // Check if the value is an IEnumerable (list)
                        if (value is IEnumerable<object> list)
                        {
                            // Check if the list contains complex objects
                            var firstItem = list.FirstOrDefault();
                            if (firstItem != null && !IsPrimitive(firstItem.GetType()))
                            {
                                // Handle complex type lists (e.g., List<Type>, List<Customer>)
                                foreach (var complexItem in list)
                                {
                                    var properties = complexItem.GetType().GetProperties();
                                    foreach (var property in properties)
                                    {
                                        var propertyValue = property.GetValue(complexItem)?.ToString() ?? string.Empty;
                                        table.Cell().Element(mergedBlock).Text(propertyValue);
                                    }
                                    var x = maxListCount - list.Count();
                                    for (var i = 0; i < x; i++)
                                    {
                                        table.Cell().Element(mergedBlock).Text("");
                                    }
                                }
                            }
                            else
                            {
                                // Handle primitive type lists (e.g., List<string>, List<int>)
                                var joinedValues = ValuesList(list);
                                table.Cell().RowSpan(maxListCount).Row(p => p.RelativeColumn().Column(column =>
                                {
                                    foreach (var value in joinedValues)
                                    {
                                        // For each value, add a text item to the column
                                        column.Item().Element(mergedBlock).AlignCenter().Text(value);
                                    }
                                }));
                            }
                        }
                        else
                        {
                            // Handle primitive types or objects directly
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
            // Return 0 if the object is null
            if (obj == null)
                return 0;

            // Extract all enumerable properties (excluding strings) and return the max count
            return obj.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      .Select(prop => prop.GetValue(obj) as IEnumerable)   // Try to cast property value to IEnumerable
                      .Where(list => list != null && !(list is string))    // Filter out null values and strings
                      .Select(list => list.Cast<object>().Count())         // Get count of each list
                      .DefaultIfEmpty(0)                                  // Handle cases where no lists exist
                      .Max();                                              // Get the maximum list count
        }

        public List<Header> ReorderHeadersByOrder (List<Header> headers)
        {// 2 3 6 
            // Separate headers with and without an Order
            var orderedHeaders = headers.Where(h => h.Order.HasValue).OrderBy(h => h.Order).ToList();
            var unorderedHeaders = headers.Where(h => !h.Order.HasValue).ToList();

            // Insert unordered headers in their original positions relative to their appearance in the list
            var result = new List<Header>();
            int unorderedIndex = 0;

            for (int i = 0 ; i < headers.Count ; i++)
            {
                // If there is a header with an Order that matches the current index, insert it
                if (orderedHeaders.Any(h => h.Order == result.Count + 1))
                {
                    var nextOrdered = orderedHeaders.First(h => h.Order == result.Count + 1);
                    result.Add(nextOrdered);
                }
                else if (unorderedIndex < unorderedHeaders.Count)
                {
                    // Insert unordered headers in original positions relative to their initial list
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
        public List<object> ValuesList (IEnumerable<object> list)
        {
            // Handle null case early
            if (list == null)
                return new List<object>();

            // Pre-allocate result list if possible (optimize for collections)
            var resultList = list is ICollection<object> collection
                ? new List<object>(collection.Count)
                : new List<object>();

            foreach (var item in list)
            {
                if (item == null)
                {
                    resultList.Add(null); // Add null if the item is null
                    continue;
                }

                // Use 'is' with direct type casting for performance
                if (item is int intValue)
                {
                    resultList.Add(intValue); // Add as int
                }
                else if (item is float floatValue)
                {
                    resultList.Add(floatValue); // Add as float
                }
                else if (item is double doubleValue)
                {
                    resultList.Add(doubleValue); // Add as double
                }
                else if (item is decimal decimalValue)
                {
                    resultList.Add(decimalValue); // Add as decimal
                }
                else if (item is string stringValue)
                {
                    resultList.Add(stringValue); // Add as string
                }
                else
                {
                    resultList.Add(item); // Add other types as-is
                }
            }

            return resultList; // Return the fully constructed list
        }

        //public IEnumerable<object> ValuesList (IEnumerable<object> list)
        //{
        //    if (list == null || !list.Any())
        //        yield break; // Empty list, return early

        //    foreach (var item in list)
        //    {
        //        if (item == null)
        //        {
        //            yield return null; // Handle null values in the list
        //        }
        //        else
        //        {
        //            // Use pattern matching to handle different types
        //            switch (item)
        //            {
        //                case int intValue:
        //                    yield return intValue; // Return as int
        //                    break;

        //                case float floatValue:
        //                    yield return floatValue; // Return as float
        //                    break;

        //                case double doubleValue:
        //                    yield return doubleValue; // Return as double
        //                    break;

        //                case decimal decimalValue:
        //                    yield return decimalValue; // Return as decimal
        //                    break;

        //                case string stringValue:
        //                    yield return stringValue; // Return as string
        //                    break;

        //                default:
        //                    yield return item; // If none of the above, return as-is
        //                    break;
        //            }
        //        }
        //    }
        //}
        //public List<object> ValuesList (IEnumerable<object> list)
        //{
        //    var resultList = new List<object>();

        //    if (list == null || !list.Any())
        //        return resultList; // Return an empty list if input is null or empty

        //    foreach (var item in list)
        //    {
        //        if (item == null)
        //        {
        //            resultList.Add(null); // Handle null values in the list
        //        }
        //        else
        //        {
        //             Use pattern matching to handle different types
        //            switch (item)
        //            {
        //                case int intValue:
        //                    resultList.Add(intValue); // Add int
        //                    break;

        //                case float floatValue:
        //                    resultList.Add(floatValue); // Add float
        //                    break;

        //                case double doubleValue:
        //                    resultList.Add(doubleValue); // Add double
        //                    break;

        //                case decimal decimalValue:
        //                    resultList.Add(decimalValue); // Add decimal
        //                    break;

        //                case string stringValue:
        //                    resultList.Add(stringValue); // Add string
        //                    break;

        //                default:
        //                    resultList.Add(item); // If none of the above, add as-is
        //                    break;
        //            }
        //        }
        //    }

        //    return resultList; // Return the fully constructed list
        //}

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

               .Border(1)
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
    }
}
