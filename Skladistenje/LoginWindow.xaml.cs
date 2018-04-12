using Common.Model;
using MahApps.Metro.Controls;
using Notifications;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Skladistenje
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow, INotifyPropertyChanged
    {
        #region Fields
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _firmColor;
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _lightFirmColor;
        private System.Windows.Media.Color c3;
        private System.Windows.Media.Brush _backgroundColor;
        private System.Windows.Media.Brush _buttonColor;
        private System.Windows.Media.Brush _labelColor;
        private DeltaEximEntities dbContext = new DeltaEximEntities();
        #endregion

        #region INotifiedProperty Block
        protected void OnPropertyChanged(string porpName)
        {
            var temp = PropertyChanged;
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(porpName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        public LoginWindow()
        {
            c1 = System.Windows.Media.Color.FromArgb(255, 68, 95, 245);
            c2 = System.Windows.Media.Color.FromArgb(255, 128, 170, 255);
            c3 = System.Windows.Media.Color.FromArgb(255, 204, 179, 255);
            FirmColor = new SolidColorBrush(c1);
            LightFirmColor = new SolidColorBrush(c2);
            BackgroundColor = new SolidColorBrush(c3);
            ButtonColor = new SolidColorBrush(c1);
            LabelColor = new SolidColorBrush(c1);
            InitializeComponent();
            this.DataContext = this;
        }

        #region Constructors Block
        public Brush FirmColor
        {
            get { return _firmColor; }
            set
            {
                _firmColor = value;
                OnPropertyChanged("FirmColor");
            }
        }

        public Brush LightFirmColor
        {
            get { return _lightFirmColor; }
            set
            {
                _lightFirmColor = value;
                OnPropertyChanged("LightFirmColor");
            }
        }

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public Brush ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                OnPropertyChanged("ButtonColor");
            }
        }

        public Brush LabelColor
        {
            get { return _labelColor; }
            set
            {
                _labelColor = value;
                OnPropertyChanged("LabelColor");
            }
        }
        #endregion

        #region EventsColors
        private void usernameTextBoxFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals("Korisničko ime")) usernameTextBox.Text = "";
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(0.7), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = c1;
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat1);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void usernameTextBoxUnFocus(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(0.7), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = System.Windows.Media.Color.FromArgb(255, 177, 177, 177);
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat1);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void passwordBoxFocus(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(0.7), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = c1;
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat2);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void passwordBoxUnFocus(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(0.7), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = System.Windows.Media.Color.FromArgb(255, 177, 177, 177);
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat2);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void prijavaMouseEnter(object sender, MouseEventArgs e)
        {
            ButtonColor = LightFirmColor;
        }

        private void prijavaMouseLeave(object sender, MouseEventArgs e)
        {
            ButtonColor = FirmColor;
        }

        private void labelEnter(object sender, MouseEventArgs e)
        {
            LabelColor = LightFirmColor;
        }

        private void labelLeave(object sender, MouseEventArgs e)
        {
            LabelColor = FirmColor;
        }
        #endregion

        #region EventsLogic
        private void prijaviSe(object sender, RoutedEventArgs e)
        {
            string inputUsername = usernameTextBox.Text;
            Korisnik k = new Korisnik();
            k.korisnickoime = "";
            string inputPassword = Encryption.sha256(passBox.Password);

            if (dbContext.Korisniks.Any(p => p.korisnickoime.Equals(inputUsername)))
            {
                k = dbContext.Korisniks.First(p => p.korisnickoime.Equals(inputUsername));
                if (SecurityManager.AuthorizationPolicy.HavePermission(k.id, SecurityManager.Permission.LoginSkladistenje))
                {
                    if (k.lozinka.Equals(inputPassword))
                    {

                        if (dbContext.Korisniks.First(p => p.korisnickoime.Equals(inputUsername)).ulogovan)
                        {
                            Error er = new Error("Korisnik je već ulogovan!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(k.korisnickoime, "Neuspesna autentifikacija. Korisnik je vec ulogovan.", "Upozorenje");
                        }
                        else
                        {
                            dbContext.Korisniks.First(p => p.korisnickoime.Equals(inputUsername)).ulogovan = true;
                            dbContext.SaveChanges();
                            SecurityManager.AuditManager.AuditToDB(k.korisnickoime, "Uspesna autentifikacija. Korisnik je uspesno ulogovan", "Info");
                            MainWindow mw = new MainWindow(k);
                            mw.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        Error er = new Error("Uneta lozinka je pogrešna!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesna autentifikacija. Pogresna lozinka.", "Upozorenje");
                    }
                }
                else
                {
                    Error er = new Error("Nemate ovlaćšenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesna autorizacija. Nedozvoljena autentifikacija.", "Upozorenje");
                }
            }
            else
            {
                Error er = new Error("Ne postoji uneto korisničko ime!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesna autentifikacija. Nepostojece korisnicko ime.", "Upozorenje");
            }
        }

        private void labelClick(object sender, MouseButtonEventArgs e)
        {
            string inputUsername = usernameTextBox.Text;

            Korisnik k = new Korisnik();
            k.korisnickoime = "";

            if (dbContext.Korisniks.Any(p => p.korisnickoime.Equals(inputUsername)))
            {
                k = dbContext.Korisniks.First(p => p.korisnickoime.Equals(inputUsername));
                if (SecurityManager.AuthorizationPolicy.HavePermission(k.id, SecurityManager.Permission.ResetPassword))
                {
                    if (dbContext.Zaposlenis.Any(p => p.id.Equals(k.id)))
                    {
                        Zaposleni z = dbContext.Zaposlenis.First(p => p.id.Equals(k.zaposleni_id));
                        string email_to = z.email;
                        string email_from = "deltaexim021@gmail.com";
                        string email_from_sifra = "slavija22";
                        string new_pass = Guid.NewGuid().ToString().Substring(0, 10);
                        try
                        {
                            SmtpClient client = new SmtpClient();
                            client.Port = 587;
                            client.Host = "smtp.gmail.com";
                            client.EnableSsl = true;
                            client.Timeout = 10000;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential(email_from, email_from_sifra);

                            MailMessage mm = new MailMessage(email_from, email_to);
                            mm.BodyEncoding = UTF8Encoding.UTF8;
                            mm.Subject = @"Resetovana lozinka [DELTAEXIM]";
                            mm.Body = "Postovani,\n\nAko Vi niste inicirali reset lozinke, hitno se obratite administratorima.\n\nVasa nova lozinka je : " + new_pass + "\n\nSrdacan pozdrav,\nDELTAEXIM Admin tim";
                            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                            client.Send(mm);
                            string nova_lozinka_hash = SecurityManager.Encryption.sha256(new_pass);
                            dbContext.Korisniks.First(p => p.korisnickoime.Equals(inputUsername)).lozinka = nova_lozinka_hash;
                            dbContext.SaveChanges();
                            Success sc = new Success("Lozinka je uspešno promenjena!");
                            sc.Show();
                            SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Uspesno resetovanje lozinke.", "Info");
                        }
                        catch (Exception ex)
                        {
                            Error er = new Error("Problemi sa konekcijom!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesno resetovanje lozinke. Email nije poslat.", "Greska");
                        }
                    }
                }
                else
                {
                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesno resetovanje lozinke.", "Upozorenje");
                }
            }
            else
            {
                Error er = new Error("Ne postoji uneto korisničko ime!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(usernameTextBox.Text, "Neuspesno resetovanje lozinke. Nepostojece korisnicko ime.", "Upozorenje");
            }
        }
        #endregion EventsLogic
    }
}
