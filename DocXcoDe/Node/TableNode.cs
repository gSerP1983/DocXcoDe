using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocXcoDe.Util;

namespace DocXcoDe.Node
{
    public class TableNode : BaseQueryNode, IVisualNode
    {
        public override bool IsLeaf { get { return true; } }

        public OpenXmlElement GetElement()
        {
            Dao.ExecuteQuery(ConnectionString, GetQuery(), Data);

            var visibleCols = Data.Columns.Cast<DataColumn>()
                .Where(col => !col.ColumnName.StartsWith("_"))
                .ToArray();

            var table = new Table();

            var header = new TableRow();
            foreach (var col in visibleCols)
            {
                var cell = new TableCell();
                cell.AppendChild(new Paragraph(new Run(new Text(col.Caption))));
                header.AppendChild(cell);
            }
            table.AppendChild(header);

            foreach (var dataRow in Data.Rows.Cast<DataRow>())
            {
                var row = new TableRow();
                foreach (var col in visibleCols)
                {
                    var cell = new TableCell();
                    var value = dataRow[col].ToString();
                    cell.AppendChild(new Paragraph(new Run(new Text(value))));
                    row.AppendChild(cell);
                }
                table.AppendChild(row);
            }            

            return table;
        }
    }
}