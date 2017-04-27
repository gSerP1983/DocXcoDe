using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class ForeachNode : BaseQueryNode, IVisualNode
    {
        public override bool IsLeaf { get { return false; } }
        public OpenXmlElement GetElement()
        {
            var run = new Run();
            foreach (var dataRow in Data.Rows.Cast<DataRow>())
            {
                foreach (var node in Nodes.OfType<IVisualNode>())
                    run.AppendChild(node.GetElement());
            }
            return new Paragraph(run);
        }
    }
}