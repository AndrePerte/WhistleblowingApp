namespace WhistleblowingApp.Models
{
    public class Segnalazione
    {
        required public int Id { get; set; }

        public string? Codice { get; set; } // Codice unico a 10 cifre

        required public string Descrizione { get; set; }
        public string? PersoneCoinvolte { get; set; }
        public string? AltriConoscenza { get; set; }

        public DateTime? DataIllecito { get; set; }

        required public bool RischioRitorsione { get; set; }
        required public bool PericoloImminente { get; set; }

        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public string Stato { get; set; } = "In attesa";
        public DateTime DataCreazione { get; set; } = DateTime.Now;
    }
}