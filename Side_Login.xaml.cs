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
    public partial class Side_Login : Page          // Login-side 'partial' betyder at klassen er delt mellem denne kode-fil og XAML-filen
    {
        public Side_Login()                         // Konstruktør for Side_Login klassen
        {
            InitializeComponent();                  //  Initialiser UI-komponenter
            var medlemmer = Database_Medlemmer_CSV.HentAllefraCSV();      // Hent alle medlemmer fra CSV
        }
        private void Opret_Medlem_Klik(object sender, RoutedEventArgs e)        // Hændelseshåndtering for opret medlem knap
        {
            NavigationService?.Navigate(new Side_Opret_Medlem());       //  Naviger til opret medlem side
        }
        private void Side_Login_Log_ind_Klik(object sender, RoutedEventArgs e)  // Hændelseshåndtering for log ind knap
        {
            string idTekst = UsernameBox.Text;          // Hent medlems-ID fra tekstboks
            string kode = PasswordBox.Password;         //  Hent adgangskode fra password-boks

            if (!int.TryParse(idTekst, out int id))     //  Prøv at konvertere medlems-ID til heltal
            {
                MessageBox.Show("Medlems-ID skal være et tal.");    // Vis fejlmeddelelse hvis konvertering mislykkes
                return;                                 // Afslut metoden
            }

            var medlemmer = Database_Medlemmer_CSV.HentAllefraCSV();      //  Hent alle medlemmer fra CSV

            var bruger = medlemmer.FirstOrDefault(m => m.Id == id && m.Kode == kode);   //  Find medlem med matchende ID og kode

            if (bruger == null)                         // Hvis ingen bruger findes med de angivne oplysninger
            {
                MessageBox.Show("Forkert medlems-ID eller adgangskode.");   // Vis fejlmeddelelse
                return;                                 // Afslut metoden
            }
            if (bruger.ErAdmin)                         //      hvis bruger er en admin
            {
                NavigationService?.Navigate(new Side_Admin_Dashboard());    // Naviger til admin-dashboard
            }
            else
            {
                NavigationService?.Navigate(new Side_Medlem_Dashboard(bruger));// Naviger til medlem-dashboard med den aktuelle bruger
            }
        }
    }
}