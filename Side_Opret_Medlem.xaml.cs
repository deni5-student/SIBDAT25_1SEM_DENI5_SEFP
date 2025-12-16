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
    public partial class Side_Opret_Medlem : Page       // Opret medlem side
    {
        public Side_Opret_Medlem()                  // Constructor
        {
            InitializeComponent();                  // Initialiser UI-komponenter
            NavnTextBox.Text = "";                  // Ryd tekstboksen for navn 
            Adgangskode_TextBox.Text = "";          // Ryd tekstboksen for adgangskode
        }
        private void Opret_Nyt_Medlem_Knap(object sender, RoutedEventArgs e)    // Opret nyt medlem knap
        {
            string navn = NavnTextBox.Text;                              // Hent navn
            string kode = Adgangskode_TextBox.Text;                      // Hent adgangskode

            if (string.IsNullOrWhiteSpace(navn) || string.IsNullOrWhiteSpace(kode)) // Tjek for tomme felter
            {
                MessageBox.Show("Udfyld både navn og adgangskode.");    // Vis fejlmeddelelse
                return;
            }

            var nytMedlem = new Klasse_Medlem(0, navn, kode, false); // Opret nyt medlem (Id sættes til 0 midlertidigt)

            Database_Medlemmer_CSV.AddMember(nytMedlem);   // Tilføj medlem til databasen

            MessageBox.Show($"Bruger oprettet!\nDit medlems-ID er: {nytMedlem.Id}"); // Vis bekræftelsesmeddelelse med medlems-ID

            NavigationService?.Navigate(new Side_Login());  // Naviger tilbage til login siden
        }
        private void Cancel(object sender, RoutedEventArgs e)   // Annuller knap
        {
            NavigationService?.Navigate(new Side_Login()); // Naviger tilbage til login siden
        }
    }
}