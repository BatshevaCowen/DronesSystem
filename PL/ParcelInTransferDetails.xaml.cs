using BlApi;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInTransferDetails.xaml
    /// </summary>
    public partial class ParcelInTransferDetails : Window
    {
        public ParcelInTransferDetails(ParcelInTransfer pInT)
        {
           
            InitializeComponent();
            DataContext = pInT;
        }
    }
}
