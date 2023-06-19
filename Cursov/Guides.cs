using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cursov
{
    public class Guides : INotifyPropertyChanged
    {
        public string g_lastName;
        public string g_firstName;
        public string g_patronymic;
        public string g_document;
        public string g_date;
        public string g_region;
        public int g_age;
        public string g_all_drives;
        public string g_date_work;
        public string g_pasport;
        public string g_phone;
        public string g_email;
        public string picture;

        public int Id { get; set; }

        public string Picture
        {
            get { return picture; }
            set
            {
                picture = value;
                OnPropertyCanged("Picture");
            }
        }

        public string G_LastName
        {
            get { return g_lastName; }
            set
            {
                g_lastName = value;
                OnPropertyCanged("LastName");
            }
        }

        public string G_FirstName
        {
            get { return g_firstName; }
            set
            {
                g_firstName = value;
                OnPropertyCanged("FirstName");
            }
        }

        public string G_Patronymic
        {
            get { return g_patronymic; }
            set
            {
                g_patronymic = value;
                OnPropertyCanged("Patronymic");
            }
        }

        public string G_Document
        {
            get { return g_document; }
            set
            {
                g_document = value;
                OnPropertyCanged("Document");
            }
        }
        public string G_Date
        {
            get { return g_date; }
            set
            {
                g_date = value;
                OnPropertyCanged("Company");
            }
        }
        public string G_Region
        {
            get { return g_region; }
            set
            {
                g_region = value;
                OnPropertyCanged("Region");
            }
        }
        public string G_Pasport
        {
            get { return g_pasport; }
            set
            {
                g_pasport = value;
                OnPropertyCanged("Pasport");
            }
        }
        public string G_Phone
        {
            get { return g_phone; }
            set
            {
                g_phone = value;
                OnPropertyCanged("Phone");
            }
        }
        public string G_Email
        {
            get { return g_email; }
            set
            {
                g_email = value;
                OnPropertyCanged("Email");
            }
        }
        public int G_Age
        {
            get { return g_age; }
            set
            {
                g_age = value;
                OnPropertyCanged("Age");
            }
        }

        public string G_All_Drives
        {
            get { return g_all_drives; }
            set
            {
                g_all_drives = value;
                OnPropertyCanged("All Drives");
            }
        }

        private int g_count_drives;

        public int G_Count_Drives
        {
            get { return g_count_drives; }
            set
            {
                g_count_drives = value;
                OnPropertyCanged("G_Count_Drives");
            }
        }

        // ...existing methods...

        public void UpdateDriveCount()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Count the number of trips associated with the guide's ID
                int count = db.Trip.Count(t => t.GuideID == Id);

                // Update the G_Count_Drives property
                G_Count_Drives = count;
            }
        }

        public string G_Date_Work
        {
            get { return g_date_work; }
            set
            {
                g_date_work = value;
                OnPropertyCanged("Date_Work");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyCanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
