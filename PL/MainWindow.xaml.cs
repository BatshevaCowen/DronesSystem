using System.Windows;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// main window with user type- "user" or "admin"
    /// each user type have a different mainWindow show
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL myBL;

        /// <summary>
        /// Constractor- after signing in- shows the main window- for ADMIN
        /// </summary>
        public MainWindow(IBL bl, string username = ":-)")
        {
            InitializeComponent();
            myBL = bl;
            UserLable.Visibility = Visibility.Collapsed;
            AdminLable.Visibility = Visibility.Visible;
            btnShowParcelList.Visibility = Visibility.Visible;
            btnParcelsForUSER.Visibility = Visibility.Collapsed;
            nameOfUser_lable.Content = username + "!";
        }
        /// <summary>
        /// Constractor- after signing in- shows the main window- for USER
        /// </summary>
        public MainWindow(IBL bL, User user)
        {
            myBL = bL;
            InitializeComponent();
            AdminLable.Visibility = Visibility.Collapsed;
            UserLable.Visibility = Visibility.Visible;
            btnShowDronesList.Visibility = Visibility.Collapsed;
            btnShowStationList.Visibility = Visibility.Collapsed;
            btnShowCustumerList.Visibility = Visibility.Collapsed;
            btnShowParcelList.Visibility = Visibility.Collapsed;
            btnParcelsForUSER.Visibility = Visibility.Visible;
            nameOfUser_lable.Content = user.UserName.ToString() + "!";
            username_lbl.Content = user.UserName;
            userPassword_lbl.Content = user.Password;
        }

        /// <summary>
        /// click: show the list of the drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowDronesList_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow wnd = new DroneListWindow(myBL);
            wnd.Show();
        }
        /// <summary>
        /// click: show the list of the stations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowStationList_Click(object sender, RoutedEventArgs e)
        {
           StationListWindow wnd = new StationListWindow(myBL);
            wnd.Show();
        }

        private void btnShowCustumersList_Click(object sender, RoutedEventArgs e)
        {
            CustumerListWindow wnd = new CustumerListWindow(myBL);
            wnd.Show();
        }
        /// <summary>
        /// button to open the window of the parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowParcelesList_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindowe wnd = new ParcelListWindowe(myBL);
            wnd.Show();
        }
        /// <summary>
        /// button to open the window of the parcels for the USER- only his parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParcelsForUSER_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                UserName = username_lbl.Content.ToString(),
                Password = userPassword_lbl.Content.ToString(),
                Permission = Permit.User
            };
            ParcelListWindowe wnd = new ParcelListWindowe(myBL, user);
            wnd.Show();
        }
    }
    
}
