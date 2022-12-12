using FileImportExportLibrary;

namespace FileImportExportConsole
{
    internal class Program
    { 
        static void Main(string[] args)
        {
            string path = @"";
            string savingPath = @"";

            string filename = "csvPerson.txt";
            string seperator = ";";
            ImportExport<Person> importExport = new ImportExport<Person>();
            var personList = new List<Person>();
            personList.Add(new Person() { FirstName = "Max", LastName = "Mustermann", Birthday = new DateTime(2000, 1, 1), PersonID = 1 });
            personList.Add(new Person() { FirstName = "Pascal", LastName = "Testmann", Birthday = new DateTime(2000, 1, 1), PersonID = 1 });
            personList.Add(new Person() { FirstName = "Patrick", LastName = "Newman", Birthday = new DateTime(2000, 1, 1), PersonID = 1 });
            importExport.ImportCSV(path, filename, seperator);
            importExport.ExportCSV(personList, savingPath);

        }
    }
}