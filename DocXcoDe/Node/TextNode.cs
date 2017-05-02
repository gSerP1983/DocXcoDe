using System;
using System.Diagnostics;
using System.Linq;
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

        public string Template { get; set; }

        public override OpenXmlElement GetElement()
        {
            var paragraph = (Paragraph)null;
            if (!string.IsNullOrWhiteSpace(Template))
            {
                paragraph = GetDocumentNode().GetTemplate(Template) as Paragraph;
                if (paragraph == null)
                    throw new ApplicationException(string.Format("Для TextNode указан неверный шаблон '{0}'.", Template));

                // text
                var text = (Text) paragraph.Descendants<Text>().First().CloneNode(true);
                text.Text = AdjustQueryValue(Value);

                // run
                var run = (Run)paragraph.Descendants<Run>().First().CloneNode(true);
                InitRunProperties(run.RunProperties);
                run.RemoveAllChildren<Text>();
                run.AppendChild(text);

                // paragraph
                InitParagraphProperties(paragraph.ParagraphProperties);
                paragraph.RemoveAllChildren<Run>();
                paragraph.AppendChild(run);
            }
            return paragraph ?? CreateElement();
        }

        private OpenXmlElement CreateElement()
        {
            var run = new Run(new Text(AdjustQueryValue(Value))) { RunProperties = InitRunProperties(new RunProperties()) };
            return new Paragraph(run) { ParagraphProperties = InitParagraphProperties(new ParagraphProperties()) };
        }

        private RunProperties InitRunProperties(RunProperties runProp)
        {
            if (!string.IsNullOrEmpty(Style))
            {
                var style = GetDocumentNode().GetStyle(Style);

                if (!string.IsNullOrWhiteSpace(style.Font))
                    runProp.AppendChild(new RunFonts { Ascii = style.Font });

                if (style.Bold)
                    runProp.AppendChild(new Bold());

                if (!string.IsNullOrWhiteSpace(style.Size))
                    runProp.AppendChild(new FontSize { Val = new StringValue(style.Size) });
            }
            return runProp;
        }

        private ParagraphProperties InitParagraphProperties(ParagraphProperties patagraphProp)
        {
            if (!string.IsNullOrEmpty(Style))
            {
                var style = GetDocumentNode().GetStyle(Style);
                if (style.Align != null)
                    patagraphProp.AppendChild(new Justification { Val = style.Align });
            }
            return patagraphProp;
        }
    }
}