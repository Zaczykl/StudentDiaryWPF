using Diary.Commands;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using StudentDiary.Models;
using StudentDiary.Models.Domains;
using StudentDiary.Models.Wrappers;
using StudentDiary.Properties;
using StudentDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentDiary.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IDataErrorInfo
    {
        private bool _isServerAdressValid;
        private bool _isServerNameValid;
        private bool _isDbNameValid;
        private bool _isUserNameValid;
        public SettingsViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
            ServerAdress = Settings.Default.ServerAdress;
            ServerName = Settings.Default.ServerName;
            DbName = Settings.Default.DbName;
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public string ServerAdress { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string UserName { get; set; }
        public string Password { private get; set; }
        public string Error { get; set; }
        public bool IsValid
        {
            get
            { 
                return _isServerAdressValid && _isServerNameValid && _isDbNameValid && _isUserNameValid; 
            }
        }
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(ServerAdress):
                        if (string.IsNullOrWhiteSpace(ServerAdress))
                        {
                            Error = "Pole Adres serwera jest wymagane.";
                            _isServerAdressValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerAdressValid = true;
                        }
                        break;

                    case nameof(ServerName):
                        if (string.IsNullOrWhiteSpace(ServerName))
                        {
                            Error = "Pole Nazwa serwera jest wymagane.";
                            _isServerNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isServerNameValid = true;
                        }
                        break;

                    case nameof(DbName):
                        if (string.IsNullOrWhiteSpace(DbName))
                        {
                            Error = "Pole Nazwa bazy danych jest wymagane.";
                            _isDbNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isDbNameValid = true;
                        }
                        break;

                    case nameof(UserName):
                        if (string.IsNullOrWhiteSpace(UserName))
                        {
                            Error = "Pole Użytkownik jest wymagane.";
                            _isUserNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isUserNameValid = true;
                        }
                        break;                    

                    default:
                        break;
                }
                return Error;
            }
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }
        private void Confirm(object obj)
        {
            ConfirmStringBuild();
            CloseWindow(obj as Window);
            Application.Current.Shutdown();            
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
        private bool CanConfirm(object obj)
        {
            return IsValid;
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void ConfirmStringBuild()
        {
            ConnectionStringBuild.ServerAdress = ServerAdress;
            ConnectionStringBuild.ServerName = ServerName;
            ConnectionStringBuild.DbName = DbName;
            ConnectionStringBuild.UserName = UserName;
            ConnectionStringBuild.SetPassword(Password);
            Settings.Default.Save();
        }
    }
}
