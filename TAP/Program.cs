using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TPL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размерность матриц: ");
            int.TryParse(Console.ReadLine(), out var range);
            if (range == 0)
            {
                range = 100;
                Console.WriteLine($"\nРазмерность матрицы не задана, будет присвоенно значение {range}");
            }
            Console.WriteLine("Заполнение массивов...");
            var fillTask = new Task<int[,]>(() => FillArray(range));
            fillTask.Start();
            var secondMatrix = FillArray(range);
            fillTask.Wait();
            var firstMatrix = fillTask.Result;
            Console.WriteLine("Массивы заполнены, выполняется перемножение матриц");
            #region Вывод матриц на консоль
            //WriteArray(firstMatrix);
            //Console.WriteLine();
            //WriteArray(secondMatrix);
            //Console.WriteLine();
            #endregion

            var multiplicationTask = new Task<int[,]>(() => 
                MultiplicationParallel(firstMatrix, secondMatrix));
            multiplicationTask.Start();
            var resultConsMultiplication = Multiplication(firstMatrix, secondMatrix);

            #region Вывод результирующих матриц на консоль
            //multiplicationTask.Wait();
            //var resultParallelMultiplication = multiplicationTask.Result;
            //WriteArray(resultConsMultiplication);
            //Console.WriteLine();
            //WriteArray(resultConsMultiplication);
            #endregion

            Console.ReadLine();
        }

        static int[,] Multiplication(int[,] matrix1, int[,] matrix2)
        {
            Console.WriteLine("Начало последовательного умножения матриц...");
            int[,] r = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            Stopwatch timer = Stopwatch.StartNew();
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(0); k++)
                    {
                        r[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("Последовательное перемножение матриц заняло: " + 
                              timeTaken.ToString(@"m\:ss\.fff"));
            return r;
        }
        static int[,] MultiplicationParallel(int[,] matrix1, int[,] matrix2)
        {
            Console.WriteLine("Начало параллельного умножения матриц...");
            int[,] r = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            Stopwatch timer = Stopwatch.StartNew();
            Parallel.For(0, matrix1.GetLength(0), i =>
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(0); k++)
                    {
                        r[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            });
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("Параллельное перемножение матриц заняло: " +
                              timeTaken.ToString(@"m\:ss\.fff"));
            return r;
        }

        private static int[,] FillArray(int length, int from = 0, int to = 10)
        {
            int[,] result = new int[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    result[i,j] = GetRandom(from, to);
                }
            }
            return result;
        }

        private static int GetRandom(int lower, int higher)
        {
            higher += 1;
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            var value = rnd.Next(lower, higher);
            return value;
        }

        private static void WriteArray(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
