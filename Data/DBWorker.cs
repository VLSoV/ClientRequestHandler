using ClientRequestHandler.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientRequestHandler.Data
{
    internal static class DBWorker
    {
        /// <summary>Строка подключения к базе данных</summary>
        static string ConnectionString = "server=localhost;user=root;database=test1;password=root;";

        #region Clients
        private static ObservableCollection<Client> _Clients = new ObservableCollection<Client>();
        /// <summary>Таблица клиентов</summary>
        public static ObservableCollection<Client> Clients
        {
            get
            {
                DataTable dt = DBWorker.GetTable("clients", "", "ORDER BY Name");
                _Clients.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    _Clients.Add(new Client(row));
                }
                return _Clients;
            }
        }
        #endregion

        #region Requests
        private static ObservableCollection<Request> _Requests = new ObservableCollection<Request>();
        /// <summary>Таблица заявок</summary>
        public static ObservableCollection<Request> Requests(Client selectedClient = null)
        {
            DataTable dt = DBWorker.GetTable("requests", (selectedClient is null) ? "" : $"WHERE ClientId = {selectedClient.Id}", "ORDER BY StartDate DESC");
            _Requests.Clear();
            foreach (DataRow row in dt.Rows)
            {
                _Requests.Add(new Request(row));
            }
            return _Requests;
        }
        #endregion

        #region GetTable
        /// <summary>Загрузить таблицу из базы данных</summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="whereStr">Фильтр WHERE(если есть)</param>
        /// <param name="orderByStr">Сортировка ORDER BY(если есть)</param>
        /// <returns>Требуемая таблица</returns>
        public static DataTable GetTable(string tableName, string whereStr = null, string orderByStr = null)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM {tableName} {whereStr} {orderByStr};", connection);
            DataTable dataTable = new DataTable();
            dataTable.Load(command.ExecuteReader());
            connection.Close();
            return dataTable;
        }
        #endregion

        #region SendQuery
        /// <summary>Отправить запрос на сервер</summary>
        /// <param name="sqlQuery">Запрос к базе данныхс</param>
        public static void SendQuery(string sqlQuery)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();
                new MySqlCommand(sqlQuery.ToString(), connection).ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось подключиться к базе данных!");
            }
        }
        #endregion


        #region UpdateTables
        /// <summary>Синхронизировать таблицы с базой данных</summary>
        public static void UpdateTables()
        {
            var updClients = Clients;
            var updRequests = Requests();
        }
        #endregion

        #region InsertData
        /// <summary>Вставить данные в таблицу базы данных</summary>
        /// <param name="data">Набор объектов с данными</param>
        public static void InsertData(IEnumerable<object> data)
        {
            if (data == null || !data.Any()) return;

            string tableName;
            if (data.First() is Client) tableName = "clients";
            else if (data.First() is Request) tableName = "requests";
            else throw new ArgumentException("Неизветная модель данных!");

            StringBuilder sqlQuery = new StringBuilder($"INSERT INTO {tableName} (");  
            foreach(PropertyInfo prop in data.First().GetType().GetProperties().OrderBy(x => x.MetadataToken))
            {
                sqlQuery.Append(prop.Name + ',');
            }
            sqlQuery.Length--;
            sqlQuery.Append(") VALUES ");

            foreach (object dataInstance in data)
            {
                sqlQuery.Append('(');
                foreach (PropertyInfo prop in dataInstance.GetType().GetProperties().OrderBy(x => x.MetadataToken))
                {
                    if (prop.PropertyType == typeof(string)) 
                        sqlQuery.Append('\"' + prop.GetValue(dataInstance)?.ToString() + "\",");

                    else if (prop.PropertyType == typeof(int)) 
                        sqlQuery.Append(prop.GetValue(dataInstance)?.ToString() + ',');

                    else if (prop.PropertyType == typeof(DateTime)) 
                        sqlQuery.Append(((DateTime)prop.GetValue(dataInstance)).ToString("'\"'yyyy'-'MM'-'dd HH':'mm':'ss'\",'")); //'2022-01-01 00:00:00.000000'

                    else throw new ArgumentException("Неопределен тип свойства!");
                }
                sqlQuery.Length--;
                sqlQuery.Append("),");
            }
            sqlQuery.Length--;
            sqlQuery.Append(";");

            SendQuery(sqlQuery.ToString());
            UpdateTables();
        }
        #endregion

        #region DeleteData
        /// <summary>Удалить строку из базы данных</summary>
        /// <param name="instance">Объект данных</param>
        public static void DeleteData(object instance)
        {
            if (instance == null) return;

            string tableName;
            if (instance is Client) tableName = "clients";
            else if (instance is Request) tableName = "requests";
            else throw new ArgumentException("Неизветная модель данных!");
            string id = instance.GetType().GetProperties().Where(x => x.Name == "Id").First().GetValue(instance).ToString();

            string sqlQuery = $"DELETE FROM {tableName} WHERE Id={id}";

            SendQuery(sqlQuery.ToString());
            UpdateTables();
        }
        #endregion

        #region UodateData
        /// <summary>Изменить строку в базе данных</summary>
        /// <param name="instance">Объект данных</param>
        public static void UpdateData(object instance)
        {
            if (instance == null) return;

            string tableName;
            if (instance is Client) tableName = "clients";
            else if (instance is Request) tableName = "requests";
            else throw new ArgumentException("Неизветная модель данных!");
            string id = instance.GetType().GetProperties().Where(x => x.Name == "Id").First().GetValue(instance).ToString();

            StringBuilder sqlQuery = new StringBuilder($"UPDATE {tableName} SET ");
            foreach (PropertyInfo prop in instance.GetType().GetProperties().OrderBy(x => x.MetadataToken))
            {
                if (prop.Name == "Id") continue;

                if (prop.PropertyType == typeof(string)) 
                    sqlQuery.Append($"{prop.Name} = \"{prop.GetValue(instance)?.ToString()}\",");

                else if (prop.PropertyType == typeof(int)) 
                    sqlQuery.Append($"{prop.Name} = {prop.GetValue(instance)?.ToString()},");

                else if (prop.PropertyType == typeof(DateTime)) 
                    sqlQuery.Append($"{prop.Name} = {((DateTime)prop.GetValue(instance)).ToString("'\"'yyyy'-'MM'-'dd HH':'mm':'ss'\",'")}"); //'2022-01-01 00:00:00.000000'

                else throw new ArgumentException("Неопределен тип свойства!");
            }
            sqlQuery.Length--;
            sqlQuery.Append($" WHERE Id={id};");

            SendQuery(sqlQuery.ToString());
            UpdateTables();
        }
        #endregion

        #region CreateData
        /// <summary>Создание таблиц и триггеров в бд</summary>
        public static void CreateData()
        {
            string createTablesQuery = @"
                DROP TABLE IF EXISTS requests;

                DROP TABLE IF EXISTS clients;

                CREATE TABLE clients
                (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(256) NOT NULL,
                INN VARCHAR(20) NOT NULL,
                ActivityField VARCHAR(256),
                RequestCount INT DEFAULT 0,
                LastRequestDate DATETIME,
                Note TEXT
                ); 

                CREATE TABLE requests
                (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                ClientId INT NOT NULL,
                StartDate DATETIME,
                Name VARCHAR(256) NOT NULL,
                Description TEXT,
                Status ENUM('New','InWork','Complete'),
                FOREIGN KEY (ClientId) REFERENCES clients(Id) ON DELETE RESTRICT
                ); ";

            string createTriggersQuery = @"
                DROP TRIGGER IF EXISTS AddRequest;
                
                DROP TRIGGER IF EXISTS DeleteRequest;

                CREATE TRIGGER AddRequest
                BEFORE INSERT
                ON requests FOR EACH ROW

                BEGIN
					DECLARE max_date DATETIME;

                    SELECT MAX(LastRequestDate)
                    INTO max_date
                    FROM clients
                    WHERE Id = NEW.ClientId;
                    
                    IF max_date < NEW.StartDate THEN
						UPDATE clients
						SET LastRequestDate = NEW.StartDate
						WHERE Id = NEW.ClientId;
                    END IF;
                    
                    UPDATE clients
                    SET RequestCount = RequestCount+1
                    WHERE Id = NEW.ClientId;
                END;

                CREATE TRIGGER DeleteRequest
                BEFORE DELETE
                ON requests FOR EACH ROW

                BEGIN                    
                    UPDATE clients
                    SET RequestCount = RequestCount-1
                    WHERE Id = OLD.ClientId;
                END;";

            string AddSamplesQuery = @"
                INSERT INTO clients(Name,INN,ActivityField,RequestCount,LastRequestDate,Note) 
                VALUES
                ('Mike','5523157125','aehkfgkah',DEFAULT,NOW(),'faeahreber'),
                ('Ivan','5956008835','ajfdgjhah',DEFAULT,NOW(),'4dfjthhber');

                INSERT INTO requests(ClientId,StartDate,Name,Description,Status) 
                VALUES
                (1,'2022-01-01 00:00:00.000000','work1','daeuajstrkiswu','New'),
                (2,'2022-01-01 01:00:00.000000','work2','yae3w4ausrnwta','New'),
                (2,'2024-01-01 02:00:00.000000','work3','aw4myay4weyawe','New');
                ";

            SendQuery(createTablesQuery);
            SendQuery(createTriggersQuery);
            SendQuery(AddSamplesQuery);
        }
        #endregion
    }
}
