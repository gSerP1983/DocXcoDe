using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocXcoDe.Util;

namespace DocXcoDe.Node
{
    public abstract class BaseNode
    {
        public string Name { get; set; }

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

        public abstract OpenXmlElement GetElement();

        private BaseQueryNode GetQueryNode(string name = null)
        {
            var parent = Parent;
            while (parent != null)
            {
                var queryNode = parent as BaseQueryNode;
                if (queryNode != null && (string.IsNullOrEmpty(name) || string.Equals(name, queryNode.Name, StringComparison.InvariantCultureIgnoreCase)))
                    return queryNode;

                parent = parent.Parent;
            }
            return null;
        }

        protected string AdjustQueryValue(string value)
        {
            var matches = new Regex("{[^}]+}").Matches(value);
            for(var i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];
                var val = match.Groups[0].Value.TrimStart('{').TrimEnd('}');

                var colName = val;
                var nodeName = (string) null;

                var arr = val.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 2)
                    throw new ApplicationException(string.Format("Выражение '{0}' задано с ошибками.", val));

                if (arr.Length == 2)
                {
                    colName = arr[1];
                    nodeName = arr[0];
                }

                var queryNode = GetQueryNode(nodeName);
                if (queryNode == null)
                    throw new ApplicationException(string.Format("Для выражения '{0}' не удалось найти источник данных.", val));

                var queryVal = queryNode.Current[colName].ToString();
                value = value.Remove(match.Index, match.Length).Insert(match.Index, queryVal);
            }
            return value;
        }

        protected StyleNode GetStyle(string name)
        {
            var node = this;
            while (node.Parent != null)
                node = node.Parent;

            var style = node.Nodes.OfType<StyleNode>()
                .FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))
                ;

            if (style == null)
            {
                var err = string.Format("Style с name='{0}' не найден.", name);
                throw new ApplicationException(err);
            }

            return style;
        }
    }

    public abstract class BaseQueryNode : BaseNode
    {
        public string Query { get; set; }
        public string QueryPath { get; set; }


        public string ConnectionString { get; internal set; }

        private readonly DataTable _data = new DataTable();
        public DataTable Data { get { return _data; } }

        public DataRow Current { get; protected set; }

        public string GetQuery()
        {
            if (!string.IsNullOrWhiteSpace(Query))
                return AdjustQueryValue(Query);

            if (!string.IsNullOrWhiteSpace(QueryPath))
            {
                var query = FileHelper.ReadSmart(QueryPath);
                if (string.IsNullOrWhiteSpace(query))
                    throw new ApplicationException(string.Format("В файле '{0}' пустой запрос...", QueryPath));
                return AdjustQueryValue(query);
            }

            throw new ApplicationException(string.Format("Для {0} должно быть задано либо Query, либо QueryPath...", GetType().Name));
        }
    }
}