using ClientRequestHandler.Commands;
using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using ClientRequestHandler.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientRequestHandler.ViewModels
{
    internal class AddRequestWindowViewModel : ViewModel
    {
        #region Requests
        ObservableCollection<Request> _Requests = new ObservableCollection<Request>();
        /// <summary>Список добавляемых клиентов</summary>
        public ObservableCollection<Request> Requests
        {
            get => _Requests;
            set => Set(ref _Requests, value);
        }
        #endregion

        #region Commands
        /// <summary>Добавляет новых клиентов в базу данных</summary>
        public ICommand AddRequestCommand { get; set; }
        private void OnAddRequestExecuted(object obj)
        {
            DBWorker.InsertData(Requests);
        }
        #endregion

        public AddRequestWindowViewModel()
        {
            AddRequestCommand = new RelayCommand(OnAddRequestExecuted);
        }
    }
}
