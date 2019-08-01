#define LOGEVENTS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Security.Authentication;

namespace CPC
{
    
    public partial class CPC : ServiceBase
    {
        public CPC()
        {
            InitializeComponent();
            
            //AttachDBFileName='" | DataDirectory | + @"\MyDb.mdf'
            

        }
        private SqlConnection _conn;
        private System.Threading.Timer _timer;
        protected override void OnStart(string[] args)
        {
            _conn = new SqlConnection(@"Data Source='.\SQLEXPRESS'; Initial Catalog=;Integrated Security = true;AttachDBFileName='" + AppDomain.CurrentDomain.BaseDirectory  +@"\WINCCUServ.mdf'");
            _timer = new System.Threading.Timer(new TimerCallback(RunIt), null, 10000, 500);
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        protected override void OnContinue()
        {
            _timer.Change(1000, 500);
            base.OnContinue();
        }
        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);
        }
        protected override void OnShutdown()
        {
            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            _conn.Dispose();
            _timer.Dispose();
            base.OnShutdown();
        }
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnPause()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            base.OnPause();
        }
        public override EventLog EventLog
        {
            get
            {
                return base.EventLog;
            }
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
          
            base.OnSessionChange(changeDescription);
        }

        private void RunIt(object o)
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            SqlCommand sc = new SqlCommand();
            SqlDataReader dr;
            try
            {
                if (_conn.State == ConnectionState.Broken || _conn.State == ConnectionState.Closed)
                { _conn.Open(); }
                if (_conn.State == ConnectionState.Open)
                {
                    sc = new SqlCommand();
                    sc.Connection = _conn;
                    sc.CommandText = "select top 100000 * from CacheData";
                    dr = sc.ExecuteReader();
                    while (dr.Read())
                    {

                    }
                    dr.Close();

                }
            }
            catch (Exception ex)
            {

                this.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
            }
            finally
            {

                if (sc != null)
                {
                 

                    sc.Dispose();
                    sc = null;
                    dr = null;
                }
            }
            _timer.Change(1000, 500);
        }
    }
}
