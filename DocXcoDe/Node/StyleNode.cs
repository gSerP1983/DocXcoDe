using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class StyleNode : BaseNode
    {
        public override bool IsLeaf { get { return true; } }

        public bool Bold { get; set; }
        public string Size{ get; set; }
        public string Font { get; set; }
        public JustificationValues? Align { get; set; }

        public override OpenXmlElement GetElement()
        {
            if (Parent.GetType() != typeof(DocumentNode))
                throw new ApplicationException("Родителем для Style может быть только Document.");

            return null;
        }
    }
}