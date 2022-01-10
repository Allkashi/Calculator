using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Collections.ObjectModel;

namespace Calculator2._0
{
    class Calc
    {

        public static string Calculator(string inp)
        {
            DataTable dt = new DataTable();
            return dt.Compute(inp, "").ToString();
        }

        public static void SaveFile(string inp)
        {
            string writePath = @"C:\Users\rapun\source\repos\Calculator2.0\TextHistory.txt";

            using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(inp);
            }

        }

        public static void ReadFromDB(ObservableCollection<string> history)
        {
            using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source = C:\\Users\\rapun\\OneDrive\\Desktop\\SQLiteStudio\\calc.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "select * from list";
                var reader = command.ExecuteReader();
                while (reader.Read() && history.Count < 10)
                {
                    history.Add(System.Convert.ToString(reader["expression"]));
                }
            }
        }


        public static void LoadToDB(string inp)
        {
            using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source =  C:\\Users\\rapun\\OneDrive\\Desktop\\SQLiteStudio\\calc.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "insert into list (expression) values(" + "\'" + inp + "\'" + ")";
                var reader = command.ExecuteReader();
            }


        }


    }


}
