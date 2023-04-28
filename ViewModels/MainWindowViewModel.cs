using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ClientRequestHandler.Views;
using System.Windows.Input;
using ClientRequestHandler.Commands;

namespace ClientRequestHandler.ViewModels
{
    internal class MainWindowViewModel : ViewModel
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
                OnPropertyChanged(nameof(Requests));
            }
        }
        #endregion

        #region SelectedTabIndex
        /// <summary>Индекс выбранной вкладки TabItem</summary>
        private int _selectedTabIndex;
        /// <summary>Индекс выбранной вкладки TabItem</summary>
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                Set(ref _selectedTabIndex, _selectedTabIndex);
                SelectedClient = null;
                OnPropertyChanged(nameof(Requests));
            }
        }
        #endregion

        #region Clients
        private ObservableCollection<Client> _Clients;
        /// <summary>Таблица клиентов</summary>
        public ObservableCollection<Client> Clients
        {
            get => DBWorker.Clients;
        }
        #endregion

        #region Requests
        //private ObservableCollection<Request> _Requests;
        /// <summary>Таблица заявок</summary>
        public ObservableCollection<Request> Requests
        {
            get => DBWorker.Requests(_SelectedClient);
        }
        #endregion

        public ICommand OpenAddClientCommand { get; set; }
        private void OnAddClientExecuted(object obj) => new AddClientWindow() { Owner = App.Current.MainWindow }.Show();

        public ICommand OpenEditClientCommand { get; set; }
        private void OnEditClientExecuted(object obj) => new EditClientWindow() { Owner = App.Current.MainWindow }.Show();

        public MainWindowViewModel()
        {
            OpenAddClientCommand = new RelayCommand(OnAddClientExecuted);
            OpenEditClientCommand = new RelayCommand(OnEditClientExecuted);
            try
            {
                DBWorker.CreateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось подключиться к базе данных!");
            }
        }
    }
}
