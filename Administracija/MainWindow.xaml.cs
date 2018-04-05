using Administracija.Model;
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
        public MainWindow(Korisnik k)
        {
            InitializeComponent();
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession = k;
                }
            }           
        }

        private void auditMethodCall(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("audit");
                }
            }
        }

        private void naprednaPretragaCall(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("advancedSearch");
                }
            }
        }

        private void pregledUlogaCall(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("previewRoles");
                }
            }
        }

        private void pregledKorisnikaCall(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("previewUsers");
                }
            }
        }

        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }
    }
}
