using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StationInActionView.xaml
    /// </summary>
    public partial class StationInActionView : Window
    {
        BO.Station station;
        private StationToList? StationToList;
        IBL mybl;
        public StationInActionView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// station view - UPDATE
        /// </summary>
        /// <param name="stationToL"></param>
        /// <param name="bL"></param>
        /// <param name="stationListWindow"></param>
        public StationInActionView(StationToList stationToL, IBL bL, StationListWindow stationListWindow)
        {
            InitializeComponent();
            mybl = bL;
            DataContext = stationToL;
            this.StationToList = stationToL;
            UpdateGrid.Visibility = Visibility.Visible;
            btnClose.Visibility = Visibility.Visible;
            station = new()
            {
                Id = stationToL.Id,
                Name = stationToL.Name,
                AvailableChargingSpots = stationToL.AvailableChargingSpots
            };

            station.droneInChargings = mybl.GetStation(stationToL.Id).droneInChargings;

            if (station.droneInChargings.Count() > 0)
            {
                lable_droneincharging.Visibility = Visibility.Visible;
                foreach (DroneInCharging item in station.droneInChargings)
                {
                    DroneInCharge_ListView.Visibility = Visibility.Visible;
                    ListViewItem newItem = new ListViewItem();
                    newItem.Content = item;
                    DroneInCharge_ListView.Items.Add(newItem);
                }
            }
            else
            {
                DroneInCharge_ListView.Visibility = Visibility.Collapsed;
            }
            DataContext = station;

        }
        /// <summary>
        /// Add station view
        /// </summary>
        /// <param name="stationListWindow"></param>
        /// <param name="bL"></param>
        public StationInActionView(StationListWindow stationListWindow, IBL bL)
        {
            mybl = bL;
            InitializeComponent();
            btnClose2.Visibility = Visibility.Visible;
            AddGridStation.Visibility = Visibility.Visible;
            //LocationGridStation.Visibility= Visibility.Visible;
            DataContext = station;
            stationListWindow.StationsListView.Items.Refresh();
        }
        /// <summary>
        /// update station button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateStation_Click(object sender, RoutedEventArgs e)
        {
            String StationName = mybl.GetStation(Int32.Parse(stationIdTextBox.Text)).Name;
            string newName = NameTextBox.Text;
            //only if the name has changed by the user
            if (StationName != newName)
            {
                mybl.UpdateStetion(Int32.Parse(stationIdTextBox.Text), newName, Int32.Parse(AvailableChargingSpotsTextBox.Text));
                MessageBox.Show("Station updated seccessfuly!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please update the Station's name");
            }
        }

        /// <summary>
        /// click on button add station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddeStation_Click(object sender, RoutedEventArgs e)
        {
            //all fields shouldn't be empty
            if (stationIdTextBoxadd.Text.Length == 0)
                MessageBox.Show("Please enter station ID");
            else if (NameTextBoxadd.Text.Length == 0)
                MessageBox.Show("Please enter station name");
            else if (AvailableChargingSpotsTextBoxadd.Text.Length == 0)
                MessageBox.Show("Please enter number of available charging spots");
            else if (LongitudeTxtBox.Text.Length == 0)
                MessageBox.Show("Please enter longitude");
            else if (LatitudeTxtBox.Text.Length == 0)
                MessageBox.Show("Please enter latitude");
            else
            {
                Station station = new Station()
                {
                    Id = Int32.Parse(stationIdTextBoxadd.Text),
                    Name = (NameTextBoxadd.Text),
                    AvailableChargingSpots = Int32.Parse(AvailableChargingSpotsTextBoxadd.Text),
                };
                station.Location = new()
                {
                    Latitude = double.Parse(LatitudeTxtBox.Text),
                    Longitude = double.Parse(LongitudeTxtBox.Text),
                };
                try
                {
                    mybl.AddStation(station);
                    MessageBox.Show("Station added seccessfuly");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// double-click on a row in the drones in charging list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneInCharge_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Station station = mybl.GetStation(Int32.Parse(stationIdTextBox.Text));
            int var = 0;
            if (DroneInCharge_ListView.SelectedItems.Count > 0)
            {
                var = DroneInCharge_ListView.Items.IndexOf(DroneInCharge_ListView.SelectedItems[0]);
            }
            DroneInCharging droneInCharging = new DroneInCharging()
            {
                Id = station.droneInChargings[var].Id,
                Battery = station.droneInChargings[var].Battery
            };
            Drone d = mybl.GetDrone(droneInCharging.Id);
            DroneToList droneTo = new DroneToList()
            {
                Id = d.Id,
                Battery = d.Battery,
                Model = d.Model,
                DroneStatuses = d.DroneStatuses,
                Weight = d.Weight,
                ParcelNumberTransferred = 0,
            };
            droneTo.Location = new()
            {
                Latitude = d.Location.Latitude,
                Longitude = d.Location.Longitude,
            };
            new DroneInActionView(droneTo, mybl).Show();
        }
        ///// <summary>
        ///// window load
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    StationListWindow stationListWindow = new StationListWindow(mybl);
        //}



        /// <summary>
        /// check that the text box includes numberic values only- you can't enter something that isn't digit
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// check that the text box includes letters only- you can't enter something that isn't letters
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlphabetValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// check that the text box includes letters only- you can't enter something that isn't letters
        /// from:https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
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
        /// if the text have changed by the user- show the button "update station"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailsChanged_update(object sender, TextChangedEventArgs e)
        {
            //btnUpdateStation.Visibility = Visibility.Visible;
        }

        private void detailsChanged_update(object sender, MouseEventArgs e)
        {
            btnUpdateStation.Visibility = Visibility.Visible;

        }
    }
}
