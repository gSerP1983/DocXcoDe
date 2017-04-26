using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    [DebuggerDisplay("Text='{Value}'")]
    public class TextNode : BaseNode
    {
        public string Value { get; set; }
        public override OpenXmlElement GetElement()
        {
            return new Paragraph(new Run(new Text(Value)));
        }
    }
}