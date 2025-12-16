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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1_SEFP_program_test2
{
    public partial class Side_Medlem_Dashboard : Page       // Medlem-dashboard side
    {
        private Database_Aktiviteter_CSV _aktivitetDb = null!;  // null! betyder: Jeg lover kompileren at dette bliver sat senere - fjerner advarsel
        private Klasse_Medlem _aktueltMedlem;               //  Det medlem der er logget ind
        public Side_Medlem_Dashboard(Klasse_Medlem medlem)  // Constructor med det aktuelle medlem som parameter
        {
            InitializeComponent();                      // Initialiser UI-komponenter
            _aktueltMedlem = medlem;                // Sætter det aktuelle medlem
            LoadAktiviteter();                      // Indlæser aktiviteterne
        }
        private void LoadAktiviteter()          // Metode til at indlæse aktiviteter
        {
            _aktivitetDb = new Database_Aktiviteter_CSV();      // Hent aktiviteter fra CSV
                        
            List<Klasse_Aktivitet> aktiveAktiviteter = new List<Klasse_Aktivitet>(); // Opret en tom liste til aktiviteter som ikke er gennemført

            foreach (Klasse_Aktivitet aktivitet in _aktivitetDb.Aktiviteter)        // Gennemgå alle aktiviteter
            {
                if (!aktivitet.ErGennemfoert)                   // Hvis aktiviteten IKKE er gennemført
                {
                    aktiveAktiviteter.Add(aktivitet);       // Tilføj den til listen
                }
            }
            Aktivitet_List.ItemsSource = aktiveAktiviteter; // Vis kun de aktive aktiviteter i listen
        }
        private void Tilmeld_Aktivitet_knap_klik(object sender, RoutedEventArgs e)  //  tilmeld aktivitet knap
        {
            var aktivitet = Aktivitet_List.SelectedItem as Klasse_Aktivitet;        //  Hent den valgte aktivitet
            if (aktivitet == null)                          //  Hvis ingen aktivitet er valgt
            {
                MessageBox.Show("Vælg en aktivitet først.");        //  Vis fejlmeddelelse
                return;                                         //  Afslut metoden
            }

            int medlemsId = _aktueltMedlem.Id;                  //  Hent det aktuelle medlems Id
                    
            if (!aktivitet.HarLedigPlads())                     //  Hvis aktiviteten er fuld ved '!aktivitet'
            {
                MessageBox.Show("Aktiviteten er fuld. Du kan ikke tilmelde dig.");  // Vis fejlmeddelelse
                return;                                         //  Afslut metoden
            }

            if (aktivitet.DeltagerIds.Contains(medlemsId))      //  Hvis medlemmet allerede er tilmeldt så vis denne besked
            {
                MessageBox.Show("Du er allerede tilmeldt denne aktivitet.");    //  Vis fejlmeddelelse
                return;                                         //  Afslut metoden
            }

            aktivitet.DeltagerIds.Add(medlemsId);               //  Tilføj medlems id til deltager listen
            _aktivitetDb.OpdaterAktivitet(aktivitet);           //  Gem til CSV filen

            Aktivitet_List.Items.Refresh();                     //  Opdater aktivitetsliste

            MessageBox.Show($"Du er nu tilmeldt aktiviteten: {aktivitet.Navn}");    //  Viser besked om at medlem nu er tilmeldt aktivitet
        }
        private void Medlem_logout_knap_klik(object sender, RoutedEventArgs e)  //  logout knap
        {
            NavigationService?.Navigate(new Side_Login());      //  Naviger tilbage til login siden
        }
    }
}