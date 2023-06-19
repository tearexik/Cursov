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
    /// Логика взаимодействия для AddGuideWindow.xaml
    /// </summary>
    public partial class AddGuideWindow : Window
    {
        public Guides Guides { get; private set; }
        public AddGuideWindow(Guides guides)
        {
            InitializeComponent();
            Guides = guides;
            this.DataContext = Guides;
        }

        

        private void AddGuideButton_Click(object sender, RoutedEventArgs e)
        {
            Guides.G_FirstName = G_FirstNameTextBox.Text;
            Guides.G_LastName = G_LastNameTextBox.Text;
            Guides.G_Patronymic = G_PatronymicTextBox.Text;
            Guides.G_Document = G_DocumentTextBox.Text;
            Guides.G_Date = G_DateOfBirthPicker.SelectedDate.ToString();
            Guides.G_Date_Work = G_DateOfWorkPicker.SelectedDate.ToString();

            /**/
            Guides.G_Region = G_RegionTextBox.Text;
            Guides.G_Pasport = G_PassportCheckBox.IsChecked.ToString();
            Guides.G_Phone = G_PhoneTextBox.Text;
            Guides.G_Email = G_EmailTextBox.Text;
            if (int.TryParse(G_AgeTextBox.Text, out int age))
            {
                Guides.G_Age = age;
            }
            else
            {
                // Invalid input, handle the error
                // For example, you can show a message or set a default value
                MessageBox.Show("Invalid age input");
                Guides.G_Age = 0; // Set a default value
            }

            this.DialogResult = true;
        }

        private void ClearGuideButton_Click(object sender, RoutedEventArgs e)
        {
            G_FirstNameTextBox.Text = string.Empty;
            G_LastNameTextBox.Text = string.Empty;
            G_PatronymicTextBox.Text = string.Empty;
            G_DocumentTextBox.Text = string.Empty;
            G_DateOfBirthPicker.SelectedDate = null;
            G_DateOfWorkPicker.SelectedDate = null;
            /**/
            G_RegionTextBox.Text = string.Empty;
            G_PassportCheckBox.IsChecked = false;
            G_PhoneTextBox.Text = string.Empty;
            G_EmailTextBox.Text = string.Empty;
            G_AgeTextBox.Text = string.Empty;
        }
    }
}
