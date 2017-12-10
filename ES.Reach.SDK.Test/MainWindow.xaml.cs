using ES.Reach.SDK.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows;

namespace ES.Reach.SDK.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var reachClient = new ReachClient("43B62B22-59B2-4AF1-BF51-EFBF8DA35402", "84EBBCAA-31E3-46B0-B7ED-9CEF925A85F7");
            var nearbyIncidents = await reachClient.GlobalContext.GetNearbyIncidents(-12.051299, -77.064956, 1);

            var result1 = JsonConvert.SerializeObject(nearbyIncidents, Formatting.Indented);

            var incidentId = nearbyIncidents.First().Id;
            var incident = await reachClient.GlobalContext.GetIncidentDetail(incidentId);

            var result2 = JsonConvert.SerializeObject(incident, Formatting.Indented);

            var incidentTypes = await reachClient.GlobalContext.GetIncidentTypes();
            var result3 = JsonConvert.SerializeObject(incidentTypes, Formatting.Indented);

            var roadTypes = await reachClient.GlobalContext.GetRoadTypes();
            var result4 = JsonConvert.SerializeObject(roadTypes, Formatting.Indented);

            var countries = await reachClient.GlobalContext.GetCountries();
            var result5 = JsonConvert.SerializeObject(countries, Formatting.Indented);

            //await reachClient.GlobalContext.RegisterSOSAlert(new SOSAlert
            //{
            //    CNC = 604,
            //    Location = new GPSLocation
            //    {
            //        Lat = -12.115032,
            //        Lng = -77.046044
            //    },
            //    Sender = new User
            //    {
            //        FullName = "Desafío D - Reach",
            //        Nickname = "Seguridad Ciudadana",
            //        CountryCode = "51",
            //        PhoneNumber = "945442432"
            //    }
            //});

            var foo = JsonConvert.SerializeObject(new SOSAlert
            {
                CNC = 604,
                Location = new GPSLocation
                {
                    Lat = -12.115032,
                    Lng = -77.046044,
                    UTC = DateTime.UtcNow
                },
                Sender = new User
                {
                    FullName = "Desafío D - Reach",
                    Nickname = "Seguridad Ciudadana",
                    CountryCode = "51",
                    PhoneNumber = "945442432"
                }
            }, Formatting.Indented);
        }
    }
}