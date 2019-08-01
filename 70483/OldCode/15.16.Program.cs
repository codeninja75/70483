using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Net;
using System.Globalization;

namespace Chap15._6
{
    class Program
    {
        static void Main(string[] args)
        {
            //MailTest();
            CultureInfo usercul = System.Threading.Thread.CurrentThread.CurrentCulture;
            Console.WriteLine("The current culture of this app is :" + usercul.Name);
            Console.WriteLine("The Dispaly name of this app is :" + usercul.DisplayName);
            Console.WriteLine("The Native name of this app is :" + usercul.NativeName);
            Console.WriteLine("The ISO abbreviation of this app is :" + usercul.TwoLetterISOLanguageName);
            string cur = (100000).ToString("C");
            Console.WriteLine(cur);
            usercul = System.Threading.Thread.CurrentThread.CurrentUICulture;
            Console.WriteLine("The current culture of this app is :" + usercul.Name);
            Console.WriteLine("The Dispaly name of this app is :" + usercul.DisplayName);
            Console.WriteLine("The Native name of this app is :" + usercul.NativeName);
            Console.WriteLine("The ISO abbreviation of this app is :" + usercul.TwoLetterISOLanguageName);
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-VE");
            Console.WriteLine("The current culture of this app is :" + System.Threading.Thread.CurrentThread.CurrentCulture);
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures)) 
            {
                Console.WriteLine("culture:" + cul.Name);

            }
            usercul = System.Threading.Thread.CurrentThread.CurrentCulture;
            RegionInfo ri = new RegionInfo(usercul.LCID);
            //OR
            ri = new RegionInfo(usercul.Name);
            Console.WriteLine("English name: " + ri.EnglishName);
            Console.WriteLine("Display name: " + ri.DisplayName);
            Console.WriteLine("Currency Symbol: " + ri.CurrencySymbol);
            string[] days = usercul.DateTimeFormat.DayNames;
            foreach(string day in days)
            {
                Console.WriteLine("Day name for Venezuelan Spanish: " + day);
            }
            string[] months = usercul.DateTimeFormat.MonthNames;
            foreach(string mon in months)
            {
                Console.WriteLine("Month name in Venz. Spanish:" + mon);
            }
            Console.WriteLine("Number decimal Symbol:" + usercul.NumberFormat.NumberDecimalSeparator);
            CompareInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture.CompareInfo;
            Console.WriteLine(ci.Name);
            Console.WriteLine(ci.LCID);
            string firstString = "Coté";
            string secondString= "coté";
            ci = new CultureInfo("fr-FR").CompareInfo;
            Console.WriteLine( ci.Compare(firstString, secondString));
            Console.WriteLine( ci.Compare(firstString, secondString,CompareOptions.IgnoreCase));
            CultureAndRegionInfoBuilder build = new CultureAndRegionInfoBuilder("en-MS", CultureAndRegionModifiers.None);
            CultureInfo USCulture = new CultureInfo("en-US");
            RegionInfo USRegion = new RegionInfo("US");
            build.LoadDataFromCultureInfo(USCulture);
            build.LoadDataFromRegionInfo(USRegion);
            build.NumberFormat.CurrencySymbol = "*";
            build.NumberFormat.CurrencyDecimalSeparator = "^";
            build.Save(@"c:\msft.culture.xml");
            
            
            //build.Register();
            //System.Threading.Thread.CurrentThread.CurrentCultu
            


        }

        private static void MailTest()
        {
                    MailMessage m = new MailMessage("bob@spam.com","test1@cool.com", "This is a test email", "And here's some cool text");
                    m = new MailMessage();
                    m.From = new MailAddress("me@spam.com", "me!!!");
                    m.To.Add(new MailAddress("crash@badcode.com"));
                    m.CC.Add(new MailAddress("mm@mm.com"));
                    m.Bcc.Add(new MailAddress("ss@ss.com"));
                    m.Subject = "This is a new message";
                    m.Body = "this is some new body text";
                    m.ReplyTo = new MailAddress("info@spam.com", "reply to me");
                    m.Attachments.Add(new Attachment(@"C:\boot.ini"));
                    Stream sr = new FileStream(@"C:\boot.ini", FileMode.Open, FileAccess.Read);
                    m.Attachments.Add(new Attachment(sr, "boot.ini1", MediaTypeNames.Application.Octet));
                    m.Body = "<html><body><h1>My message</h1><br>This is an HTML message.</body></html>";
                    m.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient("smtp.abb.com");
                    client.Send(m);
                    string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:Pic1\"></body></html>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody,null, MediaTypeNames.Text.Html);
                    LinkedResource pic1 = new LinkedResource("pic.jpg", MediaTypeNames.Image.Jpeg);
                    pic1.ContentId = "Pic1";
                    avHtml.LinkedResources.Add(pic1);
                    string textBody = "You must use an email client that supports HTML messages";
                    AlternateView avText = AlternateView.CreateAlternateViewFromString(textBody, null, MediaTypeNames.Text.Plain);
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Credentials = new NetworkCredential("user", "password");
                    client.EnableSsl = true;
                    client.SendCompleted +=new SendCompletedEventHandler(client_SendCompleted);
                    client.SendAsync(m, null);
                    client.SendAsyncCancel();
        }

        static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
