using AOC24;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;


// Using linq
using System.Linq;
using System.Text.RegularExpressions;

new Day6().Run();

void Day1()
{

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

void Day4p1()
{
    string[] input = File.ReadAllLines("input4");

    string wordToFind = "XMAS";

    // Find the word in wordToFind in any direction in the input grid
    int count = 0;

    for (int i = 0; i < input.Length; i++)
    {
        for (int j = 0; j < input[i].Length; j++)
        {
            count += FindWordCountFromPos(wordToFind, input, i, j);
        }
    }

    Console.WriteLine($"Found the word {wordToFind} {count} times");

}

void Day5p1()
{
    string[] input = File.ReadAllLines("input5");

    int[][] rules = input
        .TakeWhile(l => l != "")
        .Select(s => s.Split("|")
            .Select(d => int.Parse(d))
            .ToArray())
        .ToArray();

    int[][] pages = input
        .SkipWhile(l => l != "")
        .Skip(1)
        .Select(s => s.Split(",")
            .Select(d => int.Parse(d))
            .ToArray())
        .ToArray();

    int[][] validPages = pages
        .Where(p => IsPageNumbersValid(rules, p))
        .ToArray();

    int middlePageSum = validPages
        .Select(p => GetMiddlePageNumber(p))
        .Sum();

    Console.WriteLine($"Middle page sum: {middlePageSum}");

}

void Day5p2()
{
    string[] input = File.ReadAllLines("input5");

    int[][] rules = input
        .TakeWhile(l => l != "")
        .Select(s => s.Split("|")
            .Select(d => int.Parse(d))
            .ToArray())
        .ToArray();

    int[][] pages = input
        .SkipWhile(l => l != "")
        .Skip(1)
        .Select(s => s.Split(",")
            .Select(d => int.Parse(d))
            .ToArray())
        .ToArray();

    int[][] invalidPages = pages
        .Where(p => !IsPageNumbersValid(rules, p))
        .ToArray();

    int[][] fixedInvalidPages = invalidPages
        .Select(p => FixInvalidPage(rules, p))
        .ToArray();

    int middlePageSum = fixedInvalidPages
        .Select(p => GetMiddlePageNumber(p))
        .Sum();

    // 5914 too high
    Console.WriteLine($"Middle page sum: {middlePageSum}");
}

int[] FixInvalidPage(int[][] rules, int[] pageNumbers)
{
    int[] fixedPageNumbers = new int[pageNumbers.Length];
    pageNumbers.CopyTo(fixedPageNumbers, 0);

    bool correctionMade = false;

    foreach (var rule in rules)
    {
        if (!ValidateRule(rule, pageNumbers))
        {
            int precedingNum = rule[0];
            int followingNum = rule[1];

            int precedingIndex = Array.IndexOf(pageNumbers, precedingNum);
            int followingIndex = Array.IndexOf(pageNumbers, followingNum);

            if (precedingIndex > followingIndex)
            {
                int temp = pageNumbers[precedingIndex];
                pageNumbers[precedingIndex] = pageNumbers[followingIndex];
                pageNumbers[followingIndex] = temp;
            }

            correctionMade = true;
            break;
        }
    }

    return correctionMade ? FixInvalidPage(rules, pageNumbers) : fixedPageNumbers;
}

bool IsPageNumbersValid(int[][] rules, int[] pageNumbers)
{
    foreach (var rule in rules)
    {
        if (!ValidateRule(rule, pageNumbers))
            return false;
    }
    
    return true;
}

bool ValidateRule(int[] rule, int[] pageNumbers)
{
    int precedingNum = rule[0];
    int followingNum = rule[1];

    int precedingIndex = Array.IndexOf(pageNumbers, precedingNum);
    int followingIndex = Array.IndexOf(pageNumbers, followingNum);

    if (precedingIndex == -1 || followingIndex == -1)
        return true;

    if (precedingIndex > followingIndex)
        return false;

    return true;
} 

int GetMiddlePageNumber(int[] pageNumbers)
{
    int middleIndex = pageNumbers.Length / 2;
    int middlePage = pageNumbers[middleIndex];

    return middlePage;
}

int FindWordCountFromPos(string wordToFind, string[] input, int x, int y)
{
    int count = 0;
    string reverseWord = new string(wordToFind.Reverse().ToArray());

    if (FindWordFromPosInDir(wordToFind, input, x, y, 0, 1))
        count++;

    if (FindWordFromPosInDir(reverseWord, input, x, y, 0, 1))
        count++;

    if (FindWordFromPosInDir(wordToFind, input, x, y, 1, 0))
        count++;

    if (FindWordFromPosInDir(reverseWord, input, x, y, 1, 0))
        count++;

    if (FindWordFromPosInDir(wordToFind, input, x, y, 1, 1))
        count++;

    if (FindWordFromPosInDir(reverseWord, input, x, y, 1, 1))
        count++;

    if (FindWordFromPosInDir(wordToFind, input, x, y, 1, -1))
        count++;

    if (FindWordFromPosInDir(reverseWord, input, x, y, 1, -1))
        count++;

    return count;
}

bool FindWordFromPosInDir(string wordToFind, string[] input, int startX, int startY, int dirX, int dirY)
{
    int offset = 0;
    foreach(var c in wordToFind)
    {
        int posX = startX + dirX * offset;
        int posY = startY + dirY * offset;

        if (posX < 0 || posY < 0 
            || input.Length <= posY 
            || input[1].Length <= posX 
            || input[posY][posX] != c) 
            return false;

        offset++;
    }

    return true;
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

void Day4Pt2() 
{
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

void Day2()
{
    string[] lines = File.ReadAllLines("input2");

    var reports = lines
      .Select(l => l.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray()
    ).ToArray();

    var safeReports = new List<int[]>();

    foreach (var report in reports)
    {
        if (IsReportSafe(report))
            safeReports.Add(report);

    }

    Console.WriteLine($"Safe reports: {safeReports.Count}");
}

bool IsReportSafe(int[] report)
{
    bool isAscending = true;
    for (int i = 0; i < report.Length - 1; i++)
    {
        if (report[i] >= report[i + 1])
        {
            isAscending = false;
            break;
        }
    }

    bool isDescending = true;
    for (int i = 0; i < report.Length - 1; i++)
    {
        if (report[i] <= report[i + 1])
        {
            isDescending = false;
            break;
        }
    }

    if (!isAscending && !isDescending)
        return false;

    // Not safe if difference between two adjacent numbers is less than 1 or more than 3
    for (int i = 0; i < report.Length - 1; i++)
    {
        int diff = Math.Abs(report[i] - report[i + 1]);
        if (diff > 3 || diff < 1)
        {
            return false;
        }
    }

    return true;
}

void Day2P2()
{
    string[] lines = File.ReadAllLines("input2");

    var reports = lines
      .Select(l => l.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray()
    ).ToArray();

    var safeReports = new List<int[]>();

    foreach (var reportBase in reports)
    {
        bool isSafeReportFound = false;
        for (int ri = 0; ri < reportBase.Length; ri++)
        {
            var dampenedReport = new int[reportBase.Length-1];

            for (int i = 0; i < reportBase.Length; i++)
            {
                if (i < ri)
                {
                    dampenedReport[i] = reportBase[i];
                }
                else if (i > ri)
                {
                    dampenedReport[i - 1] = reportBase[i];
                }
            }

            if (IsReportSafe(dampenedReport))
            {
                safeReports.Add(dampenedReport);
                break;
            }
        }
      }

    Console.WriteLine($"Safe reports: {safeReports.Count}");
}
