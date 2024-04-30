using Dapper;
using DBA_test.Data;
using DBA_test.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBA_test.ViewModels;

public class StreetsTableViewModel : ViewModelBase
{

    private ObservableCollection<StreetInfo> _dataset;

    public ObservableCollection<StreetInfo> Dataset {
        get { return _dataset; }
        set {
            _dataset = value;
            OnPropertyChanged("Dataset");
        }
    }

    public StreetsTableViewModel()
    {
        _dataset = new ObservableCollection<StreetInfo>(GetStreetsInfo());
        Console.WriteLine("hello world");
    }


    private static IEnumerable<StreetInfo> GetStreetsInfo() {
        var connection = MockDBConnection.GetMockConnection();
        if (connection == null) return [];
        return connection.Query<StreetInfo>("""
            select 
                str.Title as StreetName,
                (Select COUNT(*) from Abonent a where (select adr.Street_id from Address adr where a.Address_id = adr.id) = str.id) as NumAbonents                   
            from Streets str
            """);

    }

}
