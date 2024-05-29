	using Microsoft.AspNetCore.Mvc;
	using System.Text;

	namespace SifrelemeAlgoritmalari.Controllers
	{
		public class ZigzagController : Controller
		{
			public IActionResult Index()
			{
				return View();
			}

			[HttpPost]
			public IActionResult Encrypt(string inputText, int depth)
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

				string encryptedText = EncryptText(inputText, depth);
				ViewData["EncryptedText"] = encryptedText;
				return View("Index");
			}

			[HttpPost]
			public IActionResult Decrypt(string encryptedText, int depth)
			{
				if (string.IsNullOrEmpty(encryptedText))
				{
					ViewData["ErrorMessage"] = "Lütfen şifreli metni girin.";
					return View("Index");
				}

				if (!ContainsOnlyLetters(encryptedText))
				{
					ViewData["ErrorMessage"] = "Şifreli metin yalnızca harfler içermelidir.";
					return View("Index");
				}

				string decryptedText = DecryptText(encryptedText, depth);
				ViewData["DecryptedText"] = decryptedText;
				return View("Index");
			}

			 private string EncryptText(string inputText, int depth)
			{
				string encryptedText = "";
				int length = inputText.Length;
				char[,] railMatrix = new char[depth, length];
				bool down = false;
				int row = 0, col = 0;

				for (int i = 0; i < length; i++)
				{
					if (row == 0 || row == depth - 1)
						down = !down;

					railMatrix[row, col++] = inputText[i];
					row += down ? 1 : -1;
				}

				for (int i = 0; i < depth; i++)
				{
					for (int j = 0; j < length; j++)
					{
						if (railMatrix[i, j] != '\0')
							encryptedText += railMatrix[i, j];
					}
				}

				return encryptedText;
			}

			private string DecryptText(string encryptedText, int depth)
			{
				string decryptedText = "";
				int length = encryptedText.Length;
				char[,] railMatrix = new char[depth, length];
				bool down = false;
				int row = 0, col = 0;

				for (int i = 0; i < length; i++)
				{
					if (row == 0 || row == depth - 1)
						down = !down;

					railMatrix[row, col++] = '*';

					row += down ? 1 : -1;
				}

				int index = 0;
				for (int i = 0; i < depth; i++)
				{
					for (int j = 0; j < length; j++)
					{
						if (railMatrix[i, j] == '*' && index < length)
							railMatrix[i, j] = encryptedText[index++];
					}
				}

				row = 0;
				col = 0;
				down = false;

				for (int i = 0; i < length; i++)
				{
					if (row == 0 || row == depth - 1)
						down = !down;

					decryptedText += railMatrix[row, col++];
					row += down ? 1 : -1;
				}

				return decryptedText;
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
