using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRequestHandler.Data
{
    internal static class DBWorker
    {
        /// <summary>Строка подключения к базе данных</summary>
        static string ConnectionString = "server=localhost;user=root;database=test1;password=root;";

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

        public static void InsertData(DataTable data, string tableName)
        {
            string names = data.Columns.ToString();   
            string sqlQuery = $"INSERT INTO {tableName}({names}) VALUES ";
            foreach(DataRowCollection row in data.Rows)
            {
                sqlQuery += row.ToString();
            }

            //MySqlConnection connection = new MySqlConnection(ConnectionString);
            //connection.Open();
            //new MySqlCommand($"INSERT INTO {tableName}(Name,INN,ActivityField,RequestCount,LastRequestDate,Note) " +
            //    $"VALUES ('Ivan','5956008835','ajfdgjhah',DEFAULT,NOW(),'4dfjthhber');", connection).ExecuteNonQuery();
            //connection.Close();
        }

        # region CreateData
        /// <summary>Созданиетаблиц и триггеров в бд</summary>
        public static void CreateData()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();

            new MySqlCommand(@"
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
                ); ", connection).ExecuteNonQuery();


            new MySqlCommand(@"
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
                END;
                ", connection).ExecuteNonQuery();
            //samples
            new MySqlCommand(@"
                INSERT INTO clients(Name,INN,ActivityField,RequestCount,LastRequestDate,Note) 
                VALUES
                ('Mike','5523157125','aehkfgkah',DEFAULT,NOW(),'faeahreber'),
                ('Ivan','5956008835','ajfdgjhah',DEFAULT,NOW(),'4dfjthhber');

                INSERT INTO requests(ClientId,StartDate,Name,Description,Status) 
                VALUES
                (1,'2022-01-01 00:00:00.000000','work1','daeuajstrkiswu','New'),
                (2,'2022-01-01 01:00:00.000000','work2','yae3w4ausrnwta','New'),
                (2,'2024-01-01 02:00:00.000000','work3','aw4myay4weyawe','New');
                ", connection).ExecuteNonQuery();

            connection.Close();
        }
        #endregion
    }
}
