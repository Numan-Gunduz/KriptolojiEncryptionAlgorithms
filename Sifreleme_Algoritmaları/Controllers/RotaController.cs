using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace SifrelemeAlgoritmalari.Controllers
{
    public class RotaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Encrypt(string plaintext, int key)
        {
            plaintext = plaintext.ToLower();

            int rows = (int)Math.Ceiling((double)plaintext.Length / key);

            char[,] matrix = new char[rows, key];
            int index = 0;
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = key - 1; j >= 0; j--)
                {
                    if (index < plaintext.Length)
                        matrix[i, j] = plaintext[index++];
                    else
                        matrix[i, j] = 'x';
                }
            }

            string ciphertext = "";
            for (int j = key - 1; j >= 0; j--)
            {
                for (int i = 0; i < rows; i++)
                {
                    ciphertext += matrix[i, j];
                }
            }

            ViewData["Ciphertext"] = ciphertext;
            return View("Index");
        }

        [HttpPost]
        public IActionResult Decrypt(string ciphertext, int key)
        {
            ciphertext = ciphertext.ToLower();

            int rows = (int)Math.Ceiling((double)ciphertext.Length / key);

            char[,] matrix = new char[rows, key];
            int index = 0;
            for (int j = 0; j < key; j++)
            {
                for (int i = rows - 1; i >= 0; i--)
                {
                    if (index < ciphertext.Length)
                        matrix[i, j] = ciphertext[index++];
                    else
                        matrix[i, j] = ' ';
                }
            }

            string plaintext = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    plaintext += matrix[i, j];
                }
            }

            ViewData["Plaintext"] = plaintext.Trim();
            return View("Index");
        }
    }
}
