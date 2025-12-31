using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Data.Models;

public static class DataTableExtensions
{
    public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
                 this IEnumerable<T> source,
                 Func<T, TColumn> columnSelector,
                 Expression<Func<T, TRow>> rowSelector,
                 Func<IEnumerable<T>, TData> dataSelector)
    {
        DataTable table = new DataTable();
        var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
        table.Columns.Add(new DataColumn(rowName));
        var columns = source.Select(columnSelector).Distinct();

        foreach (var column in columns)
            table.Columns.Add(new DataColumn(column.ToString()));

        var rows = source.GroupBy(rowSelector.Compile())
                         .Select(rowGroup => new
                         {
                             Key = rowGroup.Key,
                             Values = columns.GroupJoin(
                                 rowGroup,
                                 c => c,
                                 r => columnSelector(r),
                                 (c, columnGroup) => dataSelector(columnGroup))
                         });

        foreach (var row in rows)
        {
            var dataRow = table.NewRow();
            var items = row.Values.Cast<object>().ToList();
            items.Insert(0, row.Key);
            dataRow.ItemArray = items.ToArray();
            table.Rows.Add(dataRow);
        }

        return table;
    }

    public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
    {
        return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
    }

    public static dynamic[] ToPivotArray<T, TColumn, TRow, TRow1, TData>(
            this IEnumerable<T> source,
            Func<T, TColumn> columnSelector,
            Expression<Func<T, TRow>> rowSelector1,
            Expression<Func<T, TRow1>> rowSelector2,
            Func<IEnumerable<T>, TData> dataSelector)
    {

        //  var outp = Expression.And(rowSelector1, rowSelector2);

        

        var arr = new List<object>();
        var cols = new List<string>();
        string row1 = ((MemberExpression)rowSelector1.Body).Member.Name;
        string row2 = ((MemberExpression)rowSelector2.Body).Member.Name;
        var columns = source.Select(columnSelector).Distinct();
        cols = (new[] { row1 }).Concat(columns.Select(x => x.ToString())).ToList();
        cols = (new[] { row2 }).Concat(columns.Select(x => x.ToString())).ToList();

        //var colTest = columnSelector.Method(columnSelector.Method)
        //var rowSelector = Expression.Lambda<Func<T, TRow1>>(combination).Compile();


        var rows = source.GroupBy(rowSelector1.Compile())
                         .Select(rowGroup => new
                         {
                             Key = rowGroup.Key,
                             Values = columns.GroupJoin(
                                 rowGroup,
                                 c => c,
                                 r => columnSelector(r),
                                 (c, columnGroup) => dataSelector(columnGroup))
                         }).ToArray();

        foreach (var row in rows)
        {
            var items = row.Values.Cast<object>().ToList();
            items.Insert(0, row.Key);
            var obj = GetAnonymousObject(cols, items);
            arr.Add(obj);
        }
        return arr.ToArray();
    }

    private static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
    {
        IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
        int i;
        for (i = 0; i < columns.Count(); i++)
        {
            eo.Add(columns.ElementAt<string>(i), values.ElementAt<object>(i));
        }
        return eo;
    }

   

    public static List<dynamic> ToDynamic(this DataTable dt)
    {
        var dynamicDt = new List<dynamic>();
        foreach (DataRow row in dt.Rows)
        {
            dynamic dyn = new ExpandoObject();
            dynamicDt.Add(dyn);
            foreach (DataColumn column in dt.Columns)
            {
                var dic = (IDictionary<string, object>)dyn;
                dic[column.ColumnName] = row[column];
            }
        }
        return dynamicDt;
    }

   
}
