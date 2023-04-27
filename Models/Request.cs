using System;

namespace ClientRequestHandler.Models
{
    internal class Request
    {
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
