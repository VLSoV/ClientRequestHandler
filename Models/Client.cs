using ClientRequestHandler.ViewModels;
using System;
using System.Data;

namespace ClientRequestHandler.Models
{
    internal class Client : ViewModel
    {
        public Client() { }
        public Client(DataRow row)
        {
            Id = (int)row.ItemArray[0];
            Name = (string)row.ItemArray[1];
            INN = (string)row.ItemArray[2];
            ActivityField = (string)row.ItemArray[3];
            RequestCount = (int)row.ItemArray[4];
            LastRequestDate = (DateTime)row.ItemArray[5];
            Note = (string)row.ItemArray[6];
        }

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }
        public string Name 
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        public string INN 
        {
            get => _INN;
            set => Set(ref _INN, value);
        }
        public string ActivityField 
        {
            get => _ActivityField;
            set => Set(ref _ActivityField, value);
        }
        public int RequestCount 
        {
            get => _RequestCount;
            set => Set(ref _RequestCount, value);
        }
        public DateTime LastRequestDate 
        {
            get => _LastRequestDate;
            set => Set(ref _LastRequestDate, value);
        }
        public string Note
        {
            get => _Note;
            set => Set(ref _Note, value);
        }


        int _Id;
        string _Name;
        string _INN;
        string _ActivityField;
        int _RequestCount;
        DateTime _LastRequestDate;
        string _Note;
    }
}
