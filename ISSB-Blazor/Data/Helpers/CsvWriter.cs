using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Data.Models;

namespace Data.Helpers;

public class CsvWriter
{
    private const string DELIMITER = ",";

    public string Write<T>(List<IndDataOutModel> list, bool includeHeader = true)
    {
        var sb = new StringBuilder();

        var type = typeof(IndDataOutModel);
        var type1 = typeof(DataCols);

        var properties = type.GetProperties();
        var properties1 = type1.GetProperties();

        if (includeHeader) sb.AppendLine(CreateCsvHeaderLine(properties));

        foreach (var item in list)
        {
            sb.AppendLine(CreateCsvLine(item, properties));
            /// sb.AppendLine(this.CreateCsvStringListItem(item.Datas, properties1);
            foreach (var dt in item.Datas) sb.AppendLine(CreateCsvLine(dt, properties1));
        }

        return sb.ToString();
    }

    //public string Write<T>(List<IndDataOutModel> list, string fileName, bool includeHeader = true)
    //{
    //    string csv = this.Write(list, includeHeader);

    //    this.WriteFile(fileName, csv);

    //    return csv;
    //}

    public string CreateCsvHeaderLine(PropertyInfo[] properties)
    {
        var propertyValues = new List<string>();

        foreach (var prop in properties)
        {
            var formatString = string.Empty;
            var value = prop.Name;

            var attribute = prop.GetCustomAttribute(typeof(DisplayAttribute));
            if (attribute != null) value = (attribute as DisplayAttribute).Name;

            CreateCsvStringItem(propertyValues, value);
        }

        return CreateCsvLine(propertyValues);
    }

    public string CreateCsvLine<T>(T item, PropertyInfo[] properties)
    {
        var propertyValues = new List<string>();

        foreach (var prop in properties)
        {
            if (prop.Name.Equals("Datas")) return CreateCsvLine(propertyValues);
            var formatString = string.Empty;
            var value = prop.GetValue(item, null);

            if (prop.PropertyType == typeof(string))
            {
                if (prop.Name.Equals("FormNumber"))
                    CreateCsvStringItem(propertyValues, "\t" + value);
                else
                    CreateCsvStringItem(propertyValues, value);
            }
            else if (prop.PropertyType == typeof(string[]))
            {
                CreateCsvStringArrayItem(propertyValues, value);
            }
            else if (prop.PropertyType == typeof(List<string>))
            {
                CreateCsvStringListItem(propertyValues, value);
            }
            else
            {
                CreateCsvItem(propertyValues, value);
            }
        }

        return CreateCsvLine(propertyValues);
    }

    private string CreateCsvLine(IList<string> list)
    {
        return string.Join(DELIMITER, list);
    }

    private void CreateCsvItem(List<string> propertyValues, object value)
    {
        if (value != null)
            propertyValues.Add(value.ToString());
        else
            propertyValues.Add(string.Empty);
    }

    private void CreateCsvStringListItem(List<string> propertyValues, object value)
    {
        var formatString = "\"{0}\"";
        if (value != null)
        {
            value = CreateCsvLine((List<string>)value);
            propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
        }
        else
        {
            propertyValues.Add(string.Empty);
        }
    }

    private void CreateCsvStringArrayItem(List<string> propertyValues, object value)
    {
        var formatString = "\"{0}\"";
        if (value != null)
        {
            value = CreateCsvLine(((string[])value).ToList());
            propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
        }
        else
        {
            propertyValues.Add(string.Empty);
        }
    }

    private void CreateCsvStringItem(List<string> propertyValues, object value)
    {
        var formatString = "\"{0}\"";
        if (value != null)
            propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
        else
            propertyValues.Add(string.Empty);
    }

    private string ProcessStringEscapeSequence(object value)
    {
        return value.ToString().Replace("\"", "\"\"");
    }

    public bool WriteFile(string fileName, string csv)
    {
        var fileCreated = false;

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            File.WriteAllText(fileName, csv);

            fileCreated = true;
        }

        return fileCreated;
    }
}