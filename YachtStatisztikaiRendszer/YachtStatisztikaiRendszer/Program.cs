using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace YachtStatisztikaiRendszer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Állítsd be a saját fájl elérési utadnak megfelelően!
            string filename = @"C:\Users\Diak\Desktop\TomaArpadKristof20250529\yacht_berlesek_2024.csv";

            List<Berles> berlesek = LoadData(filename);

            if (berlesek.Count == 0)
            {
                Console.WriteLine("Nincs adat a fájlban vagy hiba történt az olvasás során.");
                return;
            }

            // 2. feladat: bekérjük a hónapot
            Console.Write("Adjon meg egy hónapot (1-12): ");
            int honap;
            while (!int.TryParse(Console.ReadLine(), out honap) || honap < 1 || honap > 12)
            {
                Console.Write("Érvénytelen bemenet. Adjon meg egy hónapot (1-12): ");
            }

            // Számoljuk ki az adott hónapban keletkezett bevételt
            double honapBevetel = berlesek
                .Where(b => BerekeszhetikBenne(b, honap))
                .Sum(b => b.TotalPrice);

            Console.WriteLine($"\nA(z) {honap}. hónap bevétele: {honapBevetel:N0} euró");

            // 3. feladat: teljes éves bevétel
            double evesBevetel = berlesek
                .Where(b => b.StartDate.Year == 2024 || b.EndDate.Year == 2024)
                .Sum(b => b.TotalPrice);
            Console.WriteLine($"A teljes 2024-es éves bevétel: {evesBevetel:N0} euró");

            // 4. feladat: legdrágább bérlés
            var legDragabb = berlesek.OrderByDescending(b => b.TotalPrice).First();
            Console.WriteLine($"A legdrágább bérlés az {legDragabb.Name} yacht volt, teljes ár: {legDragabb.TotalPrice:N0} euró");

            // 5. feladat: különböző yachtok száma
            int yachtCount = berlesek.Select(b => b.YachtId).Distinct().Count();
            Console.WriteLine($"Összesen {yachtCount} különböző yachtot béreltek ki.");

            // 6. feladat: legtöbbször bérelt yacht
            var legtobbBeres = berlesek
                .GroupBy(b => b.Name)
                .OrderByDescending(g => g.Count())
                .First();
            Console.WriteLine($"A legtöbbször bérelt yacht: {legtobbBeres.Key} ({legtobbBeres.Count()} bérlés)");

            // 8. feladat: átlagos bérlési idő
            double atlagBeresIdo = berlesek
                .Where(b => b.StartDate.Year == 2024 || b.EndDate.Year == 2024)
                .Average(b => b.BerlesDuration);
            Console.WriteLine($"Átlagos bérlési időtartam: {atlagBeresIdo:F2} nap");
        }

        static bool BerekeszhetikBenne(Berles b, int honap)
        {
            // Ellenőrizzük, hogy a bérlés részben vagy teljesen beleesik-e a megadott hónapba
            DateTime honapElso = new DateTime(2024, honap, 1);
            DateTime honapUtolso = honapElso.AddMonths(1).AddDays(-1);
            return b.StartDate <= honapUtolso && b.EndDate >= honapElso;
        }

        static List<Berles> LoadData(string filename)
        {
            var list = new List<Berles>();
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    string headerLine = sr.ReadLine(); // fejléc
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parts = line.Split(';'); // vagy ',' ha vessző
                        if (parts.Length != 6)
                            continue;

                        int uid = int.Parse(parts[0]);
                        int yachtId = int.Parse(parts[1]);
                        DateTime startDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime endDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        double dailyPrice = double.Parse(parts[4], CultureInfo.InvariantCulture);
                        string name = parts[5];

                        list.Add(new Berles
                        {
                            Uid = uid,
                            YachtId = yachtId,
                            StartDate = startDate,
                            EndDate = endDate,
                            DailyPrice = dailyPrice,
                            Name = name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba az adatok betöltésekor: {ex.Message}");
            }
            return list;
        }
    }

    class Berles
    {
        public int Uid { get; set; }
        public int YachtId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DailyPrice { get; set; }
        public string Name { get; set; }

        public double TotalPrice => BerlesDuration * DailyPrice;

        public int BerlesDuration => (EndDate - StartDate).Days + 1;
    }
}