using System.Net.Mail;
using System.Text;

namespace GenericWebAPI.Utilities;

public class HtmlFormatter
{
    private StringBuilder _document;

    public HtmlFormatter()
    {
        _document = new StringBuilder();
    }

    public HtmlFormatter AddHeading(string text, int level)
    {
        if (level < 1 || level > 6)
        {
            throw new ArgumentException("Heading level must be between 1 and 6");
        }

        _document.Append($"<h{level}>{text}</h{level}>");

        return this;
    }

    public HtmlFormatter AddParagraph(string text)
    {
        _document.Append($"<p>{text}</p>");

        return this;
    }

    public HtmlFormatter AddLink(string url, string text)
    {
        _document.Append($"<a href='{url}'>{text}</a>");

        return this;
    }

    public HtmlFormatter AddBoldText(string text)
    {
        _document.Append($"<strong>{text}</strong>");

        return this;
    }

    public HtmlFormatter AddItalicText(string text)
    {
        _document.Append($"<em>{text}</em>");

        return this;
    }

    public HtmlFormatter AddUnorderedList(IEnumerable<string> items)
    {
        _document.Append("<ul>");
        foreach (var item in items)
        {
            _document.Append($"<li>{item}</li>");
        }

        _document.Append("</ul>");

        return this;
    }

    public HtmlFormatter AddImage(string url, string altText = "")
    {
        _document.Append($"<img src='{url}' alt='{altText}'/>");

        return this;
    }

    public HtmlFormatter AddTable(IEnumerable<IEnumerable<string>> rows)
    {
        OpenTable();
        foreach (var row in rows)
        {
            AddRow(row);
        }
        CloseTable();

        return this;
    }


    public HtmlFormatter AddRow(IEnumerable<string> columns)
    {
        _document.Append("<tr>");
        foreach (var col in columns)
        {
            _document.Append($"<td>{col}</td>");
        }

        _document.Append("</tr>");

        return this;
    }

    public HtmlFormatter AddHeader(IEnumerable<string> columns)
    {
        _document.Append("<tr>");
        foreach (var col in columns)
        {
            _document.Append($"<th>{col}</th>");
        }

        _document.Append("</tr>");

        return this;
    }

    public HtmlFormatter OpenTable()
    {
        _document.Append("<table>");

        return this;
    }

    public HtmlFormatter CloseTable()
    {
        _document.Append("</table>");

        return this;
    }

    public override string ToString()
    {
        return _document.ToString();
    }

    public string ToHtmlString()
    {
        return $"<!DOCTYPE html><html><body>{_document}</body></html>";
    }
}