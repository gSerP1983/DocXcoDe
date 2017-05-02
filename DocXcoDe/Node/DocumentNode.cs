using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class DocumentNode : BaseNode
    {
        public override bool IsLeaf { get { return false; } }

        public Dictionary<string, OpenXmlElement> Templates { get; private set; }

        public DocumentNode()
        {
            Templates = new Dictionary<string, OpenXmlElement>();
        }

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

        public void AppendTemplates(Body body)
        {
            var paragraphs = body.Descendants<Paragraph>().Where(x => !string.IsNullOrWhiteSpace(x.InnerText));
            foreach (var p in paragraphs)
                Templates.Add(p.InnerText, p.CloneNode(true));
        }

        public StyleNode GetStyle(string name)
        {
            var style = Nodes.OfType<StyleNode>()
                .FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))
                ;

            if (style == null)
            {
                var err = string.Format("Style с name='{0}' не найден.", name);
                throw new ApplicationException(err);
            }

            return style;
        }

        public OpenXmlElement GetTemplate(string name)
        {
            OpenXmlElement result;
            if (Templates.TryGetValue(name, out result)) 
                return result.CloneNode(true);
            throw new ApplicationException(string.Format("Template '{0}' не найден.", name));
        }
    }
}