using System;
using System.Security.Permissions;
using System.Security;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Formatters.Soap;
using Microsoft.VisualBasic;
namespace Chap5
{
    class Program
    {
        private static Byte[] _key = new Byte[] { 11, 188, 241, 26, 67, 172, 41, 240, 152, 19, 198, 201, 61, 35, 253, 160, 129, 101, 150, 236, 121, 72, 70, 132 };
        private static Byte[] _IV = new Byte[] { 215, 165, 95, 209, 215, 98, 33, 22 };
    

        static void Main(string[] args)
        {
            
            Console.Title = "70-536 Class";
            Console.ForegroundColor = ConsoleColor.Green;
            
            //BasicAndEncryptSerial();
            SoapAndXMLSerial();
        }
        static void BasicAndEncryptSerial()
        {
            Console.WriteLine("Basic Serialization");
            string test = "This is a big ole \n test string!\n see!\n";
            FileStream fs = new FileStream("StringSer.data", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, test);
            fs.Close();
            //fs = new FileStream("StringSer.data", FileMode.Open);
            StreamReader sr = new StreamReader("StringSer.data");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            Console.WriteLine("Datetime serialization");

            DateTime d = DateTime.Now;

            fs = new FileStream("DateSer.data", FileMode.Create);
            bf.Serialize(fs, d);
            fs.Close();
            sr = new StreamReader("DateSer.data");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            Console.WriteLine("now we encrypt the serialization!");

            Encryptor e = new Encryptor(EncryptionAlgorithm.TripleDes);
            e.IV = _IV;
            //e.Key = _key;
            MemoryStream m1 = new MemoryStream();
            bf.Serialize(m1, test);
            byte[] b1 = e.Encrypt(m1.GetBuffer(),_key);
            fs = new FileStream("StringEnc.data", FileMode.Create);
            fs.Write(b1, 0, b1.Length);
            fs.Close();

            m1 = new MemoryStream();
            bf.Serialize(m1, d);
            b1 = e.Encrypt(m1.GetBuffer(), _key);
            fs = new FileStream("DateEnc.data",FileMode.Create);
            fs.Write(b1, 0, b1.Length);
            fs.Close();

            Console.WriteLine("Now look at the encrypted serializations!");

            sr = new StreamReader("StringEnc.data");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            sr = new StreamReader("DateEnc.data");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            Console.WriteLine("Now we unserialize!");

            //sr = new StreamReader("StringSer.data");
            FileStream f = new FileStream("StringSer.data", FileMode.Open);
            string test1 = (string)bf.Deserialize(f);
            f.Close();
            Console.WriteLine(test1);
            //sr = new StreamReader("DateSer.data");
            f = new FileStream("DateSer.data",FileMode.Open);
            DateTime d1 = (DateTime)bf.Deserialize(f);
            Console.WriteLine(d1);
            f.Close();

            Console.WriteLine("Unserizling encrypted data!");

            Decryptor de = new Decryptor(EncryptionAlgorithm.TripleDes);
            de.IV = _IV;
            m1 = new MemoryStream();
            //sr = new StreamReader("StringEnc.data");
            f = new FileStream("StringEnc.data", FileMode.Open);
            BinaryReader br = new BinaryReader(f);
            List<byte> lb = new List<byte>();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                lb.Add(br.ReadByte());
            }
            lb.CopyTo(b1);
            byte[] b2 = de.Decrypt(b1, _key);
            m1 = new MemoryStream(b2);
            Console.WriteLine((string)bf.Deserialize(m1));

            m1 = new MemoryStream();
            //sr = new StreamReader("DateEnc.data");
            f = new FileStream("DateEnc.data", FileMode.Open);
            br = new BinaryReader(f);
            lb = new List<byte>();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                lb.Add(br.ReadByte());
            }
            lb.CopyTo(b1);
            b2 = de.Decrypt(b1, _key);
            m1 = new MemoryStream(b2);
            Console.WriteLine((DateTime)bf.Deserialize(m1));


            Console.ReadLine();
        }
        static void SoapAndXMLSerial()
        {
            Console.WriteLine("Soap Serialization");
            TestClass t = new TestClass();
            t.FlowDateTime = DateTime.Today;
            t.IsBad = true;
            t.IsGood = false;
            t.Val1 = 5.55;
            t.Val2 = 66.66;
            t.Val3 = 99;
            SoapFormatter sf = new SoapFormatter();
            FileStream fs = new FileStream("TestClass.soap", FileMode.Create);
            sf.Serialize(fs, t);
            fs.Close();
            StreamReader sr = new StreamReader("TestClass.soap");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            TestClass1 t1 = new TestClass1();
            t1.Date = DateTime.Today;
            t1.Val1 = 5.55;
            t1.Val2 = 66.66;
            t1.Val3 = 99;
            //t1.Alarms[0] = true;
            //t1.Alarms[1] = false;
            //t1.Alarms[6] = true;
            fs = new FileStream("TestClass1.soap", FileMode.Create);
            sf.Serialize(fs, t1);
            fs.Close();
            sr = new StreamReader("TestClass1.soap");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();

            //ONLY PUBLIC FIELDS GET XML or SOAP SERIALIZATION!
            Console.WriteLine("XML Serialization");
            Type ty = Type.GetType("Chap5.TestClass");
            XmlSerializer x = new XmlSerializer(ty);
            fs = new FileStream("TestClass.xml", FileMode.Create);
            x.Serialize(fs, t);
            fs.Close();
            sr = new StreamReader("TestClass.xml");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            
            ty = Type.GetType("Chap5.TestClass1");
            XmlSerializer x1 = new XmlSerializer(ty);
            //x = new XmlSerializer(ty);
            fs = new FileStream("TestClass1.xml", FileMode.Create);
            x1.Serialize(fs, t1);
            fs.Close();
            sr = new StreamReader("TestClass1.xml");
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();


            

            Console.ReadLine();
        }

    }

    [Serializable]
    public class TestClass : ISerializable,IDeserializationCallback
    {
        private double _Val1;
        private double _Val2;
        private int _Val3;
        private DateTime _Date;
        private BitArray _Alarms = new BitArray(10);


        public double Val1
        {
            get { return _Val1; }
            set { _Val1 = value; }
        }
        public double Val2
        {
            get { return _Val2; }
            set { _Val2 = value; }
        }
        public int Val3
        {
            get { return _Val3; }
            set { _Val3 = value; }
        }
        public DateTime FlowDateTime
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public bool IsGood   
        {
            get { return _Alarms[0]; }
            set { _Alarms[0] = value; }
        }
        public bool IsBad
        {
            get { return _Alarms[1]; }
            set { _Alarms[1] = value; }
        }

        public TestClass()
        {
            
        }


        private String _myvar;
        public String ThisVar
        {
            get { return _myvar; }
        }
	

        protected TestClass(SerializationInfo info, StreamingContext context)
        {
           
            _Val3 = info.GetInt32("_val3");
            _Val1 = info.GetDouble("_val1");
            _Val2 = info.GetDouble("_val2");
            _Date = info.GetDateTime("_Date");
            _Alarms = (BitArray)info.GetValue("_Alarms" ,  Type.GetType("System.Collections.BitArray"));
        }

        #region ISerializable Members
        /// <summary>
        /// Filling in some text
        /// </summary>
        /// <param name="info">testing</param>
        /// <param name="context">this stuff</param>
        [SecurityPermissionAttribute(SecurityAction.Demand , SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new Exception("The method or operation is not implemented.");
            info.AddValue("_val1", _Val1);
            info.AddValue("_val2", _Val2);
            info.AddValue("_val3", _Val3);
            info.AddValue("_Date", _Date);
            info.AddValue("_Alarms", _Alarms);
        }

        #endregion

        #region IDeserializationCallback Members
        // FOR XML & SOAP
        public void OnDeserialization(object sender)
        {
            //throw new Exception("The method or operation is not implemented.");
            _Val2 = 11.11;
        }

        #endregion
        
        [OnSerializing] //ONLY FOR BINARY
        private void BeforeSerialize(StreamingContext sc)
        {

        }

        [OnSerialized]//ONLY FOR BINARY
        private void AfterSerialize(StreamingContext sc)
        { }

        [OnDeserializing]//ONLY FOR BINARY
        private void DuringDeserialize(StreamingContext sc)
        { }

        [OnDeserialized]//ONLY FOR BINARY
        private void AfterDeserialized(StreamingContext sc)
        {
            this._Val1 = 1111.11;
        }

        public override string ToString()
        {
            object[] b = {_Val1,_Val2,_Val3,_Date};
            return string.Format("{0} , {1} , {2} , {3}" , b );
        }

    }

    [Serializable, XmlInclude(typeof(TestClass)),XmlInclude(typeof(BitArray))]
    public class TestClass1
    {
        public double Val1;
        public double Val2;
        public int Val3;
        [XmlAttribute]  public DateTime Date;
        [XmlIgnore] //XML can't encode the bitarray unless you do the  XMLInclude!
        //[XmlArray,XmlArrayItem(DataType = "boolean", ElementName = "bit", IsNullable = true,Namespace = "http://www.totalflow.com", Type = typeof(bool))]
        public BitArray Alarms ;
        public List<TestClass> T = new List<TestClass>();
        
        public TestClass1()
        {

        }

    }
}
