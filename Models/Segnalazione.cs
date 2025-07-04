namespace WhistleblowingApp.Models
{
    public class Segnalazione
    {
        required public int Id { get; set; }

        public string Codice { get; set; } = string.Empty;

        required public string Descrizione { get; set; }
        public string? PersoneCoinvolte { get; set; }
        public string? AltriAConoscenza { get; set; }

        public DateTime? DataIllecito { get; set; }
        public TimeSpan? OraIllecito { get; set; }

        required public bool RischioRitorsione { get; set; }
        required public bool PericoloImminente { get; set; }

        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public string Stato { get; set; } = "In attesa";
        public DateTime DataCreazione { get; set; } = DateTime.Now;
        public ICollection<MessaggioChat> MessaggiChat { get; set; } = new List<MessaggioChat>();
    }
}