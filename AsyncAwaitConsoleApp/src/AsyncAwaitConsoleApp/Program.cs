using System;
using System.Threading.Tasks;

namespace AsyncAwaitConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CallAsyncMethodsSync();

            CreateStartAndReturnTasks();

            // Exit
            Console.WriteLine("\n\nHit any key to exit ..");
            Console.ReadKey();
        }

        private static void CreateStartAndReturnTasks()
        {
        }

        /// <summary>
        /// Depending on the application type - a running Task will run and block in a context (SynchronizationContext) and may run into a deadlock depending on how it is called.
        /// The problem is that an UI Thread/Context waits for the Task to complete, and the Task waits for the UI Thread to complete = Deadlock! 
        /// ASP.Net applications have a request context that allows only one thread at a time and the same problem may happen as for example a WindowsForms application, or a Unit Test runner.
        /// 
        /// Recommendations: Don't block on async code. 
        /// 1. Use async/await all the way
        /// 2. Wrap the async code and call it where you actually have detailed control.
        /// 3. In your “library” async methods, use ConfigureAwait(false) wherever possible. Ugly hack.
        /// </summary>
        /// <remarks>
        /// http://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
		/// http://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
        /// </remarks>
        private static void CallAsyncMethodsSync()
        {
            // Unsafe. Calling .Result blocks the main thread, and can deadlock if the caller is on the thread pool itself.
            var task1 = Message.CreateMessageAsync("test 1");
            Console.WriteLine(task1.Result.Text);

            // Safer, but can still produce deadlocks.
            // Does not resume on the context. Instead, .CreateMessageAsync will resume on a thread pool thread. 
            // Avoid becuase it's just a hack and you would have to use it everywhere!
            var message = Message.CreateMessageAsync("test 2")
                .ConfigureAwait(false) // 
                .GetAwaiter()
                .GetResult();
            Console.WriteLine(message.Text);

            // Is completely safe for all application types
            var task3 = Task.Run(async () => await Message.CreateMessageAsync("test 4"));
            Console.WriteLine(task3.Result.Text);
        }
    }
}