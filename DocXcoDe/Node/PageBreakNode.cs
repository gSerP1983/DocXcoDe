using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class PageBreakNode : BaseNode, IVisualNode
    {
        public override bool IsLeaf { get { return true; } }

        public OpenXmlElement GetElement()
        {
            return new Paragraph(new Run(new Break { Type = BreakValues.Page }));
        }        
    }
}