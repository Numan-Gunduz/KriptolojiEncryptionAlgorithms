using Microsoft.AspNetCore.Mvc;
using System;
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

            string encryptedText = EncryptText(inputText, key);
            ViewData["EncryptedText"] = encryptedText;

            // Eklenen kısım: Deşifreleme için şifrelenmiş metni de View üzerinden gönderiyoruz.
            string decryptedText = DecryptText(encryptedText, key);
            ViewData["DecryptedText"] = decryptedText;

            return View("Index");
        }

        private string EncryptText(string inputText, int key)
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


        [HttpPost]
        public IActionResult Decrypt(string encryptedText, int key)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                ViewData["ErrorMessage"] = "Lütfen bir metin girin.";
                return View("Index");
            }

            string decryptedText = DecryptText(encryptedText, key);
            ViewData["DecryptedText"] = decryptedText;

            return View("Index");
        }
        private string DecryptText(string encryptedText, int key)
        {
            StringBuilder result = new StringBuilder();

            foreach (char character in encryptedText)
            {
                if (char.IsLetter(character))
                {
                    char baseChar = char.IsUpper(character) ? 'A' : 'a';
                    int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
                    int shiftedIndex = (index - key + TurkishAlphabet.Length) % TurkishAlphabet.Length; // Deşifreleme işlemi, şifreleme işleminin tersi olarak anahtardan çıkarıyoruz.
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
