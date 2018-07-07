using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace Racunovodstvo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Korisnik k)
        {
            this.DataContext = MainWindowViewModel.Instance;
            InitializeComponent();
            MainWindowViewModel.Instance.UserOnSession = k;
            MainWindowViewModel.Instance.setUserInformations();
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession = k;
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).setUserInformations();
            //    }
            //}
        }


        private void ProfaktureCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.profakture);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.profakture);
            //    }
            //}
        }

       

        private void ProizvodiCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.proizvodi);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.proizvodi);
            //    }
            //}
        }

        private void KompenzacijaCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.kompenzacije);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.kompenzacije);
            //    }
            //}
        }

        private void PoslovniPartneriCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.poslovniPartneri);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.poslovniPartneri);
            //    }
            //}
        }

        private void ZaliheCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.zalihe);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.zalihe);
            //    }
            //}
        }

        private void NaprednaPretragaCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.naprednaPretraga);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.naprednaPretraga);
            //    }
            //}
        }

        private void ObavestenjaCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.obavestenja);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.obavestenja);
            //    }
            //}
        }

        private void StatistikaCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.statistika);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.statistika);
            //    }
            //}
        }

        private void BilansiCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.bilansi);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav(Navigation.bilansi);
            //    }
            //}
        }

        private void ZaposleniCall(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.zaposleni);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("zaposleni");
            //    }
            //}
        }

        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }


        private void FaktureCall(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.izlazna);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("izlazna");
            //    }
            //}
        }
        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.ulazna);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("ulazna");
            //    }
            //}
        }
        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.storno);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("storno");
            //    }
            //}
        }


    }
    public class DigetTextBox :TextBox
    {
        private static readonly Regex regex = new Regex(@"^\d*(\.\d{0,8})?$");
        public DigetTextBox() { }
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}
