using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using BlApi;
using System.Runtime.InteropServices;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL bL;
        public DroneListWindow(IBL bl)
        {
            this.bL = bl;
            InitializeComponent();
            RefreshDronesListView();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            comboWeghitSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
        }

        /// <summary>
        /// refresh the list view that shows the drones information
        /// </summary>
        public void RefreshDronesListView()
        {
            this.DronesListView.ItemsSource = bL.ShowDroneList();
        }

        /// <summary>
        /// Combo box for drone status selecting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboStatusSelector.SelectedItem != null)
            {
                DroneStatuses droneStatuses = (DroneStatuses)comboStatusSelector.SelectedItem;
                this.DronesListView.ItemsSource = bL.ShowDroneList().Where(x => x.DroneStatuses == droneStatuses);
            }
            //if both of the comboboxes are not null
            if (comboWeghitSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
            {
                Weight weight = (Weight)comboWeghitSelector.SelectedItem;
                DroneStatuses droneStatuses = (DroneStatuses)comboStatusSelector.SelectedItem;
                //the list will show list filterd by WEIGHT and STATUS
                IEnumerable<DroneToList> dronesView = bL.ShowDroneList().Where(x => x.DroneStatuses == droneStatuses);
                this.DronesListView.ItemsSource = dronesView.Where(x => x.Weight == weight);
            }
        }

        /// <summary>
        /// Combo box for weight selecting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboWeghitSelector.SelectedItem != null)
            {
                Weight weight = (Weight)comboWeghitSelector.SelectedItem;
                this.DronesListView.ItemsSource = bL.ShowDroneList().Where(x => x.Weight == weight);
            }
            //if both of the comboboxes are not null
            if (comboWeghitSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
            {
                Weight weight = (Weight)comboWeghitSelector.SelectedItem;
                DroneStatuses droneStatuses = (DroneStatuses)comboStatusSelector.SelectedItem;
                //the list will show list filterd by WEIGHT and STATUS
                IEnumerable<DroneToList> dronesView= bL.ShowDroneList().Where(x => x.DroneStatuses == droneStatuses);
                this.DronesListView.ItemsSource = dronesView.Where(x => x.Weight == weight);
            }
        }
        /// <summary>
        /// Clear the status comboBox and the listView of the drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearStatusComboBox_Click(object sender, RoutedEventArgs e)
        {
            comboStatusSelector.Text = "";
            //if both of the combo boxes are empty now
            if (comboWeghitSelector.SelectedItem == null)
                RefreshDronesListView();
            else    //show filter by weight only
            {
                Weight weight = (Weight)comboWeghitSelector.SelectedItem;
                this.DronesListView.ItemsSource = bL.ShowDroneList().Where(x => x.Weight == weight);
            }
        }
        
        /// <summary>
        /// Clear the weight comboBox and the listView of the drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearWeightComboBox_Click(object sender, RoutedEventArgs e)
        {
            comboWeghitSelector.Text = "";
            //if both of the combo boxes are empty now
            if (comboStatusSelector.SelectedItem == null)
                RefreshDronesListView();
            else    //show filter by status only
            {
                DroneStatuses droneStatuses = (DroneStatuses)comboStatusSelector.SelectedItem;
                this.DronesListView.ItemsSource = bL.ShowDroneList().Where(x => x.DroneStatuses == droneStatuses);
            }
        }

        /// <summary>
        /// Add drone button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneInActionView(this, bL).Show();
        }

        /// <summary>
        /// double-clicking on one of the drones on the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.DroneToList? droneToList = DronesListView.SelectedItem as BO.DroneToList;
            if (droneToList != null)
            {
                new DroneInActionView(droneToList, bL, this).Show();
            }
        }
        /// <summary>
        /// refresh the list view of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshWindow(object sender, EventArgs e)
        { 
            RefreshDronesListView();
        }
        /// <summary>
        ///  Close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
