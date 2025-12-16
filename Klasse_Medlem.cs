using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1_SEFP_program_test2
{
    public class Klasse_Medlem  //  klasse til medlemmer
    {
        public int Id { get; set; }   // unikt id for medlemmet 
        public string Navn { get; set; }        //  navn på medlemmet som string
        public string Kode { get; set; }       //  kode for medlemmet som string
        public bool ErAdmin { get; set; }       //  tjek om medlemmet er admin
        public string Visning               // visningsstreng for medlemmet
        {
            get {return $"ID: {Id}, Navn {Navn}"; }      //  getter for visningsstreng
        }
        public Klasse_Medlem(int id, string navn, string kode, bool erAdmin = false) // constructor for Klasse_Medlem
        {   
            Id = id;                // sæt id
            Navn = navn;             // sæt navn
            Kode = kode;             // sæt kode
            ErAdmin = erAdmin;      // sæt erAdmin
        }
        // CSV: Id;Navn;Kode;ErAdmin
        public override string ToString()   // override af ToString-metoden
        {
            return Id + ";" + Navn + ";" + Kode + ";" + ErAdmin;  // returner CSV-linje i dette format
        }

        public static Klasse_Medlem FromCsv(string line)  // konverter CSV-linje til medlem
        {
            string[] parts = line.Split(';');       //  Del linjen op i felter ved ';'

            // Tjek at formatet er korrekt
            if (parts.Length != 4)      // hvis antallet af dele ikke er 4
            {
                throw new FormatException(          // kast en FormatException
                    "Forkert format i medlemmer.csv. Forventet: Id;Navn;Kode;ErAdmin"  // fejlbesked
                );
            }
            int id = int.Parse(parts[0]);       //  læs Id
            string navn = parts[1];             //  læs Navn
            string kode = parts[2];             //  læs Kode
            bool erAdmin = bool.Parse(parts[3]);//  læs ErAdmin

            return new Klasse_Medlem(id, navn, kode, erAdmin);      //  opret nyt medlem og returner det
        }
    }
}