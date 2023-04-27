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

namespace ClientRequestHandler.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region CurrentNote
        /// <summary>Примечание к выбранному клиенту</summary>
        string _CurrentNote;
        /// <summary>Примечание к выбранному клиенту</summary>
        public string CurrentNote 
        { 
            get => _CurrentNote; 
            set => Set(ref _CurrentNote, value);
        }
        #endregion

        #region SelectedClient
        /// <summary>Выбранная строка в таблице клиентов</summary>
        DataRow _SelectedClient;
        /// <summary>Выбранная строка в таблице клиентов</summary>
        public object SelectedClient
        {
            get => _SelectedClient;
            set
            {
                Set(ref _SelectedClient, (DataRow)(value as DataRowView)?.Row);
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
        /// <summary>Таблица клиентов</summary>
        public DataTable Clients
        {
            get => DBWorker.GetTable("clients", "", "ORDER BY Name");
        }
        #endregion

        #region Requests
        /// <summary>Таблица заявок</summary>
        public DataTable Requests
        {
            get => DBWorker.GetTable("requests", (_SelectedClient is null) ? "" :  $"WHERE ClientId = {_SelectedClient["Id"]}", "ORDER BY StartDate DESC");
        }
        #endregion

        public MainWindowViewModel()
        {
            try
            {
                DBWorker.CreateData();


                ClientWindow clientWindow = new ClientWindow();
                clientWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось подключиться к базе данных!");
            }
        }
    }
}
