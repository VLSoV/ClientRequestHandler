using System;
using System.Data;

namespace ClientRequestHandler.Models
{
    internal class Request
    {
        public Request() { }
        public Request(DataRow row)
        {
            Id = (int)row.ItemArray[0];
            ClientId = (int)row.ItemArray[1];
            StartDate = (DateTime)row.ItemArray[2];
            Name = (string)row.ItemArray[3];
            Description = (string)row.ItemArray[4];
            Status = (ProgressStatus)Enum.Parse(typeof(ProgressStatus), ((string)row.ItemArray[5]));
        }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProgressStatus Status { get; set; }

    }

    enum ProgressStatus : byte
    {
        New,
        InWork,
        Complete
    }
}
