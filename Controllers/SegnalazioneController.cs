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
            var segnalazione = _context.Segnalazioni.Find(id);
            if (segnalazione == null) return NotFound();
            return View(segnalazione);
        }
    }
}