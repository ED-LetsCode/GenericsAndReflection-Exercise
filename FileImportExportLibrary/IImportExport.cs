using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileImportExportLibrary
{
    internal interface IImportExport <T>
    {
        List<T> ImportCSV(string path, string filename, string seperator);
        List<T> ImportXML(string path, string filename);
        List<T> ImportJSON(string path, string filename);
        void ExportCSV(List<T> csvExport, string path, string filename = "Export.csv", string seperator = ";");
        void ExportXML(List<T> csvExport, string path, string filename = "Export.xml");
        void ExportJSON(List<T> csvExport, string path, string filename = "Export.json");
    }
}
