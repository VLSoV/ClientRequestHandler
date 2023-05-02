using ClientRequestHandler.Commands;
using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace ClientRequestHandler.ViewModels
{
    internal class EditClientWindowViewModel : ViewModel
    {

        #region SelectedClient
        Client _SelectedClient;
        /// <summary>Выбранная строка в таблице клиентов</summary>
        public Client SelectedClient
        {
            get => _SelectedClient;
            set
            {
                DBWorker.EditData(SelectedClient);
                Set(ref _SelectedClient, value);
            }
        }
        #endregion

        #region Clients
        /// <summary>Таблица клиентов</summary>
        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get => DBWorker.Clients;
            set
            {
                _Clients = value;
            }
        }
        #endregion

        #region Commands
        /// <summary>Удаление выбранного клиента</summary>
        public ICommand DeleteClientCommand { get; set; }
        private void OnDeleteClientExecuted(object obj)
        {
            if (SelectedClient?.RequestCount != 0)
                MessageBox.Show($"С клиентом {SelectedClient?.Name} связана часть заявок","Можно удалять только клиентов, у которых нет никаких заявок", MessageBoxButton.OK);
            else
                DBWorker.DeleteData(SelectedClient);
        }
        #endregion

        public EditClientWindowViewModel()
        {
            DeleteClientCommand = new RelayCommand(OnDeleteClientExecuted);
        }
    }
}
