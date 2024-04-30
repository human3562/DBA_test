using DBA_test.Data;
using DBA_test.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DBA_test.Commands;
using System.Windows;
using DBA_test.Dialogs;
using DBA_test.Views;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;

namespace DBA_test.ViewModels;

public class AbonentTableViewModel : ViewModelBase {

    private ObservableCollection<Abonent> test_dataset;

    public ObservableCollection<Abonent> TestDataset {
        get { return test_dataset; }
        set { 
            test_dataset = value;
            OnPropertyChanged("TestDataset");
            OnPropertyChanged("IsDatasetEmpty");
        }
    }

    private string _numberFilter = "";

    public string NumberFilter {
        get { return _numberFilter; }
        set { 
            _numberFilter = value;
            OnPropertyChanged("NumberFilter");
            OnPropertyChanged("IsNumberFiltered");
        }
    }

    public bool IsNumberFiltered => _numberFilter != "";
    public bool IsDatasetEmpty => test_dataset.Count == 0;

    public ICommand SearchCommand { get; }
    public ICommand ExportCommand { get; }
    public ICommand StreetsCommand { get; }
    public ICommand ResetNumberFilter { get; }

    private StreetsDialog? activeStreetsModal = null;

    public AbonentTableViewModel() {

        SearchCommand =  new RelayCommand(new Action<object>(SearchPressed));
        ExportCommand =  new RelayCommand(new Action<object>(ExportPressed));
        StreetsCommand = new RelayCommand(new Action<object>(StreetsPressed));
        ResetNumberFilter = new RelayCommand(new Action<object>(ResetNumberFilterPressed));

        MockDBConnection.Start();
        test_dataset = new ObservableCollection<Abonent>(GetTestData());

    }

    public void SearchPressed(object sender) {
        var window = new FindByNumberDialog();

        bool? result = window.ShowDialog();
        
        if (result ?? false) {
            string number = window.numberTextBox.Text;
            NumberFilter = number;
            TestDataset = new ObservableCollection<Abonent>(FindByNumber(NumberFilter));
        }                
    }
    
    public void ExportPressed(object dg_content) {
        DataGrid? dg = (dg_content as DataGrid) ?? null;
        if (dg == null) return;
        dg.SelectAllCells();
        dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
        ApplicationCommands.Copy.Execute(null, dg);
        dg.UnselectAllCells();
        string csv_to_save = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
        csv_to_save = csv_to_save.Replace('\n', ' ');

        string time = DateTime.Now.ToString().Replace(":", "-");
        string fileName = System.AppDomain.CurrentDomain.BaseDirectory + $"/report_{time}.csv";
        using (FileStream fs = File.Create(fileName)) {
            var data = Encoding.UTF8.GetBytes(csv_to_save);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            fs.Write(result, 0, result.Length);
        }
    }

    public void StreetsPressed(object sender) {
        if (activeStreetsModal != null) {
            activeStreetsModal.Activate();
            return;
        }
        var window = new StreetsDialog();
        activeStreetsModal = window;
        window.Closed += (sender, eventArgs) => activeStreetsModal = null;
        window.Show();
    }

    public void ResetNumberFilterPressed(object sender) {
        NumberFilter = "";
        TestDataset = new ObservableCollection<Abonent>(GetTestData());
    }

    private static IEnumerable<Abonent> FindByNumber(string number) {        
        var connection = MockDBConnection.GetMockConnection();
        if (connection == null) return [];
        return connection.Query<Abonent, Address, Street, PhoneNumber, Abonent>("""
            select 
                a.id as Id,
                a.FullName,
                adr.id as Id,
                adr.House,
                str.id as Id,
                str.Title,
                num.id as Id,
                num.Home,
                num.Work,
                num.Mobile                    
            from PhoneNumber num 
            left join Abonent a on a.PhoneNumber_id = num.id
            left join Address adr on adr.id = a.Address_id
            left join Streets str on str.id = adr.Street_id
            where num.Home = @number or num.Work = @number or num.Home = @number
            """,
            (Abonent, Address, Street, PhoneNumber) => {
                Address.AddressStreet = Street;
                Abonent.AbonentAddress = Address;
                Abonent.AbonentPhoneNumber = PhoneNumber;
                return Abonent;
            }, new { number });
        
    }

    private static IEnumerable<Abonent> GetTestData() {
        var connection = MockDBConnection.GetMockConnection();
        if (connection == null) return [];
        return connection.Query<Abonent, Address, Street, PhoneNumber, Abonent>("""
            select 
                a.id as Id,
                a.FullName,
                adr.id as Id,
                adr.House,
                str.id as Id,
                str.Title,
                num.id as Id,
                num.Home,
                num.Work,
                num.Mobile                    
            from Abonent a
            left join Address adr on adr.id = a.Address_id
            left join Streets str on str.id = adr.Street_id
            left join PhoneNumber num on num.id = a.PhoneNumber_id
            """, 
            (Abonent, Address, Street, PhoneNumber) => {
                Address.AddressStreet = Street;
                Abonent.AbonentAddress = Address;
                Abonent.AbonentPhoneNumber = PhoneNumber;
                return Abonent;
            });
        
    }
}
