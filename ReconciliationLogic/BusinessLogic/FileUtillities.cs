using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReconciliationLogic.BusinessLogic
{
    public static class FileUtillities
    {
        /// <summary>
        /// Converts any generic list to json and saves it to the File
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="CompletePath"></param>
        public static async Task WriteJasonToFile<T>(List<T> list, string CompletePath)
        {
            await Task.Run(() =>
            {
                using (StreamWriter file = File.CreateText(CompletePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, list);
                }
            });

        }


        /// <summary>
        /// Creates the CSV from a generic list.
        /// </summary>;
        /// <typeparam name="T"></typeparam>;
        /// <param name="list">The list.</param>;
        /// <param name="csvNameWithExt">Name of CSV (w/ path) w/ file ext.</param>;
        public static async Task CreateCSVFromGenericList<T>(List<T> list, string csvCompletePath)
        {
            if (list == null || list.Count == 0) return;

            await Task.Run(() =>
            {
                //get type from 0th member
                Type t = list[0].GetType();
                string newLine = Environment.NewLine;

                if (!Directory.Exists(Path.GetDirectoryName(csvCompletePath))) Directory.CreateDirectory(Path.GetDirectoryName(csvCompletePath));

                if (File.Exists(csvCompletePath))
                    File.Delete(csvCompletePath);
                File.Create(csvCompletePath).Close();


                using (var sw = new StreamWriter(csvCompletePath))
                {
                    //make a new instance of the class name we figured out to get its props
                    object o = Activator.CreateInstance(t);
                    //gets all properties
                    PropertyInfo[] props = o.GetType().GetProperties();

                    //foreach of the properties in class above, write out properties
                    //this is the header row
                    sw.Write(string.Join(",", props.Select(d => d.Name).ToArray()) + newLine);

                    //this acts as datarow
                    foreach (T item in list)
                    {
                        //this acts as datacolumn
                        var row = string.Join(",", props.Select(d => item.GetType()
                                                                        .GetProperty(d.Name)
                                                                        .GetValue(item, null)
                                                                        .ToString())
                                                                .ToArray());
                        sw.Write(row + newLine);

                    }
                }
            });
        }

    }
}
