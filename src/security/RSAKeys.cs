using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;

namespace util.security {

	public class RSAKeys {

		private readonly bool autoVerify;
		public bool AutoVerify { get { return autoVerify; } }

		// Use OAEP or PKCS#1.5
		protected const bool OAEP = true;

		private RSAParameters privateKey;
		public RSAParameters PrivateKey { get { return privateKey; } }
		public string PrivateKeyString { get { return KeyToString(privateKey); } }

		private RSAParameters publicKey;
		public RSAParameters PublicKey { get { return publicKey; } }
		public string PublicKeyString { get { return KeyToString(publicKey); } }

		public RSAKeys(bool autoVerify = true) {
			this.autoVerify = autoVerify;
			UpdateKeys();
		}

		public void UpdateKeys() {

			// Create a RSA-CSP
			var csp = new RSACryptoServiceProvider(2048);

			// Get Keys
			var privateKey = csp.ExportParameters(true); // Private
			var publicKey = csp.ExportParameters(false); // Public

			string publicKeyStr = KeyToString(publicKey); // Public
			string privateKeyStr = KeyToString(privateKey); // Private

			if (
				AutoVerify && !(
					publicKeyStr == KeyToString(GetKey(publicKeyStr)) && publicKeyStr == KeyToString(publicKey) &&
					privateKeyStr == KeyToString(GetKey(privateKeyStr)) && privateKeyStr == KeyToString(privateKey)
					)
				) { throw new CryptographicException("Failed to Validate RSA Keys!"); }

			this.privateKey = privateKey;
			this.publicKey = publicKey;

		}

		public static byte[] DecodeKey(RSAParameters key) {

			var sw = new Utf8StringWriter();
			var wrt = XmlWriter.Create(sw);
			var xs = new XmlSerializer(typeof(RSAParameters));
			xs.Serialize(wrt, key);
			return Encoding.UTF8.GetBytes(sw.ToString());

		}

		public static string KeyToString(RSAParameters key) {

			return Encoding.UTF8.GetString(DecodeKey(key));

		}

		public static RSAParameters GetKey(string input) {

			var sr = new StringReader(input);
			var xmlr = new XmlTextReader(sr) { Normalization = false } ;
			var xs = new XmlSerializer(typeof(RSAParameters));
			return (RSAParameters)xs.Deserialize(xmlr);

		}

		//
		// Encrypt
		//

		public static byte[] Encrypt(string input, RSAParameters publicKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(publicKey);
			var inputBin = Encoding.UTF8.GetBytes(input);
			return csp.Encrypt(inputBin, OAEP);

		}

		public static byte[] Encrypt(byte[] input, RSAParameters publicKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(publicKey);
			return csp.Encrypt(input, OAEP);

		}

		public static string EncryptBase64(string input, RSAParameters publicKey) {

			var output = Encrypt(input, publicKey);
			return Convert.ToBase64String(output);

		}

		public static string EncryptBase64(byte[] input, RSAParameters publicKey) {

			var output = Encrypt(input, publicKey);
			return Convert.ToBase64String(output);

		}

		//
		// Decrypt
		//

		public static string Decrypt(string base64Input, RSAParameters privateKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(privateKey);
			var inputBin = Convert.FromBase64String(base64Input);
			var outputBin = csp.Decrypt(inputBin, OAEP);
			return Encoding.UTF8.GetString(outputBin);

		}

		public static string Decrypt(byte[] inputBytes, RSAParameters privateKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(privateKey);
			var outputBin = csp.Decrypt(inputBytes, OAEP);
			return Encoding.UTF8.GetString(outputBin);

		}

		public static byte[] DecryptBytes(string base64Input, RSAParameters privateKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(privateKey);
			var inputBin = Convert.FromBase64String(base64Input);
			return csp.Decrypt(inputBin, OAEP);

		}

		public static byte[] DecryptBytes(byte[] inputBytes, RSAParameters privateKey) {

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(privateKey);
			return csp.Decrypt(inputBytes, OAEP);

		}

	}

	internal class Utf8StringWriter : StringWriter {
		public override Encoding Encoding => Encoding.UTF8;
	}

}
