namespace DocXcoDe
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = @"Data Source=DEV-DB-V-02.compulink.local\SQL2012;Initial Catalog=MobileService_Dev;Persist Security Info=True;User ID=sa;Password=sa0.123;";
            new DocumentProcessor("template.xml", "template.docx", connectionString, "1.docx")
                .Process();
        }
    }
}
