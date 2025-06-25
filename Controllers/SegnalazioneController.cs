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
            if (!ModelState.IsValid) return View("Create", segnalazione);

            segnalazione.Codice = CodeGenerator.GenerateUniqueCode(_context);
            segnalazione.DataCreazione = DateTime.Now;
            segnalazione.Stato = "In attesa";

            _context.Segnalazioni.Add(segnalazione);
            _context.SaveChanges();

            return RedirectToAction("Success", new { id = segnalazione.Id });
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
    }
}