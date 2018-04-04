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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Administracija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void auditMethodCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("audit");
        }

        private void naprednaPretragaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("advancedSearch");
        }

        private void pregledUlogaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("previewRoles");
        }

        private void pregledKorisnikaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("previewUsers");
        }

        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }
    }
}
