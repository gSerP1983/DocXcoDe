using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;

namespace DocXcoDe.Node
{
    public abstract class BaseNode
    {
        private readonly IList<BaseNode> _nodes = new List<BaseNode>();

        public BaseNode Parent { get; internal set; }

        public BaseNode[] Nodes
        {
            get { return _nodes.ToArray(); }
        }

        public void Add(BaseNode node)
        {
            _nodes.Add(node);
        }

        public abstract OpenXmlElement GetElement();
    }
}