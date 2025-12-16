using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Side_Admin_Dashboard : Page        // Admin-dashboard side 'partial' betyder at klassen er delt mellem denne kode-fil og XAML-filen
    {
        private ObservableCollection<Klasse_Medlem> Medlemmer { get; set; }     // Medlemsliste som ObservableCollection for automatisk UI-opdatering
                                                                                // UI opdateres automatisk hvori listbox skal du selv kalde Items.Refresh()
        private readonly Database_Aktiviteter_CSV _aktivitetDb;  // Database til aktiviteter
        public Side_Admin_Dashboard()           // Constructor
        {
            InitializeComponent();          // Initialiser UI-komponenter

            var alleMedlemmer = Database_Medlemmer_CSV.HentAllefraCSV();          // Hent alle medlemmer fra CSV
            Medlemmer = new ObservableCollection<Klasse_Medlem>(alleMedlemmer); // Opret ObservableCollection med medlemmer
            Medlem_ListBox.ItemsSource = Medlemmer;                     // Bind medlems-listen til UI

             _aktivitetDb = new Database_Aktiviteter_CSV();           // Hent aktiviteter fra CSV og vis dem i aktivitets-listen
            Aktivitet_ListBox.ItemsSource = _aktivitetDb.Aktiviteter;       // Bind aktivitets-listen til UI -> Aktivitet_ListBox.ItemsSource = _aktivitetDb.Aktiviteter;
        }
        // Når admin vælger en aktivitet → opdater deltager-listen
        private void Aktivitet_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)   // Hændelseshåndtering for valg af aktivitet
        {
            var valgtAktivitet = Aktivitet_ListBox.SelectedItem as Klasse_Aktivitet; // Hent den valgte aktivitet

            if (valgtAktivitet == null)     // Hvis ingen aktivitet er valgt
            {
                Aktivitet_Deltager_ListBox.ItemsSource = null;          // Ryd deltager-listen
                return;                                 // Afslut metoden
            }
            // Find de medlemmer, hvis Id findes i aktivitetens DeltagerIds
            var deltagendeMedlemmer = Medlemmer         //  Brug LINQ til at filtrere medlemmer
                .Where(m => valgtAktivitet.DeltagerIds.Contains(m.Id))  // Find medlemmer der deltager i den valgte aktivitet
                .ToList();                           // Konverter resultatet til en liste

            Aktivitet_Deltager_ListBox.ItemsSource = deltagendeMedlemmer;   // Opdater deltager listen
        }
        // Klik på "+ Opret aktivitet"
        private void Opret_Aktivitet_Klik(object sender, RoutedEventArgs e)         // Hændelseshåndtering for opret aktivitet knap
        {
            NavigationService?.Navigate(new Side_Opret_Aktivitet());      // Naviger til opret aktivitet side
        }
        private void Slet_Aktivitet_Klik(object sender, RoutedEventArgs e)          // Hændelseshåndtering for slet aktivitet knap
        {
            var valgtAktivitet = Aktivitet_ListBox.SelectedItem as Klasse_Aktivitet; // Hent den valgte aktivitet
            if (valgtAktivitet == null)         // Hvis ingen aktivitet er valgt
            {
                MessageBox.Show("Vælg en aktivitet først.");    // Vis fejlmeddelelse hvis ingen aktivitet er valgt
                return;         // Afslut metoden
            }

            var resultat = MessageBox.Show(                 //  Vis bekræftelsesdialog
                $"Vil du slette aktiviteten '{valgtAktivitet.Navn}'?",  //  'Er du sikker' spørgsmål
                "Bekræft sletning",                         
                MessageBoxButton.YesNo,                     //      ja / nej knapper
                MessageBoxImage.Warning);                   //      advarselsikon
            if (resultat != MessageBoxResult.Yes)           //      Hvis brugeren ikke bekræfter sletning
                return;                                     //      Afslut metoden

            bool fjernet = _aktivitetDb.SletAktivitet(valgtAktivitet);  // Slet aktiviteten fra databasen

            if (!fjernet)               // Hvis sletning mislykkedes
            {
                MessageBox.Show("Kunne ikke slette aktiviteten.");      // Vis fejlmeddelelse
                return;
            }
            Aktivitet_ListBox.Items.Refresh();          //  Opdater aktivitets listen
            Aktivitet_Deltager_ListBox.ItemsSource = null;      //  Ryd deltager-listen
        }
        private void Slet_medlem_Klik(object sender, RoutedEventArgs e) // funktion for slet medlem knap
        {
            var valgt = Medlem_ListBox.SelectedItem as Klasse_Medlem;       // Hent det valgte medlem
            if (valgt == null)                      //  Hvis intet medlem er valgt
            {
                MessageBox.Show("Vælg et medlem først.");  // Vis meddelelse
                return;                             // Afslut metoden
            }
            var resultat = MessageBox.Show(         // Vis bekræftelsesdialog
                $"Vil du slette medlemmet '{valgt.Navn}' (ID: {valgt.Id})?\n\n" +   //  'er du sikker' spørgsmål
                "Medlemmet vil også blive fjernet fra alle aktiviteter.",           //   mere tekst til advarsel
                "Bekræft sletning",                                                 // bekræft sletning
                MessageBoxButton.YesNo,                                             //  ja / nej knapper
                MessageBoxImage.Warning);                                            // advarselsikon

            if (resultat != MessageBoxResult.Yes)                                      //   Hvis brugeren ikke bekræfter sletning
                return;                                                             //  Afslut metoden
            
            foreach (var aktivitet in _aktivitetDb.Aktiviteter) // 1) Fjern medlemmet fra ALLE aktiviteter ved at bruge id
            {
                if (aktivitet.DeltagerIds.Remove(valgt.Id))         //  Hvis medlem blev fjernet fra denne aktivitet
                {
                    _aktivitetDb.OpdaterAktivitet(aktivitet);   // Gem ændringerne til csv filen
                }
            }
            
            Medlemmer.Remove(valgt);                    // 2) Fjern medlemmet fra medlemslisten
            Database_Medlemmer_CSV.SaveAll(Medlemmer.ToList()); //  Gem ændringerne til csv filen
            Medlem_ListBox.Items.Refresh();                 //  Opdater medlems listen
            Aktivitet_ListBox.Items.Refresh();              //  opdater aktivitets listen

            var valgtAktivitetEfter = Aktivitet_ListBox.SelectedItem as Klasse_Aktivitet; // Opdater deltager listen
            if (valgtAktivitetEfter != null)                //  Hvis en aktivitet er valgt
            {
                var deltagendeMedlemmer = Medlemmer         //  Brug LINQ til at filtrere medlemmer
                    .Where(m => valgtAktivitetEfter.DeltagerIds.Contains(m.Id))     //  Find medlemmer der deltager i den valgte aktivitet
                    .ToList();                              //  Konverter resultatet til en liste
                Aktivitet_Deltager_ListBox.ItemsSource = deltagendeMedlemmer;  //  Opdater deltager listen  
            }
            else                                           //  Hvis ingen aktivitet er valgt
            {
                Aktivitet_Deltager_ListBox.ItemsSource = null;         // Ryd deltager listen
            }
            MessageBox.Show(                                //  Vis bekræftelsesmeddelelse
                $"Medlem '{valgt.Navn}' (ID: {valgt.Id}) er nu slettet og fjernet fra alle aktiviteter.");  //  Meddelelse om succesfuld sletning
        }
        private void Log_ud_Klik(object sender, RoutedEventArgs e)  // // Klik på "Log ud"
        {
            NavigationService?.Navigate(new Side_Login());      // Naviger tilbage til login siden
        }
    }
}