using System;
using System.Security.Cryptography;
using System.IO;
namespace Chap5
{
	/// <summary>
	/// Summary description for Encryption.
	/// </summary>
	/// 
	public enum EncryptionAlgorithm {Des = 1, Rc2, Rijndael, TripleDes};
	public class Encryptor
	{
		private EncryptTransformer transformer;
		private byte[] initVec;
		private byte[] encKey;
		public byte[] IV
		{
			get{return initVec;}
			set{initVec = value;}
		}

		public byte[] Key
		{
			get{return encKey;}
		}
		public Encryptor(EncryptionAlgorithm algId)
		{
			transformer = new EncryptTransformer(algId);
		}

		public byte[] Encrypt(byte[] bytesData, byte[] bytesKey)
		{
			//Set up the stream that will hold the encrypted data.
			MemoryStream memStreamEncryptedData = new MemoryStream();
			transformer.IV = initVec;
			ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
			CryptoStream encStream = new CryptoStream(memStreamEncryptedData, 
				transform, 
				CryptoStreamMode.Write);
			try
			{
				//Encrypt the data, write it to the memory stream.
				encStream.Write(bytesData, 0, bytesData.Length);
			}
			catch(Exception ex)
			{
				throw new Exception("Error while writing encrypted data to the stream: \n" 
					+ ex.Message);
			}
			//Set the IV and key for the client to retrieve
			encKey = transformer.Key;
			initVec = transformer.IV;
			encStream.FlushFinalBlock();
			encStream.Close();

			//Send the data back.
			return memStreamEncryptedData.ToArray();
		}//end Encrypt
	}
	public class Decryptor
	{
		public Decryptor(EncryptionAlgorithm algId)
		{
			transformer = new DecryptTransformer(algId);
		}
		private DecryptTransformer transformer;
		private byte[] initVec;
		public byte[] IV
		{
			set{initVec = value;}
		}
		public byte[] Decrypt(byte[] bytesData, byte[] bytesKey)
		{
			//Set up the memory stream for the decrypted data.
			MemoryStream memStreamDecryptedData = new MemoryStream();

			//Pass in the initialization vector.
			transformer.IV = initVec;
			ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
			CryptoStream decStream = new CryptoStream(memStreamDecryptedData, 
				transform, 
				CryptoStreamMode.Write);
			try
			{
				decStream.Write(bytesData, 0, bytesData.Length);
			}
			catch(Exception ex)
			{
				throw new Exception("Error while writing encrypted data to the stream: \n" 
					+ ex.Message);
			}
			decStream.FlushFinalBlock();
			decStream.Close();
			// Send the data back.
			return memStreamDecryptedData.ToArray();
		} //end Decrypt	
	}
	internal class EncryptTransformer
	{
	
		private EncryptionAlgorithm algorithmID;
		private byte[] initVec;
		private byte[] encKey;
		internal EncryptTransformer(EncryptionAlgorithm algId)
		{
			//Save the algorithm being used.
			algorithmID = algId;
		}
		internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
		{
			// Pick the provider.
			switch (algorithmID)
			{
				case EncryptionAlgorithm.Des:
				{
					DES des = new DESCryptoServiceProvider();
					des.Mode = CipherMode.CBC;

					// See if a key was provided
					if (null == bytesKey)
					{
						encKey = des.Key;
					}
					else
					{
						des.Key = bytesKey;
						encKey = des.Key;
					}
					// See if the client provided an initialization vector
					if (null == initVec)
					{ // Have the algorithm create one
						initVec = des.IV;
					}
					else
					{ //No, give it to the algorithm
						des.IV = initVec;
					}
					return des.CreateEncryptor();
				}
				case EncryptionAlgorithm.TripleDes:
				{
					TripleDES des3 = new TripleDESCryptoServiceProvider();
					des3.Mode = CipherMode.CBC;
					// See if a key was provided
					if (null == bytesKey)
					{
						encKey = des3.Key;
					}
					else
					{
						des3.Key = bytesKey;
						encKey = des3.Key;
					}
					// See if the client provided an IV
					if (null == initVec)
					{ //Yes, have the alg create one
						initVec = des3.IV;
					}
					else
					{ //No, give it to the alg.
						des3.IV = initVec;
					}
					return des3.CreateEncryptor();
				}
				case EncryptionAlgorithm.Rc2:
				{
					RC2 rc2 = new RC2CryptoServiceProvider();
					rc2.Mode = CipherMode.CBC;
					// Test to see if a key was provided
					if (null == bytesKey)
					{
						encKey = rc2.Key;
					}
					else
					{
						rc2.Key = bytesKey;
						encKey = rc2.Key;
					}
					// See if the client provided an IV
					if (null == initVec)
					{ //Yes, have the alg create one
						initVec = rc2.IV;
					}
					else
					{ //No, give it to the alg.
						rc2.IV = initVec;
					}
					return rc2.CreateEncryptor();
				}
				case EncryptionAlgorithm.Rijndael:
				{
					Rijndael rijndael = new RijndaelManaged();
					rijndael.Mode = CipherMode.CBC;
					// Test to see if a key was provided
					if(null == bytesKey)
					{
						encKey = rijndael.Key;
					}
					else
					{
						rijndael.Key = bytesKey;
						encKey = rijndael.Key;
					}
					// See if the client provided an IV
					if(null == initVec)
					{ //Yes, have the alg create one
						initVec = rijndael.IV;
					}
					else
					{ //No, give it to the alg.
						rijndael.IV = initVec;
					}
					return rijndael.CreateEncryptor();
				} 
				default:
				{
					throw new CryptographicException("Algorithm ID '" + algorithmID + 
						"' not supported.");
				}
			}
		}
		internal byte[] IV
		{
			get{return initVec;}
			set{initVec = value;}
		}
		internal byte[] Key
		{
			get{return encKey;}
		}
	}

	internal class DecryptTransformer
	{
		private EncryptionAlgorithm algorithmID;
		private byte[] initVec;
		internal byte[] IV
		{
			set{initVec = value;}
		}
		internal DecryptTransformer(EncryptionAlgorithm deCryptId)
		{
			algorithmID = deCryptId;
		}
		internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
		{
			// Pick the provider.
			switch (algorithmID)
			{
				case EncryptionAlgorithm.Des:
				{
					DES des = new DESCryptoServiceProvider();
					des.Mode = CipherMode.CBC;
					des.Key = bytesKey;
					des.IV = initVec;
					return des.CreateDecryptor();
				}
				case EncryptionAlgorithm.TripleDes:
				{
					TripleDES des3 = new TripleDESCryptoServiceProvider();
					des3.Mode = CipherMode.CBC;
					return des3.CreateDecryptor(bytesKey, initVec);
				}
				case EncryptionAlgorithm.Rc2:
				{
					RC2 rc2 = new RC2CryptoServiceProvider();
					rc2.Mode = CipherMode.CBC;
					return rc2.CreateDecryptor(bytesKey, initVec);
				}
				case EncryptionAlgorithm.Rijndael:
				{
					Rijndael rijndael = new RijndaelManaged();
					rijndael.Mode = CipherMode.CBC;
					return rijndael.CreateDecryptor(bytesKey, initVec);
				} 
				default:
				{
					throw new CryptographicException("Algorithm ID '" + algorithmID + 
						"' not supported.");
				}
			}
		} //end GetCryptoServiceProvider
	}

}
