using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;

namespace PL
{

    /// <summary>
    /// Interaction logic for CustumerInActionView.xaml
    /// </summary>
    public partial class CustumerInActionView : Window
    {
        private CustomerToList custumerToList;
        Customer cusm;
        BlApi.IBL myBl;

        public CustumerInActionView(BlApi.IBL bl, CustumerListWindow custumerListWindow)
        {
            myBl = bl;
            InitializeComponent();
            AddCustumerGrid.Visibility = Visibility.Visible;
            btnAddCustumer_cencel.Visibility = Visibility.Visible;
            btnOKCustumer.Visibility = Visibility.Visible;
            DataContext = cusm;
            idCusTextBoxAdd.Focus();
           // CustumerListWindow.CustumerListView.Items.Refresh();

        }
        /// <summary>
        /// see customer details and update customer
        /// </summary>
        /// <param name="cusTL"></param>
        /// <param name="bL"></param>
        /// <param name="custumerListWindow"></param>
        public CustumerInActionView(CustomerToList cusTL, BlApi.IBL bL, CustumerListWindow custumerListWindow)
        {
            InitializeComponent();
            UpdatCustumereGrid.Visibility = Visibility.Visible;
            btnAddCustumer_cencel.Visibility = Visibility.Visible;
            btnUpdateCustumer.Visibility = Visibility.Visible;
            grid_showListView.Visibility = Visibility.Visible;
            listVReciverParcel.Visibility = Visibility.Visible;
            listVSenderParcel.Visibility = Visibility.Visible;
            lblR.Visibility = Visibility.Visible;
            lblS.Visibility = Visibility.Visible;
            myBl = bL;

            Customer cst = new()
            {
                Id = cusTL.Id,
                Name = cusTL.Name,
                Phone = cusTL.Phone,
            };
            Customer c = myBl.GetCustomer(cusTL.Id);
            cst.Location = new()
            {
                Latitude = c.Location.Latitude,
                Longitude = c.Location.Longitude
            };
            cst.ReceiveParcels = new List<ParcelCustomer>();
            if (c.ReceiveParcels != null && c.SentParcels != null)
            {
                grid_showListView.Visibility=Visibility.Visible;
            }
            else if (c.ReceiveParcels != null)
            {
                foreach (var item in c.ReceiveParcels)
                {
                    grid_showListView.Visibility = Visibility.Visible;
                    Sender_Grid.Visibility = Visibility.Collapsed;
                    ListViewItem newItem = new ListViewItem();
                    newItem.Content = item;
                    listVReciverParcel.Items.Add(newItem.Content);
                }
            }
            else
            {
                grid_showListView.Visibility = Visibility.Visible;
                Reciver_Grid.Visibility=Visibility.Collapsed;
            }
            cst.SentParcels = new List<ParcelCustomer>();
            if (c.SentParcels != null)
            {
                foreach (var item in c.SentParcels)
                {
                    grid_showListView.Visibility = Visibility.Visible;
                    ListViewItem newItem = new ListViewItem();
                    newItem.Content = item;
                    listVSenderParcel.Items.Add(newItem);
                }
            }
            else
            {
                grid_showListView.Visibility = Visibility.Visible;
            }
            DataContext = cst;
        }

        /// <summary>
        /// UPDATE buttonn for customer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateCustumer_Click(object sender, RoutedEventArgs e)
        {
            UpdatCustumereGrid.Visibility = Visibility.Visible;
            String CustomerName = (nameTextBox.Text);
            String CustomerPhone = (phoneTextBox.Text);
            Customer c= myBl.GetCustomer(Int32.Parse(idCusTextBox.Text));
            if (CustomerName == c.Name && CustomerPhone == c.Phone)
            {
                MessageBox.Show("Please update name or phone number");
            }
            else 
            {
                try
                {
                    myBl.UpdateCustomer(Int32.Parse(idCusTextBox.Text), CustomerName, CustomerPhone);
                    MessageBox.Show("your datails updated succesfully!");
                    this.Close();
                }
                catch(Exception ex){MessageBox.Show(ex.Message);}
              
            }
        }
        /// <summary>
        /// ADD a new customer buttonn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOKCustumer_Click(object sender, RoutedEventArgs e)
        {
            if (idCusTextBoxAdd.Text.Length == 0)
            {
                MessageBox.Show("Please enter ID");
                idCusTextBoxAdd.Focus();
            }
            else if (nameTextBoxAdd.Text.Length == 0)
            {
                MessageBox.Show("Please enter name");
                nameTextBoxAdd.Focus();
            }
            else if (phoneTextBoxAdd.Text.Length == 0)
            {
                MessageBox.Show("Please enter Phone number");
                phoneTextBoxAdd.Focus();
            }
            else if (latitudeTextBoxAdd.Text.Length == 0)
            {
                MessageBox.Show("Please enter latitude");
                latitudeTextBoxAdd.Focus();
            }
            else if (longitudeTextBoxAdd.Text.Length == 0)
            {
                MessageBox.Show("Please enter longitude");
                longitudeTextBoxAdd.Focus();
            }
            else if(ChooseUserNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter username foe the new customer");
                ChooseUserNameTextBox.Focus();
            }
            else if(ChoosePasswordTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please choose password foe the new customer");
                ChoosePasswordTextBox.Focus();
            }

            else if (idCusTextBoxAdd.Text.Length != 9)
                MessageBox.Show("ID should have 9 digits");
            else if (phoneTextBoxAdd.Text.Length < 9 || phoneTextBoxAdd.Text.Length > 10)
                MessageBox.Show("Incorrect phone number");
            else if (double.Parse(longitudeTextBoxAdd.Text) < -180 || double.Parse(longitudeTextBoxAdd.Text) > 180)
                MessageBox.Show("Incorrect Longitude");
            else if (double.Parse(latitudeTextBoxAdd.Text) < -90 || double.Parse(latitudeTextBoxAdd.Text) > 90)
                MessageBox.Show("Incorrect Latitude");
            else
            {
                Customer cusm = new Customer()
                {
                    Id = Int32.Parse(idCusTextBoxAdd.Text),
                    Name = (nameTextBoxAdd.Text),
                    Phone = phoneTextBoxAdd.Text,
                };
                cusm.Location = new()
                {
                    Latitude = double.Parse(latitudeTextBoxAdd.Text),
                    Longitude = double.Parse(longitudeTextBoxAdd.Text),
                };
                cusm.User = new()
                {
                    UserName = ChooseUserNameTextBox.Text,
                    Password = ChoosePasswordTextBox.Text,
                    Permission=Permit.User,
                    
                    
                };
                
                try
                {
                    myBl.AddCustomer(cusm);
                    myBl.AddUser(cusm.User);
                    MessageBox.Show("New customer added succesfully!");
                    this.Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// CENCEL adding customer button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustumer_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void ParcelsList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelCustomer parcelCustomer;
            if (listVReciverParcel.SelectedItem != null)
                parcelCustomer = listVReciverParcel.SelectedItem as ParcelCustomer;
            else
                parcelCustomer = listVSenderParcel.SelectedItem as ParcelCustomer;
            if (parcelCustomer != null)
            {
                //  new ParcelInActionView(myBL, myBL.GetParcel(parcelCustomer.Id)).Show();
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
    }

}
