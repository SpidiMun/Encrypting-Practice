using System;
using System.Numerics;

namespace AsymmetricEncryption
{
    /// <summary>
    /// батя дал говна
    /// </summary>
    internal class AssymetricEncryption
    {
        private static readonly Random randomizer = new Random();

        // Для ключа
        // public key - y, g, p
        // private key - x

        public BigInteger P { get; set; } // Ввод (простое)
        public BigInteger G { get; set; } // На основе p > g (простое)
        public BigInteger X { get; set; } // На основе p > x (простое)
        public BigInteger Y { get; set; } // y = g^x mod p

        // Для подписи

        public BigInteger K { get; set; } // На основе p - 1 (простое)
        public BigInteger A { get; set; } // a = g^k mod p
        public string M { get; set; } // length(M) = (xa + kb) mod (p - 1) (Найти b)
        public BigInteger B { get; set; } // На основе M

        // подпись - a и b
        // private - k

        // Верификация: y^a * a^b mod p = g^length(M) mod p
        // 6k + 1, 6k + 5 (Исключения - 2 и 3)
        
        public BigInteger[] ModN { get; set; }
        public BigInteger N { get; set; }
        public BigInteger[] exceptions = { 2, 3 };

        public AssymetricEncryption()
        {
            N = 6; // Основание модуля
            ModN = new BigInteger[] { 1, 5 }; // Классы вычета по модулю 6
        }

        private bool IsPrime(BigInteger number)
        {
            if ((number & 1) == 0)
            {
                return number == 2;
            }
            for (int i = 3; (i * i) <= number; i += 2)
            {
                if ((number % i) == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        private BigInteger GenerateSimpleNumber(BigInteger MaxNumber)
        {
            BigInteger number = 4;
            while (!IsPrime(number) || number >= MaxNumber)
            {
                BigInteger temp1 = N * Convert.ToUInt64(randomizer.Next(100000000, 1000000000)),
                    temp2 = ModN[randomizer.Next(0, ModN.Length)];
                number = temp1 + temp2;
            }
            return number;
        }

        public void GenerateKeys()
        {
            // public key

            P = GenerateSimpleNumber(1000000000000000ul);
            G = GenerateSimpleNumber(P);
            X = GenerateSimpleNumber(P);

            // private key

            Y = BigInteger.Pow(G, (int)X) % P;
        }

        public void GenerateSignature()
        {
            // private signature

            K = GenerateSimpleNumber(P - 1);

            // public signature

            A = BigInteger.Pow(G, (int)K) % P;
            BigInteger num = BigInteger.Remainder(M.Length - X * A, P - 1);
            if (num < BigInteger.Zero)
            {
                num += P - 1;
            }
            for (B = BigInteger.Zero; B < P; B++)
            {
                if (BigInteger.Remainder(K * B, P - 1) == num)
                {
                    break;
                }
            }
        }

        public bool VerificateSignatureSucsess => BigInteger.Pow(Y, (int)A) * BigInteger.Pow(A, (int)B) % P == BigInteger.Pow(G, M.Length) % P;
    }
}
