using ClientRequestHandler.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ClientRequestHandler.Models
{
    internal class Request : ViewModel
    {
        public Request() { }
        public Request(DataRow row)
        {
            Id = (int)row.ItemArray[0];
            ClientId = (int)row.ItemArray[1];
            StartDate = (DateTime)row.ItemArray[2];
            Name = (string)row.ItemArray[3];
            Description = (string)row.ItemArray[4];
            Status = (ProgressStatus)Enum.Parse(typeof(ProgressStatus), ((string)row.ItemArray[5]), true);
        }

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }
        public int ClientId
        {
            get => _ClientId;
            set => Set(ref _ClientId, value);
        }
        public DateTime StartDate
        {
            get => _StartDate;
            set => Set(ref _StartDate, value);
        }
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        public string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
        }
        public ProgressStatus Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        int _Id;
        int _ClientId;
        DateTime _StartDate;
        string _Name;
        string _Description;
        ProgressStatus _Status;

    }

    public enum ProgressStatus : byte
    {
        New,
        InWork,
        Complete
    }
    //public class EnumToStringConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is Enum)
    //        {
    //            string name = Enum.GetName(value.GetType(), value);
    //            FieldInfo field = value.GetType().GetField(name);
    //            if (field != null)
    //            {
    //                ////DisplayAttribute attr = field.GetCustomAttribute<DisplayAttribute>();
    //                //if (attr != null)
    //                //{
    //                //    return attr.Name;
    //                //}
    //            }
    //        }
    //        return value.ToString();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
