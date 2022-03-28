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
    /// Interaction logic for CustumerListWindow.xaml
    /// </summary>
    public partial class CustumerListWindow : Window
    {
        BlApi.IBL myBL;
        public CustumerListWindow(BlApi.IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            this.CustumerListView.ItemsSource = myBL.ShowCustomerList();
        }

        /// <summary>
        /// double click on customer to see customer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerInActionView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList? cusTL = CustumerListView.SelectedItem as CustomerToList;
            if (cusTL != null)
            {
                new CustumerInActionView(cusTL, myBL, this).Show();
            }
        }
        /// <summary>
        /// ADD button for customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustumer_Click(object sender, RoutedEventArgs e)
        {
            new CustumerInActionView(myBL,this).Show();
        }
        /// <summary>
        /// close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustumer_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// refresh the list view that shows the customers information
        /// </summary>
        public void RefreshCustomerListView()
        {
            this.CustumerListView.ItemsSource = myBL.ShowCustomerList();
        }
        /// <summary>
        /// refresh the list view of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshWindow(object sender, EventArgs e)
        {
            RefreshCustomerListView();
        }
        /// <summary>
        /// close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}