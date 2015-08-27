using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    static public class StringOps
    {
        static public string Join(char[] a, char b)
        {
            var lengthA = a.Length;
            var result = new char[lengthA + 1];
            Array.Copy(a, result, lengthA);
            result[lengthA] = b;
            return new string(result);
        }

        static public string Join(char[] a, char[] b)
        {
            var lengthA = a.Length;
            var lengthB = b.Length;
            var result = new char[lengthA + lengthB];
            Array.Copy(a, result, lengthA);
            Array.Copy(b, 0, result, lengthA, lengthB);
            return new string(result);
        }

        static public string Join(char[] a, string b)
        {
            var lengthA = a.Length;
            var lengthB = b.Length;
            var result = new char[lengthA + lengthB];
            Array.Copy(a, result, lengthA);
            b.CopyTo(0, result, lengthA, lengthB);
            return new string(result);
        }

        static public string Join(string a, char b)
        {
            var lengthA = a.Length;
            var result = new char[lengthA + 1];
            a.CopyTo(0, result, 0, lengthA);
            result[lengthA] = b;
            return new string(result);
        }

        static public string Join(string a, char[] b)
        {
            var lengthA = a.Length;
            var lengthB = b.Length;
            var result = new char[lengthA + lengthB];
            a.CopyTo(0, result, 0, lengthA);
            Array.Copy(b, 0, result, lengthA, lengthB);
            return new string(result);
        }

        static public string Join(string a, string b)
        {
            var lengthA = a.Length;
            var lengthB = b.Length;
            var result = new char[lengthA + lengthB];
            a.CopyTo(0, result, 0, lengthA);
            b.CopyTo(0, result, lengthA, lengthB);
            return new string(result);
        }
    }
}
