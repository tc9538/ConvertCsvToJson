using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace ImportCsvToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            dt = importDataToTable(@"C:\Users\trucc\Desktop\output2.csv");
            StreamWriter sw = new StreamWriter(@"C:\Users\trucc\Desktop\jsonFormat.json");
            sw.Write(WriteJsonFile(dt));
            sw.Close();


        }

        public static DataTable importDataToTable(string filepath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(filepath))
            {
                string[] headers = sr.ReadLine().Split(','); //first row in the csv file
                // add columns to the table by scanning the column input of the first row in the file
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }


                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);  //add data to each row in the table
                }
            }

            return dt;
        }

        public static string WriteJsonFile(DataTable dt)
        {
            var JSONString = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                JSONString.Append("[");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1) //this is for the last item in the csv file
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                        JSONString.AppendLine();
                    }


                    if (i == dt.Rows.Count - 1) //this is for the last item in the csv file
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                    JSONString.AppendLine();

                }
                JSONString.Append("]");

            }
            return JSONString.ToString();
        }

    }
}