using System;
using System.Collections.Generic;
using System.Text;

namespace Chap1
{
    class Program
    {
        #region Enum
        enum TestEnum
        {
            Mr = 1,
            Mrs = 2,
            Miss = 3,
            Frau = -1,
            Mau = -1
        }
        #endregion
        static void Main(string[] args)
        {
            Console.WriteLine("Chap1");
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            
            //Lession 1: value types
            
            double d1 = 2.5;
            int i1 = 5555;
            int i2 = i1;
            i1 = 22;
            Console.WriteLine("i1 = {0}" , i1);
            Console.WriteLine("i2 = {0}", i2);
            string s1 = "spam";
            object o1, o2, o3;
            o1 = d1;
            o2 = i1;
            o3 = s1;
            Console.WriteLine(o1.GetType().FullName);
            Console.WriteLine(o2.GetType().FullName);
            Console.WriteLine(o3.GetType().FullName);
            bool b1 = true;
            bool? b2 = true;
            Nullable<bool> b3 = new Nullable<bool>();

            Console.WriteLine("bool b1 is:{0}", b1.ToString());
            Console.WriteLine("bool b3 is null? {0}", b3 == null);
            Console.WriteLine("bool b2 is null? {0}", b2 == null);
            Console.WriteLine("bool b2 has a value? {0}", b2.HasValue.ToString());
            Console.WriteLine("Bool b3 has a value? {0}", b3.HasValue.ToString());
            ValChange v = new ValChange();
            v.Date = DateTime.Now;
            v.Value = 5.4432;
            Console.WriteLine("ValChange v =:" + v.ToString());
            //enums
            TestEnum te = TestEnum.Frau;
            Console.WriteLine("Test enum is equal to Mau:{0}", te == TestEnum.Mau);
            //strings & string builders
            string  s2, s3, s4;
            s1 = "matt";
            s2 = "test";
            s3 = "spam";
            s3.Replace("pa", "ki");
            s4 = s1;
            s1 = "abby";
            Console.WriteLine("string s1=" + s1);
            Console.WriteLine("string s2=" + s2);
            Console.WriteLine("string s3=" + s3);
            Console.WriteLine("string s4=" + s4);
            StringBuilder sb = new StringBuilder();
            sb.Append(s1);
            sb.Append(" | ");
            sb.Append(s2);
            sb.Append(" | ");
            sb.Append(s3);
            sb.Append(" | ");
            sb.Append(s4);
            Console.WriteLine("String builder sb:" + sb.ToString());
            sb.Replace("pa", "ki");
            Console.WriteLine("String builder sb:" + sb.ToString());
            //arrays

            int[] ar = { 3, 2, 1 };
            Array.Sort(ar);
            Console.WriteLine("{0}, {1}, {2}", ar[0], ar[1], ar[2]);
            //streams
            System.IO.StreamWriter sw = new System.IO.StreamWriter("text.txt");
            sw.WriteLine("Hello Totalflow!");
            sw.Close();
            System.IO.StreamReader sr = new System.IO.StreamReader("text.txt");
            Console.WriteLine(sr.ReadToEnd() );
            sr.Close();

            
            //exceptions

            try
            {
                throw new TFBigException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                throw new TFException();
            }
            catch (TFBigException ex1)
            { 
                Console.WriteLine(ex1.ToString()); 
            }
            catch (TFException ex2)
            {
                Console.WriteLine(ex2.ToString());
            }

            //inheritance
            Test1 t1 = new Test2();
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t1.foobar().ToString());
            //interface

            Test3 t3 = new Test3();
          
            IComparable t3i = t3;
            Console.WriteLine("T3 IComparable interface:{0}" ,t3i.CompareTo(new object()));
            Test2 t2 = new Test2();
            Console.WriteLine("T2 IComparable interface v1:{0}" ,t2.CompareTo(new Test2()));
            IComparable t2i = t2;
            Console.WriteLine("T2 IComparable interface v2:{0}", t2i.CompareTo(new Test2()));
            //partial classes
            //generics
            List<DateTime> ldt = new List<DateTime>();
            ldt.Add(new DateTime(2005, 12, 11));
            ldt.Add(new DateTime(2006, 4, 25));
            Console.WriteLine("generic list ldt contains 1/1/2222:" + ldt.Contains(new DateTime(2222, 1, 1)).ToString());
            Console.WriteLine("generic list ldt contains 4/25/2006:" + ldt.Contains(new DateTime(2006, 4, 25)).ToString());
            Console.WriteLine("Generic method MyGenMethod {0}", MyGenMethod<Test3>(t3).ToString());
            Gen1<Test3, ValChange> g1 = new Gen1<Test3, ValChange>(t3,v);
            Console.WriteLine("Generic class g1 to string:" + g1.ToString());
            //events
            GetMessage += new MyDelegate(Program_GetMessage);
            Console.WriteLine("Getmessage returns {0}" , GetMessage("Here we are!"));
            GetMessage += new MyDelegate(Program_GetMessage1);
            Console.WriteLine("Getmessage returns {0}", GetMessage("Here we are!"));
            //attributes
            
            //type forwarding

            //conversion

            //boxing & unboxing

            //conversion implmentation
            MyInt a1;
            a1.Value = 44;
            Int64 i64 = Convert.ToInt64(a1);
            b1 = Convert.ToBoolean(a1);

            MyIntStruct a; int b;
            a = 45;
            b = (int)a;
            Console.WriteLine("a = {0} , b ={1}", a.ToString(), b.ToString());
            
        }
        #region delegate handlers
        static event MyDelegate GetMessage;
        static int Program_GetMessage1(string Message)
        {
            Console.WriteLine(Message);
            return 6;            
        }

        static int Program_GetMessage(string Message)
        {
            Console.WriteLine(Message);
            return 5;
        }
        #endregion
        #region Generic Method
        static bool MyGenMethod<T>(T t) where T : IComparable { return true; }
        #endregion

        static void FooBarTest()
        {
            List<Foo> fList = new List<Foo>();
            fList.Add(new Foo());
            fList.Add(new Bar());
            fList.Add(new SuperBar());

        }
    }

    public class Foo
    {
        public virtual string GetfValue() { return "Foo";  }
    }
    public interface iFoo {  string GetfValue();}
    public class Bar : Foo, iFoo
    {
        public override string GetfValue() { return "Bar";}
    }
    public class SuperBar : Bar
    {
        public override string GetfValue() { return "SuperBar"; }
    }
    public delegate int MyDelegate(string Message);
    #region Exception classes
    class TFException : System.Exception , IFormattable, IConvertible
    {
        public override string ToString()
        {
            return "TFException! \n" + base.ToString();
        }

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ToString(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    class TFBigException : TFException
    {
        public override string ToString()
        {
            return "TFbigexception! \n" + base.ToString();
        }
    }
    #endregion

    #region Inheritance & interfaces
    abstract class Test1
    {
        public abstract string foo();
        public void bar()
        { }
        public virtual int foobar()
        {
            return 1;
        }
        public double testfoo()
        {
            return 2.3;
        }
        public virtual string getfoo()
        {
            return "Test1";
        }
    }
    
    [Serializable]
    class Test2 : Test1 , IComparable 
    {
        private int _testVal;

        public int testVal
        {
            set {_testVal = value; }
            get { return _testVal; }
        }
        public override string foo()
        {
            return "Test2";
        }
        public bool foo(string b)
        {
            return true;
        }
        public new void bar()
        {
            base.bar();
        }
        public new double testfoo()
        {
            return 5.6;
        }
        public sealed override string getfoo()
        {
            return base.getfoo() + " Test2";
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (this._testVal > ((Test2)obj)._testVal)
                return 1;
            Test2 t = (Test2)obj;
            if (this._testVal < t._testVal)
                return -1;
            else
                return 0;
        }

        #endregion
    }

    class Test3 : IComparable 
    {
        public new string getfoo()
        {
            return "f";
        }
    #region IComparable Members

    int  IComparable.CompareTo(object obj)
    {
 	    return 1;
    }

    #endregion
}
    #endregion

    #region Generic Class
class Gen1<T,U>
        where T : Test3 , IComparable, new() 
        where U : struct
    {
        public T cdata;
        public U vdata;
        public Gen1(T mydata , U mystruct )
        {
            cdata = mydata;
            vdata = mystruct;
        }
        public override string ToString()
        {
            return string.Format("In generic class w/ {0} , {1}", cdata.ToString(), vdata.ToString());
        }

    }
#endregion

    #region Structs
    struct ValChange
    {
        private double _val;
        private DateTime _date;

        public ValChange(double Val, DateTime Date)
        {
            this._date = Date;
            this._val = Val;
        }

        public double Value
        {
            get { return _val; }
            set { _val = value; }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public override bool Equals(object obj)
        {
            try
            {
                ValChange v = (ValChange)obj;
                if (v._date == this._date && v._val == this._val)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return (int)(_val.GetHashCode() + _date.GetHashCode()) / 2;
        }
        public override string ToString()
        {
            return "On " + _date.ToString() + " value was " + _val.ToString();
        }
    }

    struct MyInt : IConvertible 
    {
        public int Value;


        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (Value == 0)
                return true;
            else
                return false;
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return (decimal)Value;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return (double)Value;
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Value;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Value;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ToString(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
    struct MyIntStruct
    {
        public int Value;
        public static implicit operator MyIntStruct(int arg)
        {
            MyIntStruct res = new MyIntStruct();
            res.Value = arg;
            return res;
        }
        public static explicit operator int(MyIntStruct arg)
        {
            return arg.Value;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
    #endregion

}


