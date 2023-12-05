namespace AdventOfCode._2023.Day5
{
    public static class Day5
    {
        public static long Part1()
        {
            long[] seeds = null;
            var maps = new List<Map>();
            List<string> conversionPath = new List<string>();

            var text = File.ReadAllText(@".\Day5\Day5-Data.Txt");
            var lines = text.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                if (currentLine.Contains("seeds:"))
                {
                    var seedsValues = currentLine.Split(":")[1].Trim().Split(" ").Select(x => long.Parse(x));
                    seeds = seedsValues.ToArray();
                }

                if (currentLine.Contains("-to-"))
                {
                    var mapWords = currentLine.Split("-");
                    var fromMap = mapWords[0];
                    var toMap = mapWords[2].Split(" ")[0];
                    conversionPath.Add(toMap);

                    var checkIndex = i + 1;

                    var mapValues = new List<(long destination, long source, long range)>();

                    while (checkIndex < lines.Length && !(lines[checkIndex].Length == 1 && lines[checkIndex].Contains('\r')))
                    { 
                        var mapString = lines[checkIndex].Split(" ");
                        var destinationRangeStart = long.Parse(mapString[0]);
                        var sourceRangeStart = long.Parse(mapString[1]);
                        var rangeLength = long.Parse(mapString[2]);
                        mapValues.Add((destinationRangeStart, sourceRangeStart, rangeLength));
                        checkIndex += 1;
                    }

                    var sources = mapValues.Select(x => x.source).ToList();
                    var destinations = mapValues.Select(x => x.destination).ToList();
                    var ranges = mapValues.Select(x => x.range).ToList();
                    maps.Add(new Map(fromMap, toMap, sources, destinations, ranges));
                }
            }

            var closestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var value = seed;
                foreach (var conversionName in conversionPath)
                {
                    var converter = maps.FirstOrDefault(map => map.To == conversionName);
                    value = converter.Convert(value);
                }

                closestLocation = value < closestLocation ? value : closestLocation;
            }

            return closestLocation;
        }

        private class Map
        {
            public string From { get; }
            public string To { get; }

            public List<long> SourceValues { get; }

            public List<long> DestinationValues { get; }

            public List<long> RangeValues { get; }

            public Map(string from, string to, List<long> sourceValues, List<long> destinationValues, List<long> rangeValues)
            {
                From = from;
                To = to;
                SourceValues = sourceValues;
                DestinationValues = destinationValues;
                RangeValues = rangeValues;
            }

            public long Convert(long value)
            {
                var returnValue = long.MaxValue;
                var withinAnyRanges = false;

                foreach (var sourceValue in SourceValues)
                {
                    var index = SourceValues.IndexOf(sourceValue);
                    if (value >= sourceValue && value < sourceValue + RangeValues[index])
                    {
                        // TODO: Calculate
                        var offset = value - sourceValue;
                        returnValue = DestinationValues[index] + offset;

                        withinAnyRanges = true;
                        break;
                    }
                }

                if (!withinAnyRanges)
                {
                    returnValue = value;
                }

                return returnValue;
                // check if within source value + range
                // else return value
            }
        }
    }
}
