using System.Net.Mail;
using System.Text;

namespace GenericWebAPI.Utilities;

public class HtmlFormatter
{
    private StringBuilder _table;

    public HtmlFormatter()
    {
        _table = new StringBuilder();
    }

    public HtmlFormatter AddRow(IEnumerable<string> columns)
    {
        _table.Append("<tr>");
        foreach (var col in columns)
        {
            _table.Append($"<td>{col}</td>");
        }
        _table.Append("</tr>");

        return this;
    }

    public HtmlFormatter AddHeader(IEnumerable<string> columns)
    {
        _table.Append("<tr>");
        foreach (var col in columns)
        {
            _table.Append($"<th>{col}</th>");
        }
        _table.Append("</tr>");

        return this;
    }

    public HtmlFormatter OpenTable()
    {
        _table.Append("<table>");

        return this;
    }

    public HtmlFormatter CloseTable()
    {
        _table.Append("</table>");

        return this;
    }

    public override string ToString()
    {
        return _table.ToString();
    }
}