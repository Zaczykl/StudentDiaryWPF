using Diary.Commands;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using StudentDiary.Models;
using StudentDiary.Models.Domains;
using StudentDiary.Models.Wrappers;
using StudentDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentDiary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private StudentWrapper _selectedStudent;
        private ObservableCollection<StudentWrapper> _students;
        private int _selectedGroupId;
        private ObservableCollection<Group> _groups;
        private Repository _repository = new Repository();


        public MainViewModel()
        {            
            InitMainViewModel();
        }

        

        public ICommand RefreshStudentCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand EditSettingsCommand { get; set; }

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set { _groups = value; }
        }

        private void InitMainViewModel()
        {            
            if (ConnectToDatabase())
            {
                RefreshStudentCommand = new RelayCommand(RefreshStudents);
                SettingsCommand = new RelayCommand(OpenSettingsWindow);
                AddStudentCommand = new RelayCommand(AddEditStudent);
                EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
                DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
                RefreshDiary();
                InitGroups();
            }
            else
                Environment.Exit(0);
        }

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);


            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));

        }

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            _repository.DeleteStudent(SelectedStudent.Id);
            RefreshDiary();
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }
        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }
        private void OpenSettingsWindow(object obj=null)
        {
            var settingsWindow = new SettingsView();
            settingsWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }
        private bool CheckConnection()
        {
            string provider = "System.Data.SqlClient";
            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionStringBuild.sqlconnectionstringbuilder().ConnectionString;
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        private bool ConnectToDatabase()
        {
            bool isConnected = false;
            
            if (!CheckConnection())
            {
                var result = MessageBox.Show("Czy chcesz wprowadzić ustawienia?", "Brak połączenia z bazą danych.", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    OpenSettingsWindow();
                    return isConnected;
                }
                else                    
                    Environment.Exit(0);
            }
            isConnected = true;
            return isConnected;
        }


    }
}
