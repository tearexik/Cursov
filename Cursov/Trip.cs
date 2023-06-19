using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cursov
{
    public class Trip : INotifyPropertyChanged
    {
        private string route;
        private List<int> clientIds = new List<int>();
        private DateTime startDate;
        private DateTime endDate;
        private string description;
        private string hotel;

        public List<Client> Client
        {
            get
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    return db.Clients.Where(c => ClientIDs.Contains(c.Id)).ToList();
                }
            }
        }

        public int Id { get; set; }
        
        [Column("Route")]
        public string Route
        {
            get { return route; }
            set
            {
                route = value;
                OnPropertyCanged("Route");
            }
        }

        private double? price;

        public double? Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyCanged("Price");
            }
        }

        private int? guideId;

        public int? GuideID
        {
            get { return guideId; }
            set
            {
                guideId = value;
                OnPropertyCanged("GuideID");
            }
        }

        public List<int> ClientIDs
        {
            get { return clientIds; }
            set
            {
                clientIds = value;
                OnPropertyCanged("ClientIDs");
                OnPropertyCanged("NumberOfClients");
            }
        }

        public int NumberOfClients => ClientIDs?.Count ?? 0;

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyCanged("StartDate");
                OnPropertyCanged("LengthOfStay");
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyCanged("EndDate");
                OnPropertyCanged("LengthOfStay");
            }
        }

        public int LengthOfStay => (EndDate - StartDate).Days;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyCanged("Description");
            }
        }

        public string Hotel
        {
            get { return hotel; }
            set
            {
                hotel = value;
                OnPropertyCanged("Hotel");
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
