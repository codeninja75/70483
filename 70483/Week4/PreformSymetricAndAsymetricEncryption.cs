using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.E70483.DebugAndSecurity
{
    public class PreformSymetricAndAsymetricEncryption
    {
        public static void HideAllTheSecrets()
        {
            string ss = "p@ssw0rd";
            RijndaelManaged rj = new RijndaelManaged();
            byte[] salt = Encoding.ASCII.GetBytes("This is my salt!");
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(ss, salt);
            rj.Key = key.GetBytes(rj.KeySize / 8);
            rj.IV = key.GetBytes(rj.BlockSize / 8);
            string inFile = @"C:\boot.ini";
            string outFile = @"C:\boot.ini.enc";
            FileStream inFS = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            FileStream outFS = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] fileData = new byte[inFS.Length];
            inFS.Read(fileData, 0, (int)inFS.Length);
            ICryptoTransform enc = rj.CreateEncryptor();
            CryptoStream encStream = new CryptoStream(outFS, enc, CryptoStreamMode.Write);
            encStream.Write(fileData, 0, fileData.Length);
            encStream.Close();
            inFS.Close();
            outFS.Close();
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider();
            RSAParameters rsaParam = myRSA.ExportParameters(true);
            Console.WriteLine(myRSA.ToXmlString(true));

            //How to put the keys in the key store!
            CspParameters persistantCsp = new CspParameters();
            persistantCsp.KeyContainerName = "AsymetricExample";
            myRSA = new RSACryptoServiceProvider(persistantCsp);
            myRSA.PersistKeyInCsp = true;
            rsaParam = myRSA.ExportParameters(true);
            foreach (byte thisbyte in rsaParam.D)
                Console.Write(thisbyte.ToString("X2") + " ");
            string Message = "Hello, world!";
            byte[] messByte = Encoding.Unicode.GetBytes(Message);
            byte[] encMess = myRSA.Encrypt(messByte, false);
            byte[] decMess = myRSA.Decrypt(encMess, false);
            Console.WriteLine(Encoding.Unicode.GetString(decMess));

            //HASHING -non keyed
            MD5 myHash = new MD5CryptoServiceProvider();
            FileStream file = new FileStream(outFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(file);
            myHash.ComputeHash(br.ReadBytes((int)file.Length));
            Console.WriteLine(Convert.ToBase64String(myHash.Hash));
            br.Close();
            file.Close();
            //HASHING keyed
            byte[] saltValue = Encoding.ASCII.GetBytes("This is some good salt!");
            Rfc2898DeriveBytes passwordkey = new Rfc2898DeriveBytes("p@55w0r6", saltValue);
            byte[] secretkey = passwordkey.GetBytes(16);

            HMACSHA1 keyhash = new HMACSHA1(secretkey);
            file = new FileStream(outFile, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(file);
            keyhash.ComputeHash(br.ReadBytes((int)file.Length));
            Console.WriteLine(Convert.ToBase64String(keyhash.Hash));
        }
    }
}
