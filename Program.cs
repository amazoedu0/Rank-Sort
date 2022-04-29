using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;



namespace Rank_Sort

{
    internal class Program
    {
        static void Main()
        {
            int[] sizes = { 100, 500, 1000, 5000, 10000, 50000, 100000 };

            Print(sizes);

            Console.ReadKey();

        }


        public static void Print(int[] sizez)
        {
            Console.Write(new string('-', 55) + '\n');
            Console.WriteLine("N \tSingle T \tMulti T \tThreadPool");

            for (int i = 0; i < sizez.Length; i++)
            {
                double singleT, multiT, tPool;

                Stopwatch stopwatch = new Stopwatch();

                int[] A, B, v, w, x;

                A = Generator(sizez[i]);

                B = v = w = x = new int[A.Length];


                /*
                 * 
                 * ***********************************
                 *                                   *
                 *         Usig Serial Method        *
                 *                                   *
                 * *********************************** 
                 * 
                 */


                Console.Write(new string('-', 55) + '\n');


                B = A; stopwatch.Reset();

                stopwatch.Start(); B = SerialRankSort(A); stopwatch.Stop();
                singleT = stopwatch.Elapsed.TotalMilliseconds;


                /*
                 * 
                 * ***********************************
                 *                                   *
                 *      Usig Multi Thread Method     *
                 *                                   *
                 * *********************************** 
                 * 
                 */

                stopwatch.Reset(); B = A; stopwatch.Start();

                Thread t1 = new Thread(() => { v = Multi_Thread_Rank_Sort(A, 0); });
                Thread t2 = new Thread(() => { w = Multi_Thread_Rank_Sort(A, 1); });


                t1.Start();
                t2.Start();

                t1.Join();
                t2.Join();

                t1.Abort();
                t2.Abort();

                B = MergeMT(v, w);

                stopwatch.Stop();

                multiT = stopwatch.Elapsed.TotalMilliseconds;



                /*
                 * 
                 * ***********************************
                 *                                   *
                 *      Usig Thread Pool Method      *
                 *                                   *
                 * *********************************** 
                 * 
                 */

                stopwatch.Start();
                ThreadPool.QueueUserWorkItem(Thread_Pool, A);
                stopwatch.Stop();

                tPool = stopwatch.Elapsed.TotalMilliseconds;

                Console.WriteLine($"{A.Length}\t{singleT} ms\t{multiT} ms\t{tPool} ms");

            }
            Console.Write(new string('-', 55) + '\n');

        }

        /*
         * 
         * ***********************************
         *                                   *
         *          Thread Pool Logic        *
         *                                   *
         * *********************************** 
         * 
         */
        public static void Thread_Pool(object obj)
        {
            int[] A = obj as int[];
            int[] B = SerialRankSort(A);
        }


        /*
         * 
         * ***********************************
         *                                   *
         *       Random Array Generator      *
         *                                   *
         * *********************************** 
         * 
         */
        public static int[] Generator(int size)
        {
            int[] A = new int[size];

            Random random = new Random();

            for (int i = 0; i < A.Length; i++) A[i] = random.Next(1, 10000);

            return A;
        }

        /*
         * 
         * ***********************************
         *                                   *
         *         Serial Method Logic       *
         *                                   *
         * *********************************** 
         * 
         */

        public static int[] SerialRankSort(int[] A)
        {
            int[] temp;
            temp = new int[A.Length];

            for (int i = 0; i < A.Length; i++)
            {
                int position = 0;

                for (int j = 0; j < A.Length; j++)
                {
                    if (A[i] > A[j])
                    {
                        position++;
                    }
                }

                temp[position] = A[i];

            }
            return temp;
        }

        /*
         * 
         * **************************************
         *                                      *
         *         Multi Thread Logic           *
         *    This part is composed of two      *
         *   functions. One is responsible      *
         *  of sorting parallely and the other  *
         *    part merges the sorted Array.     *
         *                                      *
         * ************************************** 
         * 
         */

        //  Part 1 of Multithread
        public static int[] Multi_Thread_Rank_Sort(int[] A, int index)
        {
            int[] temp;
            temp = new int[A.Length];


            for (int i = index; i < A.Length; i += 2)
            {
                int position = 0;

                for (int j = 0; j < A.Length; j++)
                {
                    if (A[i] > A[j])
                    {
                        position++;
                    }
                }

                temp[position] = A[i];

            }

            return temp;
        }

        //  Part 2 of Multithread
        public static int[] MergeMT(int[] A, int[] B)
        {
            int[] temp;
            temp = new int[B.Length];
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == 0)
                    temp[i] = B[i];
                else if (B[i] == 0)
                    temp[i] = A[i];
            }

            return temp;
        }

    }
}
