using ClientRequestHandler.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientRequestHandler.ViewModels
{
    internal class ClientWindowViewModel : ViewModel
    {


        DataRow _SelectedClient;
        public object SelectedClient
        {
            get => _SelectedClient;
            set => Set(ref _SelectedClient, (DataRow)(value as DataRowView)?.Row);
        }

        public DataTable Clients
        {
            get
            {
                DataTable dataTable = DBWorker.GetTable("clients");
                dataTable?.Rows.Clear();
                return dataTable;
            }
        }

        public ClientWindowViewModel()
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
