using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.E70483.CreateUseTypes
{
    public class ManipulateStrings
    {
        public static void Manipulate()
        {
            PrintTest();
            Console.ReadLine();
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
            PrintTest();
            Console.ReadLine();
            RegExTest();
            Console.ReadLine();
            EncodingTest();
            Console.ReadLine();
            Inerpolate();
            Console.ReadLine();
        }

        private static void Inerpolate()
        {
            string name = "Bob";
            int age = 29;
            Console.WriteLine($"You name is {name} and your age is {age,-5:D}");
            double chedder = 9459.33;
            FormattableString balanceMsg = $"US balance :{chedder:C}";
            CultureInfo usProvider = new CultureInfo("en-US");
            Console.WriteLine(balanceMsg.ToString(usProvider));
            CultureInfo derSpiegel = new CultureInfo("de-DE");
            Console.WriteLine(balanceMsg.ToString(derSpiegel));
        }

        static void Print(string format, object val)
        {
            try
            {
                Console.WriteLine("Doing " + format + " on " + val.ToString() + " =" + string.Format(format, val));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to do: {0} to {1}", format, val);
                Console.WriteLine(ex.Message);
            }
        }
        static void PrintTest()
        {
            //alignment
            Print("->{0,10}<-", "Hello");
            Print("->{0,-10}<-", "Hello");
            // c = Currency
            Print("{0:c}", 1.24);
            Print("{0:c}", -12400);
            //d = whole number!
            Print("{0:d}", 1.24);
            Print("{0:d}", -12400);
            //f = fixed point
            Print("{0:f}", 1.24);
            Print("{0:f}", -12400);
            //e = scientific
            Print("{0:e}", 1.24);
            Print("{0:e}", -12400);
            //g = general
            Print("{0:g}", 1.24);
            Print("{0:g}", -12400);
            //n = number w/ commas for thousands
            Print("{0:n}", 1.24);
            Print("{0:n}", -12400);
            //r = round trippable (should be able to be parsed back into the same numeric value)
            Print("{0:r}", 1.24);
            Print("{0:r}", -12400);
            //x = hex
            Print("{0:x4}", 1.24);
            Print("{0:x4}", -12400);
            //p = percent
            Print("{0:p}", 1.24);
            Print("{0:p}", -12400);
            //custom numbers
            Print("{0:0000000.000}", 1.24);
            Print("{0:0000000.000}", -12400);
            Print("{0:(#).##}", 23243.4);
            Print("{0:(#).##}", -12400);
            Print("{0:0.0}", -12400);
            Print("{0:0,0}", -12400);

            Print("->{0,-25:D}<-", DateTime.Now);
            Print("->{0,40:F}<-", DateTime.Now);
            Print("{0:U}", DateTime.Now);
            //custom date
            Print("{0:MMM HH:mm:ss tt ddd gg}", DateTime.Now);
            //Enums
            Print("{0:g}", TestEnum.Bob1);
            Print("{0:f}", TestEnum.Test2);
            Print("{0:d}", TestEnum.Test4);
            Print("{0:x}", TestEnum.Test8);
        }
        enum TestEnum
        {
            Bob1,
            Test2,
            Test4,
            Test8
        }
        static void RegExTest()
        {
            Console.WriteLine(@"12345 matches via ^\d{5}$ ?: " + Regex.IsMatch("12345", @"^\d{5}$"));
            Console.WriteLine(@"1234578 matches via ^\d{5}$ ?: " + Regex.IsMatch("1234578", @"^\d{5}$"));
            Console.WriteLine("     I want to remove all the white space!  !\n" + Regex.Replace("     I want to remove all the white space!  !", @"^[ \t]+", ""));
            Console.WriteLine(
            Regex.Match(@"<INPUT> this is some input ?? (**&*%%^ </INPUT>", @"<INPUT\b[^>]*>(.*?)</INPUT>").Groups[1].ToString()
            );
        }
        static void EncodingTest()
        {
            Encoding e = Encoding.GetEncoding("Korean");
            byte[] encoded;
            encoded = e.GetBytes("Hello folks!");
            for (int i = 0; i < encoded.Length; i++)
            {
                Console.WriteLine("Byte:{0} = {1}", i, encoded[i]);
            }

            EncodingInfo[] ei = Encoding.GetEncodings();
            foreach (EncodingInfo ex in ei)
            {
                Console.WriteLine("{0} {1} {2}", ex.CodePage, ex.Name, ex.DisplayName);
            }

            StreamWriter swUTF7 = new StreamWriter("utf7.txt", false, Encoding.UTF7);
            swUTF7.WriteLine("utf7 text");
            swUTF7.WriteLine("Hello, world!");
            swUTF7.Close();

            StreamWriter swUTF8 = new StreamWriter("utf8.txt", false, Encoding.UTF8);
            swUTF8.WriteLine("utf8 text");
            swUTF8.WriteLine("Hello, world!");
            swUTF8.Close();

            StreamWriter swUTF16 = new StreamWriter("utf16.txt", false, Encoding.Unicode);
            swUTF16.WriteLine("utf16 text");
            swUTF16.WriteLine("Hello, world!");
            swUTF16.Close();

            StreamWriter swUTF32 = new StreamWriter("utf32.txt", false, Encoding.UTF32);
            swUTF32.WriteLine("utf32 text");
            swUTF32.WriteLine("Hello, world!");
            swUTF32.Close();

            //UTF 7 files must be spec'd as such when read

            StreamReader sr = new StreamReader("utf7.txt", Encoding.UTF7);
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            //
            sr = new StreamReader("utf8.txt");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            sr = new StreamReader("utf16.txt");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            sr = new StreamReader("utf32.txt");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            int intz = 5;
            intz.ToString("d");

        }
    }
}
