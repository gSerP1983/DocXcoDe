using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocXcoDe.Node;
using DocXcoDe.Util;

namespace DocXcoDe
{
    public class DocumentProcessor
    {
        private readonly string _xmlTemplatePath;
        private readonly string _connectionString;
        private readonly string _resultPath;
        private readonly string _docxTemplatePath;

        public DocumentProcessor(string xmlTemplatePath, string docxTemplatePath, string connectionString, string resultPath)
        {
            _xmlTemplatePath = xmlTemplatePath;
            _connectionString = connectionString;
            _resultPath = resultPath;
            _docxTemplatePath = docxTemplatePath;
        }


        public void Process()
        {
            var nodeTypes = GetAllNodeTypes();

            File.Copy(_docxTemplatePath, _resultPath, true);
            var rootNode = (DocumentNode) null;
            var stack = new Stack<BaseNode>();

            using (var reader = XmlReader.Create(new StreamReader(_xmlTemplatePath)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        var node = CreateNode(nodeTypes, reader);

                        if (stack.Any())
                        {
                            var parent = stack.Peek();
                            if (!parent.IsLeaf)
                            {
                                node.Parent = parent;
                                parent.Add(node);
                            }
                        }

                        var queryNode = node as BaseQueryNode;
                        if (queryNode != null)
                            queryNode.ConnectionString = _connectionString;

                        if (!reader.IsEmptyElement)
                            stack.Push(node);
                    }

                    if (reader.NodeType == XmlNodeType.EndElement)
                        rootNode = stack.Pop() as DocumentNode;
                }
            }

            if (rootNode == null)
                throw new ApplicationException("Не удалось правильно распарсить шаблон, проверьте его.");

            using (var doc = WordprocessingDocument.Open(_resultPath, true))
            {
                rootNode.AppendTemplates(doc.MainDocumentPart.Document.Body);
                doc.MainDocumentPart.Document = new Document(rootNode.GetElement());
            }
        }

        private static BaseNode CreateNode(Type[] nodeTypes, XmlReader reader)
        {
            var node = Activator.CreateInstance(GetNodeType(nodeTypes, reader.Name)) as BaseNode;
            if (node == null)
                throw new ApplicationException("Не удалось создать узел.");

            if (!reader.HasAttributes) 
                return node;

            var props = node.GetType().GetProperties();
            while (reader.MoveToNextAttribute())
            {
                var prop = props.FirstOrDefault(x => string.Equals(x.Name, reader.Name, StringComparison.InvariantCultureIgnoreCase));
                if (prop == null)
                    throw new ApplicationException(string.Format("Для типа '{0}' не найдено св-во '{1}'.", node.GetType().Name, reader.Name));

                prop.SetValue(node, SafeConverter.Convert(reader.Value, prop.PropertyType));
            }

            reader.MoveToElement();
            return node;
        }

        private static Type GetNodeType(Type[] nodeTypes, string nodeName)
        {
            var type = nodeTypes.FirstOrDefault(x => string.Equals(x.Name, nodeName + "NODE", StringComparison.InvariantCultureIgnoreCase));
            if (type == null)
                throw new ApplicationException("В шаблоне найден неизвестный элемент " + nodeName.ToUpperInvariant());

            return type;
        }

        private static Type[] GetAllNodeTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => typeof(BaseNode).IsAssignableFrom(x))
                .ToArray();
        } 
    }
}