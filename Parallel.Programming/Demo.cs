using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel.Programming
{
    internal class Demo
    {
        public static void WriteChar(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }
        public static void Write(object o)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
            }
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} process object {o}...");
            return o.ToString().Length;
        }

        public static void TaskWait()
        {
            var cts = new CancellationTokenSource();
            var t = new Task(() => {
                Console.WriteLine("Press any key to disarm; you have 5 seconds");
                bool cancelled = cts.Token.WaitHandle.WaitOne(5000);
                Console.WriteLine(cancelled ? "Bomb disarmed":"BOOM!!!");
            },cts.Token);
            t.Start();

            Console.ReadKey();
            cts.Cancel();
        }
        public static void TaskCancellation()
        {
            #region Ex1
            //var cts = new CancellationTokenSource();
            //var token = cts.Token;

            //token.Register(() =>
            //{
            //    Console.WriteLine("Cancellation has been requested...");
            //});

            //var t = new Task(() => {
            //    int i = 0;
            //    while (true)
            //    {
            //        //use any of one them from 3 below ways to cancel the task
            //        token.ThrowIfCancellationRequested();//1st way of cancel
            //        //if (cts.IsCancellationRequested)
            //        //{
            //        //    //break;//2nd way to Cancel
            //        //    throw new OperationCanceledException();//3rd way of cancel
            //        //}
            //        //else
            //            Console.Write($"{i++}\n");
            //    }
            //},token);
            //t.Start();

            //Task.Factory.StartNew(() =>
            //{
            //    token.WaitHandle.WaitOne();
            //    Console.WriteLine("Wait handle released, cancellation was requested");
            //});

            //Console.ReadKey();
            //cts.Cancel();//to be used to cancel a task
            #endregion

            #region Ex2
            var planned = new CancellationTokenSource();
            var preventive = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token,preventive.Token,emergency.Token);
            Task.Factory.StartNew(() =>
            {
                int i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}");
                    Thread.Sleep(1000);
                }
            },paranoid.Token);

            Console.ReadKey();
            emergency.Cancel();//if any of the above token cancel is called task will be cancelled
            #endregion
        }

        public static void StartMethod()
        {
            #region task1
            //Task.Factory.StartNew(() => WriteChar('.'));

            //var t = new Task(()=> WriteChar('?'));
            //t.Start();
            #endregion

            #region task2
            //Task.Factory.StartNew(Write,"--TaskFactory--/");

            //var t = new Task(Write,"??Task??/");
            //t.Start();
            #endregion

            #region task3
            //string text1 = "testing", text2 = "this";
            //var task1 = new Task<int>(TextLength,text1);
            //task1.Start();

            //Task<int> task2 = Task.Factory.StartNew<int>(TextLength,text2);
            //Console.WriteLine($"Length of '{text1}' is {task1.Result}");
            //Console.WriteLine($"Length of '{text2}' is {task2.Result}");
            #endregion

            TaskWait();
            //TaskCancellation();
            //Console.Write('-');
            Console.WriteLine("Start Method done");
        }

    }
}
