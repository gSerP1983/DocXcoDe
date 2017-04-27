using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class DocumentNode : BaseNode, IVisualNode
    {
        public override bool IsLeaf { get { return false; } }

        public OpenXmlElement GetElement()
        {
            var body = new Body();
            foreach (var node in Nodes.OfType<IVisualNode>())
                body.AppendChild(node.GetElement());
            return body;
        }
    }
}