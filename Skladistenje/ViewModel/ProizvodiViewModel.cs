﻿using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Skladistenje.ViewModel
{
    public class ProizvodiViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu i komande brisanja i pretrage
        #region Commands
        public MyICommand<string> DodajProizvodNavCommand { get; private set; }
        public MyICommand<string> IzmeniProizvodNavCommand { get; private set; }
        public MyICommand<string> IzbrisiProizvodCommand { get; private set; }
        public MyICommand<string> PretraziProizvodCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Proizvod> proizvodi;
        private Proizvod selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public ProizvodiViewModel()
        {
            DodajProizvodNavCommand = new MyICommand<string>(DodajProizvodNav);
            IzmeniProizvodNavCommand = new MyICommand<string>(IzmeniProizvodNav);
            IzbrisiProizvodCommand = new MyICommand<string>(IzbrisiProizvod);
            PretraziProizvodCommand = new MyICommand<string>(PretraziProizvod);
            textSearch = "";
            proizvodi = new ObservableCollection<Proizvod>();
            foreach (var item in dbContext.Proizvods.ToList())
            {
                proizvodi.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Proizvodi);
        }

        #region Constructors

        public ObservableCollection<Proizvod> Proizvodi
        {
            get { return proizvodi; }
            set { proizvodi = value; }
        }

        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }

        public bool SelectedInGrid
        {
            get { return selectedInGrid; }
            set
            {
                selectedInGrid = value;
                OnPropertyChanged("SelectedInGrid");
            }
        }

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex == value)
                {
                    if (_selectedIndex > -1)
                    {
                        SelectedInGrid = true;
                    }
                    return;
                }
                _selectedIndex = value;
                if (_selectedIndex > -1)
                {
                    SelectedInGrid = true;
                }
            }
        }

        public Proizvod SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }

        public Korisnik UserOnSession
        {
            get { return userOnSession; }
            set { userOnSession = value; }
        }
        #endregion

        #region CommandsImplementation
        private void PretraziProizvod(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Šifri"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvod)w).sifra.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvod)w).naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Jedinici mere"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvod)w).jedinicamere.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Proizvođaču"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvod)w).Proizvodjac.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Minimumu količine"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvod)w).minimumkolicine.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Proizvodi);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Proizvodi);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void IzbrisiProizvod(string obj)
        {
            /*TO DO 
             * foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                }
            }

            if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteUser))
            {
                string korisnickoImeBrisanog = "";
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession.korisnickoime = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime;
                    }
                }

                if (SelectedIndex > -1)
                {
                    korisnickoImeBrisanog = SelectedValue.KorisnickoIme;
                    if (dbContext.Korisniks.Any(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)))
                    {
                        dbContext.Korisniks.Remove(dbContext.Korisniks.First(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)));
                        dbContext.SaveChanges();
                        Success suc = new Success("Uspešno ste obrisali korisnika.");
                        suc.Show();

                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje korisnika {korisnickoImeBrisanog}.", "Info");
                    }
                    else
                    {
                        Error er = new Error("Greška pri pronalaženju korisnika.\nZa više informacija obratite se administratorima.");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje korisnika {korisnickoImeBrisanog}.", "Upozorenje");
                    }
                }



            }
            else
            {

                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Pokusaj brisnja korisnika", "Upozorenje");

            }*/
        }

        private void IzmeniProizvodNav(string obj)
        {
            if (SelectedValue != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditProizvod))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel = new DodajProizvodViewModel(1, selectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvod");
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Izmena Proizvoda";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspšan pokušaj izmene proizvoda.", "Upozorenje");
                        }

                    }
                }
            }
            else
            {
                Error er = new Error("Greška pri selekciji.\nZa više informacija obratite se administratorima.");
                er.Show();
                //SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspesna izmena korisnika.", "Upozorenje");
            }
        }

        private void DodajProizvodNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddProizvod))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel = new DodajProizvodViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvod");
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Novi Proizvod";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešan pokušaj dodavanja novog proizvoda", "Upozorenje");
                    }
                }
            }
        }
        #endregion
    }
}
