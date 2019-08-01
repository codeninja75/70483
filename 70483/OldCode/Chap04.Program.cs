using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Reflection;

namespace Chap4
{

    


    class Program
    {
        static void Main(string[] args)
        {

            
            Console.Title = "70-536 Class";
            CollectionOverview();
            SequenctialLists();
            DictionaryTest();
            SpecilizedTest();
            GenericTest();
            
            
        }
        static void CollectionOverview()
        {

            Console.WriteLine();
            Console.WriteLine("CollectionOverview");
            Console.WriteLine();

            //Array list
            ArrayList al = new ArrayList(5);
            string s = "Hello";
            al.Add(s);
            al.Add(5436);
            al.Add("Hello!");
            al.Add(new object());
            al.Remove(5436);
            al.Reverse();
            al.Add(22.22);
            object[] b = { "Happy", "Sad", new StringBuilder() };
            al.AddRange(b);
            al.RemoveAt(al.Count /2);
            for (int i = 0; i < al.Count; i++)
            {
                Console.WriteLine("At pos: {0}  type: {1} value:{2}", i, al[i].GetType().Name,al[i]);
            }
            //OR:
            IEnumerator ie = al.GetEnumerator(0,al.Count/2);
            while (ie.MoveNext())
            {
                Console.WriteLine("At pos: ?  type: {0} value:{1}", ie.Current.GetType().ToString(), ie.Current);
            }
            //OR:
            foreach (object o in al)
            {
                Console.WriteLine("At pos: ?  type: {0} value:{1}", o.GetType().ToString(), o);
            }
            //IF ALL items are strings you can do this:
            try
            {
                foreach (string st in al)
                {
                    Console.WriteLine(st);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Can't do this becuase:" + ex.Message);
            }
            Console.WriteLine("collection contains {0}? {1}", 22.22, al.Contains(22.22));
            Console.WriteLine("collection contains {0}? {1}", "Hello!", al.Contains("Hello!"));
            al.Clear();
            Console.WriteLine("collection has {0} items", al.Count);

            Type[] t = al.GetType().GetInterfaces();
            foreach (Type tz in t)
            {
                Console.WriteLine(tz.FullName);
            }

            //Comparing!
            al.Add(new TestStruct(DateTime.Now.AddDays(-1),"Test1",1.11,1));
            al.Add(new TestStruct(DateTime.Now.AddDays(1), "Test9", 2.11, 5));
            al.Add(new TestStruct(DateTime.Now.AddDays(-2), "Test4", 3.11, 4));
            al.Add(new TestStruct(DateTime.Now.AddDays(2), "Test3", 4.11, 7));
            al.Sort(new TestStruct());
            Console.WriteLine();
            Console.WriteLine("struct compare on date:");
            foreach (TestStruct ts in al)
            {
                Console.WriteLine(ts.ToString());
            }
            al.Sort(new AltCompare1());
            Console.WriteLine("Altcompare1:dVal");
            foreach (TestStruct ts in al)
            {
                Console.WriteLine(ts.ToString());
            }
            al.Sort(new AltCompare2());
            Console.WriteLine("Altcompare2:string");
            foreach (TestStruct ts in al)
            {
                Console.WriteLine(ts.ToString());
            }
            al.Sort(new AltCompare3());
            Console.WriteLine("Altcompare3:iVal");
            foreach (TestStruct ts in al)
            {
                Console.WriteLine(ts.ToString());
            }

            Console.ReadLine();
        }
        static void SequenctialLists()
        {
            Console.WriteLine();
            Console.WriteLine("SequenctialLists");
            Console.WriteLine();

            //FIFO=Queue
            Console.WriteLine("Queue");

            Queue q = new Queue();
            q.Enqueue("String1");
            q.Enqueue("String2");
            Console.WriteLine(q.Dequeue());
            Console.WriteLine(q.Dequeue());

            q.Enqueue("First");
            q.Enqueue("Second");
            q.Enqueue("Thrid");
            q.Enqueue("GROK!");
            while (q.Count > 0)
            {
                Console.WriteLine("Peek:"+q.Peek());
                Console.WriteLine("Remove" + q.Dequeue());
            }
            Console.WriteLine("Stack");
            //LIFO = Stack
            Stack s = new Stack();
            s.Push("First");
            s.Push("Second");
            s.Push("Thrid");
            s.Push("SPAM!");
            while (s.Count > 0)
            {
                Console.WriteLine("Peek:" + s.Peek());
                Console.WriteLine("Remove:" + s.Pop()); 
            }
            Console.ReadLine();
        }
        static void DictionaryTest()
        {
            Console.WriteLine();
            Console.WriteLine("DictionaryTest");
            Console.WriteLine();

            Hashtable h = new Hashtable();
            h.Add("spam@spam.com", "Kent , Clark");
            h.Add("me@you.com", "Parker , Peter");
            h["spoon@fork.edu"] = "Wayne, Bruce";
            IDictionaryEnumerator ide = h.GetEnumerator();
            while(ide.MoveNext())
            {
                Console.WriteLine("hashtble key:{0} value:{1}", ide.Key,ide.Value);
            }
            //OR
            foreach (DictionaryEntry de in h)
            {
                Console.WriteLine("FOREACH hashtble key:{0} value:{1}", de.Key, de.Value);
            }
            Console.WriteLine("hashtable has Spam@spam.com " + h.Contains("Spam@spam.com"));
            Console.WriteLine("hashtable has spam@spam.com " + h.Contains("spam@spam.com"));            
            //KEY must be unique, or rather have unique Hash
            Console.WriteLine("bob".GetHashCode());
            Hashtable dup = new Hashtable();
            try
            {
                dup.Add("bob", "testing");
                dup.Add("bob", "big boy!");
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); }
            Console.WriteLine(dup.Count);
            dup.Clear();
            try
            {
                dup.Add(new TestStruct(DateTime.Now, "spam", 5.55, 6), "cool");
                dup.Add(new TestStruct(DateTime.Now.AddDays(-1), "spam1", 77.77, 6), "very cool");
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); }
            Console.WriteLine(dup.Count);
            dup.Clear();
            try
            {
                dup.Add(new TestStruct2(DateTime.Now, "spam", 5.55, 6), "cool");
                dup.Add(new TestStruct2(DateTime.Now.AddDays(-1), "spam1", 77.77, 6), "very cool");
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); }
            Console.WriteLine(dup.Count);
            h = new Hashtable(new InsensitiveComparer());
            h.Clear();
            try
            {
                h.Add("FIRST", 1);
                h.Add("second", 2);
                h.Add("THRID", 3);
                h.Add("fourth", 4);
                h.Add("FOURTH", 5);
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); }
            Console.WriteLine(h.Count);

            SortedList sl = new SortedList();
            sl["first"] = 1;
            sl["Second"] = 2;
            sl["Third"] = 3;
            sl["FOURTH"] = 4;
            sl["fourth"] = 5;
            foreach (DictionaryEntry de in sl)
            {
                Console.WriteLine("{0} = {1}", de.Key, de.Value);
            }
            sl = new SortedList(new DescendingComparer());
            sl["first"] = 1;
            sl["Second"] = 2;
            sl["Third"] = 3;
            sl["FOURTH"] = 4;
            sl["fourth"] = 5;
            foreach (DictionaryEntry de in sl)
            {
                Console.WriteLine("{0} = {1}", de.Key, de.Value);
            }

            Console.ReadLine();

            //ListDictionary a hashtable for 10 or fewer elements
            //HybridDictionary: It's a listdictionary for 10 or fewer elements, 11 or more it becomes a hash table
            //OrderedDictionary: a sorted hashtable
        }
        static void SpecilizedTest()
        {
            Console.WriteLine();
            Console.WriteLine("SpecilizedTest");
            Console.WriteLine();

            BitArray ba = new BitArray(4);
            ba[0] = true;
            ba[1] = false;
            ba[2] = false;
            ba[3] = true;
            BitArray b2 = new BitArray(4);
            b2[0] = true;
            b2[1] = true;
            b2[2] = false;
            b2[3] = false;

            BitArray b3 = ba.Xor(b2);
            //can also do .And .Or and .Not
            foreach (bool bo in b3)
            {
                Console.WriteLine(bo);
            }

            BitVector32 bv = new BitVector32(0);

            int FirstBit = BitVector32.CreateMask();
            int SecondBit = BitVector32.CreateMask(FirstBit);
            int ThirdBit = BitVector32.CreateMask(SecondBit);
            int ForthBit = BitVector32.CreateMask(ThirdBit);
            bv[FirstBit] = true;
            bv[SecondBit] = true;
            Console.WriteLine("{0} should be 3", bv.Data);
            Console.WriteLine(bv);
            BitVector32 bv2 = new BitVector32(4);
            bool bit1 = bv2[FirstBit];
            bool bit2 = bv2[SecondBit];
            bool bit3 = bv2[ThirdBit];
            Console.WriteLine("{0} {1} {2}", bit1, bit2, bit3);
            BitVector32.Section sec1 = BitVector32.CreateSection(15000);
            BitVector32.Section sec2 = BitVector32.CreateSection(15000, sec1);
            BitVector32.Section sec3 = BitVector32.CreateSection(6, sec2);
            BitVector32.Section sec4 = BitVector32.CreateSection(100, sec3);
            BitVector32 packed = new BitVector32(0);
            packed[sec1] = 18000;
            packed[sec2] = 33;
            packed[sec3] = 433;
            Console.WriteLine(packed);
            try
            {
                packed[sec4] = 52255256;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(packed);

            StringCollection sc = new StringCollection();
            //just like an arraylist, only for strings
            StringDictionary sd = new StringDictionary();
            //just like a hashtable,but key & value must be strings.
            Console.ReadLine();
        }
        static void GenericTest()
        {
            Console.WriteLine();
            Console.WriteLine("GenericsTest");
            Console.WriteLine();
            List<int> intlist = new List<int>();
            intlist.Add(2);
            intlist.Add(4);
            //intlist.Add("5");  //THIS WONT COMPILE!
            Queue<Type> qt = new Queue<Type>();
            qt.Enqueue(Type.GetType("System.Int32"));
            qt.Enqueue(Type.GetType("System.Threading.Thread"));
            Console.WriteLine(qt.Dequeue());
            Console.WriteLine(qt.Dequeue());
            Stack<double> sd = new Stack<double>();
            sd.Push(2.22);
            sd.Push(5.55);
            Console.WriteLine(sd.Pop());
            Console.WriteLine(sd.Pop());

            SortedDictionary<int, string> mysort = new SortedDictionary<int, string>();
            mysort.Add(4, "spam");
            mysort.Add(2, "spam1");
            mysort.Add(55, "f");
            mysort.Add(-5, "test");
            mysort[22] = "implicit add";
            foreach (KeyValuePair<int,string> kvp in mysort)
            {
                Console.WriteLine("{0} {1}", kvp.Key, kvp.Value);
            }



            LinkedList<string> lls = new LinkedList<string>();
            lls.AddFirst("First");
            lls.AddAfter(lls.First, "Second");
            lls.AddLast("Thrid");
            Console.WriteLine(lls.First.Next.Next.Value);
            List<TestStruct> t = new List<TestStruct>();
            t.Add(new TestStruct(DateTime.Now.AddDays(2),"spam",22.2,31));
            t.Add(new TestStruct(DateTime.Now,"Holy Hand grenade", 1.4433,587));
            t.Sort(new TestStruct());
            foreach (TestStruct ts in t)
            {
                Console.WriteLine(ts.ToString());
            }
            Console.ReadLine();
        }
    }
    #region Other stuff
    public class DescendingComparer : IComparer 
    {

        #region IComparer Members

        public int Compare(object x, object y)
        {
            string sx, sy;
            sx = (string)x;
            sy = (string)y;
            return -1 * string.Compare(sx, sy);
        }

        #endregion
    }

    public struct TestStruct : IComparer,IComparer<TestStruct>
    {
        public DateTime date;
        public string name;
        public double dVal;
        public int iVal;

        public TestStruct(DateTime d, string s, double n, int i)
        {
            date = d;
            name = s;
            dVal = n;
            iVal = i;
        }

        #region IComparer Members

        int IComparer.Compare(object x, object y)
        {
            TestStruct tx, ty;
            tx = (TestStruct)x;
            ty = (TestStruct)y;
            return DateTime.Compare(tx.date, ty.date);
        }

        #endregion
        public override string ToString()
        {
            object[] o = { this.date, this.iVal, this.dVal, this.name };
            return string.Format("date:{0} iVal:{1} dVal:{2} name:{3}", o);
        }
        public override int GetHashCode()
        {
            return iVal.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            TestStruct t = (TestStruct)obj;
            return this.iVal == t.iVal;
        }

        #region IComparer<TestStruct> Members

        public int Compare(TestStruct x, TestStruct y)
        {
            return -1*DateTime.Compare(x.date, y.date);
        }

        #endregion
    }

    public struct TestStruct2 : IComparer
    {
        public DateTime date;
        public string name;
        public double dVal;
        public int iVal;

        public TestStruct2(DateTime d, string s, double n, int i)
        {
            date = d;
            name = s;
            dVal = n;
            iVal = i;
        }

        #region IComparer Members

        int IComparer.Compare(object x, object y)
        {
            TestStruct tx, ty;
            tx = (TestStruct)x;
            ty = (TestStruct)y;
            return DateTime.Compare(tx.date, ty.date);
        }

        #endregion
        public override string ToString()
        {
            object[] o = { this.date, this.iVal, this.dVal, this.name };
            return string.Format("date:{0} iVal:{1} dVal:{2} name:{3}", o);
        }
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            TestStruct2 t = (TestStruct2)obj;
            return t.name == this.name;
        }
    }


    public class AltCompare1 : IComparer
    {

        #region IComparer Members

        public int Compare(object x, object y)
        {
            TestStruct tx, ty;
            tx = (TestStruct)x;
            ty = (TestStruct)y;
            if (tx.dVal > ty.dVal)
                return 1;
            else if (tx.dVal < ty.dVal)
                return -1;
            else
                return 0;

        }

        #endregion
    }

    public class AltCompare2 : IComparer
    {

        #region IComparer Members

        public int Compare(object x, object y)
        {
            TestStruct tx, ty;
            tx = (TestStruct)x;
            ty = (TestStruct)y;
            return string.Compare(tx.name, ty.name);
        }

        #endregion
    }

    public class AltCompare3 : IComparer
    {

        #region IComparer Members

        public int Compare(object x, object y)
        {
            TestStruct tx, ty;
            tx = (TestStruct)x;
            ty = (TestStruct)y;
            if (tx.iVal > ty.iVal)
                return 1;
            else if (tx.iVal < ty.iVal)
                return -1;
            else
                return 0;
        }

        #endregion
    }

    public class InsensitiveComparer : IEqualityComparer
    {

        CaseInsensitiveComparer _comp = new CaseInsensitiveComparer();

        #region IEqualityComparer Members


        public int GetHashCode(object obj)
        {
            return obj.ToString().ToLowerInvariant().GetHashCode();
        }

        public new bool Equals(object x, object y)
        {
            return _comp.Compare(x, y) == 0;
        }

        #endregion

        #region IEqualityComparer Members

        bool IEqualityComparer.Equals(object x, object y)
        {
            //throw new Exception("The method or operation is not implemented.");
            return this.Equals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return this.GetHashCode(obj);
        }

        #endregion
    }
    #endregion
}