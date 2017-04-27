using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocXcoDe.Util;

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
        public string QueryPath { get; set; }

        private readonly DataTable _data = new DataTable();
        public DataTable Data { get { return _data; } }

        public string GetQuery()
        {
            if (!string.IsNullOrWhiteSpace(Query))
                return Query;

            if (!string.IsNullOrWhiteSpace(QueryPath))
            {
                var query = FileHelper.ReadSmart(QueryPath);
                if (string.IsNullOrWhiteSpace(query))
                    throw new ApplicationException(string.Format("В файле '{0}' пустой запрос...", QueryPath));
                return query;
            }

            throw new ApplicationException(string.Format("Для {0} должно быть задано либо Query, либо QueryPath...", GetType().Name));
        }
    }

    public interface IVisualNode
    {
        OpenXmlElement GetElement();
    }
}