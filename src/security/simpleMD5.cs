using System.Security.Cryptography;
using System.Text;

namespace util.security {

	public static class simpleMD5 {

		public static string getMD5(string input) {

			using (MD5 md5 = MD5.Create()) {
				byte[] inputBin = Encoding.UTF8.GetBytes(input);
				byte[] inputMD5Bin = md5.ComputeHash(inputBin);
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < inputMD5Bin.Length; i++) {
					sb.Append(inputMD5Bin[i].ToString("X2"));
				}
				return sb.ToString();
			}

		}

	}

}