using System;

namespace WhistleblowingApp.Models
{
    public class MessaggioChat
    {
        public int Id { get; set; }
        public string Testo { get; set; } = string.Empty;
        public string Mittente { get; set; } = "Operatore";
        public DateTime DataInvio { get; set; } = DateTime.Now;

        public int SegnalazioneId { get; set; }
        public Segnalazione? Segnalazione { get; set; }
    }
}