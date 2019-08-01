using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Collections;
using System.ComponentModel;
namespace CPC
{


        [RunInstallerAttribute(true)]//**1**
        public class ProjectInstaller : ServiceProcessInstaller 
        {  //**2**
            
            private System.ServiceProcess.ServiceInstaller serviceInstaller1; //**3**
            private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1; //**4**

            public ProjectInstaller()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
                this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();

                this.serviceInstaller1.DisplayName = "CPC";  //**5**
                this.serviceInstaller1.ServiceName = "CPC";  //**6**

                this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;  //**7**

                this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;  //**8**
                //this.serviceProcessInstaller1.Password = null;  //**9**
                //this.serviceProcessInstaller1.Username = null;

                //**10**
                this.Installers.AddRange(new System.Configuration.Install.Installer[] {

	this.serviceProcessInstaller1,
	this.serviceInstaller1});
            }
            private void test()
            {
                ServiceController sc = new ServiceController("CPC");

                //sc = new ServiceController(
                //sc.ExecuteCommand(55);
            }

        }
}

