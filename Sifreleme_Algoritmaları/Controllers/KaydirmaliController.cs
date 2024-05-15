using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Sifreleme_Algoritmaları.Controllers
{
    public class KaydirmaController : Controller
    {
        private const string TurkceAlfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        private const int AlfabeBoyutu = 29;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sifrele(string girisMetni, int kaydirmaMiktari)
        {
            if (string.IsNullOrEmpty(girisMetni))
            {
                ViewData["HataMesaji"] = "Lütfen bir metin girin.";
                return View("Index");
            }

            string sifrelenmisMetin = SifreleMetni(girisMetni, kaydirmaMiktari);
            ViewData["SifrelenmisMetin"] = sifrelenmisMetin;
            return View("Index");
        }

        private string SifreleMetni(string girisMetni, int kaydirmaMiktari)
        {
            StringBuilder sonuc = new StringBuilder();

            foreach (char karakter in girisMetni)
            {
                if (char.IsLetter(karakter))
                {
                    char tabanKarakter = char.IsUpper(karakter) ? 'A' : 'a';
                    int indeks = TurkceAlfabe.IndexOf(char.ToUpper(karakter));
                    int kaydirilmisIndeks = (indeks + kaydirmaMiktari) % AlfabeBoyutu;
                    char sifrelenmisKarakter = TurkceAlfabe[kaydirilmisIndeks];
                    sonuc.Append(char.IsLower(karakter) ? char.ToLower(sifrelenmisKarakter) : sifrelenmisKarakter);
                }
                else
                {
                    sonuc.Append(karakter);
                }
            }

            return sonuc.ToString();
        }
        private string CozMetni(string sifrelenmisMetin, int kaydirmaMiktari)
        {
            StringBuilder cozulmusMetin = new StringBuilder();

            foreach (char karakter in sifrelenmisMetin)
            {
                if (char.IsLetter(karakter))
                {
                    char tabanKarakter = char.IsUpper(karakter) ? 'A' : 'a';
                    int indeks = TurkceAlfabe.IndexOf(char.ToUpper(karakter));
                    int cozulmusIndeks = (indeks - kaydirmaMiktari + AlfabeBoyutu) % AlfabeBoyutu;
                    char cozulmusKarakter = TurkceAlfabe[cozulmusIndeks];
                    cozulmusMetin.Append(char.IsLower(karakter) ? char.ToLower(cozulmusKarakter) : cozulmusKarakter);
                }
                else
                {
                    cozulmusMetin.Append(karakter);
                }
            }

            return cozulmusMetin.ToString();
        }
        [HttpPost]
        public IActionResult Coz(string sifrelenmisMetin, int kaydirmaMiktari)
        {
            if (string.IsNullOrEmpty(sifrelenmisMetin))
            {
                ViewData["HataMesaji"] = "Lütfen bir metin girin.";
                return View("Index");
            }

            string cozulmusMetin = CozMetni(sifrelenmisMetin, kaydirmaMiktari);
            ViewData["CozulmusMetin"] = cozulmusMetin;
            return View("Index");
        }

    }
}
