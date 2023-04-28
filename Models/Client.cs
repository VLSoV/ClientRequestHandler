using System;
using System.Data;

namespace ClientRequestHandler.Models
{
    internal class Client
    {
        public Client() { }
        public Client(DataRow row)
        {
            Id = (int)row.ItemArray[0];
            Name = (string)row.ItemArray[1];
            INN = (string)row.ItemArray[2];
            ActivityField = (string)row.ItemArray[3];
            RequestCount = (int)row.ItemArray[4];
            LastRequestDate = (DateTime)row.ItemArray[5];
            Note = (string)row.ItemArray[6];
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string INN { get; set; }
        public string ActivityField { get; set; }
        public int RequestCount { get; set; }
        public DateTime LastRequestDate { get; set; }
        public string Note { get; set; }

    }
}
