using Xunit;

using Library;

namespace Library.UnitTests
{

    public class UnitTest1
    {

        // The [Fact] attribute declares a test method that's run by the test runner. From the PrimeService.Tests folder, run dotnet test.
        [Fact]
        public void Test1()
        {
            double a = 13;
            double b = 31;
            double expectedValue = a + b;
            double actualValue = Library.MyMath.Add(a, b);
            Assert.True(expectedValue == actualValue, $"actual value {actualValue} should be equal to {expectedValue}");
        }
    
        // [Theory] represents a suite of tests that execute the same code but have different input arguments.
        // [InlineData] attribute specifies values for those inputs.
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        // Finally fix the test, so the image gets built:
        // [InlineData(2)]
        public void IsPrime_ValuesLessThan2_ReturnFalse(int value)
        {
            var result = Library.MyMath.IsPrime(value);

            Assert.False(result, $"{value} should not be prime");
        }



    }

} // namespace Library.UnitTests