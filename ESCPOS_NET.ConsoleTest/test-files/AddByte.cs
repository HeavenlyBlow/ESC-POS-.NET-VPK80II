using System;
using System.Collections.Generic;

namespace ESCPOS_NET.ConsoleTest.test_files
{
    public static class AddByte
    {
        public static byte[] AddBytes(this byte[] bytes, byte[] addBytes)
        {
            if (addBytes == null)
                return bytes;

            var list = new List<byte>();
            list.AddRange(bytes);
            list.AddRange(addBytes);
            return list.ToArray();
        }
        public static byte ToByte(this Enum c)
        {
            return (byte)Convert.ToInt16(c);
        }
    }
}