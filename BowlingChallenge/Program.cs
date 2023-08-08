using BowlingChallenge;

BowlingScoreboard bowlingScoreboard = new BowlingScoreboard();

// example game
List<int> exampleRolls = new() { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
foreach (var t in exampleRolls)
    bowlingScoreboard.RegisterRollToBoard(t);
Console.WriteLine(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

bowlingScoreboard.ClearScoreboard();

// gutter game
RegisterRepeatedRolls(0, 20);
Console.WriteLine(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

bowlingScoreboard.ClearScoreboard();

// perfect game
RegisterRepeatedRolls(10, 12);
Console.WriteLine(bowlingScoreboard.ToString());
Console.WriteLine($"Total Score: {bowlingScoreboard.GetTotalScore()}\n");

void RegisterRepeatedRolls(int pins, int rolls)
{
    for (int i = 0; i < rolls; ++i)
        bowlingScoreboard.RegisterRollToBoard(pins);
}