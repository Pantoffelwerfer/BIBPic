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
		private ObservableCollection<ClassNames> _classNames;
        private string _classValue;
        private List<Student> _students;
        
        private Logger _logger = new Logger();
        private ObservableCollection<Logger> _logs;

        public ObservableCollection<Logger> Logging
        {
            get { return _logs; }
            set
            {
                _logs = value;
                OnPropertyChanged();
            }
        }



        public ObservableCollection<ClassNames> ClassNamesList
		{
			get { return _classNames; }
            set
            {
                    _classNames = value;
                    OnPropertyChanged();
            }
		}

		public List<Student> Students
		{
			get { return _students; }
            set
            {
                //GetStudentsByQuery(ClassValue);
                _students = value;
            }
		}

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
            _classNames = new ObservableCollection<ClassNames>();
            GetClassNamesFromExcel();
            _students = new List<Student>();
        }


        //Get the class names from excel file.
        public void GetClassNamesFromExcel()
        {
            DirectoryHelper directoryHelper = new DirectoryHelper();
            List<ClassNames> list = directoryHelper.GetClassNamesFromExcel();
            foreach (var className in list)
            {
                ClassNamesList.Add(className);
            }
        }

        

        public void GetStudentsByQuery(string className)
        {
            //string query = @"
            //    SELECT 
            //        Student.Name, 
            //        Student.Surname, 
            //        Student.StudentID
            //        Student.Class
            //    FROM 
            //        Student
            //    WHERE 
            //        Student.Class = @className
            //    ORDER BY 
            //        Student.Surname";

            //// Erstellen und Ausführen des SQL-Befehls
            //using (SqlCommand command = new SqlCommand(query, _sqlConnect.Connection))
            //{
            //    // Erstellen des Parameters
            //    if (ClassValue != "Alle")
            //    {
            //        command.Parameters.AddWithValue("@className", className);
            //    }
            //    else
            //    {
            //        command.Parameters.AddWithValue("@className", "*");
            //    }


            //    // Ausführen der Abfrage und Verarbeiten der Ergebnisse
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            // Lesen der Daten und Hinzufügen zum Liste
            //            Student student = new Student((string)reader["Name"], (string)reader["Surname"], (string)reader["Class"] , (int)reader["StudentID"]);
            //            Students.Add(student);
            //        }
            //    }
            //}
        }

	}
}
