using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1_SEFP_program_test2
{
    public class Klasse_Aktivitet // klasse til aktiviteter
    {
        public int Id { get; set; }     // unikt id for aktiviteten
        public string Navn { get; set; } = "";  // navn på aktiviteten
        public int MaxDeltagere { get; set; } // maks antal deltagere
        public List<int> DeltagerIds { get; set; } = new List<int>(); // liste af deltager-id'er
        public DateTime Dato { get; set; } = DateTime.Today; // dato for aktiviteten

        public string Visning   // visningsstreng for aktiviteten
        {
            get // getter for visningsstreng
            {
                return $"{Dato:dd-MM-yyyy} - {Navn} ({DeltagerIds.Count}/{MaxDeltagere})"; // format visning
            }
        }
        public bool ErGennemfoert // tjek om aktiviteten er gennemført
        {
            get //  getter for gennemført-status
            {
                return Dato.Date < DateTime.Today; // returner sandt hvis datoen er før i dag
            }
        }

        public bool HarLedigPlads() // tjek om der er ledig plads
        {
            return DeltagerIds.Count < MaxDeltagere; // returner sandt hvis antal deltagere er mindre end maks deltagere
        }
        public override string ToString() // override af ToString-metoden
        {
            return $"{Navn} ({DeltagerIds.Count}/{MaxDeltagere})"; // returner navn og antal deltagere
        }
        // CSV: Id;Navn;MaxDeltagere;id1|id2|id3
        public string TilCsvLinje() // konverter aktivitet til CSV-linje
        {
            string idListe = string.Join('|', DeltagerIds); // join deltager-id'er med '|'
            return $"{Id};{Navn};{MaxDeltagere};{Dato:yyyy-MM-dd};{idListe}"; // returner CSV-linje i dette format
        }
        public string VisningMedStatus // visningsstreng med gennemført-status
        {
            get //  getter for visningsstreng med status
            {
                return ErGennemfoert // hvis aktiviteten er gennemført
                    ? $"[Gennemført] {Visning}" //  returner med [Gennemført] prefix
                    : Visning; // ellers returner almindelig visning
            }
        }
        public static Klasse_Aktivitet FraCsvLinje(string line) // konverter CSV-linje til aktivitet
        {
            string[] parts = line.Split(';'); //   Del linjen op i felter ';'

            Klasse_Aktivitet aktivitet = new Klasse_Aktivitet(); // opret ny aktivitet

            aktivitet.Id = int.Parse(parts[0]); // læs Id
            aktivitet.Navn = parts[1];          // læs Navn
            aktivitet.MaxDeltagere = int.Parse(parts[2]); // læs MaxDeltagere
            aktivitet.Dato = DateTime.Parse(parts[3]); // læs Dato

            if (parts.Length >= 5 && !string.IsNullOrWhiteSpace(parts[4])) // tjek om der er deltager-id'er, hvis der er nogen
            {
                string[] deltagerTekst = parts[4].Split('|'); // deler deltager-id'er op ved '|'

                foreach (string id in deltagerTekst) // foreach gennemgår hver deltager id
                {
                    aktivitet.DeltagerIds.Add(int.Parse(id)); // tilføjer deltager id
                }
            }
            return aktivitet; // returner den oprettede aktivitet
        }
    }
}
