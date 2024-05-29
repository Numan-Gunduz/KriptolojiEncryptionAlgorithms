using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SifrelemeAlgoritmalari.Controllers
{
	public class SezarController : Controller
	{
		private const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Encrypt(string inputText, int key)
		{
			if (string.IsNullOrEmpty(inputText))
			{
				ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(inputText))
			{
				ViewData["ErrorMessage"] = "Metin yalnızca harfler içermelidir.";
				return View("Index");
			}

			string encryptedText = EncryptText(inputText, key);
			ViewData["EncryptedText"] = encryptedText;

			// Eklenen kısım: Deşifreleme için şifrelenmiş metni de View üzerinden gönderiyorum
			string decryptedText = DecryptText(encryptedText, key);
			ViewData["DecryptedText"] = decryptedText;

			return View("Index");
		}

		[HttpPost]
		public IActionResult Decrypt(string encryptedText, int key)
		{
			if (string.IsNullOrEmpty(encryptedText))
			{
				ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(encryptedText))
			{
				ViewData["ErrorMessage"] = "Metin yalnızca harfler içermelidir.";
				return View("Index");
			}

			string decryptedText = DecryptText(encryptedText, key);
			ViewData["DecryptedText"] = decryptedText;

			return View("Index");
		}

		private bool ContainsOnlyLetters(string text)
		{
			foreach (char c in text)
			{
				if (!char.IsLetter(c))
				{
					return false;
				}
			}
			return true;
		}

		private string EncryptText(string inputText, int key=3)
		{
			StringBuilder result = new StringBuilder();

			foreach (char character in inputText)
			{
				if (char.IsLetter(character))
				{
					char baseChar = char.IsUpper(character) ? 'A' : 'a';
					int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
					int shiftedIndex = (index + key) % TurkishAlphabet.Length;
					char shiftedChar = TurkishAlphabet[shiftedIndex];
					result.Append(char.IsLower(character) ? char.ToLower(shiftedChar) : shiftedChar);
				}
				else
				{
					result.Append(character);
				}
			}

			return result.ToString();
		}

		private string DecryptText(string encryptedText, int key=3)
		{
			StringBuilder result = new StringBuilder();

			foreach (char character in encryptedText)
			{
				if (char.IsLetter(character))
				{
					char baseChar = char.IsUpper(character) ? 'A' : 'a';
					int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
					int shiftedIndex = (index - key + TurkishAlphabet.Length) % TurkishAlphabet.Length;
					char shiftedChar = TurkishAlphabet[shiftedIndex];
					result.Append(char.IsLower(character) ? char.ToLower(shiftedChar) : shiftedChar);
				}
				else
				{
					result.Append(character);
				}
			}

			return result.ToString();
		}
	}
}
