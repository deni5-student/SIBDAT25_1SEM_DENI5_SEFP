using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1_SEFP_program_test2
{
    internal static class Database_Medlemmer_CSV // intern statisk klasse til database af medlemmer i csv fil
    {
        private static readonly string FilSti = "medlemmer.csv"; // medlemmer.csv får navnet 'FilSti' som string
        public static List<Klasse_Medlem> HentAllefraCSV() // Henter alle medlemmer fra CSV
        {
            
            if (!File.Exists(FilSti)) // Hvis filen ikke findes, opret admin
            {
                Klasse_Medlem admin = new Klasse_Medlem(1, "admin", "1", true); // Opret admin med id 1, navn "admin", kode "1" og ErAdmin = true
                File.WriteAllText(FilSti, admin.ToString() + Environment.NewLine);  // Gem admin i filen
            }

            List<Klasse_Medlem> medlemmer = new List<Klasse_Medlem>(); // opret en tom liste til medlemmer

            string[] linjer = File.ReadAllLines(FilSti);    // læs alle linjer fra filen

            foreach (string linje in linjer)    // gennemgå hver linje
            {
                if (linje != "")    // hvis linjen ikke er tom
                {
                    medlemmer.Add(Klasse_Medlem.FromCsv(linje)); // konverter linjen til et medlem og tilføj til listen
                }
            }
            return medlemmer; // returner listen af medlemmer
        }
        public static void SaveAll(List<Klasse_Medlem> medlemmer)        // Gem alle medlemmer
        {
            List<string> linjer = new List<string>(); // opret en tom liste til linjer

            foreach (Klasse_Medlem medlem in medlemmer) // foreach gennemgår hver medlem i listen navngivet medlemmer
            {
                linjer.Add(medlem.ToString()); // tilføjer hver medlem til csv
            }

            File.WriteAllLines(FilSti, linjer); // gemmer alle linjer til filen
        }
        public static void AddMember(Klasse_Medlem medlem) // Tilføj et nyt medlem
        {
            List<Klasse_Medlem> medlemmer = HentAllefraCSV(); // finder/henter alle medlemmer

            int nextId = medlemmer.Count + 1; // tager næste id ved at tælle medlemmer og lægge 1 til
            medlem.Id = nextId; // sætter det nye medlems id til næste id

            File.AppendAllText(FilSti, medlem.ToString() + Environment.NewLine); // tilføjer det nye medlem til filen
        }
    }
}
