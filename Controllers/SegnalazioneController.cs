using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhistleblowingApp.Models;
using WhistleblowingApp.Data;
//using WhistleblowingApp.Helpers;

namespace WhistleblowingApp.Controllers
{
    public class SegnalazioneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SegnalazioneController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Segnalazione segnalazione)
        {
            Console.WriteLine("Submit() invocato");
            Console.WriteLine($"ModelState IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Errore di validazione:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View("Create", segnalazione);
            }

            try
            {
                segnalazione.Codice = CodeGenerator.GenerateUniqueCode(_context);
                segnalazione.DataCreazione = DateTime.Now;
                segnalazione.Stato = "In attesa";

                _context.Segnalazioni.Add(segnalazione);
                _context.SaveChanges();

                var messaggio = new MessaggioChat
                {
                    Testo = "Salve, le chiedo di allegare una screenshot nell'apposito riquadro qui sopra e darmi conferma.",
                    Mittente = "Operatore",
                    SegnalazioneId = segnalazione.Id
                };
                _context.ChatMessaggi.Add(messaggio);
                _context.SaveChanges();

                Console.WriteLine($"Reindirizzo a Success con ID: {segnalazione.Id}");

                return RedirectToAction("Success", new { id = segnalazione.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'invio: {ex.Message}");
                ModelState.AddModelError("", "Si è verificato un errore durante l'invio.");
                return View("Create", segnalazione);
            }
        }

        public IActionResult Success(int id)
        {
            var segnalazione = _context.Segnalazioni.Find(id);
            if (segnalazione == null) return NotFound();
            return View(segnalazione);
        }

        [HttpPost]
        public IActionResult Access(string codice)
        {
            if (string.IsNullOrEmpty(codice))
            {
                TempData["CodiceError"] = "Nessun codice inserito";
            }
            else if (codice.Length != 10 || !codice.All(char.IsDigit))
            {
                TempData["CodiceError"] = "Il codice deve essere di 10 cifre";
            }
            else
            {
                var segnalazione = _context.Segnalazioni.FirstOrDefault(s => s.Codice == codice);

                if (segnalazione == null)
                {
                    TempData["CodiceError"] = "Nessuna segnalazione con questo codice";
                }
                else
                {
                    return RedirectToAction("Dettagli", new { id = segnalazione.Id });
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Dettagli(int id)
        {
            var segnalazione = _context.Segnalazioni
                .Include(s => s.Allegati)
                .Include(s => s.MessaggiChat)
                .FirstOrDefault(s => s.Id == id);

            if (segnalazione == null) return NotFound();
            return View(segnalazione);
        }

        [HttpPost]
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

            return RedirectToAction("Dettagli", new { id = segnalazioneId });
        }

        [HttpPost]
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

            return RedirectToAction("Dettagli", new { id = segnalazioneId });
        }
        
        [HttpPost]
        public IActionResult DeleteAttachment(int id)
        {
            var allegato = _context.Allegati.FirstOrDefault(a => a.Id == id);

            if (allegato == null)
            {
                return NotFound();
            }
            
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), allegato.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Allegati.Remove(allegato);
            _context.SaveChanges();

            return RedirectToAction("Dettagli", new { id = allegato.SegnalazioneId });
        }

        [HttpPost]
        public IActionResult UpdateDetails(Segnalazione model)
        {
            var segnalazione = _context.Segnalazioni.Find(model.Id);
            if (segnalazione == null) return NotFound();

            segnalazione.Nome = model.Nome;
            segnalazione.Cognome = model.Cognome;
            segnalazione.Email = model.Email;
            segnalazione.Telefono = model.Telefono;

            _context.SaveChanges();
            return RedirectToAction("Dettagli", new { id = model.Id });
        }
    }
}