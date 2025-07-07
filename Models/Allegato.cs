using System.ComponentModel.DataAnnotations;

namespace WhistleblowingApp.Models
{
    public class Allegato
    {
        public int Id { get; set; }

        [Required]
        public string NomeFile { get; set; } = string.Empty;

        public string TipoMime { get; set; } = string.Empty;

        public long Dimensione { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.Now;

        public int SegnalazioneId { get; set; }

        public Segnalazione? Segnalazione { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}