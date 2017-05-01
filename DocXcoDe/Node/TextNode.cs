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
            var runProp = new RunProperties();
            var patagraphProp = new ParagraphProperties();
            if (!string.IsNullOrEmpty(Style))
            {
                var style = GetStyle(Style);

                if (!string.IsNullOrWhiteSpace(style.Font))
                    runProp.AppendChild(new RunFonts { Ascii = style.Font });

                if (style.Bold)
                    runProp.AppendChild(new Bold());

                if (!string.IsNullOrWhiteSpace(style.Size))
                    runProp.AppendChild(new FontSize { Val = new StringValue(style.Size) });

                if (style.Align != null)
                    patagraphProp.AppendChild(new Justification { Val = style.Align });
            }
            
            var run = new Run(new Text(AdjustQueryValue(Value))) {RunProperties = runProp};
            return new Paragraph(run) { ParagraphProperties = patagraphProp };
        }
    }
}