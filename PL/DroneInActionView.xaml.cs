using BO;
using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneInActionView.xaml
    /// </summary>
    public partial class DroneInActionView : Window
    {

        BO.Drone drone;
        private DroneToList? droneToList;
        private Drone newDrone;
        IBL mybl;

        /// <summary>
        /// Add drone window
        /// </summary>
        /// <param name="droneListWindow"></param>
        /// <param name="bL"></param>
        public DroneInActionView(DroneListWindow droneListWindow, IBL bL)
        {
            InitializeComponent();
            drone = new BO.Drone();
            DataContext = drone;
            AddGrid.Visibility = Visibility.Visible;
            Buttons_Grid.Visibility = Visibility.Collapsed;
            UpdateGrid.Visibility = Visibility.Collapsed;
            AddButtonsGrid.Visibility= Visibility.Visible;
            droneWeightComboBox.ItemsSource = Enum.GetValues(typeof(Weight));
            //AddDroneStatusComboBox.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            mybl = bL;
            IEnumerable<StationToList> listStationToList = mybl.ShowStationList();
            List<int> stationIDs = new();
            foreach (StationToList station in listStationToList)
            {
                stationIDs.Add(station.Id);
            }
            stationsComboBox.ItemsSource = stationIDs;
            droneListWindow.DronesListView.Items.Refresh();
        }

        /// <summary>
        /// Update drone
        /// </summary>
        /// <param name="droneToList"></param>
        /// <param name="bL"></param>
        /// <param name="droneListWindow"></param>
        public DroneInActionView(DroneToList droneToList, IBL bL, DroneListWindow droneListWindow=null)
        {
            InitializeComponent();
            mybl = bL;
            UpdateGrid.Visibility = Visibility.Visible;
            Buttons_Grid.Visibility = Visibility.Visible;
            AddButtonsGrid.Visibility = Visibility.Collapsed;
            AddGrid.Visibility= Visibility.Collapsed;
            this.droneToList = droneToList;
            droneStatusComboBox.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            drone = new BO.Drone()
            {
                Id = droneToList.Id,
                Battery = droneToList.Battery,
                Model = droneToList.Model,
                DroneStatuses = droneToList.DroneStatuses,
                Weight = droneToList.Weight,
                Location = new Location()
                {
                    Latitude = droneToList.Location.Latitude,
                    Longitude = droneToList.Location.Longitude
                }
            };
            droneStatusComboBox.Text = drone.DroneStatuses.ToString();
            if (droneToList.ParcelNumberTransferred != 0)
            {
                Parcel parcel = mybl.GetParcelByDroneId(drone.Id);
                btnShowParcelInTrnsfer.Visibility = Visibility.Visible;
                drone.ParcelInTransfer = new()
                {
                    Id = parcel.Id,
                    Weight = (Weight)parcel.Weight,
                    Priority = (Priority)parcel.Priority
                };
                if (parcel.CollectionTime != DateTime.MinValue)
                {
                    drone.ParcelInTransfer.ParcelTransferStatus = ParcelTransferStatus.WaitingToBePickedUp;
                }
                if (parcel.CollectionTime == DateTime.MinValue && parcel.SupplyTime != DateTime.MinValue)
                {
                    drone.ParcelInTransfer.ParcelTransferStatus = ParcelTransferStatus.OnTheWayToDestination;
                }
                Customer customerReciver = mybl.GetCustomer(parcel.Resiver.Id);
                Customer customerSender = mybl.GetCustomer(parcel.Sender.Id);
                drone.ParcelInTransfer.SupplyTargetLocation = new()
                {
                    Latitude = customerSender.Location.Latitude,
                    Longitude = customerSender.Location.Longitude,
                };

                drone.ParcelInTransfer.CollectingLocation = new()
                {
                    Latitude = customerReciver.Location.Latitude,
                    Longitude = customerReciver.Location.Longitude,
                };
                drone.ParcelInTransfer.Reciver = new()
                {
                    Id = customerReciver.Id,
                    Name = customerReciver.Name,
                };
                drone.ParcelInTransfer.Sender = new()
                {
                    Id = customerReciver.Id,
                    Name = customerReciver.Name,
                };
                double distance = mybl.CalculateDistance(customerReciver.Location.Longitude, customerReciver.Location.Latitude,
                customerSender.Location.Longitude, customerSender.Location.Latitude);
                drone.ParcelInTransfer.TransportDistance = distance;
            }
            else
            {
                btnShowParcelInTrnsfer.Visibility = Visibility.Collapsed;
            }
            DataContext = drone;
        }

        /// <summary>
        /// update drone's name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateDrone_Click(object sender, RoutedEventArgs e)
        {
            String droneName = mybl.GetDrone(Int32.Parse(idTextBox.Text)).Model;
            string newName = droneModelTextBox.Text;
            //only if the name has changed by the user
            if (droneName != droneModelTextBox.Text)
            {
                mybl.UpdateDroneName(Int32.Parse(idTextBox.Text), droneModelTextBox.Text);
                MessageBox.Show("Drone updated seccessfuly!");
                droneModelTextBox.Text = newName;
            }
            else
            {
                MessageBox.Show("Please update the drone's name");
            }
            //newDrone = mybl.GetDrone(Int32.Parse(idTextBox.Text));
            //DataContext = newDrone;
        }

        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
            if (droneStatusComboBox.Text == "Available")
            {
                try
                {
                    mybl.ShowDroneList();
                    mybl.UpdateChargeDrone(Int32.Parse(idTextBox.Text));
                    MessageBox.Show("Drone sent to charge seccessfuly!");
                    droneStatusComboBox.Text = "Maintenance";
                    btnDroneToCharge.Visibility = Visibility.Collapsed;
                    btnCollectParcel.Visibility = Visibility.Collapsed;
                    btnDroneToDelivery.Visibility = Visibility.Collapsed;
                    btnDischarge.Visibility = Visibility.Visible;
                    btnParcelDelivery.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// discharge drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDischarge_Click(object sender, RoutedEventArgs e)
        {
            if (droneStatusComboBox.Text == "Maintenance")
            {
                //input box- so the user will insert the charging time--- HH:MM
                TimeSpan chargingTime = TimeSpan.Parse(Interaction.InputBox("Please insert time of charging in: HH:MM", "Time of charging", ""));
                
                try
                {
                    mybl.DischargeDrone(Int32.Parse(idTextBox.Text), chargingTime);
                    MessageBox.Show("Drone discharged seccessfuly!");
                    droneStatusComboBox.Text = "Available";
                    batteryStatusTextBox.Text = mybl.GetDrone(Int32.Parse(idTextBox.Text)).Battery.ToString();

                    btnDroneToCharge.Visibility = Visibility.Visible;
                    btnDroneToDelivery.Visibility = Visibility.Visible;
                    btnDischarge.Visibility = Visibility.Collapsed;
                    btnCollectParcel.Visibility = Visibility.Collapsed;
                    btnParcelDelivery.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// sending drone to delivery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneToDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (droneStatusComboBox.Text == "Available")
            {
                try
                {
                    mybl.UpdateParcelToDrone(Int32.Parse(idTextBox.Text));
                    MessageBox.Show("Drone sent to delivery seccessfuly!");
                    droneStatusComboBox.Text = "Shipping";
                    btnDroneToCharge.Visibility = Visibility.Collapsed;
                    btnDroneToDelivery.Visibility = Visibility.Collapsed;
                    btnDischarge.Visibility = Visibility.Collapsed;
                    btnCollectParcel.Visibility = Visibility.Visible;
                    btnParcelDelivery.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Can't send drone to delivery");
        }

        /// <summary>
        /// collect parcel by drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCollectParcel_Click(object sender, RoutedEventArgs e)
        {

            Drone drone = mybl.GetDrone(Int32.Parse(idTextBox.Text));
            //only if the drone is on Maintenance
            //and the parcel have not been collected yet
            if (drone.ParcelInTransfer != null)
            {
                if (droneStatusComboBox.Text == "Shipping" && drone.ParcelInTransfer != null)
                {
                    mybl.UpdateParcelPickUpByDrone(Int32.Parse(idTextBox.Text));
                    MessageBox.Show("Drone picked up the parcel seccessfully!");
                    droneStatusComboBox.Text = "Shipping";

                    btnDroneToCharge.Visibility = Visibility.Collapsed;
                    btnDroneToDelivery.Visibility = Visibility.Collapsed;
                    btnDischarge.Visibility = Visibility.Collapsed;
                    btnCollectParcel.Visibility = Visibility.Visible;
                    btnParcelDelivery.Visibility = Visibility.Visible;
                }
            }
            else
                MessageBox.Show("Can't send drone to pickup parcel");
        }

        /// <summary>
        /// parcel supply
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParcelDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (drone.ParcelInTransfer != null)
            {
                if (droneStatusComboBox.Text == "Shipping" && drone.ParcelInTransfer != null)
                {
                    try
                    {
                        mybl.UpdateParcelSuppliedByDrone(Int32.Parse(idTextBox.Text));
                        MessageBox.Show("Drone picked up the parcel seccessfully!");
                        droneStatusComboBox.Text = "Available";
                        btnDroneToCharge.Visibility = Visibility.Visible;
                        btnDroneToDelivery.Visibility = Visibility.Collapsed;
                        btnDischarge.Visibility = Visibility.Collapsed;
                        btnCollectParcel.Visibility = Visibility.Visible;
                        btnParcelDelivery.Visibility = Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
                MessageBox.Show("Can't supply parcel by the drone");
        }
        /// <summary>
        /// window load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //אם החבילה במצב מישלוח יש ביא הרחפן מקושר לחבילה
            Parcel p=new();
            if (droneStatusComboBox.Text == "shipping")
            {
                p = mybl.GetParcelByDroneId(drone.Id);
            }


            if (droneStatusComboBox.Text == "Available")
            {
                btnDroneToCharge.Visibility = Visibility.Visible;
                btnDroneToDelivery.Visibility = Visibility.Visible;
                btnDischarge.Visibility = Visibility.Collapsed;
                btnCollectParcel.Visibility = Visibility.Collapsed;
                btnParcelDelivery.Visibility = Visibility.Collapsed;
            }
            else if (droneStatusComboBox.Text == "Maintenance")
            {
                btnDroneToCharge.Visibility = Visibility.Collapsed;
                btnDroneToDelivery.Visibility = Visibility.Collapsed;
                btnDischarge.Visibility = Visibility.Visible;
                btnCollectParcel.Visibility = Visibility.Collapsed;
                btnParcelDelivery.Visibility = Visibility.Collapsed;
            }
            // the drone status in shipping

            else if (droneStatusComboBox.Text == "shipping" && drone.ParcelInTransfer.Id != 0 && p.CollectionTime != DateTime.MinValue)
            {
                btnDroneToCharge.Visibility = Visibility.Collapsed;
                btnDroneToDelivery.Visibility = Visibility.Collapsed;
                btnDischarge.Visibility = Visibility.Collapsed;
                btnCollectParcel.Visibility = Visibility.Visible;
                btnParcelDelivery.Visibility = Visibility.Visible;
            }
            else if (droneStatusComboBox.Text == "shipping" && drone.ParcelInTransfer.Id != 0 && p.CollectionTime == DateTime.MinValue)
            {
                btnDroneToCharge.Visibility = Visibility.Collapsed;
                btnDroneToDelivery.Visibility = Visibility.Collapsed;
                btnDischarge.Visibility = Visibility.Collapsed;
                btnCollectParcel.Visibility = Visibility.Visible;
                btnParcelDelivery.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// adds the drone to the BL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {

            int stationId = Int32.Parse(stationsComboBox.Text);  // = 12345;

            Drone drone = new Drone()
            {
                Id = Int32.Parse(idTextBox.Text),
                Model = droneModelTextBox.Text,
                DroneStatuses = DroneStatuses.Maintenance,
                Weight = (Weight)droneWeightComboBox.SelectedItem,
            };
            
            drone.ParcelInTransfer = new()
            {
                Id = 0
            };

            mybl.AddDrone(drone, stationId);

            MessageBox.Show("Drone added seccessfuly!");
            this.Close();
        }

        /// <summary>
        /// cenceling the adding of the drone by closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_cencel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// if the drone is shipping- show the parcel details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowParcelInTrnsfer_Click(object sender, RoutedEventArgs e)
        {
            ParcelInTransferDetails parcelInTransfer= new ParcelInTransferDetails(drone.ParcelInTransfer);
            parcelInTransfer.Show();
        }
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
        /// close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

