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
using static QuestPDF.Helpers.Colors;
using System.Reflection.PortableExecutable;
namespace QuestPdfDemo.Report
{
    public class ReportGeneration
    {


        public byte [] GeneratePdf<T> (ReportOptions<T> _options)
        {
            return
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

                        page.Header().Component(new HeaderComponent(_options.PageHeader));

                        //page.Header()
                        //    .Element(c => ComposeHeader(c, _options.PageHeader));

                        page.Content()
                            .Element(c => ComposeBody(c, _options.TableHeaders, _options.TableData, _options.Language));
                        ;

                        page.Footer()
                            .Component<FooterComponent>();
                        //page.Footer()
                        //    .Element(ComposeFooter);
                    });
            }).GeneratePdf();

        }

         
        private void ComposeBody<T> (IContainer container, List<Header> headers, List<T> data, string language)
        {
            List<Header> headerList =ReportHelper.ReorderHeadersByOrder(headers);
            List<Header> headersWithChildren = ReportHelper.GetHeadersWithChildHeaders(headerList);
            container.PaddingBottom(1).Extend().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    foreach (var header in headerList)
                    {

                        if (header.Type == HeaderType.ComplexType)
                        {
                            columns.RelativeColumn();
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
                    if (headersWithChildren.Count == 0)
                    {
                        foreach (var header in headerList)
                        {
                            string name = language == "Ar" ? header.arName : header.enName;
                            headerRow.Cell().headerBlock().Text(name);
                        }

                    }
                    else
                    {
                        foreach (var header in headerList)
                        {

                            string name = language == "Ar" ? header.arName : header.enName;
                            int numberOfSubProperities = header.ChildHeaders?.Count ?? 0;

                            if (numberOfSubProperities > 0)
                            {


                                headerRow.Cell().ColumnSpan((uint)numberOfSubProperities).headerBlock().Text(name);

                            }
                            else
                            {
                                headerRow.Cell().RowSpan(2).headerBlock().Text(name);
                            }

                        }
                        foreach (var header in headersWithChildren)
                        {
                            foreach (var childheader in header.ChildHeaders)
                            {
                                string name = language == "Ar" ? childheader.arName : childheader.enName;
                                headerRow.Cell().SecondryHeaderBlock().Text(name);
                            }
                        }

                    }

                });
                foreach (var row in data)
                {
                    var maxListCount = (uint)ReportHelper.GetMaxListCount(row);
                    foreach (var header in headerList)
                    {
                        var value = header.Accessor(row);
                        if (value is IEnumerable<object> list)
                        {

                            if (header.Type == HeaderType.ComplexType)
                            {
                                var myProperities = header.ChildHeaders.Count;
                                table.Cell().RowSpan(maxListCount).ColumnSpan((uint)myProperities).Column(column =>
                                {
                                    int renderedRows = 0;
                                    foreach (var complexItem in list)
                                    {
                                        column.Item().Row(row =>
                                        {
                                            foreach (var subHeader in header.ChildHeaders)
                                            {
                                                var subHeaderValue = subHeader.Accessor(complexItem);
                                                row.RelativeColumn().mergedBlock().AlignCenter().Text(subHeaderValue?.ToString() ?? string.Empty);
                                            }
                                        });

                                        renderedRows++;
                                    }
                                    while (renderedRows < maxListCount)
                                    {
                                        column.Item().Row(row =>
                                        {
                                            row.RelativeColumn().mergedBlock().AlignCenter().Text("");
                                        });

                                        renderedRows++;
                                    }
                                });
                            }
                            else
                            {
                                int renderedRows = 0;
                                table.Cell().RowSpan(maxListCount).Row(p => p.RelativeColumn().Column(
                                    column =>
                                {
                                    foreach (var value in list)
                                    {
                                        column.Item().mergedBlock().AlignCenter().Text(value);
                                        renderedRows++;
                                    }
                                    while (renderedRows < maxListCount)
                                    {
                                        column.Item().Row(row =>
                                        {
                                            row.RelativeColumn().mergedBlock().AlignCenter().Text("");
                                        });

                                        renderedRows++;
                                    }
                                }));
                            }
                        }
                        else
                        {
                            if (maxListCount > 0)
                            {
                                table.Cell().RowSpan(maxListCount).MyBlock().Text(value?.ToString() ?? string.Empty);
                            }
                            else
                            {
                                table.Cell().MyBlock().Text(value?.ToString() ?? string.Empty);
                            }
                        }
                    }
                }
            });
        }


  

    }
    public class FooterComponent : IComponent
    {
        public void Compose (IContainer container)
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
    public class HeaderComponent : IComponent
    {
        private readonly PageHeaderViewModel header;

        public HeaderComponent (PageHeaderViewModel _header)
        {
            header = _header;
        }
        public void Compose (IContainer container)
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
                    column.Item().Text(header.ReportSubTitle).Style(TypographyStyle.Title).AlignCenter();

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
    }
    public static class BlockExtentions
    {
        public static IContainer headerBlock (this IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Green.Lighten3)
                .PaddingBottom(1)
                .ShowOnce()
                .AlignCenter()
                .AlignMiddle();
        }
        public static IContainer mergedBlock (this IContainer container)
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
        public static IContainer MyBlock (this IContainer container)
        {
            return container
              .Border(1)
              .BorderColor(Colors.Blue.Accent4)
              .Background(Colors.Grey.Lighten3)

              .ShowEntire()

              .AlignMiddle()
              .AlignCenter();
        }public static IContainer SecondryHeaderBlock (this IContainer container)
        {
            return container
                        .Border(1)
                        .Background(Colors.Green.Lighten3)
                        .PaddingBottom(1)
                        .ShowOnce()
                        .AlignCenter()
                .AlignMiddle();
        }
    }
    public static class ReportHelper
    {
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
        public static List<Header> ReorderHeadersByOrder (List<Header> headers)
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
        public static List<Header> GetHeadersWithChildHeaders (List<Header> headers)
        {
            var headersWithChildren = new List<Header>();

            foreach (var header in headers)
            {
                if (header.ChildHeaders != null && header.ChildHeaders.Any())
                {
                    headersWithChildren.Add(header); // Add headers with child headers to the list
                }
            }

            return headersWithChildren; // Return the list of headers with child headers
        }
    }
    public static class TypographyStyle
    {

        public static TextStyle Title => TextStyle
            .Default
            .FontFamily("Helvetica")
            .FontColor(Colors.Blue.Medium)
            .FontSize(20)
            .Bold();


        public static TextStyle Headline => TextStyle
            .Default
            .FontFamily("Helvetica")
            .FontColor(Colors.Blue.Medium)
            .FontSize(14);


        public static TextStyle Normal => TextStyle
            .Default
            .FontFamily("Helvetica")
            .FontColor("#000000")
            .FontSize(10)
            .LineHeight(1.25f);

    }
}
