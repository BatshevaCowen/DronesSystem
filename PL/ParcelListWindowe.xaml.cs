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
    /// Interaction logic for ParcelListWindowe.xaml
    /// </summary>
    public partial class ParcelListWindowe : Window
    {
        BlApi.IBL myBL;
        /// <summary>
        /// Constractor- shows the all of the parcels- FOR ADMIN
        /// </summary>
        /// <param name="bl"></param>
        public ParcelListWindowe(BlApi.IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            this.ParcelListView.ItemsSource = myBL.ShowParcelList();
        }

        /// <summary>
        /// Constractor- shows the parcels of the specific USER (after signing in)
        /// </summary>
        /// <param name="bl"></param>
        public ParcelListWindowe(BlApi.IBL bl, User user)
        {
            myBL = bl;
            InitializeComponent();
            this.ParcelListView.ItemsSource = myBL.ShowParcelList(user);
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelToList? parTL = ParcelListView.SelectedItem as ParcelToList;
            if (parTL != null)
            {
                new ParcelInActionView(parTL, myBL, this).Show();
            }
        }
        /// <summary>
        /// A button to add a new parcel
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelInActionView( myBL).Show();
        }
        /// <summary>
        /// A button that refresh the list of parcels order by the reciver
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void RefreshBaseParcelTargetButton_Click(object sender, RoutedEventArgs e)
        {
            ParcelListView.ItemsSource = myBL.ShowParcelList().OrderBy(p => p.ReciverName);
            ParcelListView.Items.Refresh();
        }

        /// <summary>
        /// BONUS-- Show deleted parcels by flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDeletedParcels_Click(object sender, RoutedEventArgs e)
        {
            //לממש!!!!
        }

        /// <summary>
        /// double click on parcel in the list will open the ParcelInActionView Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInActionView? parcelInActionView = ParcelListView.SelectedItem as ParcelInActionView;
            if (parcelInActionView != null)
                parcelInActionView.Show();
        }

        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
