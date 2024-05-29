using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Linq;

namespace Sifreleme_Algoritmaları.Controllers
{
	public class KaydirmaController : Controller
	{
		private const string TurkceAlfabeBuyuk = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
		private const string TurkceAlfabeKucuk = "abcçdefgğhıijklmnoöprsştuüvyz";
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

			if (!SadeceTurkceHarfler(girisMetni))
			{
				ViewData["HataMesaji"] = "Metin yalnızca Türkçe harfler içermelidir.";
				return View("Index");
			}

			string sifrelenmisMetin = SifreleMetni(girisMetni, kaydirmaMiktari);
			ViewData["SifrelenmisMetin"] = sifrelenmisMetin;
			return View("Index");
		}

		[HttpPost]
		public IActionResult Coz(string sifrelenmisMetin, int kaydirmaMiktari)
		{
			if (string.IsNullOrEmpty(sifrelenmisMetin))
			{
				ViewData["HataMesaji"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (!SadeceTurkceHarfler(sifrelenmisMetin))
			{
				ViewData["HataMesaji"] = "Metin yalnızca Türkçe harfler içermelidir.";
				return View("Index");
			}

			string cozulmusMetin = CozMetni(sifrelenmisMetin, kaydirmaMiktari);
			ViewData["CozulmusMetin"] = cozulmusMetin;
			return View("Index");
		}

		private bool SadeceTurkceHarfler(string metin)
		{
			foreach (char karakter in metin)
			{
				if (!TurkceAlfabeBuyuk.Contains(char.ToUpper(karakter)) && !TurkceAlfabeKucuk.Contains(char.ToLower(karakter)))
				{
					return false;
				}
			}
			return true;
		}

		private string SifreleMetni(string girisMetni, int kaydirmaMiktari)
		{
			StringBuilder sonuc = new StringBuilder();

			foreach (char karakter in girisMetni)
			{
				if (TurkceAlfabeBuyuk.Contains(char.ToUpper(karakter)))
				{
					if (char.IsUpper(karakter))
					{
						int indeks = TurkceAlfabeBuyuk.IndexOf(karakter);
						int kaydirilmisIndeks = (indeks + kaydirmaMiktari) % AlfabeBoyutu;
						char sifrelenmisKarakter = TurkceAlfabeBuyuk[kaydirilmisIndeks];
						sonuc.Append(sifrelenmisKarakter);
					}
					else
					{
						int indeks = TurkceAlfabeKucuk.IndexOf(karakter);
						int kaydirilmisIndeks = (indeks + kaydirmaMiktari) % AlfabeBoyutu;
						char sifrelenmisKarakter = TurkceAlfabeKucuk[kaydirilmisIndeks];
						sonuc.Append(sifrelenmisKarakter);
					}
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
				if (TurkceAlfabeBuyuk.Contains(char.ToUpper(karakter)))
				{
					if (char.IsUpper(karakter))
					{
						int indeks = TurkceAlfabeBuyuk.IndexOf(karakter);
						int cozulmusIndeks = (indeks - kaydirmaMiktari + AlfabeBoyutu) % AlfabeBoyutu;
						char cozulmusKarakter = TurkceAlfabeBuyuk[cozulmusIndeks];
						cozulmusMetin.Append(cozulmusKarakter);
					}
					else
					{
						int indeks = TurkceAlfabeKucuk.IndexOf(karakter);
						int cozulmusIndeks = (indeks - kaydirmaMiktari + AlfabeBoyutu) % AlfabeBoyutu;
						char cozulmusKarakter = TurkceAlfabeKucuk[cozulmusIndeks];
						cozulmusMetin.Append(cozulmusKarakter);
					}
				}
				else
				{
					cozulmusMetin.Append(karakter);
				}
			}

			return cozulmusMetin.ToString();
		}
	}
}
