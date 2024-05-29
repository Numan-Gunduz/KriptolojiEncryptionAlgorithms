using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Sifreleme_Algoritmaları.Controllers
{
    public class LinearAffineController : Controller
    {
        private const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        private const int AlphabetLength = 29;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Encrypt(string inputText, int a, int b)
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

            string encryptedText = EncryptText(inputText, a, b);
            ViewData["EncryptedText"] = encryptedText;
            return View("Index");
        }

        private string EncryptText(string inputText, int a, int b)
        {
            StringBuilder result = new StringBuilder();

            foreach (char character in inputText)
            {
                if (char.IsLetter(character))
                {
                    int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
                    int shiftedIndex = (a * index + b) % AlphabetLength;
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
        public IActionResult Decrypt(string encryptedText, int a, int b)
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

            string decryptedText = DecryptText(encryptedText, a, b);
            ViewData["DecryptedText"] = decryptedText;
            return View("Index");
        }

        private string DecryptText(string encryptedText, int a, int b)
        {
            StringBuilder result = new StringBuilder();
            int aInverse = ModInverse(a, AlphabetLength);

            foreach (char character in encryptedText)
            {
                if (char.IsLetter(character))
                {
                    int index = TurkishAlphabet.IndexOf(char.ToUpper(character));
                    int shiftedIndex = (aInverse * (index - b + AlphabetLength)) % AlphabetLength;
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

        private int ModInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }
            throw new Exception("Modular inverse does not exist.");
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