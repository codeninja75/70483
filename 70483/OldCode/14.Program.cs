using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Chap14
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Reflection!
            Assembly theAssembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("Entry point:{0}", theAssembly.EntryPoint.Name);
            Console.WriteLine("FullName:{0}", theAssembly.FullName);
            Console.WriteLine("In GAC?:{0}", theAssembly.GlobalAssemblyCache);
            Console.WriteLine("Location:{0}", theAssembly.Location);
            Console.WriteLine("Reflection Only?:{0}", theAssembly.ReflectionOnly);
            Console.ReadLine();
            string fullName = "System.Transactions, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assembly nextA = Assembly.ReflectionOnlyLoad(fullName);
            //Console.WriteLine("Entry point:{0}", nextA.EntryPoint.Name);
            Console.WriteLine("FullName:{0}", nextA.FullName);
            Console.WriteLine("In GAC?:{0}", nextA.GlobalAssemblyCache);
            Console.WriteLine("Location:{0}", nextA.Location);
            Console.WriteLine("Reflection Only?:{0}", nextA.ReflectionOnly);
            Module[] mods = nextA.GetModules(); 
            foreach (Module m in mods)
            {
                Console.WriteLine("Module Name:{0}", m.Name);
            }
            nextA = Assembly.Load(fullName);
            object[] attribs = nextA.GetCustomAttributes(false);
            foreach (Attribute a in attribs)
            {
                Console.WriteLine("Attribute:{0}", a.GetType().Name);
            }
            Type aT = typeof(AssemblyDescriptionAttribute);
            attribs = nextA.GetCustomAttributes(aT, false);
            if (attribs.Length > 0)
            {
                AssemblyDescriptionAttribute ada = (AssemblyDescriptionAttribute)attribs[0];
                Console.WriteLine("Found Description!");
                Console.WriteLine("Desc:{0}", ada.Description);
            }
            Type[] t = nextA.GetTypes();
            bool Done1 =false, Done2=false, Done3=false, Done4=false, Done5=false, Done6 = false , Done7 = false;
            foreach (Type t1 in t)
            {
                if (!Done7)
                {
                    Console.WriteLine("Found Type:{0}", t1.Name);
                    Console.WriteLine("Full Name:{0}", t1.FullName);
                    Console.WriteLine("Has Element Type:{0}", t1.HasElementType);
                    Console.WriteLine("Is Abstract?:{0}", t1.IsAbstract);
                    Console.WriteLine("Is Class?:{0}", t1.IsClass);
                    if (t1.IsClass)
                    {
                        if (!Done1)
                        {
                            attribs = t1.GetCustomAttributes(false);
                            foreach (Attribute a in attribs)
                            {
                                Console.WriteLine(" Attrib:{0}", a.GetType().Name);
                                Done1 = true;
                            }
                        }
                        //Now we get all the fields of the class!

                        if (!Done2)
                        {
                            FieldInfo[] f = t1.GetFields();
                            foreach (FieldInfo f1 in f)
                            {
                                Console.WriteLine("Field:{0}", f1.Name);
                                Console.WriteLine("type of Field:{0}", f1.FieldType.Name);
                                Done2 = true;
                            }
                        }

                        if (!Done3)
                        {
                            MemberInfo[] mem = t1.GetMembers();
                            foreach (MemberInfo m1 in mem)
                            {
                                Console.WriteLine("Member Name:{0} of type {1}", m1.Name, m1.GetType().Name);
                                Done3 = true;
                            }
                        }
                        if (!Done4 && !Done5 && !Done6)
                        {
                            MethodInfo[] m = t1.GetMethods();
                            foreach (MethodInfo m1 in m)
                            {
                                if (!Done5)
                                {
                                    Console.WriteLine("Method Name:{0}", m1.Name);
                                    Console.WriteLine("Return Type:{0}", m1.ReturnType.Name);
                                    ParameterInfo[] p = m1.GetParameters();
                                    foreach (ParameterInfo p1 in p)
                                    {
                                        Console.WriteLine("Param:{0} of type {1}", p1.Name, p1.GetType().Name);
                                        Done5 = true;
                                    }
                                }
                                if (!Done6)
                                {
                                    MethodBody mb = m1.GetMethodBody();
                                    if (mb != null)
                                    {
                                        Done6 = true;
                                        Console.WriteLine(" Max Stack:{0}", mb.MaxStackSize);
                                        foreach (LocalVariableInfo local in mb.LocalVariables)
                                        {
                                            Console.WriteLine("Local Var:{0} of type{1}", local.LocalIndex, local.LocalType);

                                        }
                                        foreach (Byte b0 in mb.GetILAsByteArray())
                                        {
                                            Console.WriteLine("{0:x2}", b0);
                                        }
                                    }
                                }
                            }
                            Done4 = Done5 & Done6;
                        }
                    }
                    Console.WriteLine("Is Interface?:{0}", t1.IsInterface);
                    Console.WriteLine("Is Primitive?:{0}", t1.IsPrimitive);
                    Console.WriteLine("Is Public?:{0}", t1.IsPublic);
                    Console.WriteLine("Is Value Type?:{0}", t1.IsValueType);
                    BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    Type ts = typeof(String);
                    
                    MemberInfo[] mx = t1.GetMembers(bf);
                    foreach (MemberInfo mz in mx)
                    {
                        Console.WriteLine("Member:{0}", mz.Name);
                    }
                    Done7 = Done1 &  Done3 & Done4 & Done5 & Done6;
                }
            }
            Console.ReadLine();
            #endregion

            #region Dynamic Code!
            string path = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll";
            Assembly a0 = Assembly.LoadFile(path);
            Type hashType = a0.GetType("System.Collections.Hashtable");
            Type[] argType = Type.EmptyTypes;
            ConstructorInfo ctor = hashType.GetConstructor(argType);
            object newhash = ctor.Invoke(new object[] { });
            MethodInfo meth = hashType.GetMethod("Add");
            meth.Invoke(newhash, new object[] { "Hi", "Hello" });
            PropertyInfo prop = hashType.GetProperty("Count");
            int count = (int)prop.GetValue(newhash, null);
            Type consoleType = typeof(Console);
            MethodInfo wl = consoleType.GetMethod("WriteLine", new Type[] { typeof(string) });
            wl.Invoke(null,new object[]{count.ToString()});
            Console.ReadLine();

            #endregion

            #region Reflection Emit!
            AssemblyName tempName = new AssemblyName();
            tempName.Name = "MyTempAssembly";
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(tempName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder modBldr = ab.DefineDynamicModule("MainMod", tempName.Name + ".dll");
            TypeBuilder tBldr = modBldr.DefineType("MyNewType", TypeAttributes.Public | TypeAttributes.Class,
                typeof(System.Collections.Hashtable), new Type[] { typeof(IDisposable) }
                );
            ConstructorBuilder ctorBld = tBldr.DefineDefaultConstructor(MethodAttributes.Public);
            MethodBuilder methBldr = tBldr.DefineMethod("Add", MethodAttributes.Public, null, new Type[] { typeof(string) });
            ILGenerator codeGen = methBldr.GetILGenerator();
            codeGen.Emit(OpCodes.Ret);
            FieldBuilder fieldBldr = tBldr.DefineField("_count", typeof(int), FieldAttributes.Private);
            PropertyBuilder propBldr = tBldr.DefineProperty("Count", PropertyAttributes.None, typeof(int), Type.EmptyTypes);
            MethodAttributes methAtt = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            MethodBuilder propGetBldr = tBldr.DefineMethod("get_Count", methAtt, typeof(int), Type.EmptyTypes);
            propBldr.SetGetMethod(propGetBldr);
            //The assembly won't save as I haven't defined IL for all methods, etc...
            //ab.Save(tempName.Name + ".dll");
            Console.ReadLine();
            #endregion 
        }
    }
}
