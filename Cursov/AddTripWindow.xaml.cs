using Microsoft.Win32;
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

namespace Cursov
{
    /// <summary>
    /// Логика взаимодействия для AddTripWindow.xaml
    /// </summary>
    public partial class AddTripWindow : Window
    {
        ApplicationContext db;
        public AddTripWindow(Trip trips)
        {
            InitializeComponent();
            db = new ApplicationContext();
            Trip = trips;

            AddTripWindowViewModel viewModel = new AddTripWindowViewModel(Trip);
            this.DataContext = viewModel;
        }

        public Trip Trip { get; private set; }

        private void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            AddTripWindowViewModel viewModel = (AddTripWindowViewModel)DataContext;
            Trip newTrip = viewModel.Trip;

            // Set the properties of the new trip
            newTrip.Route = routeTextBox.Text;
            newTrip.Price = double.Parse(priceTextBox.Text);
            newTrip.Description = descriptionTextBox.Text;
            newTrip.Hotel = hotelTextBox.Text;
            newTrip.StartDate = startDatePicker.SelectedDate.Value;
            newTrip.EndDate = endDatePicker.SelectedDate.Value;

            // Set the guide ID
            if (guideComboBox.SelectedItem is Guides selectedGuide)
            {
                newTrip.GuideID = selectedGuide.Id;
            }
            else
            {
                // Handle the case when no guide is selected
                newTrip.GuideID = null;
            }

            // Set the client IDs
            List<int> clientIds = new List<int>();
            foreach (Client client in clientListBox.SelectedItems)
            {
                clientIds.Add(client.Id);
            }
            newTrip.ClientIDs = clientIds;

            // Save the new trip to the database
            /*using (ApplicationContext db = new ApplicationContext())
            {
                db.Trip.Add(newTrip);
                db.SaveChanges();
            }*/

            // Close the window
            this.DialogResult = true;
        }
    }
}
