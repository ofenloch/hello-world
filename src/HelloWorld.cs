// This is a .NET 5 (and earlier) console app template
// (See https://aka.ms/new-console-template for more information)

using System;

namespace MyApp
{

    internal class HelloWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            double x = 1.234;
            double y = 4.321;
            double sum = MyMath.Add(x, y);
            double prod = MyMath.Multiply(x, y);
            Console.WriteLine(String.Format("{0} plus {1} makes {2}", x, y, sum));
            Console.WriteLine(String.Format("{0} times {1} makes {2}", x, y, prod));
        }

    } // class HelloWorld

} // namespace MyApp
