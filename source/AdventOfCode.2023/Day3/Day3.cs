using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023.Day3
{
    public static class Day3
    {
        private const char Default = '.';

        public static int Part1()
        {
            using var fileStream = File.OpenRead(@".\Day3\Day3-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            var charLines = new List<List<char>>();
            var numberMask = new List<List<long>>();

            int partSum = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var chars = line.ToCharArray().ToList();
                charLines.Add(chars);

                var numberMaskRow = new long[chars.Count()];
                numberMask.Add(numberMaskRow.ToList());
            }

            char[][] engineSchematic = charLines.Select(charLine => charLine.ToArray()).ToArray();
            long[][] mask = numberMask.Select(x => x.ToArray()).ToArray();

            var height = engineSchematic.Length;
            var width = engineSchematic[0].Length;

            for (int x = 0; x < height; x += 1)
            {
                var indexesToUpdate = new List<(int x, int y)>();
                var currentNumberString = string.Empty;
                bool currentlyCapturingNumber = false;

                for (int y = 0; y < width; y += 1)
                {
                    if (char.IsDigit(engineSchematic[x][y]))
                    {
                        indexesToUpdate.Add((x, y));
                        currentNumberString = currentNumberString + engineSchematic[x][y];
                        currentlyCapturingNumber = true;
                    }

                    if ((!char.IsDigit(engineSchematic[x][y]) && currentlyCapturingNumber) || (y + 1 == width && currentlyCapturingNumber))
                    {
                        UpdateIndexes(mask, long.Parse(currentNumberString), indexesToUpdate);
                        currentlyCapturingNumber = false;
                        currentNumberString = string.Empty;
                        indexesToUpdate.Clear();
                    }
                }

            }

            for (int x = 0; x < height; x += 1)
            {
                for (int y = 0; y < width; y += 1)
                {
                    var currentValue = engineSchematic[x][y];
                    if (currentValue != Default)
                    {
                        if (!char.IsDigit(currentValue) && !char.IsLetter(currentValue))
                        {
                            var localSum = CalculateLocalSum(mask, x, y, height, width);

                            partSum += localSum;
                        }
                    }
                }
            }

            return partSum;
            // 527364
        }

        public static int Part2()
        {
            using var fileStream = File.OpenRead(@".\Day3\Day3-Data.txt");
            using var streamReader = new StreamReader(fileStream);

            var charLines = new List<List<char>>();
            var numberMask = new List<List<long>>();

            int gearRatioSum = 0;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var chars = line.ToCharArray().ToList();
                charLines.Add(chars);

                var numberMaskRow = new long[chars.Count()];
                numberMask.Add(numberMaskRow.ToList());
            }

            char[][] engineSchematic = charLines.Select(charLine => charLine.ToArray()).ToArray();
            long[][] mask = numberMask.Select(x => x.ToArray()).ToArray();

            var height = engineSchematic.Length;
            var width = engineSchematic[0].Length;

            for (int x = 0; x < height; x += 1)
            {
                var indexesToUpdate = new List<(int x, int y)>();
                var currentNumberString = string.Empty;
                bool currentlyCapturingNumber = false;

                for (int y = 0; y < width; y += 1)
                {
                    if (char.IsDigit(engineSchematic[x][y]))
                    {
                        indexesToUpdate.Add((x, y));
                        currentNumberString = currentNumberString + engineSchematic[x][y];
                        currentlyCapturingNumber = true;
                    }

                    if ((!char.IsDigit(engineSchematic[x][y]) && currentlyCapturingNumber) || (y + 1 == width && currentlyCapturingNumber))
                    {
                        UpdateIndexes(mask, long.Parse(currentNumberString), indexesToUpdate);
                        currentlyCapturingNumber = false;
                        currentNumberString = string.Empty;
                        indexesToUpdate.Clear();
                    }
                }

            }

            for (int x = 0; x < height; x += 1)
            {
                for (int y = 0; y < width; y += 1)
                {
                    var currentValue = engineSchematic[x][y];
                    if (currentValue != Default)
                    {
                        if (!char.IsDigit(currentValue) && !char.IsLetter(currentValue))
                        {
                            var localGearRatioSum = CalculateGearRatio(mask, x, y, height, width);

                            gearRatioSum += localGearRatioSum;
                        }
                    }
                }
            }

            return gearRatioSum;
        }

        private static void UpdateIndexes(long[][] mask, long value, List<(int x, int y)> indexesToUpdate)
        {
            foreach (var index in indexesToUpdate)
            {
                mask[index.x][index.y] = value;
            }
        }

        private static int CalculateGearRatio(long[][] mask, int height, int width, int maxHeight, int maxWidth)
        {
            var gearRatio = 1;
            var gearCount = 0;

            var leftIndex = width - 1;
            if (leftIndex >= 0)
            {
                var leftIndexValue = mask[height][leftIndex];
                if (int.Parse(leftIndexValue.ToString()) != 0)
                {
                    gearCount += 1;
                    gearRatio *= int.Parse(leftIndexValue.ToString());
                }
                RemoveUsedValue(height, leftIndex, mask);

                var previousRowValue = height - 1;
                if (previousRowValue >= 0)
                {
                    var upperLefttValue = mask[previousRowValue][leftIndex];
                    if (int.Parse(upperLefttValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(upperLefttValue.ToString());
                    }
                    RemoveUsedValue(previousRowValue, leftIndex, mask);

                    var upperValue = mask[previousRowValue][width];
                    if (int.Parse(upperValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(upperValue.ToString());
                    }
                    RemoveUsedValue(previousRowValue, width, mask);
                }

                var nextRowValue = height + 1;
                if (nextRowValue <= maxHeight)
                {
                    var lowerLeftValue = mask[nextRowValue][leftIndex];
                    if (int.Parse(lowerLeftValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(lowerLeftValue.ToString());
                    }
                    RemoveUsedValue(nextRowValue, leftIndex, mask);

                    var lowerValue = mask[nextRowValue][width];
                    if (int.Parse(lowerValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(lowerValue.ToString());
                    }
                    RemoveUsedValue(nextRowValue, width, mask);
                }
            }

            var rightIndex = width + 1;
            if (rightIndex <= maxWidth)
            {
                var rightIndexValue = mask[height][rightIndex];
                if (int.Parse(rightIndexValue.ToString()) != 0)
                {
                    gearCount += 1;
                    gearRatio *= int.Parse(rightIndexValue.ToString());
                }
                RemoveUsedValue(height, rightIndex, mask);

                var previousRowValue = height - 1;
                if (previousRowValue >= 0)
                {
                    var upperRightValue = mask[previousRowValue][rightIndex];
                    if (int.Parse(upperRightValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(upperRightValue.ToString());
                    }
                    RemoveUsedValue(previousRowValue, rightIndex, mask);
                }

                var nextRowValue = height + 1;
                if (nextRowValue <= maxHeight)
                {
                    var lowerRightValue = mask[nextRowValue][rightIndex];
                    if (int.Parse(lowerRightValue.ToString()) != 0)
                    {
                        gearCount += 1;
                        gearRatio *= int.Parse(lowerRightValue.ToString());
                    }
                    RemoveUsedValue(nextRowValue, rightIndex, mask);
                }
            }

            return gearCount == 2 ? gearRatio : 0;
        }

        private static int CalculateLocalSum(long[][] mask, int height, int width, int maxHeight, int maxWidth)
        {
            var localSum = 0;

            var leftIndex = width - 1;
            if (leftIndex >= 0)
            {
                var leftIndexValue = mask[height][leftIndex];
                localSum += int.Parse(leftIndexValue.ToString());
                RemoveUsedValue(height, leftIndex, mask);

                var previousRowValue = height - 1;
                if (previousRowValue >= 0)
                {
                    var upperLefttValue = mask[previousRowValue][leftIndex];
                    localSum += int.Parse(upperLefttValue.ToString());
                    RemoveUsedValue(previousRowValue, leftIndex, mask);

                    var upperValue = mask[previousRowValue][width];
                    localSum += int.Parse(upperValue.ToString());
                    RemoveUsedValue(previousRowValue, width, mask);
                }

                var nextRowValue = height + 1;
                if (nextRowValue <= maxHeight)
                {
                    var lowerLeftValue = mask[nextRowValue][leftIndex];
                    localSum += int.Parse(lowerLeftValue.ToString());
                    RemoveUsedValue(nextRowValue, leftIndex, mask);

                    var lowerValue = mask[nextRowValue][width];
                    localSum += int.Parse(lowerValue.ToString());
                    RemoveUsedValue(nextRowValue, width, mask);
                }
            }

            var rightIndex = width + 1;
            if (rightIndex <= maxWidth)
            {
                var rightIndexValue = mask[height][rightIndex];
                localSum += int.Parse(rightIndexValue.ToString());
                RemoveUsedValue(height, rightIndex, mask);

                var previousRowValue = height - 1;
                if (previousRowValue >= 0)
                {
                    var upperRightValue = mask[previousRowValue][rightIndex];
                    localSum += int.Parse(upperRightValue.ToString());
                    RemoveUsedValue(previousRowValue, rightIndex, mask);
                }

                var nextRowValue = height + 1;
                if (nextRowValue <= maxHeight)
                {
                    var lowerRightValue = mask[nextRowValue][rightIndex];
                    localSum += int.Parse(lowerRightValue.ToString());
                    RemoveUsedValue(nextRowValue, rightIndex, mask);
                }
            }

            return localSum;
        }

        private static void RemoveUsedValue(int x, int y, long[][] mask)
        {
            if (mask[x][y] == 0)
            {
                return;
            }
            else
            {
                mask[x][y] = 0;

                var nextIndex = y + 1;

                while(nextIndex < mask[x].Length && mask[x][nextIndex] != 0)
                {
                    mask[x][nextIndex] = 0;
                    nextIndex += 1;
                }

                var previousIndex = y - 1;

                while (previousIndex >= 0 && mask[x][previousIndex] != 0)
                {
                    mask[x][previousIndex] = 0;
                    previousIndex -= 1;
                }
            }
        }
    }
}
