using System;
using System.Collections.Generic;

// Using linq
using System.Linq;

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