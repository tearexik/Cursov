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
using System.Data.SqlClient;

namespace Cursov
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    /// 
    public partial class LoginWindow : Window
    {
        private Dictionary<string, string> guideCredentials;
        private Dictionary<string, string> managerCredentials;

        public LoginWindow()
        {
            InitializeComponent();
            LoadCredentialsFromDatabase();
        }

        private void LoadCredentialsFromDatabase()
        {
            guideCredentials = new Dictionary<string, string>();
            managerCredentials = new Dictionary<string, string>();

            using (ApplicationContext db = new ApplicationContext())
            {
                // Query the Log table to retrieve the credentials
                var logEntries = db.Login.ToList();

                foreach (var entry in logEntries)
                {
                    string email = entry.Email;
                    string password = entry.Password;
                    string jobTitle = entry.Job_title;

                    if (jobTitle == "Guide")
                    {
                        guideCredentials[email] = password;
                    }
                    else if (jobTitle == "Manager")
                    {
                        managerCredentials[email] = password;
                    }
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = new System.Net.NetworkCredential(string.Empty, PasswordBox.SecurePassword).Password;

            if (guideCredentials.ContainsKey(email) && guideCredentials[email] == password)
            {
                MainWindow guideMainWindow = new MainWindow("Guide");
                guideMainWindow.Show();
                this.Hide();
            }
            else if (managerCredentials.ContainsKey(email) && managerCredentials[email] == password)
            {
                MainWindow managerMainWindow = new MainWindow("Manager");
                managerMainWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
