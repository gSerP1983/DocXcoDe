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

            var table = new Table(CreateTableProperties());

            var header = new TableRow();
            foreach (var col in visibleCols)
            {
                var cell = new TableCell { TableCellProperties = CreateTableHeaderCellProperties() };
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

        private static TableProperties CreateTableProperties()
        {
            UInt32Value borderWidth = 8;
            return new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = borderWidth }
                )
            );
        }

        private static TableCellProperties CreateTableHeaderCellProperties()
        {
            var tcp = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Auto }
                );
            var shading = new Shading
            {
                Color = "auto",
                Fill = "ABCDEF",
                Val = ShadingPatternValues.Clear
            };
            tcp.AppendChild(shading);
            return tcp;
        }
    }
}