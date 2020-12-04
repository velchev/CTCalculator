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

        [Theory]
        [InlineAutoData("1,\n")]
        [InlineAutoData("1\n,")]
        public void Given_consecutive_delimiters_should_return_format_exception(string inputString, StringCalculator sut)
        {
            var exception = Assert.Throws<FormatException>(() => sut.Add(inputString));
            
            Assert.Contains("Two consecutive delimiters are not allowed.", exception.Message);
        }

        [Theory]
        [InlineAutoData(";")]
        [InlineAutoData("&")]
        [InlineAutoData("P")]
        [InlineAutoData("p")]
        public void Given_a_custom_delimiter_definition_should_return_the_sum_of_all_number(char customDelimiter, StringCalculator sut,
            int multipleNumbers, Generator<int> intGenerator)
        {
            var integersToTest = intGenerator.Take(multipleNumbers).ToList();
            var lowercaseCharacterDelimiter = customDelimiter.ToString().ToLower();
            var inputString = $"//{lowercaseCharacterDelimiter}\n{string.Join(lowercaseCharacterDelimiter, integersToTest)}";


            var result = sut.Add(inputString);
            
            Assert.Equal(integersToTest.Sum(), result);
        }

        [Theory]
        [InlineAutoData("-2", "-2")]
        [InlineAutoData("-2,-3", "-2,-3")]
        public void Given_only_negative_numbers_should_throw_an_exception(string inputString, string negativeInputString,
            StringCalculator sut)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(inputString));

            Assert.Contains("Negatives not allowed", exception.Message);
            Assert.Contains(negativeInputString, exception.Message);
        }

        [Theory]
        [InlineAutoData("-2,3", "-2", "3")]
        [InlineAutoData("-2,-3,1", "-2,-3", "1")]
        public void Given_a_negative_number_should_throw_an_exception(string inputString, string negativeInputString, 
            string positiveInputString,
            StringCalculator sut)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(inputString));

            Assert.Contains("Negatives not allowed", exception.Message);
            Assert.Contains(negativeInputString, exception.Message);
            Assert.DoesNotContain(positiveInputString, exception.Message);
        }

        [Theory, AutoData]
        public void Given_a_negative_numbers_should_throw_an_exception(StringCalculator sut, int multipleNumbers, Generator<int> intGenerator)
        {
            var integersToTest = intGenerator.Take(multipleNumbers).ToList();
            var negativeInputString = string.Join(",", integersToTest.Take(multipleNumbers).ToList().Select(x => -1 * x));
            var positiveInputString = string.Join(",", integersToTest.Skip(multipleNumbers / 2));
            var inputString = $"{negativeInputString},{positiveInputString}";

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(inputString));

            Assert.Contains("Negatives not allowed", exception.Message);
            Assert.Contains(negativeInputString, exception.Message);
            Assert.DoesNotContain(positiveInputString, exception.Message);
        }


        [Theory]
        [InlineAutoData(2, 1001)]
        [InlineAutoData(1000, 1001)]
        public void Given_numbers_greater_than_1000_should_return_sum_ignoring_those_numbers(int smallNumber, int bigNumber, StringCalculator sut)
        {
            var inputString = $"{string.Join(",", smallNumber, bigNumber)}";
            var result = sut.Add(inputString);
            Assert.Equal(smallNumber, result);
        }

        [Theory]
        [InlineAutoData("2,1001,1", 3)]
        [InlineAutoData("1,2,3,4,2000,3000,2001", 10)]
        [InlineAutoData("1001", 0)]
        [InlineAutoData("2,1001,13", 15)]
        public void Given_many_numbers_greater_than_1000_should_return_sum_ignoring_those_numbers(string inputString, int expectedResult, StringCalculator sut)
        {
            var result = sut.Add(inputString);
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineAutoData("//*%\n1*2%3", 6)]
        [InlineAutoData("//*%$zx\n1*2%3$1z1x1", 9)]
        [InlineAutoData("//*%$zx\n1*2%3$1z1x1\n1", 10)]
        [InlineAutoData("//*%1%1", 2)]
        public void Given_many_delimiters_should_return_sum_using_those_delimiters(string inputString, int expectedResult,
            StringCalculator sut)
        {
            var actual = sut.Add(inputString);
            Assert.Equal(expectedResult, actual);
        }
    }
}
