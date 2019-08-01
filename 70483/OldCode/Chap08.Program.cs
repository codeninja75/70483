using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Principal;
using System.Security.Policy;
using System.Security.Permissions;
using System.Security.Authentication;

namespace Chap8
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain ad = AppDomain.CreateDomain("TestDomain");
            Console.WriteLine("Host Domain:" + AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Child Domain:" + ad.FriendlyName);
            ad.ExecuteAssembly("Chap1.exe");
            //or:
            //
            ad.ExecuteAssemblyByName("Chap1");
            AppDomain.Unload(ad);
            object[] hostEv = { new Zone(SecurityZone.Internet) };
            Evidence ev = new Evidence(hostEv, null);
            AppDomain d = AppDomain.CreateDomain("Domain1");
            try
            {
                d.ExecuteAssembly("Chap1.exe", ev);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            object[] hostEv1 = { new Zone(SecurityZone.MyComputer)};
            Evidence ev1 = new Evidence(hostEv1, null);
            AppDomain d1 = AppDomain.CreateDomain("Domain2", ev1);
            d1.ExecuteAssemblyByName("Chap1");

            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = "file://" + System.Environment.CurrentDirectory;
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = true;
            ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            AppDomain ad2 = AppDomain.CreateDomain("New domain", null, ads);
            ads = AppDomain.CurrentDomain.SetupInformation;
            Console.WriteLine(ads.ConfigurationFile);
            Console.WriteLine(ads.CachePath );
            Console.WriteLine(ads.ApplicationBase );
            Console.WriteLine(ads.ShadowCopyDirectories );
            Console.WriteLine(ads.PrivateBinPath );
            Console.WriteLine(ads.LicenseFile );
        }
    }
}
