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
        ObservableCollection<Client> _Clients = new ObservableCollection<Client>();
        /// <summary>Список добавляемых клиентов</summary>
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
        }

        /// <summary>Добавляет новых клиентов в базу данных</summary>
        public ICommand AddClientCommand { get; set; }
        private void OnAddClientExecuted(object obj)
        {
            DBWorker.InsertData(Clients);
        }
        private bool CanAddClientExecute(object obj) => (Clients.Count != 0);


        public AddClientWindowViewModel()
        {
            AddClientCommand = new RelayCommand(OnAddClientExecuted, CanAddClientExecute);
            try
            {
                Clients.Add(new Client() { Name="yaewru4", INN="479497"});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось подключиться к базе данных!");
            }
        }

    }
}
