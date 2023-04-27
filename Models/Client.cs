using System;

namespace ClientRequestHandler.Models
{
    internal class Client
    {
        public string Name { get; set; }
        public string INN { get; set; }
        public string ActivityField { get; set; }
        public int RequestCount { get; set; }
        public DateTime LastRequestDate { get; set; }
        public string Note { get; set; }

    }
}
