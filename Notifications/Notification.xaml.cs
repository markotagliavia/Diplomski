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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notifications
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window, INotifyPropertyChanged
    {
        #region Fields
        private string _message;

       
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

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }


        public Notification(string text)
        {
            this.Message = text;
            InitializeComponent();
            this.DataContext = this;

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
