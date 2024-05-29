using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace Sifreleme_Algoritmaları.Controllers
{
    public class YerineKoyma : Controller
    {
        private const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        private const string SubstitutionAlphabet = "MNOPRSTUVYZABCÇDEFGĞHIİJKLMNOÖP";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Encrypt(string inputText)
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

            string encryptedText = EncryptText(inputText);
            ViewData["EncryptedText"] = encryptedText;
            return View("Index");
        }

        private string EncryptText(string inputText)
        {
            StringBuilder result = new StringBuilder();

            foreach (char character in inputText)
            {
                if (char.IsLetter(character))
                {
                    char upperChar = char.ToUpper(character);
                    int index = TurkishAlphabet.IndexOf(upperChar);
                    char encryptedChar = SubstitutionAlphabet[index];
                    result.Append(char.IsLower(character) ? char.ToLower(encryptedChar) : encryptedChar);
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        [HttpPost]
        public IActionResult Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                ViewData["ErrorMessage"] = "Lütfen bir şifreli metin girin.";
                return View("Index");
            }

            if (!ContainsOnlyLetters(encryptedText))
            {
                ViewData["ErrorMessage"] = "Şifreli metin yalnızca harfler içermelidir.";
                return View("Index");
            }

            string decryptedText = DecryptText(encryptedText);
            ViewData["DecryptedText"] = decryptedText;
            return View("Index");
        }

        private string DecryptText(string encryptedText)
        {
            StringBuilder result = new StringBuilder();

            foreach (char character in encryptedText)
            {
                if (char.IsLetter(character))
                {
                    char upperChar = char.ToUpper(character);
                    int index = SubstitutionAlphabet.IndexOf(upperChar);
                    char decryptedChar = TurkishAlphabet[index];
                    result.Append(char.IsLower(character) ? char.ToLower(decryptedChar) : decryptedChar);
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
