using Mini.Common.Models;
using System.Reflection;

namespace Wms.Models;

/*
    id: 1, 
    title: "What is AJAX", 
    body: "AJAX stands for Asynchronous JavaScript..."
 */
//public class JsTask
//{
//    public int id { get; set; }
//    public string? title { get; set; }
//    public string? body { get; set; }
//}

//function DataRequest()
//{
//    this.Page = 1;
//    this.PageSize = 10;
//    this.DataType = "User";
//    this.SortFields = [];
//}
public class DataRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string DataType { get; set; } = string.Empty;
    public IList<SortField> SortFields { get; set; } = new List<SortField>();
}

public class DataResponse<T>
{
    public IList<string> FieldNames { get; set; } = new List<string>();

    public IEnumerable<T>? Data { get; set; } = null;

    public ulong TotalRecordCount { get; set; } = 0;

    public uint Page { get; set; } = 0;

    public uint PageSize { get; set; } = 0;

    public DataResponse()
    {
        this.FieldNames = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(r => r.Name).ToList();
    }

    public DataResponse(PagedData<T> pagedData) : this()
    {
        this.Data = pagedData.Data;

        this.TotalRecordCount = pagedData.TotalRecordCount;

        this.Page = pagedData.Page;

        this.PageSize = pagedData.PageSize;
    }
}

public enum SortDirection
{
    Ascending,
    Descending
}

public class SortField
{
    public string FieldName { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; }
}