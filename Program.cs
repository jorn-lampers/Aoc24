using System;
using System.Collections.Generic;

// Using linq
using System.Linq;
using System.Text.RegularExpressions;

Day4Pt2();

void Day1() {
  string[] lines = File.ReadAllLines("input1");

  List<int> leftList = lines
    .Select(l => int.Parse(l.Split(" ")[0]))
    .OrderBy(n => n)
    .ToList();

  List<int> rightList = lines
    .Select(l => int.Parse(l.Split(" ").Where(s => s.Length > 0).ToArray()[1]))
    .OrderBy(n => n)
    .ToList();

  int totalDistance = 0;
  for (int i = 0; i < leftList.Count; i++)
  {
    int distance = Math.Abs(leftList[i] - rightList[i]);
    totalDistance += distance;
  }

  Console.WriteLine($"Total distance: {totalDistance}");

  var totalSimilarityScore = 0;
  for (int i = 0; i < leftList.Count; i++)
  {
    int amountOfOccurencesInRightList = rightList.Count(n => n == leftList[i]);
    int similarityScore = amountOfOccurencesInRightList * leftList[i];

    totalSimilarityScore += similarityScore;
  }

  Console.WriteLine($"Total similarity score: {totalSimilarityScore}");
}

void Day3p1() {
  string input = File.ReadAllText("input3");

  // regex match mul\(\d+,\d+\)
  var regex = new Regex(@"mul\(\d+,\d+\)");
  
  var matches = regex.Matches(input);

  int total = 0;
  foreach (var match in matches)
  {

    total += ResolveMul(match.ToString());
  }

  Console.WriteLine($"Sum of products: {total}");
}

void Day3p2() {
  string input = File.ReadAllText("input3");

  string doParts = FindDoParts(input);

  var regex = new Regex(@"mul\(\d+,\d+\)");
  var matches = regex.Matches(doParts);

  int total = 0;
  foreach (var match in matches)
  {
    total += ResolveMul(match.ToString());
  }

  Console.WriteLine($"Sum of products: {total}");
}

string FindDoParts(string input) {

  List<string> doParts = new List<string>();

  int startIndex = 0;
  while (startIndex != -1)
  {
    int endOfDo = input.IndexOf("don't()", startIndex);
    if (endOfDo == -1)
    {
      endOfDo = input.Length;
    }

    doParts.Add(input.Substring(startIndex, endOfDo - startIndex));

    startIndex = input.IndexOf("do(", endOfDo);
  }

  return doParts.Aggregate((a, b) => a + b);
}

int ResolveMul(string input) {
  int n1 = int.Parse(input.Split(",")[0].Replace("mul(", ""));
  int n2 = int.Parse(input.Split(",")[1].Replace(")", ""));

  return n1 * n2;
}

void Day4Pt2() {
  string[] input = File.ReadAllLines("input4");

  int width = input[0].Length;
  int height = input.Length;

  int xMasCount = 0;
  for (int x = 1; x < width - 1; x++)
  {
    for (int y = 1; y < height - 1; y++)
    {
      if (input[y][x] == 'A')
      {
        char[] lineA = {
          input[y - 1][x - 1], 
          input[y + 1][x + 1]
        };
        
        char[] lineB = {
          input[y + 1][x - 1], 
          input[y - 1][x + 1]
        };

        if (lineA.Contains('M') && lineA.Contains('S') 
            && lineB.Contains('M') && lineB.Contains('S'))
          xMasCount++;

      }
    }
  }

  Console.WriteLine($"XMAS count: {xMasCount}");
}