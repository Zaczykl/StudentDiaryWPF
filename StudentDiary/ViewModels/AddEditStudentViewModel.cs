using Diary.Commands;
using StudentDiary.Models;
using StudentDiary.Models.Domains;
using StudentDiary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentDiary.ViewModels
{
    public class AddEditStudentViewModel : ViewModelBase
    {
        //Pola
        private StudentWrapper _student;
        private ObservableCollection<Group> _groups;
        private bool _isUpdate;
        private Repository _repository = new Repository();

        
        public AddEditStudentViewModel(StudentWrapper student=null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm,CanConfirm);
            if (student == null)
            {
                IsUpdate = false;
                Student = new StudentWrapper();
            }
            else
            {
                IsUpdate = true;
                Student = student;
            }

            InitGroups();
        }

        private bool CanConfirm(object obj)
        {
            return Student.IsValid;
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public StudentWrapper Student
        {
            get { return _student; }
            set 
            {
                _student = value; 
                OnPropertyChanged(); 
            }
        }

        public bool IsUpdate
        {
            get { return _isUpdate; }
            set 
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }        

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            { 
                _groups = value;
                OnPropertyChanged();
            }
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }


        private void Confirm(object obj)
        {
            if (!Student.IsValid)
                return;

            if (!IsUpdate)
                AddStudent();
            else
                UpdateStudent();
            CloseWindow(obj as Window);
        }

        private void UpdateStudent()
        {
            _repository.UpdateStudent(Student);
        }

        private void AddStudent()
        {
            _repository.AddStudent(Student);
        }

       
        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "--brak--" });

            Groups = new ObservableCollection<Group>(groups);
        }
    }
}
