using ClientRequestHandler.Commands;
using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientRequestHandler.ViewModels
{
    internal class EditRequestWindowViewModel : ViewModel
    {

        #region SelectedRequest
        Request _SelectedRequest;
        /// <summary>Выбранная строка в таблице заявок</summary>
        public Request SelectedRequest
        {
            get => _SelectedRequest;
            set
            {
                Set(ref _SelectedRequest, value);
            }
        }
        #endregion

        #region Requests
        /// <summary>Таблица клиентов</summary>
        private ObservableCollection<Request> _Requests;
        public ObservableCollection<Request> Requests
        {
            get => DBWorker.Requests();
            set
            {
                _Requests = value;
            }
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
        /// <summary>Удаление выбранного клиента</summary>
        public ICommand DeleteRequestCommand { get; set; }
        private void OnDeleteRequestExecuted(object obj)
        {
            DBWorker.DeleteData(SelectedRequest);
        }
        #endregion

        public EditRequestWindowViewModel()
        {
            DeleteRequestCommand = new RelayCommand(OnDeleteRequestExecuted);
        }
    }
}
