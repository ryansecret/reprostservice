#region

using System;
using System.Text;

#endregion

namespace WorkQualityReport.Utility
{
    public class Crypt
    {
        public static string Decrypt(string text)
        {
            var value = Encoding.UTF8.GetBytes(text);
            var temp = new byte[value.Length - 1];
            byte key = value[value.Length - 1];
            int Index = 0;
            foreach (byte item in value)
            {
                temp[Index] = (byte) (item ^ key);

                Index++;
                if (Index == value.Length - 1)
                {
                    break;
                }
            }

            return Encoding.UTF8.GetString(temp);
        }

        public static string Encryption(string text)
        {
            byte[] Value64 = Encoding.UTF8.GetBytes(text);
            var temp = new byte[Value64.Length + 1];
            var rm = new Random(DateTime.Now.Millisecond);
            var Key = (byte) rm.Next(2, 254);
            int Index = 0;
            foreach (byte item in Value64)
            {
                temp[Index] = (byte) (item ^ Key);
                Index++;
            }

            temp[Index] = Key;

            return Encoding.UTF8.GetString(temp);
        }
    }
}