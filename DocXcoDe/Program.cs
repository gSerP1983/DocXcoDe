using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocXcoDe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Wordprocessing document. 
            /*using (WordprocessingDocument package = WordprocessingDocument.Create("1.docx", WordprocessingDocumentType.Document))
            {
                // Add a new main document part. 
                package.AddMainDocumentPart();

                // Create the Document DOM. 
                package.MainDocumentPart.Document =
                  new Document(
                    new Body(
                      new Paragraph(
                        new Run(
                          new Text("Hello World!")))));

                // Save changes to the main document part. 
                package.MainDocumentPart.Document.Save();
            }*/

            const string connectionString = @"Data Source=DEV-DB-V-02\SQL2012;Initial Catalog=MobileService_Dev;Persist Security Info=True;User ID=sa;Password=sa0.123;";
            new DocumentProcessor("template.xml", connectionString, "1.docx")
                .Process();
        }
    }
}
