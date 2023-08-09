using BowlingChallenge;

BowlingScoreboard bowlingScoreboard = new BowlingScoreboard();

// example game
Console.WriteLine("Example Game:");
List<int> exampleRolls = new() { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
foreach (int roll in exampleRolls)
    bowlingScoreboard.RegisterRollToBoard(roll);
Console.Write(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

bowlingScoreboard.ClearScoreboard();

// gutter game
Console.WriteLine("Gutter Game:");
RegisterRepeatedRolls(0, 20);
Console.Write(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

bowlingScoreboard.ClearScoreboard();

// perfect game
Console.WriteLine("Perfect Game:");
RegisterRepeatedRolls(10, 12);
Console.Write(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

void RegisterRepeatedRolls(int pins, int rolls)
{
    for (int i = 0; i < rolls; ++i)
        bowlingScoreboard.RegisterRollToBoard(pins);
}
