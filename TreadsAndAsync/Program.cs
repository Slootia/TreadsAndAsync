using System;
using System.Numerics;
using System.Threading;

namespace TreadsAndAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите число n: ");
            int n = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Thread factThread = new Thread(ComputeFact);
            factThread.Start(n);
            Thread sumThread = new Thread(ComputeSum);
            sumThread.Start(n);
            Console.ReadLine();
        }

        private static void ComputeFact(object n)
        {
            BigInteger result = 1;

            for (int i = 1; i <= (int)n; i++)
            {
                result *= i;
            }

            Console.WriteLine($"Факториал числа {n} - {result}");
        }

        private static void ComputeSum(object n)
        {
            BigInteger result = 1;

            for (int i = 1; i <= (int)n; i++)
            {
                result += i;
            }

            Console.WriteLine($"Сумма числа {n} - {result}");
        }
    }
}
