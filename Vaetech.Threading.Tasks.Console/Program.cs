﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vaetech.Data.ContentResult;
using Vaetech.Threading.Tasks;
using Parallel = Vaetech.Threading.Tasks.Parallel;

namespace Vaetech.Threading.Tasks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Method5();            
            System.Console.ReadKey();
        }                
        public static void Invoke()
        {
            ActionResult actionResult = Parallel.Invoke(() => SampleMethod("Process 0"));           

            if(actionResult.IB_Exception)
                System.Console.WriteLine("[0] Error: {0}", actionResult.Message);
            else
                System.Console.WriteLine("[0] successful process!");
        }
        public static void InvokeAndRunAll()
        {
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run All [1]");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            Parallel.Invoke(ProcessType.RunAll,
                () => SampleMethod("[1] Process 1"),
                () => SampleMethod("[1] Process 2"),
                () => SampleMethod("[1] Process 3"),
                () => SampleMethod("[1] Process 4"),
                () => SampleMethod("[1] Process 5")
                );
            
            System.Console.WriteLine("[1] successful process!");
        }
        public static void InvokeAndRunInOrder()
        {
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run RunInOrder [2] - Asynchronous");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            Parallel.Invoke(ProcessType.RunInOrder,
                () => SampleMethod("[2] Process 5"),
                () => SampleMethod("[2] Process 4"),
                () => SampleMethod("[2] Process 3"),
                () => SampleMethod("[2] Process 2"),
                () => SampleMethod("[2] Process 1")
                );

            System.Console.WriteLine("[2] successful process!");
        }
        public static async Task InvokeAsync()
        {
            await Parallel.InvokeAsync(() => SampleMethodAsync("Async Process 0"));
            System.Console.WriteLine("[0] successful async process!");
        }
        public static async Task InvokeAsync_WithResult()
        {
            ActionResult<DateTime[]> result = await Parallel.InvokeAsync(() => SampleMethodWithResultAsync("Async Process 0"));            
            System.Console.WriteLine("successful async process!");
        }
        public static async Task InvokeAndRunAllAsync()
        {
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run All [1] - Asynchronous");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            await Parallel.InvokeAsync(ProcessType.RunAll,
                () => SampleMethodAsync("[1] Process 1"),
                () => SampleMethodAsync("[1] Process 2"),
                () => SampleMethodAsync("[1] Process 3"),
                () => SampleMethodAsync("[1] Process 4"),
                () => SampleMethodAsync("[1] Process 5")
                );      
        }
        public static async Task InvokeAndRunInOrderAsync()
        {
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run RunInOrder [2] - Asynchronous");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            await Parallel.InvokeAsync(ProcessType.RunInOrder,
                () => SampleMethodAsync("[2] Process 5"),
                () => SampleMethodAsync("[2] Process 4"),
                () => SampleMethodAsync("[2] Process 3"),
                () => SampleMethodAsync("[2] Process 2"),
                () => SampleMethodAsync("[2] Process 1")
                );
        }
        public static void SampleMethod(string processName, int sleep= 3000)
        {
            System.Console.WriteLine("begin {0} {1}", processName, DateTime.Now.ToString("hh:mm:ss fff"));
            Thread.Sleep(sleep);
            System.Console.WriteLine("end {0} {1}", processName, DateTime.Now.ToString("hh:mm:ss fff"));
        }
        public static DateTime[] SampleMethodWithResult(string processName, int sleep = 3000)
        {
            DateTime[] dateTimes = new DateTime[2];
            
            System.Console.WriteLine("begin {0} {1}", processName, (dateTimes[0] = DateTime.Now).ToString("hh:mm:ss fff"));
            Thread.Sleep(sleep);
            System.Console.WriteLine("end {0} {1}", processName, (dateTimes[1] = DateTime.Now).ToString("hh:mm:ss fff"));

            return dateTimes;
        }
        public static async Task SampleMethodAsync(string processName, int sleep = 3000)
        {
            await Task.Run(() => 
            {
                System.Console.WriteLine("begin {0} {1}", processName, DateTime.Now.ToString("hh:mm:ss fff"));
                Thread.Sleep(sleep);
                System.Console.WriteLine("end {0} {1}", processName, DateTime.Now.ToString("hh:mm:ss fff"));                
            });
        }
        public static async Task<DateTime[]> SampleMethodWithResultAsync(string processName, int sleep = 3000)
        {
            return await Task.Run(() => 
            {
                DateTime[] dateTimes = new DateTime[2];

                System.Console.WriteLine("begin {0} {1}", processName, (dateTimes[0] = DateTime.Now).ToString("hh:mm:ss fff"));
                Thread.Sleep(sleep);
                System.Console.WriteLine("end {0} {1}", processName, (dateTimes[1] = DateTime.Now).ToString("hh:mm:ss fff"));

                return dateTimes;
            });
        }
        public static void Method4()
        {            
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run All [3]");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Parallel.Invoke(ProcessType.RunAll,
                () => SampleMethod("[3] method1"),
                () => SampleMethod("[3] method2"),
                () => SampleMethod("[3] method3"),
                () => SampleMethod("[3] method4"),
                () => SampleMethod("[3] method5")
                );

            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run RunInOrder [4]");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Parallel.Invoke(ProcessType.RunInOrder,
                () => SampleMethod("[4] method5"),
                () => SampleMethod("[4] method4"),
                () => SampleMethod("[4] method3"),
                () => SampleMethod("[4] method2"),
                () => SampleMethod("[4] method1")
                );
        }

        public static async void Method5() {
            List<Task> tasks = new List<Task>();

            string data1 = null;
            string data2 = null;
            string data3 = null;
            string data4 = null;

            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("Run All [1] - Asynchronous");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            await Parallel.InvokeAsync(
                () => Method6("123", 200),
                () => Method7("123", 200),
                () => Method6("123", 200)
                );

            await Parallel.InvokeAsync(
                () => (Method6("123", 200), a => { }),
                () => (Method7("123", 200), a => { }),
                () => (Method6("123", 200), a => { }),
                () => (Method8("123", 200), a => { })
                );
            
            await Parallel.InvokeAsync(ProcessType.RunInOrder,
               () => (Method6("123", 2000), async (ActionResult a) => 
               {                   
                   System.Console.WriteLine("-> end [1] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
                   await Task.Delay(2000);
                   System.Console.WriteLine("-> end [2] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
               }),
               () => (Method8("123", 200), async (ActionResult a) => 
               {
                   System.Console.WriteLine("-> end [1] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
                   await Task.Delay(2000);
                   System.Console.WriteLine("-> end [2] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
               }),
               () => (Method6("123", 200), async (ActionResult a) =>
               {
                   System.Console.WriteLine("-> end [1] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
                   await Task.Delay(2000);
                   System.Console.WriteLine("-> end [2] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
               }),
               () => (Method7("123", 200), async (ActionResult a) => 
               {
                   System.Console.WriteLine("-> end [1] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
                   await Task.Delay(2000);
                   System.Console.WriteLine("-> end [2] {0} {1}", a.Data, DateTime.Now.ToString("hh:mm:ss ffff"));
               })
               );            

            System.Console.WriteLine("data1: {0}", data1);
            System.Console.WriteLine("data2: {0}", data2);
            System.Console.WriteLine("data3: {0}", data3);
            System.Console.WriteLine("data4: {0}", data4);
        }
        
        public static async Task<ActionResult<string>> Method6(string processName, int sleep = 3000)
        {
            System.Console.WriteLine("begin {0} {1}", processName, DateTime.Now.ToString("hh:mm:ss ffff"));
            await Task.Delay(sleep);
            return await Task.Run(() => new ActionResult<string>(processName));
        }
        public static async Task<ActionResult<DateTime[]>> Method7(string processName, int sleep = 3000)
        {
            DateTime[] dateTimes = new DateTime[2];
            dateTimes[0] = DateTime.Now;

            System.Console.WriteLine("begin {0} {1}", processName, dateTimes[0].ToString("hh:mm:ss ffff"));
            await Task.Delay(sleep);
            dateTimes[1] = DateTime.Now;
            System.Console.WriteLine("end {0} {1}", processName, dateTimes[1].ToString("hh:mm:ss ffff"));

            return await Task.Run(() => new ActionResult<DateTime[]>(dateTimes));
        }
        public static async Task<DateTime[]> Method8(string processName, int sleep = 3000)
        {
            DateTime[] dateTimes = new DateTime[2];
            dateTimes[0] = DateTime.Now;

            System.Console.WriteLine("begin {0} {1}", processName, dateTimes[0].ToString("hh:mm:ss ffff"));
            await Task.Delay(sleep);
            dateTimes[1] = DateTime.Now;
            System.Console.WriteLine("end {0} {1}", processName, dateTimes[1].ToString("hh:mm:ss ffff"));

            return dateTimes;
        }
        public async Task<ActionResult<string>> MethodTestString1(int sleep)
        {
            return await Task.Run(() => new ActionResult<string>("1"));
        }
        public async Task<ActionResult<int>> MethodTest4_1(int sleep)
        {
            return await Task.Run(() => new ActionResult<int>(sleep));
        }
        public async Task<string> MethodTestString(int sleep)
        {
            return await Task.Run(() => "1");
        }
        public async Task<int> MethodTest4(int sleep)
        {
            return await Task.Run(() => 1);
        }
        public async Task MethodTest()
        {
            await Parallel.InvokeAsync(ProcessType.RunInOrder,
                () => Parallel.RunAsync(() => MethodTestString(3), (ActionResult<string> a) => { }),
                () => Parallel.RunAsync(() => MethodTest4(3), (ActionResult<int> a) => { }),
                () => Parallel.RunAsync(() => MethodTest4(3), (ActionResult<int> a) => { }),
                () => Parallel.RunAsync(() => MethodTestString1(3), (ActionResult<string> a) => { }),
                () => Parallel.RunAsync(() => MethodTest4_1(3), (ActionResult<int> a) => { })
                );

            await Parallel.InvokeAsync(ProcessType.RunInOrder,
                () => Parallel.RunAsync(() => MethodTestString1(3), (ActionResult<string> a) => { }),
                () => Parallel.RunAsync(() => MethodTest4_1(3), (ActionResult<int> a) => { }),
                () => Parallel.RunAsync(() => MethodTest4_1(3), (ActionResult<int> a) => { })
                );
        }
    }
}
