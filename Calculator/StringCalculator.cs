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
                int GetNewLineIndex()
                {
                    for (var i = 0; i < numbers.Length; i++)
                    {
                        if (numbers[i]=='\n')
                        {
                            return i;
                        }
                    }
                    throw new ArgumentException("Newline delimiter not found");
                }

                var newLineIndex = GetNewLineIndex();
                var extractDelimiters = numbers.Substring(2, newLineIndex - 2).ToCharArray();
                //added new delimiters but making sure they are lowercase
                _numbersDelimiters.AddRange(extractDelimiters.Select(x=>x.ToString()
                    .ToLower()).Select(f=>char.Parse(f)));
                numbers = numbers.Substring(newLineIndex + 1);
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