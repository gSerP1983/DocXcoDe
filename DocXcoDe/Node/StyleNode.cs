using DocumentFormat.OpenXml;

namespace DocXcoDe.Node
{
    public class StyleNode : BaseNode
    {
        public override bool IsLeaf { get { return true; } }

        public bool Bold { get; set; }
        public string FontSize{ get; set; }

        public override OpenXmlElement GetElement()
        {
            return null;
        }
    }
}