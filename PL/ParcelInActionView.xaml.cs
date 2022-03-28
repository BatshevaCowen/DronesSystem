using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInActionView.xaml
    /// </summary>
    public partial class ParcelInActionView : Window
    {
        BlApi.IBL myBl;
        Parcel parcel;
        /// <summary>
        /// option to add parcel detailes
        /// </summary>
        /// <param name="bl"></param>
        public ParcelInActionView(BlApi.IBL bl)
        {
            InitializeComponent();
            ADDParcelGrid.Visibility = Visibility.Visible;
            Grid_ShowParcel.Visibility = Visibility.Collapsed;
            btnOK.Visibility = Visibility.Visible;
            myBl = bl;
            parcel = new();
            DataContext = parcel;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(Weight));
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            //select option of custumer id and name to reciver or sender parcel
           
            IEnumerable<CustomerInParcel> c = from customer in myBl.ShowCustomerList()
                                              let cs = new CustomerInParcel { Id = customer.Id, Name = customer.Name }
                                              select cs;
            senderComboBox.ItemsSource = c;
            ReciverComboBox.ItemsSource = c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelListWindowe"></param>
        /// <param name="bl"></param>
        public ParcelInActionView(ParcelListWindowe parcelListWindowe, BlApi.IBL bl)
        {
            InitializeComponent();
            btnOK.Visibility = Visibility.Visible;           
            parcel = new BO.Parcel();
            DataContext = parcel;

        }
        /// <summary>
        /// to show the parcel datailes
        /// </summary>
        /// <param name="parTL"></param>
        /// <param name="bl"></param>
        /// <param name="parcelListWindowe"></param>
        public ParcelInActionView(ParcelToList parTL, BlApi.IBL bl, ParcelListWindowe parcelListWindowe)
        {
            InitializeComponent();

            Grid_ShowParcel.Visibility = Visibility.Visible;
            ADDParcelGrid.Visibility = Visibility.Collapsed;
            btnUpdateParcel.Visibility = Visibility.Visible;
            btnShowDrone.Visibility = Visibility.Visible;
            btnShowCustumerReciver.Visibility = Visibility.Visible;
            btnShowCustumerSender.Visibility = Visibility.Visible;
            btnRemuveParcel.Visibility = Visibility.Visible;
            myBl = bl;
            Parcel p = myBl.GetParcel(parTL.Id);
            //If there is no drone associated with the parcel hiden all the detailes
            if (p.DroneInParcel == null)
            {
                lblBatteryDrone.Visibility = Visibility.Collapsed;
                lblDroneInParcel.Visibility = Visibility.Collapsed;
                lblIDDrone.Visibility = Visibility.Collapsed;
                lbllongiDrone.Visibility = Visibility.Collapsed;
                batteryDroneParcelTXB.Visibility = Visibility.Collapsed;
                IdDroneParcelTXB.Visibility = Visibility.Collapsed;
                LatitudeDroneParcelTXB.Visibility = Visibility.Collapsed;
            }
            DataContext = p;
        }

        /// <summary>
        /// Checks the status of the package and updates it accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateParcel_Click(object sender, RoutedEventArgs e)
        {
            //there are drone!
            if(lblDroneInParcel.Visibility== Visibility.Visible)
            {
             Drone drone= myBl.GetDrone(int.Parse(IdDroneParcelTXB.Text));
                Parcel pr = myBl.GetParcel(int.Parse(idTXB.Text));

                //Assign parcel to drone 
                if (drone.DroneStatuses == DroneStatuses.Available)
                {
                    try
                    {
                        myBl.UpdateParcelToDrone(int.Parse(IdDroneParcelTXB.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                    //Updete that the parcel has picked up by a drone
                    if (pr.AssignmentToParcelTime == DateTime.MinValue&& pr.CollectionTime == DateTime.MinValue)
                    {
                        try
                        {
                            myBl.UpdateParcelPickUpByDrone(int.Parse(IdDroneParcelTXB.Text));
                            MessageBox.Show("Updete parcel has picked up by a drone sucssesfully!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    //Update that the parcel supplied to the reciver (by the drone)
                    if (pr.CollectionTime != DateTime.MinValue&& pr.SupplyTime == DateTime.MinValue)
                    {
                        try
                        {
                            myBl.UpdateParcelSuppliedByDrone(int.Parse(IdDroneParcelTXB.Text));
                            MessageBox.Show("update parcel supplied to the reciver sucssefully! ");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                   
                
            }
            else
            {
                MessageBox.Show("ther are no drone to update ");
            }
        }
        /// <summary>
        /// open drone windowes parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowDrone_Click(object sender, RoutedEventArgs e)
        {
            if (IdDroneParcelTXB.Text != "")
            {
                Drone drone = myBl.GetDrone(Int32.Parse(IdDroneParcelTXB.Text));
                DroneToList drTL = new()
                {
                    Id = drone.Id,
                    Battery = drone.Battery,
                    DroneStatuses = drone.DroneStatuses,
                    Weight = drone.Weight,
                    Model = drone.Model,
                    //ParcelNumberTransferred = drone.ParcelInTransfer.Id,

                };
                if (drTL.ParcelNumberTransferred > 0) 
                {
                    drTL.ParcelNumberTransferred = drone.ParcelInTransfer.Id;
                }
                drTL.Location = new()
                {
                    Latitude = drone.Location.Latitude,
                    Longitude = drone.Location.Longitude,
                };
                new DroneInActionView(drTL, myBl).Show();
            }
            else
            {
                MessageBox.Show("there is no drone to supply!!");
            }
        }
        /// <summary>
        /// open cusrumer reciver window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowCustumerReciver_Click(object sender, RoutedEventArgs e)
        {
            Customer c = myBl.GetCustomer(Int32.Parse(ReciverIdTXB.Text));
            CustomerToList cTL = new()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
            };
            new CustumerInActionView(cTL, myBl, null).Show();
        }
        /// <summary>
        /// open custumer sender window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowCustumerSender_Click(object sender, RoutedEventArgs e)
        {
            Customer c = myBl.GetCustomer(Int32.Parse(SenderIdTXB.Text));
            CustomerToList cTL = new()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
            };
            new CustumerInActionView(cTL, myBl, null).Show();
        }
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// add the parcel to layer bl=>dal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBl.AddParcel(parcel);
                MessageBox.Show("Parcel added succesfuly!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// remove parcel if its not suplly yet to drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel p = myBl.GetParcel(Int32.Parse(idTXB.Text));
           
           
          
            if(p.DroneInParcel!=null)
            {
                try
                {
                    myBl.RemoveParcel(p.Id);
                    MessageBox.Show("parcel removed sucssecfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("parcel is waiting to drone and can't be removed");
            }
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
    }
}