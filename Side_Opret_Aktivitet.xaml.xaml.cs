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
    public partial class Side_Opret_Aktivitet : Page    // Opret aktivitet side
    {
        public Side_Opret_Aktivitet()                   // Constructor
        {
            InitializeComponent();                      // Initialiser UI-komponenter
            Ny_aktivitet_TextBox.Text = "";             // Ryd tekstboksen for aktivitetens navn
            Max_Deltagere_TextBox.Text = "";            // Ryd tekstboksen for maks deltagere
        }

        private Database_Aktiviteter_CSV _aktivitetDb = new Database_Aktiviteter_CSV(); // Hent aktiviteter fra CSV

        private void Opret_Aktivitet_Klik(object sender, RoutedEventArgs e) // opret aktivitet knap
        {
            string navn = Ny_aktivitet_TextBox.Text;            // Hent aktivitetens navn

            if (!int.TryParse(Max_Deltagere_TextBox.Text, out int max))     // Prøv at konvertere maks deltagere til heltal
            {
                MessageBox.Show("Maks deltagere skal være et tal.");        // Vis fejlmeddelelse hvis konvertering mislykkes
                return;                                                 // Afslut metoden
            }

            DateTime dato = Aktivitet_DatoPicker.SelectedDate ?? DateTime.Today;    // Hent den valgte dato eller brug i dag som standard

            var aktivitet = new Klasse_Aktivitet                    // Opret en ny aktivitet
            {
                Navn = navn,                                        // Sæt navn
                MaxDeltagere = max,                                 // Sæt maks deltagere
                Dato = dato                                         // Sæt dato
            };

            _aktivitetDb.OpretAktivitet_i_DB(aktivitet.Navn, aktivitet.MaxDeltagere);   // Opret aktiviteten i databasen
            _aktivitetDb.Aktiviteter.Last().Dato = dato;                // Sæt dato bagefter
            _aktivitetDb.OpdaterAktivitet(_aktivitetDb.Aktiviteter.Last()); // Opdater aktiviteten i databasen

            MessageBox.Show("Aktivitet oprettet!");                     // Vis bekræftelsesmeddelelse
            NavigationService?.Navigate(new Side_Admin_Dashboard());    // Naviger tilbage til admin-dashboard
        }
        private void Aktivitet_DatoPicker_Loaded(object sender, RoutedEventArgs e)  // Dato-picker indlæst
        {
            Aktivitet_DatoPicker.BlackoutDates.Add(                     // Deaktiver alle datoer før i dag
                new CalendarDateRange(                                  // Opret et nyt datointerval
                    DateTime.MinValue,                                  // Start fra den tidligste dato
                    DateTime.Today.AddDays(-1)                          // igår
                )
            );
            Aktivitet_DatoPicker.DisplayDate = DateTime.Today;          // Start visning på i dag
        }
        private void Cancel(object sender, RoutedEventArgs e)       // Annuller knap
        {
            NavigationService?.Navigate(new Side_Admin_Dashboard());    // Naviger tilbage til admin-dashboard
        }
    }
}