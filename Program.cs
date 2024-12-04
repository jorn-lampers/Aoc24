using System;
using System.Collections.Generic;
using System.IO;


// Using linq
using System.Linq;
using System.Text.RegularExpressions;

Day3p2();

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