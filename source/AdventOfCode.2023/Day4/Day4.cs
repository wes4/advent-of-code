namespace AdventOfCode._2023.Day4
{
    public static class Day4
    {
        public static int Part1()
        {
            var games = GenerateGames(@".\Day4\Day4-Data.txt");

            double totalCardSum = 0;

            foreach (var game in games)
            {
                var wins = game.WinningNumbers.Intersect(game.HeldNumbers).Count();

                if (wins > 0)
                {
                    totalCardSum += Math.Pow(2, wins - 1);
                }
            }

            return (int)totalCardSum;
            // 21213
        }

        public static int Part2()
        {
            var games = GenerateGames(@".\Day4\Day4-Data.txt");

            foreach (var game in games)
            {
                var gameIndex = games.IndexOf(game);
                var numberOfWins = game.WinningNumbers.Intersect(game.HeldNumbers).Count();
                var indexToUpdate = gameIndex + 1;

                foreach (var win in Enumerable.Range(1, numberOfWins))
                {
                    if (indexToUpdate < games.Count)
                    {
                        games[indexToUpdate].WinCount += (game.HeldCount + game.WinCount);

                        indexToUpdate += 1;
                    }
                }
            }

            return games.Sum(x => x.HeldCount) + games.Sum(y => y.WinCount);
            // 8549735
        }

        private static List<Game> GenerateGames(string filepath)
        {
            var text = File.ReadAllText(filepath);
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

            return games;
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
