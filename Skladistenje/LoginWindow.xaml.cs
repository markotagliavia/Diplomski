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

namespace Skladistenje
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _firmColor;
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _lightFirmColor;

        

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
            c2 = System.Windows.Media.Color.FromArgb(255, 100, 105, 245);
            FirmColor = new SolidColorBrush(c1);
            InitializeComponent();
            this.DataContext = this;
        }

        #region Constructors Block
        public Brush FirmColor {
            get { return _firmColor; }
            set
            {
                _firmColor = value;
                OnPropertyChanged("FirmColor");
            }
        }

        public Brush LightFirmColor {
            get { return _lightFirmColor; }
            set
            {
                _lightFirmColor = value;
                OnPropertyChanged("LightFirmColor");
            }
        }
        #endregion


        private void usernameTextBoxFocus(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(1), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = c1;
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(1));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat1);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void usernameTextBoxUnFocus(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard() { Duration = TimeSpan.FromSeconds(1), BeginTime = TimeSpan.Zero };

            ColorAnimation colAnim = new ColorAnimation();
            colAnim.To = System.Windows.Media.Color.FromArgb(255,177,177,177);
            colAnim.Duration = new Duration(TimeSpan.FromSeconds(1));
            colAnim.AutoReverse = false;

            sb.Children.Add(colAnim);

            Storyboard.SetTarget(colAnim, kvadrat1);
            Storyboard.SetTargetProperty(colAnim, new PropertyPath("Fill.Color"));
            sb.Begin();
        }

        private void passwordBoxFocus(object sender, RoutedEventArgs e)
        {

        }

        private void passwordBoxUnFocus(object sender, RoutedEventArgs e)
        {

        }

        private void prijavaMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void prijavaMouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
