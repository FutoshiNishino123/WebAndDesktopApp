using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Common.Utils
{
    public class PasswordUtils
	{
		private const string SALT = "{E0FC4B06-A85A-4AA9-B392-CBFE6D5E008F}";

		public static string GetHash(string input)
		{
			byte[] data = Encoding.UTF8.GetBytes(input + SALT);
			byte[] hash = SHA256.Create().ComputeHash(data);

			var sb = new StringBuilder();
			foreach (byte b in hash)
			{
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}
	}
}
