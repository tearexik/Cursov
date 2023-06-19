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
    /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                BitmapImage bitmapImage = new BitmapImage(new Uri(selectedFileName));
                PhotoImage.Source = bitmapImage;
            }*/

    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
       /* private string selectedImagePath;*/ // Store the selected image path here

        public Client Client { get; private set; }

        public AddClientWindow(Client client)
        {
            InitializeComponent();
            Client = client;
            this.DataContext = Client;
        }

        

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            Client.FirstName = FirstNameTextBox.Text;
            Client.LastName = LastNameTextBox.Text;
            Client.Patronymic = PatronymicTextBox.Text;
            Client.Document = DocumentTextBox.Text;
            Client.Date = DateOfBirthPicker.SelectedDate.ToString();
            /*Client.Picture = selectedImagePath;*/ // Use the stored image path
            Client.Region = RegionTextBox.Text;
            Client.Pasport = PassportCheckBox.IsChecked.ToString();
            Client.Phone = PhoneTextBox.Text;
            Client.Email = EmailTextBox.Text;
            Client.Vip = VIPClientCheckBox.IsChecked == true ? "yes" : "no";
            if (int.TryParse(AgeTextBox.Text, out int age))
            {
                Client.Age = age;
            }
            else
            {
                MessageBox.Show("Invalid age input");
                Client.Age = 0; // Set a default value
            }

            this.DialogResult = true;
        }

        private void ClearClientButton_Click(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            PatronymicTextBox.Text = string.Empty;
            DocumentTextBox.Text = string.Empty;
            DateOfBirthPicker.SelectedDate = null;
            /**/
            RegionTextBox.Text = string.Empty;
            PassportCheckBox.IsChecked = false;
            PhoneTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            VIPClientCheckBox.IsChecked = false;
            AgeTextBox.Text = string.Empty;
        }
    }
}
