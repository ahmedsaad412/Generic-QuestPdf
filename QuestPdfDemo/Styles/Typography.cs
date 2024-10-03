using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPdfDemo.Styles
{
    // single typography class can help with keeping a consistent look & feel in the document
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
