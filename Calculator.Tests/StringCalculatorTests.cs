using System;
using Xunit;

namespace Calculator.Tests
{
    using AutoFixture.Xunit2;

    public class StringCalculatorTests
    {
        [Fact]
        public void Given_an_empty_string_should_return_zero()
        {
            var result = new StringCalculator().Add(string.Empty);
            Assert.Equal(0, result);
        }

        [Theory, AutoData]
        public void Given_a_single_number_should_return_the_number(StringCalculator sut, int number)
        {
            var inputString = $"{number}";
            var result = sut.Add(inputString);
            Assert.Equal(number, result);
        }
    }
}
