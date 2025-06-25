using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WhistleblowingApp.Models;
using WhistleblowingApp.Data;

public static class CodeGenerator
{
    public static string GenerateUniqueCode(ApplicationDbContext context)
    {
        var random = new Random();
        while (true)
        {
            string code = new string(Enumerable.Repeat("0123456789", 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            if (!context.Segnalazioni.Any(s => s.Codice == code))
                return code;
        }
    }
}