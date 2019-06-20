using System;

namespace DotNet.E70483.ProgramFlow
{
    public class ExceptionHandling
    {
        public static void ThrowAllTheExceptions()
        {
            try
            {
                throw new VNBigException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                throw new VNException();
            }
            catch (VNBigException ex1)
            {
                Console.WriteLine(ex1.ToString());
            }
            catch (VNException ex2)
            {
                Console.WriteLine(ex2.ToString());
            }
            finally
            {
                Console.WriteLine("clean stuff up");
            }
        }
    }
    #region Exception classes
    class VNException : System.Exception, IFormattable, IConvertible
    {
        public override string ToString()
        {
            return "VNException! \n" + base.ToString();
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

    class VNBigException : VNException
    {
        public override string ToString()
        {
            return "VNbigexception! \n" + base.ToString();
        }
    }
    #endregion
}
