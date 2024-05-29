using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace SifrelemeAlgoritmalari.Controllers
{
	public class RotaController : Controller
	{
		private const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Encrypt(string plaintext, int key)
		{
			if (string.IsNullOrEmpty(plaintext))
			{
				ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(plaintext))
			{
				ViewData["ErrorMessage"] = "Metin yalnızca harfler içermelidir.";
				return View("Index");
			}

			plaintext = plaintext.ToUpper(); // Harfleri büyük yapıyoruz çünkü Türk alfabesini büyük harflerle kullanıyoruz.

			int rows = (int)Math.Ceiling((double)plaintext.Length / key);
			char[,] matrix = new char[rows, key];

			int index = 0;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < key; j++)
				{
					if (index < plaintext.Length)
						matrix[i, j] = plaintext[index++];
					else
						matrix[i, j] = 'X'; // Boş kalan yerleri 'X' ile dolduruyoruz.
				}
			}

			StringBuilder ciphertext = new StringBuilder();
			for (int j = 0; j < key; j++)
			{
				for (int i = rows - 1; i >= 0; i--)
				{
					ciphertext.Append(matrix[i, j]);
				}
			}

			ViewData["Ciphertext"] = ciphertext.ToString();
			return View("Index");
		}

		[HttpPost]
		public IActionResult Decrypt(string ciphertext, int key)
		{
			if (string.IsNullOrEmpty(ciphertext))
			{
				ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
				return View("Index");
			}

			if (!ContainsOnlyLetters(ciphertext))
			{
				ViewData["ErrorMessage"] = "Metin yalnızca harfler içermelidir.";
				return View("Index");
			}

			ciphertext = ciphertext.ToUpper(); // Harfleri büyük yapıyoruz çünkü Türk alfabesini büyük harflerle kullanıyoruz.

			int rows = (int)Math.Ceiling((double)ciphertext.Length / key);
			char[,] matrix = new char[rows, key];

			int index = 0;
			for (int j = 0; j < key; j++)
			{
				for (int i = rows - 1; i >= 0; i--)
				{
					if (index < ciphertext.Length)
						matrix[i, j] = ciphertext[index++];
				}
			}

			StringBuilder plaintext = new StringBuilder();
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < key; j++)
				{
					if (matrix[i, j] != 'X')
					{
						plaintext.Append(matrix[i, j]);
					}
				}
			}

			ViewData["Plaintext"] = plaintext.ToString();
			return View("Index");
		}

		private bool ContainsOnlyLetters(string text)
		{
			foreach (char c in text)
			{
				if (!TurkishAlphabet.Contains(char.ToUpper(c)))
				{
					return false;
				}
			}
			return true;
		}
	}
}
