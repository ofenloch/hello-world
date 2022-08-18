namespace Library
{

    public class MyMath
    {
        public static double Add(double a, double b)
        {
            return a + b;
        }
        public static double Subtract(double a, double b)
        {
            return a - b;
        }
        public static double Multiply(double a, double b)
        {
            return a * b;
        }
        public static double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new ArgumentException(String.Format("divisor must not be {0}", b));
            }
            return a / b;
        }

        public static bool IsPrime(int candidate)
        {
            if (candidate <= 1)
            {
                return false;
            }
            throw new NotImplementedException("Not fully implemented.");
        }
    } // public class MyMath

} // namespace Library