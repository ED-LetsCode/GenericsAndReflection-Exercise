using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;

namespace FileImportExportLibrary
{
    public class ImportExport<T> : IImportExport<T>
    {
        public List<T> ImportCSV(string path, string filename, string seperator)
        {
            List<List<string>> csvData = new List<List<string>>();
            List<T> csvProps = new List<T>();
            try
            {
                //Read csv data
                using StreamReader sr = new StreamReader($"{path}\\{filename}");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    List<string> csvLine = new List<string>();
                    line.Split(seperator).ToList().ForEach(x => csvLine.Add(x));
                    csvData.Add(csvLine);
                }
                sr.Close();

                //Get type of object
                Type type = typeof(T);

                //Line index starts at 1 because the first line of every csv file are the column names
                int lineIndex = 1, parsedElements = 0;
                
                //While lineIndex is smaller then the length of the csvData lines
                while (lineIndex < csvData.Count)
                {
                    //Create an instance of T
                    T obj = (T)Activator.CreateInstance(typeof(T));

                    //Get every property from T
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        //For loop every value in the line
                        for (int i = 0; i < csvData[lineIndex].Count; i++)
                        {
                            //If the property name contains the column name of the csvData
                            if (csvData[0][i].ToLower().Contains(prop.Name.ToLower()))
                            {
                                var actualPropValue = csvData[lineIndex][i];

                                //Get type of property and try to parse the value
                                switch (Type.GetTypeCode(prop.PropertyType))
                                {
                                    case TypeCode.String:
                                        prop.SetValue(obj, actualPropValue);
                                        parsedElements++;
                                        break;

                                    case TypeCode.Int32:
                                        if (int.TryParse(actualPropValue, out int valueInt))
                                        {
                                            prop.SetValue(obj, valueInt);
                                            parsedElements++;
                                        }
                                        break;

                                    case TypeCode.DateTime:
                                        if (DateTime.TryParse(actualPropValue, out DateTime valueDate))
                                        {
                                            prop.SetValue(obj, valueDate);
                                            parsedElements++;
                                        }
                                        break;

                                    case TypeCode.Double:
                                        if (double.TryParse(actualPropValue, out double valueDouble))
                                        {
                                            prop.SetValue(obj, valueDouble);
                                            parsedElements++;
                                        }
                                        break;

                                    case TypeCode.Boolean:
                                        if (bool.TryParse(actualPropValue, out bool valueBool))
                                        {
                                            prop.SetValue(obj, valueBool);
                                            parsedElements++;
                                        }
                                        break;

                                    case TypeCode.Char:
                                        if (char.TryParse(actualPropValue, out char valueChar))
                                        {
                                            prop.SetValue(obj, valueChar);
                                            parsedElements++;
                                        }
                                        break;

                                    case TypeCode.Decimal:
                                        if (decimal.TryParse(actualPropValue, out decimal valueDecimal))
                                        {
                                            prop.SetValue(obj, valueDecimal);
                                            parsedElements++;
                                        }
                                        break;

                                    default:
                                        throw new Exception("Type not found");
                                        break;

                                };
                            }
                            //If first value from csv is parsed for the property then break for loop
                            if (parsedElements-- == 1) break;
                        }
                    }
                    //Add object to T list
                    csvProps.Add(obj);
                    //Increase lineIndex and change to next line in csvData
                    lineIndex++;
                }
            }
            catch (Exception ex) { throw new Exception(); }
            return csvProps;
        }

        public List<T> ImportJSON(string path, string filename)
        {
            List<T> jsonImport = new List<T>();
            try
            {
                string json = File.ReadAllText(path + filename);
                jsonImport = JsonConvert.DeserializeObject<List<T>>(json)!;
            }
            catch { throw new Exception(); }
            return jsonImport;
        }

        public List<T> ImportXML(string path, string filename)
        {
            List<T> xmlImport = new List<T>();
            try
            {
                using StreamReader sr = new StreamReader(path + filename);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                xmlImport = (List<T>)xmlSerializer.Deserialize(sr);
                sr.Close();
            }
            catch { throw new Exception(); }
            return xmlImport;
        }

        public void ExportCSV(List<T> csvExport, string path, string filename = "Export.csv", string seperator = ";")
        {
            try
            {
                //Create a new StringBuilder
                StringBuilder sb = new StringBuilder();

                //Get type of T
                Type type = typeof(T);

                //Get all properties from T
                PropertyInfo[] props = type.GetProperties();

                //Create a new list of strings
                List<string> csvData = new List<string>();

                //For loop every property in props
                for (int i = 0; i < props.Length; i++)
                {
                    //Add the property name to the csvData list
                    csvData.Add(props[i].Name);
                }

                //Add the csvData list to the StringBuilder
                sb.AppendLine(string.Join(seperator, csvData));

                //For loop every object in csvExport
                foreach (T obj in csvExport)
                {
                    //Clear the csvData list
                    csvData.Clear();

                    //For loop every property in props
                    for (int i = 0; i < props.Length; i++)
                    {
                        //Add the property value to the csvData list
                        csvData.Add(props[i].GetValue(obj).ToString());
                    }

                    //Add the csvData list to the StringBuilder
                    sb.AppendLine(string.Join(seperator, csvData));
                }

                //Write the StringBuilder to a file
                File.WriteAllText(path + filename, sb.ToString());
            }
            catch { throw new Exception(); }
        }

        public void ExportJSON(List<T> csvExport, string path, string filename = "Export.json")
        {
            try
            {
                string json = JsonConvert.SerializeObject(csvExport, Formatting.Indented);
                File.WriteAllText(path + filename, json);
            }
            catch { throw new Exception(); }
        }

        public void ExportXML(List<T> csvExport, string path, string filename = "Export.xml")
        {
            try
            {
                using StreamWriter sw = new StreamWriter(path + filename);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                xmlSerializer.Serialize(sw, csvExport);
                sw.Close();
            }
            catch { throw new Exception(); }
        }
    }
}