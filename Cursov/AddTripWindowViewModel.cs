using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursov
{
    public class AddTripWindowViewModel : INotifyPropertyChanged
    {
        private Trip trip;
        private List<Guides> guides;
        private List<Client> clients;

        public string ClientLastNames
        {
            get
            {
                if (Clients != null)
                {
                    return string.Join(", ", Clients.Select(c => c.LastName));
                }
                return string.Empty;
            }
        }

        public AddTripWindowViewModel(Trip trip)
        {
            this.trip = trip;
            LoadData();
        }

        public Trip Trip
        {
            get { return trip; }
            set
            {
                trip = value;
                OnPropertyChanged(nameof(Trip));
            }
        }

        public List<Guides> Guides
        {
            get { return guides; }
            set
            {
                guides = value;
                OnPropertyChanged(nameof(Guides));
            }
        }

        public List<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        private void LoadData()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Guides = db.Guides.ToList();
                Clients = db.Clients.ToList();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
