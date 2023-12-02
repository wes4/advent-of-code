using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day2
{
    public static class Day2
    {
        private static Regex RedFilter = new Regex(@"\d+\s+[r]");
        private static Regex GreenFilter = new Regex(@"\d+\s+[g]");
        private static Regex BlueFilter = new Regex(@"\d+\s+[b]");
        private static Regex GameIdFilter = new Regex(@"(\d+):");

        public static int Part1()
        {
            using var fileStream = File.OpenRead(@".\Day2\Day2-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            var maxRedCubes = 12;
            var maxGreenCubes = 13;
            var maxBlueCubes = 14;
            int possibleGames = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var gameIdMatchString = GameIdFilter.Match(line).ToString();
                var gameId = int.Parse(gameIdMatchString.Substring(0, gameIdMatchString.Length - 1));

                var presentRedCubes = RedFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max();
                var presentGreenCubes = GreenFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max();
                var presentBlueCubes = BlueFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max();

                if (maxRedCubes >= presentRedCubes && maxGreenCubes >= presentGreenCubes && maxBlueCubes >= presentBlueCubes)
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
                    RedFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max()
                    * GreenFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max()
                    * BlueFilter.Matches(line).Select(m => int.Parse(m.ToString().Substring(0, m.Length - 2))).Max();
            }

            return gamePowerSum;
        }
    }
}
