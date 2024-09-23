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
                            .Element(c => ComposeBody(c, _options.TableHeaders, _options.TableData));
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

        private void ComposeBody<T> (IContainer container, List<Header> headers, List<T> data)
        {
            container.PaddingBottom(1).Extend().Table(table =>
            {

                table.ColumnsDefinition(columns =>
                {
                    foreach (var header in headers)
                    {
                        columns.RelativeColumn(header.Width == 0 ? 2 : header.Width);
                    }
                });

                table.Header(headerRow =>
                {
                    foreach (var header in headers)
                    {
                        headerRow.Cell().Element(headerBlock).Text(header.Name);
                    }
                });
                foreach (var row in data)
                {
                    foreach (var header in headers)
                    {
                        // Use the accessor function to get the value for the column
                        var value = header.Accessor(row)?.ToString() ?? string.Empty;
                        table.Cell().Element(Block).Text(value);
                    }
                }
                //foreach (var row in data)
                //{
                //    foreach (var header in headers)
                //    {
                //        var property = typeof(T).GetProperty(header.Name.Replace(" ", ""));

                //        var value = property != null ? property.GetValue(row)?.ToString() : string.Empty;
                //        table.Cell().Element(Block).Text(value);
                //    }
                //}
            });
        }
        IContainer Block (IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Grey.Lighten3)
                .PaddingVertical(5)
                .ShowEntire()
                .AlignCenter()
                .AlignMiddle()
                .AlignCenter();
        }
        IContainer headerBlock (IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Grey.Lighten3)
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
