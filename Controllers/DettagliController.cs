using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhistleblowingApp.Models;
using WhistleblowingApp.Data;

namespace WhistleblowingApp.Controllers
{
    [Route("dettagli")]
    public class DettagliController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DettagliController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("invia-messaggio")]
        public IActionResult InviaMessaggio(int segnalazioneId, string testo)
        {
            if (string.IsNullOrWhiteSpace(testo)) return BadRequest();

            var messaggio = new MessaggioChat
            {
                Testo = testo,
                Mittente = "Utente",
                SegnalazioneId = segnalazioneId
            };

            _context.ChatMessaggi.Add(messaggio);
            _context.SaveChanges();

            return RedirectToAction("Dettagli", "Segnalazione", new { id = segnalazioneId });
        }

        [HttpPost("carica-file")]
        public async Task<IActionResult> CaricaFile(int segnalazioneId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nessun file selezionato");

            if (file.Length > 25 * 1024 * 1024)
                return BadRequest("Il file supera i 25 MB");

            var segnalazione = _context.Segnalazioni.Include(s => s.Allegati).FirstOrDefault(s => s.Id == segnalazioneId);

            if (segnalazione == null)
                return NotFound();

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
            var fileName = $"{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var allegato = new Allegato
            {
                NomeFile = file.FileName,
                TipoMime = file.ContentType,
                Dimensione = file.Length,
                DataCaricamento = DateTime.Now,
                SegnalazioneId = segnalazioneId,
                FilePath = Path.Combine("files", fileName)
            };

            _context.Allegati.Add(allegato);
            _context.SaveChanges();

            TempData["NomeFile"] = allegato.NomeFile;

            return RedirectToAction("Dettagli", "Segnalazione", new { id = segnalazioneId });
        }

        [HttpPost("delete-attachment")]
        public IActionResult DeleteAttachment(int id)
        {
            var allegato = _context.Allegati.FirstOrDefault(a => a.Id == id);

            if (allegato == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + allegato.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Allegati.Remove(allegato);
            _context.SaveChanges();

            return RedirectToAction("Dettagli", "Segnalazione", new { id = allegato.SegnalazioneId });
        }

        [HttpPost("update-details")]
        public IActionResult UpdateDetails(Segnalazione model)
        {
            var segnalazione = _context.Segnalazioni.Find(model.Id);
            if (segnalazione == null) return NotFound();

            segnalazione.Nome = model.Nome;
            segnalazione.Cognome = model.Cognome;
            segnalazione.Email = model.Email;
            segnalazione.Telefono = model.Telefono;

            _context.SaveChanges();
            return RedirectToAction("Dettagli", "Segnalazione", new { id = model.Id });
        }
    }
}