using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;

namespace DocXcoDe.Node
{
    public abstract class BaseNode
    {
        public abstract bool IsLeaf { get; }

        public BaseNode Parent { get; internal set; }

        private readonly IList<BaseNode> _nodes = new List<BaseNode>();

        public BaseNode[] Nodes
        {
            get { return _nodes.ToArray(); }
        }

        public void Add(BaseNode node)
        {
            _nodes.Add(node);
        }
    }

    public abstract class BaseQueryNode : BaseNode
    {
        public string Query { get; set; }
        public string Path { get; set; }

        private readonly DataTable _data = new DataTable();
        public DataTable Data { get { return _data; } }
    }

    public interface IVisualNode
    {
        OpenXmlElement GetElement();
    }
}