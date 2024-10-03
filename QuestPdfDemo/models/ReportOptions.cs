using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestPdfDemo.models
{
    public class ReportOptions<T>
    {
        public string Orientation { get; set; } // "Portrait" or "Landscape"
        public string Language { get; set; } // "AR" or "EN"
        public PageHeaderViewModel PageHeader { get; set; } = new PageHeaderViewModel();
        public List<Header> TableHeaders { get; set; } 
        public List<T> TableData { get; set; } 

        public ReportOptions(string orientation , string language,
            PageHeaderViewModel pageHeader,
            List<Header> headers, List<T> data)
        {
            Orientation = orientation;
            Language = language;
            PageHeader = pageHeader;
            TableHeaders = headers;
            TableData = data;
        } // Rows of data
    }
    public enum HeaderType
    {
        Primitive,
        ListOfPrimitives,
        ComplexType
    }

    public class Header
    {
        public string arName { get; set; }
        public string enName { get; set; }
        public float? Width { get; set; }
        public int? Order { get; set; }
        public Func<object, object> Accessor { get; set; }
        public List<Header>? ChildHeaders { get; set; } 
        public HeaderType Type { get; set; } 

        public Header (string en_name, string ar_name, Func<object, object> accessor, int? order = null, float width = 1, List<Header>? childHeaders = null, HeaderType type = HeaderType.Primitive)
        {
            enName = en_name;
            arName = ar_name;
            Width = width;
            Order = order;
            Accessor = accessor;
            ChildHeaders = childHeaders;
            Type = type;
        }
    }
 
     public class PageHeaderViewModel()
    {
        public string ministryName { get; set; }
        public string? ministryImg { get; set; }
        public string ReportTitle { get; set; }
        public string ReportSubTitle { get; set; }
        public string EmployeeName { get; set; }
    }

}
