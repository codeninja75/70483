using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Security.Permissions;
using System.Net.Security;
// This is assembly level DECLARATIVE security
// The runtime will throw an exception if the assembly cannot get this permission at launch.
[assembly:FileIOPermission(SecurityAction.RequestMinimum, Read=@"C:\config.sys")]
//this permission will be disallowed by the assembly even if granted it by the zone or calling assembly.
//this line cannot throw an exception, if you refuse something you don't have,  you cause no exception.
[assembly:RegistryPermission(SecurityAction.RequestRefuse , Write=@"HKEY_LOCAL_MACHINE\System")]
[assembly:UIPermission(SecurityAction.RequestMinimum, Unrestricted=true)]
//runtime will not throw this as an exception if you don't get it...
//[assembly: FileIOPermission(SecurityAction.RequestOptional, Read = @"C:\config.sys")]

namespace Chap11
{
    class Program
    {
        //METHOD level Declareative security
        //Use permit only to limit the permissions available to each method
        [RegistryPermission(SecurityAction.PermitOnly , Read=@"HKEY_LOCAL_MACHINE\System")]
        //Use deny to remove permissions from the permission set available to the method
        [DataProtectionPermission (SecurityAction.Deny , Unrestricted = true)]
        //Use link demand to check if the immediate caller has the right permissions
        [FileIOPermission(SecurityAction.LinkDemand , Read=@"C:\config.sys")]
        //use demand to check the entire calling chain to make sure all entities in the 
        //call stack have the appropriate 
        [FileIOPermission(SecurityAction.Demand, Read = @"C:\config.sys")]
        //Use Assert to short circut total chain checks (demand)  this will throw
        //an exception if the assembly does not have the permission
        [FileIOPermission(SecurityAction.Assert , Write=@"C:\code\")]
        //Inherit demand enforces that any code which inherits from this code must have the following permission
        [PublisherIdentityPermission(SecurityAction.InheritanceDemand  , CertFile=@"test.crt")]
        static void Main(string[] args)
        {
            
            
            try
            {
                EnvironmentPermission ep = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "PROGRAMFILES");
                ep.Demand();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            //System.Security.Permissions
            //EnvironmentPermission 
            //FileDialogPermission 
            //FileIOPermission
            //IsolatedStorageFilePermission 
            //KeyContainerPermission 
            //PublisherIdentityPermission
            //ReflectionPermission
            //RegistryPermission
            //SiteIdentityPermission
            //StorePermission
            //StrongNameIdentityPermission 
            //UIPermission 
            //UrlIdentityPermission
            //ZoneIdentityPermission
            //DataProtectionPermission 
            //GacIdentityPermission 
            
            //System.Net
            //SocketPermission 
            //DnsPermission 
            //EndpointPermission 
            //WebPermission 
            //Mail.SmtpPermission 
            
            //System.ServiceProcess.ServiceControllerPermission 
            //System.Data.Common.DBDataPermission 
            //System.Diagnostics.EventLogPermission
            //System.Diagnostics.PerformanceCounterPermission
            //System.DirectoryServices.DirectoryServicesPermission 
            //System.Messaging.MessageQueuePermission 
            //System.Configuration.ConfigurationPermission            
            //System.Data.Odbc.OdbcPermission

            //System.Web.AspNetHostingPermission 

        }
    }
}
