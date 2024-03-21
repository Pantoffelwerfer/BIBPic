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
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace BIBPic.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<ClassNames> _classNames;
        private string _selectedClassValue;
        private List<Student> _students;
        private Logger _logger = new Logger();
        private ObservableCollection<Logger> _logs;
        private BitmapImage _imageSource;
        private readonly IFileExplorerService _fileExplorerService;
        private ICommand _openCommand;

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(param => Open());
                }

                return _openCommand;
            }
        }

        public MainViewModel(IFileExplorerService fileExplorerService)
        {
            _fileExplorerService = fileExplorerService;
        }

        private string _selectedFilePath;

        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

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
                //GetStudentsByQuery(SelectedClassValue);
                _students = value;
            }
        }

        public string SelectedClassValue
        {
            get { return _selectedClassValue; }
            set
            {
                _selectedClassValue = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _classNames = new ObservableCollection<ClassNames>();
            LoadClassNamesFromExcel();
            LoadImage();
            _students = new List<Student>();
        }


        //Get the class names from excel file.
        public void LoadClassNamesFromExcel()
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
            //    if (SelectedClassValue != "Alle")
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


        private void Open()
        {
            FileExplorerService fileExplorerService = new FileExplorerService();
            string selectedFile = fileExplorerService.OpenFileDialog();
            if (!string.IsNullOrEmpty(selectedFile))
            {
                SelectedFilePath = selectedFile;
            }
        }

        private void LoadImage()
        {

            string filePath = "Ressources\\bibPic.png";
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Relative);
            bitmap.EndInit();

            ImageSource = bitmap;
        }
    }

}
