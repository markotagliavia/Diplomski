﻿using System;
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

namespace Racunovodstvo
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

        private void ProfaktureCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("profakture");
        }

        private void FaktureCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("fakture");
        }

        private void ProizvodiCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("proizvodi");
        }

        private void KompenzacijaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("kompenzacije");
        }

        private void PoslovniPartneriCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("poslovnipartneri");
        }

        private void ZaliheCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("zalihe");
        }

        private void NaprednaPretragaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("naprednaPretraga");
        }

        private void ObavestenjaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("obavestenja");
        }

        private void StatistikaCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("statistika");
        }

        private void BilansiCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("bilansi");
        }

        private void ZaposleniCall(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).OnNav("zaposleni");
        }

        private void deselect(object sender, RoutedEventArgs e)
        {
            listView.SelectedIndex = -1;
        }
    }
}
