using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Sifreleme_Algoritmaları.Controllers
{
	public class VigenereController : Controller
	{
		private const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Encrypt(string inputText, string key)
		{
			if (string.IsNullOrEmpty(inputText))
			{
				ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (string.IsNullOrEmpty(key))
			{
				ViewData["ErrorMessage"] = "Lütfen bir anahtar girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(inputText) || !ContainsOnlyLetters(key))
			{
				ViewData["ErrorMessage"] = "Metin ve anahtar yalnızca harfler içermelidir.";
				return View("Index");
			}

			string encryptedText = EncryptText(inputText, key);
			ViewData["EncryptedText"] = encryptedText;
			return View("Index");
		}

		private string EncryptText(string inputText, string key)
		{
			StringBuilder result = new StringBuilder();
			int keyIndex = 0;

			for (int i = 0; i < inputText.Length; i++)
			{
				char character = inputText[i];
				if (char.IsLetter(character))
				{
					char baseChar = char.IsUpper(character) ? 'A' : 'a';
					int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
					int keyCharIndex = TurkishAlphabet.IndexOf(char.ToUpper(key[keyIndex % key.Length]));

					int shiftedIndex = (index + keyCharIndex) % TurkishAlphabet.Length;
					char shiftedChar = TurkishAlphabet[shiftedIndex];
					result.Append(char.IsLower(character) ? char.ToLower(shiftedChar) : shiftedChar);

					keyIndex++;
				}
				else
				{
					result.Append(character);
				}
			}

			return result.ToString();
		}

		[HttpPost]
		public IActionResult Decrypt(string encryptedText, string key)
		{
			if (string.IsNullOrEmpty(encryptedText))
			{
				ViewData["ErrorMessage"] = "Lütfen bir şifreli metin girin.";
				return View("Index");
			}

			if (string.IsNullOrEmpty(key))
			{
				ViewData["ErrorMessage"] = "Lütfen bir anahtar girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(encryptedText) || !ContainsOnlyLetters(key))
			{
				ViewData["ErrorMessage"] = "Şifreli metin ve anahtar yalnızca harfler içermelidir.";
				return View("Index");
			}

			string decryptedText = DecryptText(encryptedText, key);
			ViewData["DecryptedText"] = decryptedText;
			return View("Index");
		}

		private string DecryptText(string encryptedText, string key)
		{
			StringBuilder result = new StringBuilder();
			int keyIndex = 0;

			for (int i = 0; i < encryptedText.Length; i++)
			{
				char character = encryptedText[i];
				if (char.IsLetter(character))
				{
					char baseChar = char.IsUpper(character) ? 'A' : 'a';
					int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
					int keyCharIndex = TurkishAlphabet.IndexOf(char.ToUpper(key[keyIndex % key.Length]));

					int shiftedIndex = (index - keyCharIndex + TurkishAlphabet.Length) % TurkishAlphabet.Length;
					char shiftedChar = TurkishAlphabet[shiftedIndex];
					result.Append(char.IsLower(character) ? char.ToLower(shiftedChar) : shiftedChar);

					keyIndex++;
				}
				else
				{
					result.Append(character);
				}
			}

			return result.ToString();
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
	}
}
