public class Day7 {

  public string[] Input;
  public Equation[] Equations;

  public Day7()
  {
    // Load input
    Input = System.IO.File.ReadAllLines("input7");

    Equations = Input
      .Select(x => new Equation(x))
      .ToArray();
  }

  public void Run() 
  {
    // Part 1
    System.Console.WriteLine("Part 1: " + Part1());

    // Part 2
    System.Console.WriteLine("Part 2: " + Part2());

  }

  public long Part1()
  {
    long answer = Equations
      .Where(x => x.TestValidity())
      .Sum(e => e.TestValue);

    return answer;
  }

  public long Part2()
  {
    long answer = Equations
      .Where(x => x.TestValidityP2())
      .Sum(e => e.TestValue);

    return answer;  
  }

  public class Equation 
  {
    public long TestValue { get; set; }
    public int[] Values { get; }

    public Equation(string line)
    {
      string[] parts1 = line.Split(": ");

      TestValue = long.Parse(parts1[0]);

      string[] parts2 = parts1[1].Split(" ");

      Values = parts2
        .Select(x => int.Parse(x))
        .ToArray();
    }

    public bool TestValidity()
    {
      int operatorCount = Values.Length - 1;

      // Try all combinations of operators (0 = add, 1 = multiply)
      for (int i = 0; i < Math.Pow(2, operatorCount); i++)
      {
        long value = Values[0];
        for (int j = 0; j < operatorCount; j++)
        {
          int operand = Values[j + 1];

          // Get j'th digit of i as a binary number, this will determine the operator to use
          int op = i / (int)Math.Pow(2, j) % 2;

          if (op == 0)
          {
            value += operand;
          }
          else
          {
            value *= operand;
          }
        }

        if (value == TestValue)
        {
          return true;
        }
      }
      
      return false;
    }
  

    public bool TestValidityP2()
    {
      int operatorCount = Values.Length - 1;

      // Try all combinations of operators: 0 = add, 1 = multiply, 2 = concatenate
      for (int i = 0; i < Math.Pow(3, operatorCount); i++)
      {
        long value = Values[0];

        for (int j = 0; j < operatorCount; j++)
        {
          long operand = Values[j + 1];

          // Get j'th digit of i as a ternary number, this will determine the operator to use
          int op = i / (int)Math.Pow(3, j) % 3;

          switch (op)
          {
            case 0:
              value += operand;
              break;
            case 1:
              value *= operand;
              break;
            case 2:
              value = long.Parse($"{value}{operand}");
              break;
            default:
              throw new Exception("Invalid operator");
          }
          
        }

        if (value == TestValue)
        {
          return true;
        }
      }

      return false;
    }
  }
}