namespace AdventOfCode._2023.Day4
{
    public static class Day4
    {
        public static int Part1()
        {
            using var fileStream = File.OpenRead(@".\Day4\Day4-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            var totalCardSum = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var numbers = GetNumbers(line);

                var localCardSum = 0;

                foreach (var number in numbers.heldNumbers)
                {
                    if (numbers.winningNumbers.Contains(number))
                    {
                        if (localCardSum == 0)
                        {
                            localCardSum += 1;
                        }
                        else
                        {
                            localCardSum *= 2;
                        }
                    }
                }

                totalCardSum += localCardSum;
            }

            return totalCardSum;
            // 21213
        }

        public static int Part2() 
        {
            var text = File.ReadAllText(@".\Day4\Day4-Data.txt");
            var lines = text.Split('\n');
            var games = new List<Game>();

            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var numbers = GetNumbers(line);
                    games.Add(new Game(numbers.winningNumbers, numbers.heldNumbers));
                }
            }

            foreach (var game in games)
            {
                var matches = game.WinningNumbers.Intersect(game.HeldNumbers);
                var matchCount = matches.Count();

                var gameIndex = games.IndexOf(game);

                foreach (var attempt in Enumerable.Range(1, game.HeldCount + game.WinCount))
                {
                    var indexToUpdate = gameIndex + 1;
                    foreach (var match in matches)
                    {
                        if (indexToUpdate < games.Count)
                        {
                            games[indexToUpdate].WinCount += 1;

                            indexToUpdate += 1;
                        }
                    }
                }
            }

            return games.Sum(x => x.HeldCount) + games.Sum(y => y.WinCount);
            //8549735
        }

        private static (IEnumerable<int> winningNumbers, IEnumerable<int> heldNumbers) GetNumbers(string gameString)
        {
            var allNumbers = gameString.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            var numberGroups = allNumbers.Split("|", StringSplitOptions.RemoveEmptyEntries);
            var winningNumbersStrings = numberGroups[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var heldNumbersStrings = numberGroups[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var winningNumbers = winningNumbersStrings.Select(x => int.Parse(x));
            var heldNumbers = heldNumbersStrings.Select(x => int.Parse(x));

            return (winningNumbers, heldNumbers);
        }

        private class Game
        {
            public IEnumerable<int> WinningNumbers { get; set; }
            public IEnumerable<int> HeldNumbers { get; set; }
            public int HeldCount { get; set; }
            public int WinCount { get; set; }

            public Game(IEnumerable<int> winningNumbers, IEnumerable<int> heldNumbers)
            {
                WinningNumbers = winningNumbers;
                HeldNumbers = heldNumbers;
                HeldCount = 1;
                WinCount = 0;
            }
        }
    }
}
