using System;

namespace Calculator
{
    using System.Collections.Generic;
    using System.Linq;

    public class StringCalculator
    {
        private readonly List<char> _numbersDelimiters = new List<char>() { ',', '\n' };
        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers))
            {
                return 0;
            }

            if (numbers.Any(x=>_numbersDelimiters.Contains(x)))
            {
                var itemsToParse = numbers.Split(_numbersDelimiters.ToArray());

                if (itemsToParse.Any(x => string.IsNullOrWhiteSpace(x)))
                {
                    throw new FormatException("Two consecutive delimiters are not allowed.");
                }

                return numbers.Split(_numbersDelimiters.ToArray()).Select(x => int.Parse(x)).Sum();
            }

            return int.Parse(numbers);

        }
    }
}
