using System;
using Xunit;

namespace Calculator.Tests
{
    using System.Linq;
    using AutoFixture;
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


        [Theory]
        [AutoData]
        public void Given_two_numbers_should_return_the_sum_of_both_numbers(StringCalculator sut, Generator<int> generator)
        {
            var numbers = generator.Take(2).ToList();
            var inputString = string.Join(",", numbers);
            var result = sut.Add(inputString);

            Assert.Equal(numbers.Sum(), result);
        }

        [Theory, AutoData]
        public void Given_unknown_amount_of_numbers_should_return_the_sum_of_all_numbers(StringCalculator sut, int multipleNumbers, Generator<int> intGenerator)
        {
            var integersToTest = intGenerator.Take(multipleNumbers).ToList();
            var inputString = string.Join(",", integersToTest);
            var result = sut.Add(inputString);

            Assert.Equal(integersToTest.Sum(), result);
        }

        [Theory, AutoData]
        public void Given_new_line_as_a_delimiter_should_return_the_sum_of_all_numbers(StringCalculator sut, int multipleNumbers, Generator<int> intGenerator)
        {
            var integersToTest = intGenerator.Take(multipleNumbers).ToList();
            var inputString = $"{integersToTest[0]}\n{string.Join(",", integersToTest.Skip(1).ToList())}";
            var result = sut.Add(inputString);

            Assert.Equal(integersToTest.Sum(), result);
        }

        [Theory, AutoData]
        public void Given_new_line_as_a_delimiter_after_comma_should_return_format_exception(StringCalculator sut)
        {
            var inputString = "1,\n";
            var exception = Assert.Throws<FormatException>(() => sut.Add(inputString));
            Assert.Contains("Two consecutive delimiters are not allowed.", exception.Message);
        }

        [Theory, AutoData]
        public void Given_consecutive_delimiters_should_return_format_exception(StringCalculator sut)
        {
            string[] inputStrings = { "1,\n", "1\n," };
            foreach (var inputString in inputStrings)
            {
                var exception = Assert.Throws<FormatException>(() => sut.Add(inputString));
                Assert.Contains("Two consecutive delimiters are not allowed.", exception.Message);
            }
        }
    }
}
