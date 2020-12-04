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
                return numbers.Split(_numbersDelimiters.ToArray()).Select(x => int.Parse(x)).Sum();
            }

            return int.Parse(numbers);

        }
    }
}
