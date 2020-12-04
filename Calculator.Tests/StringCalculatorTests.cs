using System;
using Xunit;

namespace Calculator.Tests
{
    public class StringCalculatorTests
    {
        [Fact]
        public void Given_an_empty_string_should_return_zero()
        {
            var result = new StringCalculator().Add(string.Empty);
            Assert.Equal(0, result);
        }
    }
}
