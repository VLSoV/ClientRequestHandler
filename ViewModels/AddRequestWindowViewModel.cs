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
        #region SelectedClient
        Client _SelectedClient;
        /// <summary>Выбранная строка в таблице клиентов</summary>
        public Client SelectedClient
        {
            get => _SelectedClient;
            set
            {
                Set(ref _SelectedClient, value);
                OnPropertyChanged(nameof(Requests));
            }
        }
        #endregion

        #region Clients
        /// <summary>Таблица клиентов</summary>
        public ObservableCollection<Client> Clients
        {
            get => DBWorker.Clients;
        }
        #endregion

        #region Requests
        ObservableCollection<Request> _Requests = new ObservableCollection<Request>();
        /// <summary>Список добавляемых заявок</summary>
        public ObservableCollection<Request> Requests
        {
            get => _Requests;
            set => Set(ref _Requests, value);
        }
        #endregion

        #region StatusList
        /// <summary>Список вариантов статуса прогресса заявки</summary>
        public List<string> StatusList
        {
            get => new List<string>() { "Новая", "В работе", "Выполнена" };
        }
        #endregion

        #region Commands
        /// <summary>Добавление новых заявок в базу данных</summary>
        public ICommand AddRequestCommand { get; set; }
        private void OnAddRequestExecuted(object obj)
        {
            foreach(Request request in Requests)
            {
                request.ClientId = _SelectedClient.Id;
            }
            DBWorker.InsertData(Requests);
            var upd = DBWorker.Requests();//синхронизация заявки с бд
        }
        private bool CanAddRequestExecute(object obj) => (_SelectedClient != null);
        #endregion

        public AddRequestWindowViewModel()
        {
            AddRequestCommand = new RelayCommand(OnAddRequestExecuted, CanAddRequestExecute);
        }
    }
}
