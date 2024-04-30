using DBA_test.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DBA_test.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для StreetsDialog.xaml
    /// </summary>
    public partial class StreetsDialog : Window
    {
        public StreetsDialog()
        {
            DataContext = new StreetsTableViewModel();
            InitializeComponent();
        }
    }
}
