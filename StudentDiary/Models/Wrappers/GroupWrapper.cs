﻿using System.ComponentModel;

namespace StudentDiary.Models.Wrappers
{
    public class GroupWrapper : IDataErrorInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Error { get; set; }
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Id):
                        if (Id == 0)
                            Error = "Należy wybrać grupę";
                        else
                            Error = string.Empty;
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }
        public bool IsValid
        { 
            get 
            {
                return string.IsNullOrWhiteSpace(Error);
            }
        }
    }
}

