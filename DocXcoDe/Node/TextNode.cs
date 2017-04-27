using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    [DebuggerDisplay("Text='{Value}'")]
    public class TextNode : BaseNode, IVisualNode
    {
        public override bool IsLeaf { get { return true; } }

        public string Value { get; set; }
        public OpenXmlElement GetElement()
        {
            return new Paragraph(new Run(new Text(AdjustQueryValue(Value))));
        }
    }
}