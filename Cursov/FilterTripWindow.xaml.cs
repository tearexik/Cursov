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
    /// Логика взаимодействия для FilterTripWindow.xaml
    /// </summary>
    public partial class FilterTripWindow : Window
    {
        public FilterTripWindow()
        {
            InitializeComponent();
        }

        public string RouteFilter { get { return routeTextBox.Text; } }
        public string PriceFilter { get { return priceTextBox.Text; } }
        public string DescriptionFilter { get { return descriptionTextBox.Text; } }
        public string HotelFilter { get { return hotelTextBox.Text; } }
       /* public string StartDateFilter { get { return startDatePicker.Text; } }
        public string EndDateFilter { get { return endDatePicker.Text; } }*/

        public string GuideFilter
        {
            get
            {
                if (guideComboBox.SelectedItem != null)
                {
                    return ((ComboBoxItem)guideComboBox.SelectedItem).Content.ToString();
                }
                return string.Empty; // or any default value you prefer
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
