using System;

namespace DotNet.E70483.CreateUseTypes
{
    public class CreateTypes
    {
        public static void ExerciseTypes()
        {
            MyInt a1;
            a1.Value = 44;
            Int64 i64 = Convert.ToInt64(a1);
            bool b1 = Convert.ToBoolean(a1);

            MyIntStruct a; int b;
            a = 45;
            b = (int)a;
            Console.WriteLine("a = {0} , b ={1}", a.ToString(), b.ToString());

            Test1 t1 = new Test2();
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t1.foobar().ToString());
            //interface
            ValChange v = new ValChange();
            v.Date = DateTime.Now;
            v.Value = 5.4432;
            Console.WriteLine("ValChange v =:" + v.ToString());

            Test3 t3 = new Test3();
            IComparable t3i = t3;
            Console.WriteLine("T3 IComparable interface:{0}", t3i.CompareTo(new object()));
            Test2 t2 = new Test2();
            Console.WriteLine("T2 IComparable interface v1:{0}", t2.CompareTo(new Test2()));
            IComparable t2i = t2;
            Console.WriteLine("T2 IComparable interface v2:{0}", t2i.CompareTo(new Test2()));

            TestEnum te = TestEnum.Frau;
            Console.WriteLine("Test enum is equal to Mau:{0}", te == TestEnum.Mau);
            Gen1<Test3, ValChange> g1 = new Gen1<Test3, ValChange>(t3, v);
            Console.WriteLine("Generic class g1 to string:" + g1.ToString());

        }
    }

    #region Generic Class
    class Gen1<T, U>
            where T : Test3, IComparable, new()
            where U : struct
    {
        public T Cdata { get; set; }
        public U Vdata { get; set; }
        public Gen1(T mydata, U mystruct)
        {
            Cdata = mydata;
            Vdata = mystruct;
        }
        public override string ToString()
        {
            return string.Format("In generic class w/ {0} , {1}", Cdata.ToString(), Vdata.ToString());
        }

    }
    #endregion

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
    public class Foo
    {
        public virtual string GetfValue() { return "Foo"; }
    }
    public interface iFoo { string GetfValue(); }
    public class Bar : Foo, iFoo
    {
        public override string GetfValue() { return "Bar"; }
    }
    public class SuperBar : Bar
    {
        public override string GetfValue() { return "SuperBar"; }
    }

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
    class Test2 : Test1, IComparable
    {
        private int _testVal;

        public int testVal
        {
            set { _testVal = value; }
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

        int IComparable.CompareTo(object obj)
        {
            return 1;
        }

        #endregion
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
