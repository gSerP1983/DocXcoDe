using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class DocumentNode : BaseNode
    {
        public override OpenXmlElement GetElement()
        {
            var body = new Body();
            foreach (var node in Nodes)
                body.AppendChild(node.GetElement());
            return body;
        }
    }
}