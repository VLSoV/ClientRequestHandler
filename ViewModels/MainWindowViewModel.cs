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
        
        #region SelectedRequest
        Request _SelectedRequest;
        /// <summary>Выбранная строка в таблице заявок</summary>
        public Request SelectedRequest
        {
            get
            {
                return _SelectedRequest;
            }
            set
            {
                DBWorker.EditData(_SelectedRequest);
                Set(ref _SelectedRequest, value);
            }
        }
        #endregion

        #region SelectedTabIndex
        private int _selectedTabIndex;
        /// <summary>Индекс выбранной вкладки TabItem</summary>
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                Set(ref _selectedTabIndex, value);
                SelectedClient = null;
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
        /// <summary>Таблица заявок</summary>
        public ObservableCollection<Request> Requests
        {
            get => DBWorker.Requests(_SelectedClient);
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
        /// <summary>Открытие окна добавления клиента</summary>
        public ICommand OpenAddClientCommand { get; set; }
        private void OnAddClientExecuted(object obj) => new AddClientWindow() { Owner = App.Current.MainWindow }.Show();

        /// <summary>Открытие окна редактирования/удаления клиента</summary>
        public ICommand OpenEditClientCommand { get; set; }
        private void OnEditClientExecuted(object obj) => new EditClientWindow() { Owner = App.Current.MainWindow }.Show();

        /// <summary>Открытие окна добавления заявки</summary>
        public ICommand OpenAddRequestCommand { get; set; }
        private void OnAddRequestExecuted(object obj) => new AddRequestWindow() { Owner = App.Current.MainWindow }.Show();

        /// <summary>Открытие окна удаления заявки</summary>
        public ICommand OpenEditRequestCommand { get; set; }
        private void OnEditRequestExecuted(object obj) => new EditRequestWindow() { Owner = App.Current.MainWindow }.Show();

        /// <summary>Синхронизация с базой данных</summary>
        public ICommand SyncCommand { get; set; }
        private void OnSyncExecuted(object obj) => DBWorker.UpdateTables();
        #endregion

        public MainWindowViewModel()
        {
            OpenAddClientCommand = new RelayCommand(OnAddClientExecuted);
            OpenEditClientCommand = new RelayCommand(OnEditClientExecuted); 
            OpenAddRequestCommand = new RelayCommand(OnAddRequestExecuted);
            OpenEditRequestCommand = new RelayCommand(OnEditRequestExecuted);
            SyncCommand = new RelayCommand(OnSyncExecuted);

            DBWorker.CreateTables();
            DBWorker.CreateTriggers();
            DBWorker.GenerateData(5,10);
        }
    }
}
