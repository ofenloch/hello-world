using Xunit;

using Library;

namespace Library.UnitTests
{

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            double a = 13;
            double b = 31;
            double expectedValue = a + b;
            double actualValue = Library.MyMath.Add(a, b);
            Assert.True(expectedValue == actualValue , $"actual value {actualValue} should be equal to {expectedValue}");
        }
    }

} // namespace Library.UnitTests