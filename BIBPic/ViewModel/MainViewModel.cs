using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIBPic.Ressources;
using BIBPic.Model;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;


namespace BIBPic.ViewModel
{
    internal class MainViewModel: BaseViewModel
    {
		private static List<ClassNames> _classNames;
        private List<Students> _students;
        private SQLConnect _sqlConnect = new SQLConnect();
        


        public List<ClassNames> ClassNamesList
		{
			get { return _classNames; }
            set
            {
                if (_classNames != value)
                {
                    GetClassNamesByQuery();
                    _classNames = value;
                    OnPropertyChanged();
                }
            }
		}

		public List<Students> Students
		{
			get { return _students; }
            set
            {
                GetStudentsByQuery(ClassValue);
                _students = value;
            }
		}

        private string _classValue;

        public string ClassValue
        {
            get { return _classValue; }
            set
            {
                _classValue = value;
                OnPropertyChanged();
            }
        }




        public MainViewModel()
        {
            _classNames = new List<ClassNames>();
            _students = new List<Students>();
        }

        public void GetClassNamesByQuery()
        {
            ClassNamesList.Clear();
            ClassNamesList.Add(new ClassNames("Alle"));
            string query = @"
                SELECT 
                    Class.ClassName
                FROM 
                    Class
                ORDER BY 
                    Class.ClassName";

            // Erstellen und Ausführen des SQL-Befehls
            using (SqlCommand command = new SqlCommand(query, _sqlConnect.Connection))
            {
                // Ausführen der Abfrage und Verarbeiten der Ergebnisse
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Lesen der Daten und Hinzufügen zum Liste
                        ClassNames classNames = new ClassNames((string)reader["Class"]);
                        
                        ClassNamesList.Add(classNames);
                    }
                }
            }
        }

        public void GetStudentsByQuery(string className)
        {
            string query = @"
                SELECT 
                    Student.Name, 
                    Student.Surname, 
                    Student.StudentID
                    Student.Class
                FROM 
                    Student
                WHERE 
                    Student.Class = @className
                ORDER BY 
                    Student.Surname";

            // Erstellen und Ausführen des SQL-Befehls
            using (SqlCommand command = new SqlCommand(query, _sqlConnect.Connection))
            {
                // Erstellen des Parameters
                if (ClassValue != "Alle")
                {
                    command.Parameters.AddWithValue("@className", className);
                }
                else
                {
                    command.Parameters.AddWithValue("@className", "*");
                }


                // Ausführen der Abfrage und Verarbeiten der Ergebnisse
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Lesen der Daten und Hinzufügen zum Liste
                        Students student = new Students((string)reader["Name"], (string)reader["Surname"], (string)reader["Class"] , (int)reader["StudentID"]);
                        Students.Add(student);
                    }
                }
            }
        }

	}
}
