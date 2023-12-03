namespace AdventOfCode._2023.Day3
{
    public static class Day3
    {
        public static int Part1()
        {
            var input = File.ReadAllText(@".\Day3\Day3-Data.txt");

            var arrays = CreateSchematicAndMask(input);

            return FindSymbolsAndPerformFunctionOnSurroundingValues(arrays.engineSchematic, arrays.mask, CalculateLocalSum);
            // 527364
        }

        public static int Part2()
        {
            var input = File.ReadAllText(@".\Day3\Day3-Data.txt");

            var arrays = CreateSchematicAndMask(input);

            return FindSymbolsAndPerformFunctionOnSurroundingValues(arrays.engineSchematic, arrays.mask, CalculateGearRatio);
            // 79026871
        }

        private static (char[,] engineSchematic, int[,] mask) CreateSchematicAndMask(string input)
        {
            var lines = input.Split('\n');
            var height = lines.Length;
            var width = lines[0].Length - 1; // lazy handling of not including newline characters

            int x = 0, y = 0;
            char[,] engineSchematic = new char[height, width];
            int[,] mask = new int[height, width];
            foreach (var row in input.Split('\n'))
            {
                var indexesToUpdate = new List<(int x, int y)>();
                var currentNumberString = string.Empty;
                bool currentlyCapturingNumber = false;
                var trimmedRow = row.Trim().ToCharArray();

                y = 0;
                foreach (var col in trimmedRow)
                {
                    engineSchematic[x, y] = col;

                    if (char.IsDigit(engineSchematic[x, y]))
                    {
                        indexesToUpdate.Add((x, y));
                        currentNumberString = currentNumberString + engineSchematic[x, y];
                        currentlyCapturingNumber = true;
                    }

                    if ((!char.IsDigit(engineSchematic[x, y]) && currentlyCapturingNumber) || (y + 1 == width && currentlyCapturingNumber))
                    {
                        PopulateMaskIndexesWithValue(mask, int.Parse(currentNumberString), indexesToUpdate);
                        currentlyCapturingNumber = false;
                        currentNumberString = string.Empty;
                        indexesToUpdate.Clear();
                    }

                    y++;
                }
                x++;
            }

            return (engineSchematic, mask);
        }

        private static void PopulateMaskIndexesWithValue(int[,] mask, int value, List<(int x, int y)> indexesToUpdate)
        {
            foreach (var index in indexesToUpdate)
            {
                mask[index.x, index.y] = value;
            }
        }

        private static int FindSymbolsAndPerformFunctionOnSurroundingValues(char[,] engineSchematic, int[,] mask, Func<SymbolIndex, int> function)
        {
            var sum = 0;

            var height = engineSchematic.GetLength(0);
            var width = engineSchematic.GetLength(1);

            for (int x = 0; x < height; x += 1)
            {
                for (int y = 0; y < width; y += 1)
                {
                    var currentValue = engineSchematic[x, y];
                    if (currentValue != '.'
                        && !char.IsDigit(currentValue)
                        && !char.IsLetter(currentValue))
                    {
                        var symbolIndex = new SymbolIndex(mask, x, y, height, width);
                        var value = function(symbolIndex);

                        sum += value;
                    }
                }
            }

            return sum;
        }

        private static int CalculateLocalSum(SymbolIndex symbolIndex)
        {
            int localSum = 0;

            var x = symbolIndex.X;
            var y = symbolIndex.Y;
            var maxHeight = symbolIndex.MaxHeight;
            var maxWidth = symbolIndex.MaxWidth;
            int[,] mask = symbolIndex.Mask;

            var leftIndex = y - 1 >= 0 ? y - 1 : 0;
            var rightIndex = y + 1 <= maxWidth ? y + 1 : 0;

            var upIndex = x - 1 >= 0 ? x - 1 : 0;
            var downIndex = x + 1 <= maxHeight ? x + 1 : 0;

            localSum += GetMaskIndexValueAndRemoveUsedValues(upIndex, leftIndex, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(upIndex, y, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(upIndex, rightIndex, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(x, rightIndex, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(downIndex, rightIndex, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(downIndex, y, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(downIndex, leftIndex, mask);
            localSum += GetMaskIndexValueAndRemoveUsedValues(x, leftIndex, mask);

            return localSum;
        }

        private static int CalculateGearRatio(SymbolIndex symbolIndex)
        {
            int localSum = 0;

            var x = symbolIndex.X;
            var y = symbolIndex.Y;
            var maxHeight = symbolIndex.MaxHeight;
            var maxWidth = symbolIndex.MaxWidth;
            int[,] mask = symbolIndex.Mask;

            var leftIndex = y - 1 >= 0 ? y - 1 : 0;
            var rightIndex = y + 1 <= maxWidth ? y + 1 : 0;

            var upIndex = x - 1 >= 0 ? x - 1 : 0;
            var downIndex = x + 1 <= maxHeight ? x + 1 : 0;

            var upperLeftValue = GetMaskIndexValueAndRemoveUsedValues(upIndex, leftIndex, mask);
            var upperValue = GetMaskIndexValueAndRemoveUsedValues(upIndex, y, mask);
            var upperRightValue = GetMaskIndexValueAndRemoveUsedValues(upIndex, rightIndex, mask);
            var rightValue = GetMaskIndexValueAndRemoveUsedValues(x, rightIndex, mask);
            var lowerRightValue = GetMaskIndexValueAndRemoveUsedValues(downIndex, rightIndex, mask);
            var lowerValue = GetMaskIndexValueAndRemoveUsedValues(downIndex, y, mask);
            var lowerLeftValue = GetMaskIndexValueAndRemoveUsedValues(downIndex, leftIndex, mask);
            var leftValue = GetMaskIndexValueAndRemoveUsedValues(x, leftIndex, mask);

            var values = new List<int> { upperLeftValue, upperValue, upperRightValue, rightValue, lowerRightValue, lowerValue, lowerLeftValue, leftValue };

            var gearCount = 0;
            var gearRatio = 1;
            foreach (var value in values.Where(v => v > 0))
            {
                gearCount += 1;
                gearRatio *= value;
            }

            return gearCount == 2 ? gearRatio : 0;
        }

        private static int GetMaskIndexValueAndRemoveUsedValues(int x, int y, int[,] mask)
        {
            var value = mask[x, y];
            RemoveUsedValueFromMask(x, y, mask);

            return value;
        }

        private static void RemoveUsedValueFromMask(int x, int y, int[,] mask)
        {
            if (mask[x, y] == 0)
            {
                return;
            }
            else
            {
                mask[x, y] = 0;

                var nextIndex = y + 1;

                while (nextIndex < mask.GetLength(1) && mask[x, nextIndex] != 0)
                {
                    mask[x, nextIndex] = 0;
                    nextIndex += 1;
                }

                var previousIndex = y - 1;

                while (previousIndex >= 0 && mask[x, previousIndex] != 0)
                {
                    mask[x, previousIndex] = 0;
                    previousIndex -= 1;
                }
            }
        }
    }

    public class SymbolIndex
    {
        public int[,] Mask { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int MaxHeight { get; set; }

        public int MaxWidth { get; set; }

        public SymbolIndex(int[,] mask, int x, int y, int maxHeight, int maxWidth) 
        {
            Mask = mask;
            X = x;
            Y = y;
            MaxHeight = maxHeight;
            MaxWidth = maxWidth;
        }
    }
}
