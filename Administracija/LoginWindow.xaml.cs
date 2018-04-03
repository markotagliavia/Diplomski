using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace Administracija
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
            if (usernameTextBox.Text.Equals("Korisnicko ime")) usernameTextBox.Text = "";
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
            //TO DO: prijava
        }

        private void labelClick(object sender, MouseButtonEventArgs e)
        {
            //TO DO: zaboravljena lozinka
        }
        #endregion EventsLogic
    }
}
