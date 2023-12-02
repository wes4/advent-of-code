namespace AdventOfCode._2023.Day1
{
    public static class Day1
    {
        private const string Zero = "zero";
        private const string One = "one";
        private const string Two = "two";
        private const string Three = "three";
        private const string Four = "four";
        private const string Five = "five";
        private const string Six = "six";
        private const string Seven = "seven";
        private const string Eight = "eight";
        private const string Nine = "nine";
        private const string ReverseZero = "orez";
        private const string ReverseOne = "eno";
        private const string ReverseTwo = "owt";
        private const string ReverseThree = "eerht";
        private const string ReverseFour = "ruof";
        private const string ReverseFive = "evif";
        private const string ReverseSix = "xis";
        private const string ReverseSeven = "neves";
        private const string ReverseEight = "thgie";
        private const string ReverseNine = "enin";

        private static List<string> NumberWords = new List<string>
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
        };

        public static int Part1()
        {
            using var fileStream = File.OpenRead(@".\Day1\Day1-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            int count = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                count = count + int.Parse(line.FirstOrDefault(c => char.IsDigit(c)).ToString() + line.LastOrDefault(c => char.IsDigit(c)).ToString());
            }

            return count;
        }

        public static int Part2()
        {
            using var fileStream = File.OpenRead(@".\Day1\Day1-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            int count = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var firstDigit = FindFirstDigit(line, NumberWords);
                var lastDigit = FindFirstDigit(Reverse(line), NumberWords.Select(nw => Reverse(nw)).ToList());

                count = count + int.Parse(firstDigit + lastDigit);
            }

            return count;
        }

        private static string FindFirstDigit(string line, List<string> numberWords)
        {
            var firstDigit = line.FirstOrDefault(c => char.IsDigit(c));
            var firstDigitIndex = line.IndexOf(firstDigit) != -1 ? line.IndexOf(firstDigit) : line.Length;

            var startingIndexOfFirstNumberWord = line.Length;
            var currentNumberWord = string.Empty;

            foreach (var numberWord in numberWords)
            {
                if (line.Contains(numberWord))
                {
                    var indexOfEarliestNumberWord = line.IndexOf(numberWord);
                    if (indexOfEarliestNumberWord < startingIndexOfFirstNumberWord)
                    {
                        startingIndexOfFirstNumberWord = indexOfEarliestNumberWord;
                        currentNumberWord = numberWord;
                    }
                }
            }

            return firstDigitIndex < startingIndexOfFirstNumberWord ? firstDigit.ToString() : ConvertNumberWordToNumberString(currentNumberWord);
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static string ConvertNumberWordToNumberString(string s)
        {
            switch (s)
            {
                case Zero: return "0";
                case One: return "1";
                case Two: return "2";
                case Three: return "3";
                case Four: return "4";
                case Five: return "5";
                case Six: return "6";
                case Seven: return "7";
                case Eight: return "8";
                case Nine: return "9";
                case ReverseZero: return "0";
                case ReverseOne: return "1";
                case ReverseTwo: return "2";
                case ReverseThree: return "3";
                case ReverseFour: return "4";
                case ReverseFive: return "5";
                case ReverseSix: return "6";
                case ReverseSeven: return "7";
                case ReverseEight: return "8";
                case ReverseNine: return "9";
            }

            return string.Empty;
        }
    }
}
