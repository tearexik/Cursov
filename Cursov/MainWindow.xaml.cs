using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cursov
{

    public class GuideIdToLastNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int guideId)
            {
                // Replace this logic with your actual implementation to retrieve the guide's last name based on the ID
                string lastName = GetGuideLastName(guideId);
                return lastName;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private string GetGuideLastName(int guideId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Guides guide = db.Guides.FirstOrDefault(g => g.Id == guideId);
                if (guide != null)
                {
                    return guide.G_LastName;
                }
            }

            return string.Empty; // Return an empty string if the guide with the specified ID is not found
        }
    }

    public class ClientListToLastNamesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0 && values[0] is IEnumerable<int> clientIDs)
            {
                List<string> lastNames = new List<string>();

                using (ApplicationContext db = new ApplicationContext())
                {
                    foreach (int clientId in clientIDs)
                    {
                        Client client = db.Clients.FirstOrDefault(c => c.Id == clientId);
                        if (client != null)
                        {
                            lastNames.Add(client.LastName);
                        }
                    }
                }

                return string.Join(", ", lastNames);
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string userRole;

        ApplicationContext db;
        public MainWindow(string role)
        {
            InitializeComponent();

            userRole = role;
            db = new ApplicationContext();

            // Load the data from the database
            db.Clients.Load();
            db.Guides.Load();
            db.Trip.Load();

            var viewModel = new AddTripWindowViewModel(db.Trip.Local.FirstOrDefault());
            this.DataContext = viewModel;

            clientList.ItemsSource = db.Clients.Local;
            clientVipList.ItemsSource = db.Clients.Local;
            tripList.ItemsSource = db.Trip.Local;
            guideList.ItemsSource = db.Guides.Local;

            Loaded += MainWindow_Load;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                if (tabControl.SelectedItem == vipTabItem)
                {
                    // Show only VIP clients in the "VIP" tab
                    CollectionViewSource.GetDefaultView(clientVipList.ItemsSource).Filter = IsVIPFilter;
                }
                else if (tabControl.SelectedItem == allTabItem)
                {
                    // Show all clients in the "All" tab
                    CollectionViewSource.GetDefaultView(clientVipList.ItemsSource).Filter = null;
                }
            }
        }

        private bool IsVIPFilter(object item)
        {
            Client client = item as Client;
            if (client != null)
            {
                if (client.Vip == "да" && clientTabControl.SelectedItem == vipTabItem)
                {
                    // Show only VIP clients in the "VIP" tab
                    return true;
                }
                else if (clientTabControl.SelectedItem == allTabItem)
                {
                    // Show all clients in the "All" tab
                    return true;
                }
            }
            return false;
        }

        public void UpdateDriveCount(List<Guides> guides)
        {
            foreach (Guides guide in guides)
            {
                int driveCount = db.Trip.Count(t => t.GuideID == guide.Id);
                guide.G_Count_Drives = driveCount;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            List<Guides> guides = db.Guides.ToList();
            UpdateDriveCount(guides);

            if (userRole == "Guide")
            {
                // Customize UI for Guide
                AddClientButton.Visibility = Visibility.Collapsed;
                AddTrip.Visibility = Visibility.Collapsed;
                MainTabControl.Visibility = Visibility.Visible;
                GuidesGrid.Visibility = Visibility.Collapsed;
                ClientsGrid.Visibility = Visibility.Collapsed;
                Trips.Visibility = Visibility.Visible;
                // ...
            }
            else if (userRole == "Manager")
            {
                // Customize UI for Manager
                AddClientButton.Visibility = Visibility.Visible;
                AddTrip.Visibility = Visibility.Visible;
                MainTabControl.Visibility = Visibility.Visible;
                GuidesGrid.Visibility = Visibility.Visible;
                ClientsGrid.Visibility = Visibility.Visible;
                Trips.Visibility = Visibility.Visible;
                // ...
            }
            CollectionViewSource.GetDefaultView(clientVipList.ItemsSource).Filter = IsVIPFilter;
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private FiltersWindow findCientWindow;
        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {
            var find = new FiltersWindow(); // create a new instance of the filter dialog window
            findCientWindow = find;
            if (find.ShowDialog() == true) // show the dialog window and wait for user input
            {
                var name = find.NameFilter;
                var surname = find.SurnameFilter;
                var patronymic = find.PatronymicFilter;
                var passportId = find.PassportIdFilter;
                var age = find.AgeFilter;
                var email = find.EmailFilter;
                var phone = find.PhoneFilter;
                var region = find.RegionFilter;
                var vip = find.IsVipFilter;
                var pass = find.IsPassFilter;

                // perform the filtering
                var filteredClients = db.Clients.Local;

                if (!string.IsNullOrEmpty(name))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.FirstName.ToLower().Contains(name.ToLower())));
                }

                if (!string.IsNullOrEmpty(surname))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.LastName.ToLower().Contains(surname.ToLower())));
                }

                if (!string.IsNullOrEmpty(patronymic))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Patronymic.ToLower().Contains(patronymic.ToLower())));
                }

                if (!string.IsNullOrEmpty(passportId))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Document.ToLower().Contains(passportId.ToLower())));
                }

                if (!string.IsNullOrEmpty(age))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Age.ToString().Equals(age)));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Email.ToLower().Contains(email.ToLower())));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Phone.ToLower().Contains(phone.ToLower())));
                }


                if (!string.IsNullOrEmpty(region))
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Region.ToLower().Contains(region.ToLower())));
                }

                if (vip)
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Vip == "да"));
                }

                if (pass)
                {
                    filteredClients = new ObservableCollection<Client>(filteredClients.Where(c => c.Pasport == "да"));
                }

                // update the client list view with the filtered results
                clientList.ItemsSource = filteredClients.OrderBy(c => c.FirstName).ToList();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text;
            // Perform search based on the entered search query
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            Client newClient = new Client(); // Create a new instance of the Client class

            AddClientWindow addClientWindow = new AddClientWindow(newClient);
            if (addClientWindow.ShowDialog() == true)
            {
                newClient = addClientWindow.Client; // Get the modified client from the AddClientWindow
                db.Clients.Add(newClient); // Assuming 'db' is your data context
                db.SaveChanges(); // Save the changes to the database
            }
            else
            {
                MessageBox.Show("Adding a new client was canceled.");
            }
        }
        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear any existing filters and display all data
            clientList.ItemsSource = db.Clients.Local.OrderBy(c => c.FirstName).ToList();
        }

        private void ClearFiltersGuideButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear any existing filters and display all data
            guideList.ItemsSource = db.Guides.Local.OrderBy(c => c.G_FirstName).ToList();
        }

        private void ClearFiltersTripButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear any existing filters and display all data
            tripList.ItemsSource = db.Trip.Local.OrderBy(c => c.Route).ToList();
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientList.SelectedItem == null) return;
            Client client = clientList.SelectedItem as Client;
            AddClientWindow clientWindow = new AddClientWindow(new Client
            {
                Id = client.Id,
                LastName = client.LastName,
                FirstName = client.FirstName,
                Patronymic = client.Patronymic,
                Document = client.Document,
                Date = client.Date,
                Region = client.Region,
                Pasport = client.Pasport,
                Phone = client.Phone,
                Email = client.Email,
                Vip = client.Vip,
                Age = client.Age
            });
            if (clientWindow.ShowDialog() == true)
            {
                client = db.Clients.Find(clientWindow.Client.Id);

                if (client != null)
                {
                    client.LastName = clientWindow.Client.LastName;
                    client.FirstName = clientWindow.Client.FirstName;
                    client.Patronymic = clientWindow.Client.Patronymic;
                    client.Document = clientWindow.Client.Document;
                    client.Date = clientWindow.Client.Date;
                    client.Region = clientWindow.Client.Region;
                    client.Pasport = clientWindow.Client.Pasport;
                    client.Phone = clientWindow.Client.Phone;
                    client.Email = clientWindow.Client.Email;
                    client.Vip = clientWindow.Client.Vip;
                    client.Age = clientWindow.Client.Age;
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        private void EditGuideButton_Click(object sender, RoutedEventArgs e)
        {
            if (guideList.SelectedItem == null) return;
            Guides guide = guideList.SelectedItem as Guides;
            AddGuideWindow guideWindow = new AddGuideWindow(new Guides
            {
                Id = guide.Id,
                G_LastName = guide.G_LastName,
                G_FirstName = guide.G_FirstName,
                G_Patronymic = guide.G_Patronymic,
                G_Document = guide.G_Document,
                G_Date = guide.G_Date,
                G_Region = guide.G_Region,
                G_Pasport = guide.G_Pasport,
                G_Phone = guide.G_Phone,
                G_Email = guide.G_Email,
                G_All_Drives = guide.G_All_Drives,
                G_Age = guide.G_Age,
                G_Count_Drives = guide.G_Count_Drives,
                G_Date_Work = guide.G_Date_Work
            });
            if (guideWindow.ShowDialog() == true)
            {
                guide = db.Guides.Find(guideWindow.Guides.Id);
                if (guide != null)
                {
                    guide.G_LastName = guideWindow.Guides.G_LastName;
                    guide.G_FirstName = guideWindow.Guides.G_FirstName;
                    guide.G_Patronymic = guideWindow.Guides.G_Patronymic;
                    guide.G_Document = guideWindow.Guides.G_Document;
                    guide.G_Date = guideWindow.Guides.G_Date;
                    guide.G_Region = guideWindow.Guides.G_Region;
                    guide.G_Pasport = guideWindow.Guides.G_Pasport;
                    guide.G_Phone = guideWindow.Guides.G_Phone;
                    guide.G_Email = guideWindow.Guides.G_Email;
                    guide.G_All_Drives = guideWindow.Guides.G_All_Drives;
                    guide.G_Age = guideWindow.Guides.G_Age;
                    guide.G_Count_Drives = guideWindow.Guides.G_Count_Drives;
                    guide.G_Date_Work = guideWindow.Guides.G_Date_Work;
                    db.Entry(guide).State = EntityState.Modified;
                    db.SaveChanges();
                    guideList.ItemsSource = db.Guides.Local;
                    guideList.Items.Refresh();
                }
            }
        }
        
        
        private void EditTripsButton_Click(object sender, RoutedEventArgs e)
        {
            if (tripList.SelectedItem == null) return;
            Trip trip = tripList.SelectedItem as Trip;
            AddTripWindow tripWindow = new AddTripWindow(new Trip
            {
                Id = trip.Id,
                Route = trip.Route,
                Price = trip.Price,
                GuideID = trip.GuideID,
                ClientIDs = trip.ClientIDs,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Description = trip.Description,
                Hotel = trip.Hotel
            });
            if (tripWindow.ShowDialog() == true)
            {
                trip = db.Trip.Find(tripWindow.Trip.Id);
                if (trip != null)
                {
                    trip.Route = tripWindow.Trip.Route;
                    trip.Price = tripWindow.Trip.Price;
                    trip.GuideID = tripWindow.Trip.GuideID;
                    trip.ClientIDs = tripWindow.Trip.ClientIDs;
                    trip.StartDate = tripWindow.Trip.StartDate;
                    trip.EndDate = tripWindow.Trip.EndDate;
                    trip.Description = tripWindow.Trip.Description;
                    trip.Hotel = tripWindow.Trip.Hotel;
                    db.Entry(trip).State = EntityState.Modified;
                    db.SaveChanges();
                    tripList.ItemsSource = db.Trip.Local;
                    tripList.Items.Refresh();
                }
            }
        }

        private void DelClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientList.SelectedItem is Client selectedClient)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this client?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    db.Clients.Remove(selectedClient); // Remove the selected client from the database
                    db.SaveChanges(); // Save the changes to the database
                    MessageBox.Show("Client deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No client selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelGuideButton_Click(object sender, RoutedEventArgs e)
        {
            if (guideList.SelectedItem is Guides selectedGuide)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this guide?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    db.Guides.Remove(selectedGuide); // Remove the selected client from the database
                    db.SaveChanges(); // Save the changes to the database
                    MessageBox.Show("Guide deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No guide selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void AddGuideButton_Click(object sender, RoutedEventArgs e)
        {
            Guides newGuides = new Guides(); // Create a new instance of the Client class

            AddGuideWindow addGuideWindow = new AddGuideWindow(newGuides);
            if (addGuideWindow.ShowDialog() == true)
            {
                newGuides = addGuideWindow.Guides; // Get the modified client from the AddClientWindow
                db.Guides.Add(newGuides); // Assuming 'db' is your data context
                db.SaveChanges(); // Save the changes to the database
            }
            else
            {
                MessageBox.Show("Adding a new guide was canceled.");
            }
        }

       

        private FiltersGuidesWindow findGuideWindow;
        private void FiltersGuidesButton_Click(object sender, RoutedEventArgs e)
        {
            var find = new FiltersGuidesWindow(); // create a new instance of the filter dialog window

            if (find.ShowDialog() == true) // show the dialog window and wait for user input
            {
                var name = find.NameFilter;
                var surname = find.SurnameFilter;
                var patronymic = find.PatronymicFilter;
                var passportId = find.PassportIdFilter;
                var age = find.AgeFilter;
                var email = find.EmailFilter;
                var phone = find.PhoneFilter;
                var region = find.RegionFilter;
                var count_drives = find.CountDrivesFilter;
               
                var experience = find.ExperienceFilter;
                var pass = find.PassFilter;

                // perform the filtering
                var filteredGuides = db.Guides.Local;

                if (!string.IsNullOrEmpty(name))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_FirstName.ToLower().Contains(name.ToLower())));
                }

                if (!string.IsNullOrEmpty(surname))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_LastName.ToLower().Contains(surname.ToLower())));
                }

                if (!string.IsNullOrEmpty(patronymic))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Patronymic.ToLower().Contains(patronymic.ToLower())));
                }

                if (!string.IsNullOrEmpty(passportId))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Document.ToLower().Contains(passportId.ToLower())));
                }

                if (!string.IsNullOrEmpty(age))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Age.ToString().Equals(age)));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Email.ToLower().Contains(email.ToLower())));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Phone.ToLower().Contains(phone.ToLower())));
                }


                if (!string.IsNullOrEmpty(region))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Region.ToLower().Contains(region.ToLower())));
                }

               
                
                if (!string.IsNullOrEmpty(region))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Count_Drives.ToString().Equals(age)));
                }

                if (!string.IsNullOrEmpty(region))
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Date_Work.ToString().Equals(age)));
                }

                if (pass)
                {
                    filteredGuides = new ObservableCollection<Guides>(filteredGuides.Where(c => c.G_Pasport == "да"));
                }

                // update the client list view with the filtered results
                guideList.ItemsSource = filteredGuides.OrderBy(c => c.G_FirstName).ToList();
            }
        }

        private void SearchGuideTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Perform search logic based on the entered text in the SearchGuideTextBox
            string searchText = SearchGuideTextBox.Text;

            // ...
        }
        private void SearchTripsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Perform search logic based on the entered text in the SearchGuideTextBox
            string searchText = SearchTripsTextBox.Text;

            // ...
        }

        private FilterTripWindow findTripWindow;

        private void FiltersTripsButton_Click(object sender, RoutedEventArgs e)
        {
            var find = new FilterTripWindow(); // create a new instance of the filter dialog window

            if (find.ShowDialog() == true) // show the dialog window and wait for user input
            {
                var route = find.RouteFilter;
                var price = find.PriceFilter;
                var description = find.DescriptionFilter;
                var hotel = find.HotelFilter;
                var guide = find.GuideFilter;
                

                // perform the filtering
                var filteredTrips = db.Trip.Local;

                if (!string.IsNullOrEmpty(route))
                {
                    filteredTrips = new ObservableCollection<Trip>(filteredTrips.Where(c => c.Route.ToLower().Contains(route.ToLower())));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    filteredTrips = new ObservableCollection<Trip>(filteredTrips.Where(c => c.Description.ToLower().Contains(description.ToLower())));
                }

                if (!string.IsNullOrEmpty(hotel))
                {
                    filteredTrips = new ObservableCollection<Trip>(filteredTrips.Where(c => c.Hotel.ToLower().Contains(hotel.ToLower())));
                }

                if (!string.IsNullOrEmpty(price))
                {
                    filteredTrips = new ObservableCollection<Trip>(filteredTrips.Where(c => c.Price.ToString().Equals(price)));
                }

                if (!string.IsNullOrEmpty(guide))
                {
                    int? guideValue = int.Parse(guide);
                    filteredTrips = new ObservableCollection<Trip>(filteredTrips.Where(n => n.GuideID == guideValue));
                }

                // update the client list view with the filtered results
                tripList.ItemsSource = filteredTrips.OrderBy(c => c.Route).ToList();
            }
        }

        private void AddTripsButton_Click(object sender, RoutedEventArgs e)
        {
            Trip newTrip = new Trip(); 

            AddTripWindow addTripWindow = new AddTripWindow(newTrip);
            if (addTripWindow.ShowDialog() == true)
            {
                newTrip = addTripWindow.Trip; 
                db.Trip.Add(newTrip); 
                db.SaveChanges(); 
            }
            else
            {
                MessageBox.Show("Adding a new trip was canceled.");
            }
        }



        

    }

}
