using System;
using System.Numerics;
using System.Text;

namespace Project_RSA
{
    class Program
    {
        public static BigInteger n = 0, Pie_Of_n = 0, e = 0, d = 0;
        public static bool CheckPrime(BigInteger c)
        {
            for (BigInteger i = 2; i <c; i++)
            {
                if (c % i == 0)
                {
                    Console.WriteLine("INVALID INPUT! PLEASE ENTER A PRIME NUMBER.");
                    return false;
                }

            }
            return true;
        }
        public static void CalculateBasicValues(BigInteger x, BigInteger y)
        {
            n = x * y;
            Console.WriteLine("Value of n = " + n);
            Pie_Of_n = (x - 1) * (y - 1);
            Console.WriteLine("pie of n = " + Pie_Of_n);
            for (BigInteger i = n/4; i < Pie_Of_n; i++)
            {
                if (Pie_Of_n % i != 0)
                {
                    e = i;
                    Console.WriteLine("I have choosed e = " + e);
                    break;
                }
            }
            BigInteger v = 0;
            for (; ; )
            {
                BigInteger temp3 = e * v;
                if (temp3 % Pie_Of_n == 1)
                {
                    d = v;
                    Console.WriteLine("I have choosed d = " + d);

                    break;
                }
                v = v + 1;
            }
        }
        public static char[] map = new char[10] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };
        public static char MappingOfIntegers(int t)
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (t == i)
                {
                    return map[i];
                }
            }
            return 'N';
        }
        public static int ReverseMapping(char c)
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (c == map[i])
                {
                    return i;
                }
            }

            return 24; //bug
        }
        static void Main(string[] args)
        {
            try
            {
                again:
                Console.Write("Please enter First prime number: ");
                string FirstNumber = Console.ReadLine();
                BigInteger p = BigInteger.Parse(FirstNumber);
                if (CheckPrime(p) != true)
                {
                    goto again;
                }
                again1:
                Console.Write("Please enter Second prime number: ");
                string SecondNumber = Console.ReadLine();
                BigInteger q = BigInteger.Parse(SecondNumber);
                if (CheckPrime(q) != true)
                {
                    goto again1;
                }
                CalculateBasicValues(p, q);
                // enter string to be encrypted
                Console.Write("Please enter Statment to be encypted: ");
                string UserString = Console.ReadLine();
                // converting string to ASCII bytes
                byte[] ASCIIbytesInUserString = Encoding.ASCII.GetBytes(UserString);
                // new code for all p and q
                BigInteger[] EncryptedArray = new BigInteger[ASCIIbytesInUserString.Length];
                BigInteger temp;
                for (int i = 0; i < ASCIIbytesInUserString.Length; i++)
                {
                    temp = BigInteger.ModPow(ASCIIbytesInUserString[i], e, n);
                    EncryptedArray[i] = temp;
                }
                string[] NewMappedEncryptedArray = new string[EncryptedArray.Length];

                for (int i = 0; i < EncryptedArray.Length; i++)
                {
                    string ETemp = Convert.ToString(EncryptedArray[i]);
                    char[] MappedChar = new char[ETemp.Length];
                    for (int j = 0; j < ETemp.Length; j++)
                    {
                        string ETemp1 = Convert.ToString(ETemp[j]);
                        int IntTemp1 = Convert.ToInt16(ETemp1);
                        MappedChar[j] = MappingOfIntegers(IntTemp1); 
                    }
                    NewMappedEncryptedArray[i] = new string(MappedChar);               
                }
                Console.WriteLine("\n\n=-=-=-=-=-=-=-=-=-=-=-EnCrYpTeD MeSsAgE=-=-=-=-=-=-=-=-=-=-=-=-=-\n\n");
                String EncryptedString = string.Join(" ", NewMappedEncryptedArray);
                Console.WriteLine(EncryptedString);
                // ***************************************Decryption************************************
                string[] NewDecryptedArray = EncryptedString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] result = new string[NewDecryptedArray.Length];
                for(int i = 0; i < NewDecryptedArray.Length; i++)
                {
                    string DTemp = NewDecryptedArray[i];
                    //Console.WriteLine("DTemp value: " + DTemp);
                    int[] ReverseMappedInt = new int[DTemp.Length];
                    for (int j = 0; j < DTemp.Length; j++)
                    {
                        int DTemp1 = ReverseMapping(DTemp[j]);
                        //Console.WriteLine("reverse mapped value: " + DTemp1);
                        ReverseMappedInt[j] = DTemp1;
                    }
                    result[i] = string.Join("", ReverseMappedInt); // joing integer array
                }
                BigInteger[] DecipheredData = new BigInteger[NewMappedEncryptedArray.Length];
                for(int i = 0; i < NewMappedEncryptedArray.Length; i++)
                {
                    DecipheredData[i] = BigInteger.Parse(result[i]);
                }
                byte[] DecryptedBytes = new byte[ASCIIbytesInUserString.Length];
                BigInteger temp1;
                for (int i = 0; i < ASCIIbytesInUserString.Length; i++)
                {
                    temp1 = BigInteger.ModPow(DecipheredData[i], d, n);
                    byte B = (byte)temp1;
                    DecryptedBytes[i] = B;
                }
                // displaying Decipered text.
                Console.WriteLine("\n\n<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Deciphered Text>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n\n");
                string DecryptedString = System.Text.ASCIIEncoding.Default.GetString(DecryptedBytes);
                Console.WriteLine(DecryptedString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
