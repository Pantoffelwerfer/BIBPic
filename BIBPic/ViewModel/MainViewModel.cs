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
using Microsoft.IdentityModel.Tokens;


namespace BIBPic.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<ClassNames> _classNames;
        private ObservableCollection<Logger> _logs;
        private List<Student> _students;

        private string _selectedClassValue;
        private string _destSelectedFolderPath;
        private string _originSelectedFolderPath;
        private bool _isButtonEnabled;

        private Logger _logger = new Logger();
        private BitmapImage _imageSource;

        private readonly IFileExplorerService _fileExplorerService;
        private ICommand _openCommand;
        private ICommand _startCommand;

        #endregion

        #region Properties

        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(param => DirectoryHelper.GetFiles(), param => IsButtonEnabled);
                }
                return _startCommand;

            }

        }


        public string OriginSelectedFolderPath
        {
            get { return _originSelectedFolderPath; }
            set
            {
                if (_originSelectedFolderPath != value)
                {

                    _originSelectedFolderPath = value;
                    OnPropertyChanged();
                    UpdateButtonState();
                }
            }
        }
        public string DestSelectedFolderPath
        {
            get { return _destSelectedFolderPath; }
            set
            {
                if (_destSelectedFolderPath != value)
                {

                    _destSelectedFolderPath = value;
                    OnPropertyChanged();
                    UpdateButtonState();
                }

            }
        }

        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {

                _isButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(param => Open(param));
                }

                return _openCommand;
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
        #endregion

        public DirectoryHelper DirectoryHelper = new DirectoryHelper();


        private void UpdateButtonState()
        {
            // Aktualisieren Sie den Zustand des Buttons basierend auf den Feldwerten
            IsButtonEnabled = !string.IsNullOrWhiteSpace(DestSelectedFolderPath) && !string.IsNullOrWhiteSpace(OriginSelectedFolderPath);
        }

        

        public MainViewModel(IFileExplorerService fileExplorerService)
        {
            _fileExplorerService = fileExplorerService;
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
            
            List<ClassNames> list = DirectoryHelper.GetClassNamesFromExcel();
            foreach (var className in list)
            {
                ClassNamesList.Add(className);
            }
        }

        //Get the students by query.
        //ToDo: Implement the method to get the students by query.
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


        private void Open(object param)
        {
            FileExplorerService fileExplorerService = new FileExplorerService();
            string selectedFolder = fileExplorerService.OpenFileDialog();
            if (!string.IsNullOrEmpty(selectedFolder) && Convert.ToInt32(param) == 1)
            {
                OriginSelectedFolderPath = selectedFolder;
                DirectoryHelper.OriginFolderPath = OriginSelectedFolderPath;
            }
            else if (!string.IsNullOrEmpty(selectedFolder) && Convert.ToInt32(param) == 2)
            {
                DestSelectedFolderPath = selectedFolder;
                DirectoryHelper.TargetFolderPath = DestSelectedFolderPath;
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
