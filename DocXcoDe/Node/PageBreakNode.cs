﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe.Node
{
    public class PageBreakNode : BaseNode
    {
        public override bool IsLeaf { get { return true; } }

        public override OpenXmlElement GetElement()
        {
            return new Paragraph(new Run(new Break { Type = BreakValues.Page }));
        }        
    }
}