public abstract class Day {
    int Number { get; set; }
    public string[] Input { get; }

    public Day(int number)
    {
        Number = number;
        // Load input
        Input = System.IO.File.ReadAllLines("input" + Number);
    }

    public abstract object Part1();
    public abstract object Part2();
}