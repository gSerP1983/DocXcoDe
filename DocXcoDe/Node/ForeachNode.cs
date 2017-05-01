using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocXcoDe.Util;

namespace DocXcoDe.Node
{
    public class ForeachNode : BaseQueryNode
    {
        public override bool IsLeaf { get { return false; } }
        public override OpenXmlElement GetElement()
        {
            Dao.ExecuteQuery(ConnectionString, GetQuery(), Data);

            var run = new Run();
            foreach (var dataRow in Data.Rows.Cast<DataRow>())
            {
                Current = dataRow;
                foreach (var node in Nodes)
                {
                    var el = node.GetElement();
                    if (el != null)
                        run.AppendChild(el);
                }
            }
            return new Paragraph(run);
        }
    }
}