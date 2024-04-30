using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    /// Логика взаимодействия для FindByNumberDialog.xaml
    /// </summary>
    public partial class FindByNumberDialog : Window
    {
        public FindByNumberDialog()
        {
            InitializeComponent();

            numberTextBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}
