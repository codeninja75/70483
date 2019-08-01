using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Threading;
using System.Security;
using System.Security.Permissions;
using System.Security.AccessControl;
using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;

namespace Chap12
{
    class Program
    {
        static void Main(string[] args)
        {
            //Authnetication = Who are you?
            //Authorization = What can you do?

            //Get current Identity
            WindowsIdentity curr = WindowsIdentity.GetCurrent();
            //Get anonymous identity
            curr = WindowsIdentity.GetAnonymous();
            //curr = WindowsIdentity.Impersonate();
            curr = WindowsIdentity.GetCurrent();
            Console.WriteLine("Name:" + curr.Name);
            Console.WriteLine("Token:" + curr.Token.ToString());
            Console.WriteLine("Authentication Type:" + curr.AuthenticationType);
            Console.WriteLine("Is anonymous? {0}", curr.IsAnonymous);
            Console.WriteLine("Is authenticated? {0}", curr.IsAuthenticated);
            Console.WriteLine("Is a guest? {0}", curr.IsGuest);
            Console.WriteLine("Is System? {0}", curr.IsSystem);

            //WindowsPrincipal = group info.  You can retrieve this by using an identity
            WindowsPrincipal wingroup = new WindowsPrincipal(curr);
            //or by pulling the group info from the current thread.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            wingroup = (WindowsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine("Current user is a member of the following roles:");
            Console.WriteLine("In role Administrator? {0}", wingroup.IsInRole(WindowsBuiltInRole.Administrator));
            Console.WriteLine("In role AccountOperator? {0}", wingroup.IsInRole(WindowsBuiltInRole.AccountOperator));
            Console.WriteLine("In role Guest? {0}", wingroup.IsInRole(WindowsBuiltInRole.Guest));
            Console.WriteLine("In role PowerUser? {0}", wingroup.IsInRole(WindowsBuiltInRole.PowerUser));
            Console.WriteLine(@"In  role Atlas\Admin? {0}", wingroup.IsInRole(@"Atlas\Admin"));

            //AdminOnly();
            AlsoAdminOnly();

            string r = System.Environment.MachineName + @"\VS Developers";
            try 
            {	        
                PrincipalPermission p = new PrincipalPermission(null,r,true);
                p.Demand();
                Console.WriteLine("You are in " + r);
            }
	        catch (SecurityException ex)
	        {
                Console.WriteLine("You are not in " + r);		
	        }

            //DACL = Restricts or grants control
            //SACL = Used for logging accesses to a resource...

            DirectorySecurity ds = new DirectorySecurity(@"C:\Program Files\", AccessControlSections.All);
            //FileSystemRights.AppendData 
            //FileSystemRights.ChangePermissions 
            //FileSystemRights.CreateDirectories
            //FileSystemRights.CreateFiles
            //FileSystemRights.Delete
            //FileSystemRights.DeleteSubdirectoriesAndFiles
            //FileSystemRights.ExecuteFile
            //FileSystemRights.FullControl
            //FileSystemRights.ListDirectory
            //FileSystemRights.Modify
            //FileSystemRights.Read
            //FileSystemRights.ReadAndExecute
            //FileSystemRights.ReadAttributes
            //FileSystemRights.ReadData
            //FileSystemRights.ReadExtendedAttributes
            //  FileSystemRights.ReadPermissions
            //FileSystemRights.Synchronize
            //FileSystemRights.TakeOwnership
            //FileSystemRights.Traverse
            //FileSystemRights.Write
            //  FileSystemRights.WriteAttributes
            //FileSystemRights.WriteData
            //FileSystemRights.WriteExtendedAttributes
            AuthorizationRuleCollection arc = ds.GetAccessRules(true, true, typeof(NTAccount));
            foreach (FileSystemAccessRule ar in arc)
            {
                Console.WriteLine(ar.IdentityReference + " : " + ar.AccessControlType + " " + ar.FileSystemRights);
            }

            RegistrySecurity rs = Registry.LocalMachine.GetAccessControl();
            arc = rs.GetAccessRules(true, true, typeof(NTAccount));
            foreach (RegistryAccessRule rar in arc)
            {
                Console.WriteLine(rar.IdentityReference + ":" + rar.AccessControlType + " " + rar.RegistryRights);
            }
            //RegistryRights.ChangePermissions
            //RegistryRights.CreateLink
            //RegistryRights.CreateSubKey
            //RegistryRights.Delete
            //RegistryRights.EnumerateSubKeys;
            //RegistryRights.ExecuteKey;
            //RegistryRights.FullControl;
            //RegistryRights.Notify;
            //RegistryRights.QueryValues;
            //RegistryRights.ReadKey;
            //RegistryRights.ReadPermissions;
            //RegistryRights.SetValue;
            //RegistryRights.TakeOwnership;
            //RegistryRights.WriteKey;
            string dir = @"C:\logs";
            ds = Directory.GetAccessControl(dir);
            ds.AddAccessRule(new FileSystemAccessRule("Guest", FileSystemRights.Read, AccessControlType.Allow));
            Directory.SetAccessControl(dir, ds);
            //THE ABOVE IS 20X easier than anything before .net 2.0

            //SYMETRIC ENCRYPTION = Encryption & decryption is done w/ the same key
            string ss = "p@ssw0rd";
            RijndaelManaged rj = new RijndaelManaged();
            byte[] salt = Encoding.ASCII.GetBytes("This is my salt!");
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(ss,salt);
            rj.Key = key.GetBytes(rj.KeySize / 8);
            rj.IV = key.GetBytes(rj.BlockSize/8);
            string inFile = @"C:\boot.ini";
            string outFile = @"C:\boot.ini.enc";
            FileStream inFS = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            FileStream outFS = new FileStream(outFile , FileMode.OpenOrCreate, FileAccess.Write);
            byte[] fileData = new byte[inFS.Length];
            inFS.Read(fileData,0,(int)inFS.Length);
            ICryptoTransform enc = rj.CreateEncryptor();
            CryptoStream encStream = new CryptoStream(outFS,enc,CryptoStreamMode.Write);
            encStream.Write(fileData,0, fileData.Length);
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

        //Declarative!
        [PrincipalPermission(SecurityAction.Demand, Role= @"BUILTIN\Administrators")]                
        static void AdminOnly()
        {
            Console.WriteLine("This can only be ran by administrators!");
        }

        static void AlsoAdminOnly()
        {
            //Imperative!
            PrincipalPermission p = new PrincipalPermission(null, @"BUILTIN\Administrators", true);
            p.Demand();
        }
    }

    class CustomIdentity: IIdentity
    {
        private bool _IsAuthenticated;
        private string _name, _authtype;
        private string _firstName, _lastName, _address, _city, _state, _zip;

        public CustomIdentity ()
	{
            this._name = string.Empty;
            this._IsAuthenticated = false;
            this._authtype = "None";
            this._firstName = string.Empty;
            this._lastName = string.Empty;
            this._address = string.Empty;
            this._city = string.Empty;
            this._state = string.Empty;
            this._zip = string.Empty;
	}
        public CustomIdentity (bool isLogon, string AuthType, string newFirstName, string newLastName, string newAddress, string newCity, string newState, string newZip)
	    {
            _name = newFirstName + " " + newLastName;
            _firstName = newFirstName;
            _lastName = newLastName;
            _address = newAddress;
            _city = newCity;
            _state = newState;
            _zip = newZip;
            _authtype = AuthType;
            _IsAuthenticated = isLogon;
	    }
        #region IIdentity Members

        public string  AuthenticationType
        {
	        get { return this._authtype; }
        }

        public bool  IsAuthenticated
        {
	        get { return this._IsAuthenticated; }
        }

        public string  Name
        {
	        get { return this._name; }
        }

        #endregion

        public string FirstName
        {
            get{return this._firstName;}
        }
        public string LastName
        {
            get{return this._lastName;}
        }
        public string Address
        {
            get { return this._address;}
        }
        public string City
        {
            get{return this._city;}
        }
        public string State
        {
            get{return this._state;}
        }
        public string Zip
        {
            get{return _zip;}
        }


    }

    class CustomPrincipal : IPrincipal
    {

        private IIdentity _id;
        private string[] _roles;

        public CustomPrincipal (IIdentity Identity, string[] roles)
	{
            _id = Identity;
            _roles = new string[roles.Length];
            roles.CopyTo(_roles,0);
            Array.Sort(_roles);
	}
    #region IPrincipal Members

public IIdentity  Identity
{
	get { return _id; }
}

public bool  IsInRole(string role)
{
    return Array.BinarySearch(_roles, role)>=0 ? true: false;
}

#endregion
}
}
