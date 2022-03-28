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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        BlApi.IBL bL;
        public StationListWindow(IBL bl)
        {
            this.bL = bl;
            InitializeComponent();
            StationsListView.ItemsSource = bL.ShowStationList().ToList();
        }

        private void Availble_charging_spote_station_Click(object sender, RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bL.ShowStationList().Where(x => x.AvailableChargingSpots > 0);
        }
        /// <summary>
        /// double clicking on a station from the list eill lead you to update the station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList? station = StationsListView.SelectedItem as StationToList;
            if (station != null)
            {
                new StationInActionView(station, bL, this).Show();
            }
        }
        /// <summary>
        /// add station::::::::::::::::::
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            new StationInActionView(this, bL).Show();
        }
        /// <summary>
        /// close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// refresh the list view that shows the drones information
        /// </summary>
        public void RefreshStationListView()
        {
            this.StationsListView.ItemsSource = bL.ShowStationList();
        }
        /// <summary>
        /// refresh the list view of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshWindow(object sender, EventArgs e)
        {
            RefreshStationListView();
        }
    }
}
