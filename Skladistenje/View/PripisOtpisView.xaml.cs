using Common.Model;
using Skladistenje.ViewModel;
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
using System.Windows.Shapes;

namespace Skladistenje.View
{
    /// <summary>
    /// Interaction logic for PripisOtpisView.xaml
    /// </summary>
    public partial class PripisOtpisView : Window
    {
        public PripisOtpisView(int i, int id)
        {
            InitializeComponent();
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(PripisOtpisView)))
                {
                    this.DataContext = new PripisOtpisViewModel(id);
                }
            }
        }
    }
}
