﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace HelperLibrary

{
    public class TextFileRW
    {
        public static DataTable CreateTableFromObject<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string setOutputFilename(string sourceFilename, string suffix)
        {
            return $"{sourceFilename.Substring(0, sourceFilename.Length - 4)} {suffix}{DateTime.Now.ToLongTimeString().Replace(":", "").Replace(" ", "")} .txt";
        }

        public static void writeTableToTxtFile(DataTable dtResult, string pathResult, string delimiter, bool hasHeader = true)
        {
            try
            {
                FileStream fsWrite = new FileStream(pathResult, FileMode.Create, FileAccess.Write);

                if (hasHeader)
                {
                    StringBuilder headerBuilder = new StringBuilder();
                    for (int j = 0; j < dtResult.Columns.Count; j++)
                    {
                        if (j == dtResult.Columns.Count - 1)
                        {
                            headerBuilder.Append($"\"{dtResult.Columns[j]}\"");
                        }
                        else
                        {
                            headerBuilder.Append($"\"{dtResult.Columns[j]}\"{delimiter}");
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(fsWrite, Encoding.Default, 4096, true))
                    {
                        writer.WriteLine(headerBuilder.ToString());
                    }
                }

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    StringBuilder lineBuilder = new StringBuilder();

                    for (int j = 0; j < dtResult.Columns.Count; j++)
                    {
                        if (j == dtResult.Columns.Count - 1)
                        {
                            lineBuilder.Append($"\"{dtResult.Rows[i][j]}\"");
                        }
                        else
                        {
                            lineBuilder.Append($"\"{dtResult.Rows[i][j]}\"{delimiter}");
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(fsWrite, Encoding.Default, 4096, true))
                    {
                        writer.WriteLine(lineBuilder.ToString());
                    }
                }

                fsWrite.Close();
            }
            catch (Exception ex)
            {
              //  MessageBox.Show($"Error : {ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static DataTable readTextFileToTable(string fileName, string delimiter)
        {
            DataTable table = new DataTable();
            try
            {
                int count = 0;
                using (TextFieldParser txtParser = new TextFieldParser(fileName))
                {
                    txtParser.SetDelimiters(delimiter);

                    while (!txtParser.EndOfData)
                    {
                        string[] fields = txtParser.ReadFields();

                        int l = fields.GetLength(0);
                        long line = txtParser.LineNumber;

                        if (line == 2)
                        {
                            int colIndex = 0;

                            foreach (string field in fields)
                            {
                               
                                if(table.Columns.Contains(field))
                                {
                                    colIndex++;
                                    table.Columns.Add($"{field}_{colIndex}"); // For files with repeating column names.
                                }
                                else
                                {
                                    table.Columns.Add(field);
                                }
                                
                            }
                        }
                        else
                        { table.Rows.Add(fields); }
                        count++;

                    }


                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return table;
        }

       

    }
}