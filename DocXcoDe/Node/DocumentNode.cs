using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class DocumentNode : BaseNode
    {
        public override bool IsLeaf { get { return false; } }

        public override OpenXmlElement GetElement()
        {
            var body = new Body();
            foreach (var node in Nodes)
            {
                var el = node.GetElement();
                if (el != null)
                    body.AppendChild(el);
            }
            return body;
        }
    }
}