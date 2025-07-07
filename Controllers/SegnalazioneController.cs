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

        [HttpGet]
        public IActionResult Access()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Access(string codice)
        {
            if (string.IsNullOrEmpty(codice)) ModelState.AddModelError("", "Codice non valido.");
            var segnalazione = _context.Segnalazioni.FirstOrDefault(s => s.Codice == codice);
            if (segnalazione == null) ModelState.AddModelError("", "Codice non trovato.");

            if (!ModelState.IsValid) return View();

            return RedirectToAction("Dettagli", new { id = segnalazione.Id });
        }

        public IActionResult Dettagli(int id)
        {
            var segnalazione = _context.Segnalazioni.Find(id);
            if (segnalazione == null) return NotFound();
            return View(segnalazione);
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