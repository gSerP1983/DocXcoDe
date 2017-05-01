using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    [DebuggerDisplay("Text='{Value}'")]
    public class TextNode : BaseNode
    {
        public override bool IsLeaf { get { return true; } }

        public string Value { get; set; }

        public string Style { get; set; }

        public override OpenXmlElement GetElement()
        {
            var prop = new RunProperties();
            //prop.AppendChild(new RunFonts {Ascii = "Arial"});
            //prop.AppendChild(new Bold());
            //prop.AppendChild(new FontSize { Val = new StringValue("24") });

            var run = new Run(new Text(AdjustQueryValue(Value))) {RunProperties = prop};
            return new Paragraph(run);
        }
    }
}