using Dapper;
using SmartVault.Library;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

namespace SmartVault.DataGeneration
{
    internal partial class Program
    {
        static void GenerateTables(SQLiteConnection connection)
        {
            string[] files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");
            foreach (string file in files)
            {
                XmlSerializer serializer = new(typeof(BusinessObject));
                BusinessObject? businessObject = serializer.Deserialize(new StreamReader(file)) as BusinessObject;
                connection.Execute(businessObject?.Script);
            }
        }
    }
}
