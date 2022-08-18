// This is a .NET 5 (and earlier) console app template
// (See https://aka.ms/new-console-template for more information)

using Microsoft.Extensions.Logging;
using System;

namespace MyApp
{

    internal class HelloWorld
    {
        static void Main(string[] args)
        {

            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }));

            ILogger<HelloWorld> logger = loggerFactory.CreateLogger<HelloWorld>();
            using (logger.BeginScope("[scope is enabled]"))
            {
                logger.LogInformation("Logs contain timestamp and log level.");
                logger.LogInformation("Each log message is fit in a single line.");
                logger.LogTrace("Trace");
                logger.LogDebug("Debug");
                logger.LogInformation("Info");
                logger.LogWarning("Warning");
                logger.LogError("Error");
                logger.LogCritical("Critical");

            }

            Console.WriteLine("Hello, World!");

            double x = 1.234;
            double y = 4.321;
            double sum = Library.MyMath.Add(x, y);
            double prod = Library.MyMath.Multiply(x, y);
            Console.WriteLine(String.Format("{0} plus {1} makes {2}", x, y, sum));
            Console.WriteLine(String.Format("{0} times {1} makes {2}", x, y, prod));


            Library.DataStore<int, string> myData = new Library.DataStore<int, string>();
            for (int i = 0; i < 100; i++)
            {
                string text = string.Format("This is element {0}.", i);
                myData.Add(i, text);
            }

            PrintElement(myData, 42);
            PrintElement(myData, 100);
            PrintElement(myData, 101);
            PrintElement(myData, 102);

        }
        public static void PrintElement(Library.DataStore<int, string> Store, int index)
        {
            Library.Pair<int, string>? element = Store.GetElementByIndex(index);
            if (element is Library.Pair<int, string> valueOfElment)
            {
                Console.WriteLine(String.Format("idx {0}: key {1}, value {2}", index,
                    element.GetKey(), element.GetValue()));
            }
            else
            {
                Console.WriteLine(String.Format("idx {0}: no such element in DataStore", index));
            }
        }

    } // class HelloWorld

} // namespace MyApp
