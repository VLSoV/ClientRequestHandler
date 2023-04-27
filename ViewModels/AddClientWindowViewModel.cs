using ClientRequestHandler.Data;
using ClientRequestHandler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientRequestHandler.ViewModels
{
    internal class AddClientWindowViewModel : ViewModel
    {
        ObservableCollection<Client> _Clients = new ObservableCollection<Client>();
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
        }

        public AddClientWindowViewModel()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось подключиться к базе данных!");
            }
        }

    }
}
