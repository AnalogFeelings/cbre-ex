using System.IO;
using System.Linq;
using System.Text;

namespace CBRE.Packages
{
    public static class BinaryExtensions
    {
        public static string ReadFixedLengthString(this BinaryReader br, Encoding encoding, int length)
        {
            byte[] bstr = br.ReadBytes(length).TakeWhile(b => b != 0).ToArray();
            return encoding.GetString(bstr);
        }

        public static void WriteFixedLengthString(this BinaryWriter bw, Encoding encoding, int length, string str)
        {
            byte[] arr = new byte[length];
            encoding.GetBytes(str, 0, str.Length, arr, 0);
            bw.Write(arr, 0, length);
        }

        public static string ReadNullTerminatedString(this BinaryReader br)
        {
            string str = "";
            char c;
            while ((c = br.ReadChar()) != 0)
            {
                str += c;
            }
            return str;
        }

        public static void WriteNullTerminatedString(this BinaryWriter bw, string str)
        {
            bw.Write(str.ToCharArray());
            bw.Write((char)0);
        }

        public static byte[] ReadByteArray(this BinaryReader br, int num)
        {
            byte[] arr = new byte[num];
            for (int i = 0; i < num; i++) arr[i] = br.ReadByte();
            return arr;
        }

        public static short[] ReadShortArray(this BinaryReader br, int num)
        {
            short[] arr = new short[num];
            for (int i = 0; i < num; i++) arr[i] = br.ReadInt16();
            return arr;
        }

        public static int[] ReadIntArray(this BinaryReader br, int num)
        {
            int[] arr = new int[num];
            for (int i = 0; i < num; i++) arr[i] = br.ReadInt32();
            return arr;
        }

        public static decimal[] ReadSingleArrayAsDecimal(this BinaryReader br, int num)
        {
            decimal[] arr = new decimal[num];
            for (int i = 0; i < num; i++) arr[i] = br.ReadSingleAsDecimal();
            return arr;
        }

        public static float[] ReadSingleArray(this BinaryReader br, int num)
        {
            float[] arr = new float[num];
            for (int i = 0; i < num; i++) arr[i] = br.ReadSingle();
            return arr;
        }

        public static string ReadCString(this BinaryReader br)
        {
            // GH#87: RMF strings aren't prefixed in the same way .NET's BinaryReader expects
            // Read the byte length and then read that number of characters.
            byte len = br.ReadByte();
            char[] chars = br.ReadChars(len);
            return new string(chars).Trim('\0');
        }

        const int MaxVariableStringLength = 127;

        public static void WriteCString(this BinaryWriter bw, string str)
        {
            // GH#87: RMF strings aren't prefixed in the same way .NET's BinaryReader expects
            // Write the byte length (+1) and then write that number of characters plus the null terminator.
            // Hammer doesn't like RMF strings longer than 128 bytes...
            if (str == null) str = "";
            if (str.Length > MaxVariableStringLength) str = str.Substring(0, MaxVariableStringLength);
            bw.Write((byte)(str.Length + 1));
            bw.Write(str.ToCharArray());
            bw.Write('\0');
        }


        public static decimal ReadSingleAsDecimal(this BinaryReader br)
        {
            return (decimal)br.ReadSingle();
        }

        public static void WriteDecimalAsSingle(this BinaryWriter bw, decimal dec)
        {
            bw.Write((float)dec);
        }
    }
}