using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1_SEFP_program_test2
{
    class Database_Aktiviteter_CSV // klasse til database af aktiviteter i csv fil
    {
        private readonly string _filSti = "aktiviteter.csv"; // aktiviteter.csv får navnet _filSti som string

        public List<Klasse_Aktivitet> Aktiviteter { get; private set; } = new List<Klasse_Aktivitet>(); // opret en tom liste til aktiviteter

        // Constructor: Når vi laver databasen → hent data fra filen
        public Database_Aktiviteter_CSV() // når en instans af klassen bliver oprettet
        {
            HentFraFil(); // kald metoden HentFraFil for at indlæse aktiviteter fra filen
        }
        private void HentFraFil() // privat metode til at hente aktiviteter fra fil
        {
            Aktiviteter.Clear(); // ryd listen for at undgå dubletter ved genindlæsning

            if (!File.Exists(_filSti)) // 
                return; // filen findes ikke endnu → ingen fejl

            var linjer = File.ReadAllLines(_filSti); // læs alle linjer fra filen

            foreach (var linje in linjer) // gennemgå hver linje
            {
                if (string.IsNullOrWhiteSpace(linje)) // hvis linjen er tom eller kun indeholder mellemrum
                    continue; // spring den over

                try // forsøg at konvertere linjen til en aktivitet
                {
                    var aktivitet = Klasse_Aktivitet.FraCsvLinje(linje); // konverter linjen til en aktivitet
                    Aktiviteter.Add(aktivitet); // tilføj aktiviteten til listen
                }
                catch // hvis der opstår en fejl under konvertering
                {
                    // Hvis en linje er ødelagt, spring den over
                }
            }
        }
        private void GemTilFil() // privat metode til at gemme aktiviteter til fil
        {
            var linjer = Aktiviteter // brug LINQ til at konvertere hver aktivitet til en CSV-linje
                .Select(a => a.TilCsvLinje()) // konverter hver aktivitet til en CSV-linje
                .ToArray(); // konverter resultatet til et array af strenge

            File.WriteAllLines(_filSti, linjer); // gem alle linjer til filen
        }
        private int FindNaesteId() // privat metode til at finde det næste ledige Id
        {
            if (!Aktiviteter.Any()) // hvis listen er tom
                return 1; // start med Id 1

            return Aktiviteter.Max(a => a.Id) + 1; // returner det højeste Id + 1
        }
        public Klasse_Aktivitet OpretAktivitet_i_DB(string navn, int maxDeltagere) // offentlig metode til at oprette en ny aktivitet
        {
            var aktivitet = new Klasse_Aktivitet    // opret en ny aktivitet
            {
                Id = FindNaesteId(),    //  find det næste ledige Id
                Navn = navn, // sæt navnet
                MaxDeltagere = maxDeltagere // sæt maks deltagere
            };

            Aktiviteter.Add(aktivitet); // tilføj aktiviteten til listen
            GemTilFil(); // gem listen til fil

            return aktivitet; // returner den oprettede aktivitet
        }
        public Klasse_Aktivitet? FindAktivitetMedId(int id) // offentlig metode til at finde en aktivitet ved Id
        {
            return Aktiviteter.FirstOrDefault(a => a.Id == id); // returner den første aktivitet med det givne Id eller null hvis ikke fundet
        }
        public bool SletAktivitet(Klasse_Aktivitet aktivitet) // offentlig metode til at slette en aktivitet
        {
            bool fjernet = Aktiviteter.Remove(aktivitet); // forsøg at fjerne aktiviteten fra listen

            if (fjernet) // hvis aktiviteten blev fjernet
            {
                GemTilFil(); // gem listen til fil
            }
            return fjernet; // returner om aktiviteten blev fjernet
        }
        public void OpdaterAktivitet(Klasse_Aktivitet aktivitet) // offentlig metode til at opdatere en aktivitet
        {
            // Aktiviteten findes allerede → vi behøver kun gemme hele listen igen
            GemTilFil(); // gem listen til fil
        }
        public void Genindlaes() // offentlig metode til at genindlæse aktiviteter fra fil
        {
            HentFraFil(); // kald metoden HentFraFil for at indlæse aktiviteter fra filen
        }
    }
}
