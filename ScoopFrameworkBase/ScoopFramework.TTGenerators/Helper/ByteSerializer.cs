using System;
using System.Collections.Generic;

namespace ScoopFramework.TTGenerators.Helper
{
    public class ByteSerializer
    {
        public static byte[] Serialize(object obj)
        {
            var t = obj.GetType();
            if (t.IsValueType)
                return GetBytes2(obj);

            List<byte[]> byteList = new List<byte[]>();
            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                var data = GetBytes2(prop.GetValue(obj));
                byteList.Add(data);
            }
            int size = 0;
            for (int i = 0; i < byteList.Count; i++)
                size += byteList[i].Length;
            
            var result = new byte[size];
            for (int i = 0, o = 0; i < byteList.Count; o += byteList[i].Length, i++)
                Array.Copy(byteList[i], 0, result, o, byteList[i].Length);

            return result;
        }
        public static T Merge<T>(byte[] data)
        {
            return default(T);
        }

        private static byte[] GetBytes2(object obj)
        {
            Type t = obj.GetType();
            
            if (!t.IsArray)
            {

                if (t == typeof(Int16))
                    return BitConverter.GetBytes((short)obj);
                else if (t == typeof(Int32))
                    return BitConverter.GetBytes((int)obj);
                else if (t == typeof(Int64))
                    return BitConverter.GetBytes((long)obj);
                else if (t == typeof(UInt16))
                    return BitConverter.GetBytes((ushort)obj);
                else if (t == typeof(UInt32))
                    return BitConverter.GetBytes((uint)obj);
                else if (t == typeof(Int64))
                    return BitConverter.GetBytes((ulong)obj);
                else if (t == typeof(Boolean))
                    return BitConverter.GetBytes((bool)obj);
                else if (t == typeof(Char))
                    return BitConverter.GetBytes((char)obj);
                else if (t == typeof(Double))
                    return BitConverter.GetBytes((double)obj);
                else if (t == typeof(float))
                    return BitConverter.GetBytes((float)obj);
                else if (t == typeof(DateTime))
                    return BitConverter.GetBytes(((DateTime)obj).Ticks);
                else if (t.IsClass)
                    return Serialize(obj);
            }
            else
            {
                var tt = t.UnderlyingSystemType.GetElementType();
                tt = Nullable.GetUnderlyingType(tt) ?? tt;
                var isNullable = Nullable.GetUnderlyingType(tt) != null;
                if (tt == typeof(Int16))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 2];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((short)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Int32))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 4];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((int)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Int64))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((long)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                            data[offset++] = d[4];
                            data[offset++] = d[5];
                            data[offset++] = d[6];
                            data[offset++] = d[7];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(UInt16))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 2];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((ushort)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(UInt32))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 4];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((uint)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Int64))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((ulong)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                            data[offset++] = d[4];
                            data[offset++] = d[5];
                            data[offset++] = d[6];
                            data[offset++] = d[7];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Boolean))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((bool)v);
                            data[offset++] = d[0];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Char))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((char)v);
                            data[offset++] = d[0];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(Double))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((double)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                            data[offset++] = d[4];
                            data[offset++] = d[5];
                            data[offset++] = d[6];
                            data[offset++] = d[7];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(float))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes((float)v);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                            data[offset++] = d[4];
                            data[offset++] = d[5];
                            data[offset++] = d[6];
                            data[offset++] = d[7];
                        }
                    }
                    return data;
                }
                else if (tt == typeof(DateTime))
                {
                    int offset = 0;
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes(((DateTime)v).Ticks);
                            data[offset++] = d[0];
                            data[offset++] = d[1];
                            data[offset++] = d[2];
                            data[offset++] = d[3];
                            data[offset++] = d[4];
                            data[offset++] = d[5];
                            data[offset++] = d[6];
                            data[offset++] = d[7];
                        }
                    }
                    return data;
                }
                else if (tt.IsClass)
                {
                    Array a = (Array)obj;
                    byte[] data = new byte[a.Length * 8];
                    for (int i = 0; i < a.Length; i++)
                    {
                        var v = a.GetValue(i);
                        if (v != null)
                        {
                            var d = BitConverter.GetBytes(((DateTime)v).Ticks);

                        }
                    }
                    return data;
                }
            }
            return null;
        }
    }
}
