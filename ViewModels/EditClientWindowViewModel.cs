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
        /// <summary>Выбранная строка в таблице клиентов</summary>
        Client _SelectedClient;
        /// <summary>Выбранная строка в таблице клиентов</summary>
        public Client SelectedClient
        {
            get => _SelectedClient;
            set
            {
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
        /// <summary>Удаляет выбранного клиента</summary>
        public ICommand DeleteClientCommand { get; set; }
        private void OnDeleteClientExecuted(object obj)
        {
            if (SelectedClient?.RequestCount != 0)
                MessageBox.Show($"С клиентом {SelectedClient?.Name} связана часть заявок","Можно удалять только клиентов, у которых нет никаких заявок", MessageBoxButton.OK);
            else
                DBWorker.DeleteData(SelectedClient);
        }

        /// <summary>Изменяет выбранного клиента</summary>
        public ICommand EditClientCommand { get; set; }
        private void OnEditClientExecuted(object obj)
        {
            DBWorker.UpdateData(SelectedClient);
        }
        #endregion

        public EditClientWindowViewModel()
        {
            DeleteClientCommand = new RelayCommand(OnDeleteClientExecuted);
            EditClientCommand = new RelayCommand(OnEditClientExecuted);
        }
    }
}
