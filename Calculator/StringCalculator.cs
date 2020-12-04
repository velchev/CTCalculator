using System;

namespace Calculator
{
    using System.Linq;

    public class StringCalculator
    {
        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers))
            {
                return 0;
            }

            if (numbers.Contains(","))
            {
                return numbers.Split(",").Select(x => int.Parse(x)).Sum();
            }

            return int.Parse(numbers);

        }
    }
}
