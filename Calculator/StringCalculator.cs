namespace Calculator
{
    using System;
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

            if (numbers.StartsWith("//"))
            {
                var singleCharacterDelimiter = numbers[2].ToString().ToLower();
                numbers = numbers.Substring(4);

                _numbersDelimiters.Add(char.Parse(singleCharacterDelimiter));
            }

            var itemsToParse = numbers.Split(_numbersDelimiters.ToArray());

            if (itemsToParse.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                throw new FormatException("Two consecutive delimiters are not allowed.");
            }
            
            var arguments = itemsToParse.Select(x => int.Parse(x.Trim())).ToArray();
            if (arguments.Any(x => x < 0))
            {
                throw new ArgumentOutOfRangeException($"Negatives not allowed ({string.Join(",", arguments.Where(x => x < 0))})");
            }

            return arguments.Where(x=>x<=1000).Sum();

        }
    }
}
