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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }

        private void SkladistaSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("skladista");
        }

        private void ZaliheSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("zalihe");
        }

        private void SkladisteniDokumentiSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("skladisteniDokumenti");
        }

        private void ProizvodiSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("proizvodi");
        }

        private void PopisSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("popisi");
        }

        private void ZaposleniSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("zaposleni");
        }

        private void ObavestenjaSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("obavestenja");
        }

        private void NaprednaPretragaSelected(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("naprednaPretraga");
        }
    }
}
