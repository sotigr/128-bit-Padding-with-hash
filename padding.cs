using System;
using System.Collections.Generic; 
using System.Security.Cryptography;
using System.Text; 

namespace padding
{
    class Program
    {

        static void Main(string[] args)
        {
            byte[] message = Encoding.ASCII.GetBytes("this is a message");
            PrintArray(message);
            string randhash = sha256("safsdggadsad");

            byte[] padded = Pad(message, 64, randhash);
            Console.WriteLine("\nPadded message:\n");
            PrintArray(padded);
            Console.WriteLine("\nUnpadded message:\n");
            PrintArray(UnPad(padded, randhash));
            Console.ReadKey();

            //
            //THE HASH RESAULT FROM THE PADDING MUSH BE ENCODED TO BASE64 WITH RSA.
            //
        }
        static byte[] Pad(byte[] message, int length, string hash128)
        {
            int mLen = message.Length;
            if (mLen > length || length > 64)
            {
                return null;
            }

            byte[] hash = Encoding.ASCII.GetBytes(hash128);

            int[] mask = CreateMask(mLen, length);
            Console.WriteLine("\nPadding mask:\n");
            PrintArray(mask);
            byte[] padded = new byte[length];
            int msgcn = 0;
            for (int i = 0; i < length; i++)
            {
                if (mask[i] == 0)
                {
                    padded[i] = message[msgcn];
                    msgcn++;
                }
                else
                    padded[i] = hash[i];
            }

            return padded;
        }
        static byte[] UnPad(byte[] message, string hash128)
        {
            int mLen = message.Length;
            byte[] hash = Encoding.ASCII.GetBytes(hash128);
            List<byte> unpdaded = new List<byte>();

            for (int i = 0; i < mLen; i++)
            {
                if (message[i] != hash[i])
                    unpdaded.Add(message[i]);
            }
            return unpdaded.ToArray();
        }
        static int[] CreateMask(int text_length, int masklength)
        {
            Random random = new Random();
            List<int> existing = new List<int>();
            int[] arr = new int[masklength];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = -1;
            }
            int cn = 0;
            while (cn < text_length)
            {
                int rand = random.Next(0, masklength);
                if (!existing.Contains(rand))
                {
                    existing.Add(rand);
                    arr[rand] = 0;
                    cn += 1;
                }
            }
            return arr;
        }
        static byte[] RandomBytes(int length)
        {
            Random random = new Random();
            byte[] array = new byte[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = (byte)random.Next(1, 255);
            }
            return array;
        }

        static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password), 0, Encoding.ASCII.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
        static string sha256(byte[] password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(password, 0, password.Length);
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
        static void AppendToArray(ref byte[] old_array, ref byte[] new_array, byte bytetoappend, int length)
        {
            for (int i = 0; i < old_array.Length; i++)
            {
                new_array[i] = old_array[i];
            }
            for (int i = old_array.Length; i < old_array.Length + length; i++)
            {
                new_array[i] = 0x00;
            }
        }

        static void PrintArray<t>(t[] array)
        {
            foreach (t item in array)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine("");
        }
    }
}
