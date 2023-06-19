using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cursov
{
    public class Login : INotifyPropertyChanged
    {
        [Key] // Add this attribute to specify the primary key
        public int Id { get; set; }
        private string job_title;
        private string password;
        private string email;


        public string Job_title
        {
            get { return job_title; }
            set
            {
                job_title = value;
                OnPropertyCanged("Job_title");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyCanged("Password");
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyCanged("Email");
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
