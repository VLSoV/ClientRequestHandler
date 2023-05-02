using ClientRequestHandler.Commands;
using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ClientRequestHandler.ViewModels
{
    internal class AddClientWindowViewModel : ViewModel
    {
        #region Clients
        ObservableCollection<Client> _Clients = new ObservableCollection<Client>();
        /// <summary>Список добавляемых клиентов</summary>
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
        }
        #endregion

        #region Commands
        /// <summary>Добавление новых клиентов в базу данных</summary>
        public ICommand AddClientCommand { get; set; }
        private void OnAddClientExecuted(object obj)
        {
            DBWorker.InsertData(Clients);
            var upd = DBWorker.Clients;//синхронизируем клиентов с бд
        }
        private bool CanAddClientExecute(object obj) => (Clients.Count != 0);
        #endregion

        public AddClientWindowViewModel()
        {
            AddClientCommand = new RelayCommand(OnAddClientExecuted, CanAddClientExecute);

            Clients.Add(new Client() { Name="Добавленный клиент1", INN="11111"});
        }
    }
}
