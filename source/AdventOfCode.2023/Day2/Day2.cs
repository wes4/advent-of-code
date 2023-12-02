using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day2
{
    public static class Day2
    {
        private const int MaximumNumberOfRedCubes = 12;
        private const int MaximumNumberOfGreenCubes = 13;
        private const int MaximumNumberOfBlueCubes = 14;

        private static Regex RedFilter = new Regex(@"\d+\s+[r]");
        private static Regex GreenFilter = new Regex(@"\d+\s+[g]");
        private static Regex BlueFilter = new Regex(@"\d+\s+[b]");
        private static Regex GameIdFilter = new Regex(@"(\d+):");

        public static int Part1()
        {
            using var fileStream = File.OpenRead(@".\Day2\Day2-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            int possibleGames = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var gameIdMatchString = GameIdFilter.Match(line).ToString();
                var gameId = int.Parse(gameIdMatchString.Substring(0, gameIdMatchString.Length - 1));

                var presentRedCubes = GetCubeCountsFromMatches(RedFilter.Matches(line)).Max();
                var presentGreenCubes = GetCubeCountsFromMatches(GreenFilter.Matches(line)).Max();
                var presentBlueCubes = GetCubeCountsFromMatches(BlueFilter.Matches(line)).Max();

                if (PresentCubeCountIsValid(presentRedCubes, MaximumNumberOfRedCubes) 
                    && PresentCubeCountIsValid(presentGreenCubes, MaximumNumberOfGreenCubes) 
                    && PresentCubeCountIsValid(presentBlueCubes, MaximumNumberOfBlueCubes))
                {
                    possibleGames += gameId;
                }
            }

            return possibleGames;
        }

        public static int Part2()
        {
            using var fileStream = File.OpenRead(@".\Day2\Day2-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            int gamePowerSum = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                gamePowerSum +=
                    GetCubeCountsFromMatches(RedFilter.Matches(line)).Max()
                    * GetCubeCountsFromMatches(GreenFilter.Matches(line)).Max()
                    * GetCubeCountsFromMatches(BlueFilter.Matches(line)).Max();
            }

            return gamePowerSum;
        }

        private static IEnumerable<int> GetCubeCountsFromMatches(IEnumerable<Match> matches)
        {
            return matches.Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2)));
        }

        private static bool PresentCubeCountIsValid(int numberOfCubesPresent, int maximumNumberOfCubes)
        {
            if (numberOfCubesPresent >= maximumNumberOfCubes)
            {
                return true;
            }

            return false;
        }
    }
}
