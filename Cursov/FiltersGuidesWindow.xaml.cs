﻿using System;
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
    /// Логика взаимодействия для FiltersGuidesWindow.xaml
    /// </summary>
    public partial class FiltersGuidesWindow : Window
    {
        public FiltersGuidesWindow()
        {
            InitializeComponent();
        }

        public string NameFilter { get { return NameTextBox.Text; } }
        public string SurnameFilter { get { return SurnameTextBox.Text; } }
        public string PatronymicFilter { get { return PatronymicTextBox.Text; } }
        public string PassportIdFilter { get { return PassportIdTextBox.Text; } }
        public string AgeFilter { get { return AgeTextBox.Text; } }
        public string EmailFilter { get { return EmailTextBox.Text; } }
        public string PhoneFilter { get { return PhoneTextBox.Text; } }
        public string RegionFilter { get { return RegionTextBox.Text; } }
        public string CountDrivesFilter { get { return CountDrivesTextBox.Text; } }
        
        public string ExperienceFilter { get { return ExperienceTextBox.Text; } }

        public bool PassFilter { get { return PassCheckBox.IsChecked ?? false; } }

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
