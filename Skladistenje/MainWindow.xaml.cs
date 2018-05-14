using Common.Model;
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

namespace Skladistenje
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
                    ((MainWindowViewModel)((MainWindow)w).DataContext).setUserInformations();
                }
            }
        }

        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }

        private void SkladistaSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("skladista");
                }
            }
        }

        private void ZaliheSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("zalihe");
                }
            }
        }

        private void SkladisteniDokumentiSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("skladisteniDokumenti");
                }
            }
        }

        private void ProizvodiSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("proizvodi");
                }
            }
        }

        private void PopisSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("popisi");
                }
            }
        }

        private void ZaposleniSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("zaposleni");
                }
            }
        }

        private void ObavestenjaSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("obavestenja");
                }
            }
        }

        private void NaprednaPretragaSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("naprednaPretraga");
                }
            }
        }

        private void ProizvodjaciSelected(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("proizvodjaci");
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("spoljni");
                }
            }
        }
        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("interni");
                }
            }
        }
        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("korekcioni");
                }
            }
        }
        private void MenuItem3_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("storno");
                }
            }
        }
    }
}
