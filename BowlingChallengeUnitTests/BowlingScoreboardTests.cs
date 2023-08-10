namespace BowlingChallengeUnitTests;

using BowlingChallenge;

[TestClass]
public class BowlingScoreboardTest
{
    private readonly BowlingScoreboard _bowlingScoreboard = new();
    private readonly List<int> _exampleRolls = new() { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };

    // ---------- Part 1: Tests for RegisterRollToBoard
    // The next 2 tests test for RegisterRollToBoard with a roll of pins < 0 and > 10.
    [TestMethod]
    public void RegisterRollToBoard_RollNeg1_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _bowlingScoreboard.RegisterRollToBoard(-1));
    }

    [TestMethod]
    public void RegisterRollToBoard_Roll11_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _bowlingScoreboard.RegisterRollToBoard(11));
    }

    // The next test tests for RegisterRollToBoard with a roll of 0 <= pins <= 10.
    [TestMethod]
    public void RegisterRollToBoard_Roll5_ThrowsNoException()
    {
        try
        {
            _bowlingScoreboard.RegisterRollToBoard(5);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    // The next test tests for RegisterRollToBoard with two rolls of sum > 10.
    [TestMethod]
    public void RegisterRollToBoard_Roll7And4_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterTwoRollsToBoard(7, 4)); // sum 7 + 4 > 10
    }

    // The next test tests for RegisterRollToBoard with two rolls of 0 <= sum <= 10.
    [TestMethod]
    public void RegisterRollToBoard_Roll5And4_ThrowsNoException()
    {
        try
        {
            RegisterTwoRollsToBoard(5, 4);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    // The next 5 tests test for RegisterRollToBoard with rolls
    // that give a game of open, spare, strike, and mixed frames.
    [TestMethod]
    public void RegisterRollToBoard_AllGutterFrames_ThrowsNoException()
    {
        try
        {
            RegisterGutterFramesToBoard(10);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [TestMethod]
    public void RegisterRollToBoard_AllOneFrames_ThrowsNoException()
    {
        try
        {
            RegisterRepeatedRollsToBoard(1, 20); // 10 frames of 1's
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [TestMethod]
    public void RegisterRollToBoard_AllSpareFrames_ThrowsNoException()
    {
        try
        {
            RegisterRepeatedRollsToBoard(5, 21); // 10 frames of 5's and a bonus roll of 5
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [TestMethod]
    public void RegisterRollToBoard_AllStrikeFrames_ThrowsNoException()
    {
        try
        {
            RegisterRepeatedRollsToBoard(10, 12); // 10 frames of 10's and 2 bonus rolls of 10's
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [TestMethod]
    public void RegisterRollToBoard_ExampleGameOfMixedFrames_ThrowsNoException()
    {
        try
        {
            foreach (int roll in _exampleRolls) // a normal game of mixed open, spare, and open frames
                _bowlingScoreboard.RegisterRollToBoard(roll);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    // The next test tests for RegisterRollToBoard of a 1 bonus roll to the last spare frame.
    [TestMethod] // spare last frame
    public void RegisterRollToBoard_Rolls0And10And6After9GutterFrames_ThrowsNoException()
    {
        RegisterGutterFramesToBoard(9);
        RegisterTwoRollsToBoard(0, 10); // last frame spare

        try
        {
            _bowlingScoreboard.RegisterRollToBoard(6); // okay
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    // The next 2 tests test for RegisterRollToBoard of 2 bonus rolls to the last strike frame.
    [TestMethod] // strike last frame, 1st bonus = 10
    public void RegisterRollToBoard_Roll10And10And7After9GutterFrames_ThrowsNoException()
    {
        RegisterGutterFramesToBoard(9);
        RegisterTwoRollsToBoard(10, 10); // last frame starts with two 10's

        try
        {
            _bowlingScoreboard.RegisterRollToBoard(7); // okay
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [TestMethod] // strike last frame 1st bonus != 10, 1st bonus + 2nd bonus > 10 illegal
    public void RegisterRollToBoard_Roll10And5And6After9GutterFrames_ThrowsArgumentException()
    {
        RegisterGutterFramesToBoard(9);
        RegisterTwoRollsToBoard(10, 5); // last frame strike

        Assert.ThrowsException<ArgumentException>(() => _bowlingScoreboard.RegisterRollToBoard(6));
    }

    // The next 4 tests test for RegisterRollToBoard of attempting to register a roll to a complete game.
    [TestMethod] // 2 rolls per open frame => 20 rolls
    public void RegisterRollToBoard_21stRollToAllOpenFrames_ThrowsArgumentException()
    {

        Assert.ThrowsException<ArgumentException>(() => RegisterRepeatedRollsToBoard(1, 21));
    }

    [TestMethod] // 2 rolls per spare frame + 1 bonus roll last frame => 21 rolls 
    public void RegisterRollToBoard_22ndRollToAllSpareFrames_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterRepeatedRollsToBoard(5, 22));
    }

    [TestMethod] // 1 roll per strike frame + 2 bonus rolls last frame => 12 rolls
    public void RegisterRollToBoard_13thRollToAllStrikeFrames_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterRepeatedRollsToBoard(10, 13));
    }

    [TestMethod]
    public void RegisterRollToBoard_ExtraRollToExampleGame_ThrowsArgumentException()
    {
        foreach (int roll in _exampleRolls)  // example game
            _bowlingScoreboard.RegisterRollToBoard(roll);
        Assert.ThrowsException<ArgumentException>(() => _bowlingScoreboard.RegisterRollToBoard(1));
    }

    // ---------- Part 2: tests for GetTotalScore
    // The next test tests for GetTotalScore before a game starts.
    [TestMethod]
    public void GetTotalScore_NoRoll_Returns0()
    {
        Assert.AreEqual(0, _bowlingScoreboard.GetTotalScore());
    }

    // The next 3 tests test for GetTotalScore in the middle of the game when a frame score cannot be determined
    [TestMethod] // non-strike frame with one roll of 1
    public void GetTotalScore_Roll1After5GutterFrames_Returns1()
    {
        RegisterGutterFramesToBoard(5);
        _bowlingScoreboard.RegisterRollToBoard(1);

        Assert.AreEqual(1, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // spare non-last frame, bonus unknown
    public void GetTotalScore_Roll2And8After5GutterFrames_Returns10()
    {
        RegisterGutterFramesToBoard(5);
        RegisterTwoRollsToBoard(2, 8); // spare frame

        Assert.AreEqual(10, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // strike non-last frame, bonus unknown
    public void GetTotalScore_Roll10After5GutterFrames_Returns10()
    {
        RegisterGutterFramesToBoard(5);
        _bowlingScoreboard.RegisterRollToBoard(10); // strike frame

        Assert.AreEqual(10, _bowlingScoreboard.GetTotalScore());
    }

    // The next 3 tests test for GetTotalScore in the middle of the game when a frame score can be determined.
    [TestMethod] // open non-last frame of 1 and 4
    public void GetTotalScore_Roll1And4After5GutterFrames_Returns5()
    {
        RegisterGutterFramesToBoard(5);
        RegisterTwoRollsToBoard(1, 4); // open frame

        Assert.AreEqual(5, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // spare non-last frame, bonus 2
    public void GetTotalScore_Roll6And4And2After5GutterFrames_Returns14()
    {
        RegisterGutterFramesToBoard(5);
        RegisterThreeRollsToBoard(6, 4, 2); // spare frame (score 12) + open frame (score 2)

        Assert.AreEqual(14, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // strike non-last frame, bonus 0 and 5
    public void GetTotalScore_Roll10And0And5After5GutterFrames_Returns20()
    {
        RegisterGutterFramesToBoard(5);
        RegisterThreeRollsToBoard(10, 0, 5); // strike frame (score 15) + open frame (score 5)

        Assert.AreEqual(20, _bowlingScoreboard.GetTotalScore());
    }

    // The next 5 tests test for GetTotalScore of a complete game ending with an open, spare, and strike last frame.
    [TestMethod] // open last frame of 1 and 1
    public void GetTotalScore_Roll1And1After9GutterFrames_Returns2()
    {
        RegisterGutterFramesToBoard(9);
        RegisterRepeatedRollsToBoard(1, 2); // open last frame

        Assert.AreEqual(2, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // spare last frame, bonus 5
    public void GetTotalScore_Roll5And5And5After9GutterFrames_Returns15()
    {
        RegisterGutterFramesToBoard(9);
        RegisterRepeatedRollsToBoard(5, 3); // spare last frame

        Assert.AreEqual(15, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // strike last frame, bonus 5 and 5
    public void GetTotalScore_Roll10And5And5After9GutterFrames_Returns20()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 5, 5); // strike last frame

        Assert.AreEqual(20, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // strike last frame, bonus 10 and 5
    public void GetTotalScore_Roll10And10And5After9GutterFrames_Returns25()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 10, 5); // strike last frame

        Assert.AreEqual(25, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod] // strike last frame, bonus 10 and 10
    public void GetTotalScore_Roll10And10And10After9GutterFrames_Returns30()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 10, 10); // strike last frame

        Assert.AreEqual(30, _bowlingScoreboard.GetTotalScore());
    }

    // The next 3 tests test for GetTotalScore of a gutter, perfect, and the example game.
    [TestMethod]
    public void GetTotalScore_GutterGame_Returns0()
    {
        RegisterGutterFramesToBoard(10);
        Assert.AreEqual(0, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod]
    public void GetTotalScore_PerfectGame_Returns300()
    {
        RegisterRepeatedRollsToBoard(10, 12); // 10 frames of 10's and 2 bonus rolls of 10 pins each
        Assert.AreEqual(300, _bowlingScoreboard.GetTotalScore());
    }

    [TestMethod]
    public void GetTotalScore_ExampleGame_Returns133()
    {
        foreach (int roll in _exampleRolls) // example game
            _bowlingScoreboard.RegisterRollToBoard(roll);

        Assert.AreEqual(133, _bowlingScoreboard.GetTotalScore());
    }

    // ---------- Past 3: Tests for ToString
    // The next test tests for ToString an empty game.
    [TestMethod]
    public void ToString_NoRoll_ReturnsEmptyString()
    {
        Assert.AreEqual("", _bowlingScoreboard.ToString());
    }

    // The next 4 tests test for ToString in the middle of the game when a frame score cannot be determined.
    [TestMethod] // non-last frame with one roll of 1
    public void ToString_Roll1After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        _bowlingScoreboard.RegisterRollToBoard(1); // a single 1 in 5th frame

        string expectedStr = GetGutterFramesString(4); // string of 4 gutter frames
        expectedStr += $"Frame: {5,3}\t Result: {1,3}\t\t Cumulative Frame Score: {"",3}\n"; // string of a single 1 in 5th frame

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare non-last frame, bonus unknown 
    public void ToString_Roll1And9After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterTwoRollsToBoard(1, 9); // spare 5th frame of 1 and 9

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {1,3} {'/',3}\t Cumulative Frame Score: {"",3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare non-last frame, 2nd roll = 10, bonus unknown 
    public void ToString_Roll0And10After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterTwoRollsToBoard(0, 10); // spare 5th frame of 0 and 10

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {0,3} {'/',3}\t Cumulative Frame Score: {"",3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike non-last frame, bonus unknown 
    public void ToString_Roll10After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        _bowlingScoreboard.RegisterRollToBoard(10); // a strike 5th frame

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {"X",3}\t\t Cumulative Frame Score: {"",3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    // The next 4 tests test for ToString in the middle of the game when a frame score can be determined.
    [TestMethod] // open frame of 1 and 4
    public void ToString_Roll1And4After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterTwoRollsToBoard(1, 4);

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {1,3} {4,3}\t Cumulative Frame Score: {5,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare non-last frame, bonus 5
    public void ToString_Roll2And8And5After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterThreeRollsToBoard(2, 8, 5);

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {2,3} {'/',3}\t Cumulative Frame Score: {15,3}\n";
        expectedStr += $"Frame: {6,3}\t Result: {5,3}\t\t Cumulative Frame Score: {"",3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare non-last frame, 2nd roll = 10, bonus 6
    public void ToString_Roll0And10And6After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterThreeRollsToBoard(0, 10, 6);

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {0,3} {'/',3}\t Cumulative Frame Score: {16,3}\n";
        expectedStr += $"Frame: {6,3}\t Result: {6,3}\t\t Cumulative Frame Score: {"",3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike non-last frame, bonus 0 and 9
    public void ToString_Roll10And0And9After4GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(4);
        RegisterThreeRollsToBoard(10, 0, 9);

        string expectedStr = GetGutterFramesString(4);
        expectedStr += $"Frame: {5,3}\t Result: {'X',3}\t\t Cumulative Frame Score: {19,3}\n";
        expectedStr += $"Frame: {6,3}\t Result: {0,3} {9,3}\t Cumulative Frame Score: {28,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    // The next test tests for ToString of a complete game ending with an open last frame
    [TestMethod] // open last frame of 3 and 4
    public void ToString_Roll3And4After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterTwoRollsToBoard(3, 4);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {3,3} {4,3}\t Cumulative Frame Score: {7,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    // The next 3 tests test for ToString of a complete gaming ending with a spare last frame
    [TestMethod] // spare last frame, 2nd roll = 10, bonus 0
    public void ToString_Roll0And10And0After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(0, 10, 0);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {0,3} {'/',3} {0,3}\t Cumulative Frame Score: {10,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare last frame, bonus 6
    public void ToString_Roll3And7And6After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(3, 7, 6);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {3,3} {'/',3} {6,3}\t Cumulative Frame Score: {16,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // spare last frame, 2nd roll = 10, bonus 10
    public void ToString_Roll0And10And10After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(0, 10, 10);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {0,3} {'/',3} {'X',3}\t Cumulative Frame Score: {20,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    // The next 5 tests test for ToString of a complete game ending with a strike last frame
    [TestMethod] // strike last frame, bonus 0 and 0
    public void ToString_Roll10And0And0After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 0, 0);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {'X',3} {0,3} {0,3}\t Cumulative Frame Score: {10,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike last frame, bonus 10 and 0
    public void ToString_Roll10And10And0After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 10, 0);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {'X',3} {'X',3} {0,3}\t Cumulative Frame Score: {20,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike last frame, bonus 0 and 10
    public void ToString_Roll10And0And10After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 0, 10);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {'X',3} {0,3} {'/',3}\t Cumulative Frame Score: {20,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike last frame, bonus 10 and 10
    public void ToString_Roll10And10And10After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 10, 10);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {'X',3} {'X',3} {'X',3}\t Cumulative Frame Score: {30,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    [TestMethod] // strike last frame, bonus 5 and 5
    public void ToString_Roll10And5And5After9GutterFrames_ReturnsCorrespondingGameString()
    {
        RegisterGutterFramesToBoard(9);
        RegisterThreeRollsToBoard(10, 5, 5);

        string expectedStr = GetGutterFramesString(9);
        expectedStr += $"Frame: {10,3}\t Result: {'X',3} {5,3} {'/',3}\t Cumulative Frame Score: {20,3}\n";

        Assert.AreEqual(expectedStr, _bowlingScoreboard.ToString());
    }

    // The next 3 tests test for ToString for a gutter game, a perfect game, and the example game.
    [TestMethod]
    public void ToString_GutterGame_ReturnsGutterGameString()
    {
        RegisterGutterFramesToBoard(10);
        string gutterGameString = GetGutterFramesString(10);

        Assert.AreEqual(gutterGameString, _bowlingScoreboard.ToString());
    }

    [TestMethod]
    public void ToString_PerfectGame_ReturnsPerfectGameString()
    {
        RegisterRepeatedRollsToBoard(10, 12);

        string perfectGameString = "";
        for (int i = 0; i < 9; ++i)
            perfectGameString += $"Frame: {i + 1,3}\t Result: {"X",3}\t\t Cumulative Frame Score: {30 * (i + 1),3}\n";
        perfectGameString += $"Frame: {10,3}\t Result: {'X',3} {'X',3} {'X',3}\t Cumulative Frame Score: {300,3}\n";

        Assert.AreEqual(perfectGameString, _bowlingScoreboard.ToString());
    }

    [TestMethod]
    public void ToString_ExampleGame_ReturnExampleGameString()
    {
        foreach (int roll in _exampleRolls)
            _bowlingScoreboard.RegisterRollToBoard(roll);

        string exampleGameString = "";
        exampleGameString += $"Frame: {1,3}\t Result: {1,3} {4,3}\t Cumulative Frame Score: {5,3}\n";
        exampleGameString += $"Frame: {2,3}\t Result: {4,3} {5,3}\t Cumulative Frame Score: {14,3}\n";
        exampleGameString += $"Frame: {3,3}\t Result: {6,3} {'/',3}\t Cumulative Frame Score: {29,3}\n";
        exampleGameString += $"Frame: {4,3}\t Result: {5,3} {'/',3}\t Cumulative Frame Score: {49,3}\n";
        exampleGameString += $"Frame: {5,3}\t Result: {'X',3}\t\t Cumulative Frame Score: {60,3}\n";
        exampleGameString += $"Frame: {6,3}\t Result: {0,3} {1,3}\t Cumulative Frame Score: {61,3}\n";
        exampleGameString += $"Frame: {7,3}\t Result: {7,3} {'/',3}\t Cumulative Frame Score: {77,3}\n";
        exampleGameString += $"Frame: {8,3}\t Result: {6,3} {'/',3}\t Cumulative Frame Score: {97,3}\n";
        exampleGameString += $"Frame: {9,3}\t Result: {'X',3}\t\t Cumulative Frame Score: {117,3}\n";
        exampleGameString += $"Frame: {10,3}\t Result: {2,3} {'/',3} {6,3}\t Cumulative Frame Score: {133,3}\n";

        Assert.AreEqual(exampleGameString, _bowlingScoreboard.ToString());
    }

    // a couple of helper functions
    private void RegisterRepeatedRollsToBoard(int pins, int rolls)
    {
        for (int i = 0; i < rolls; ++i)
            _bowlingScoreboard.RegisterRollToBoard(pins);
    }

    private void RegisterGutterFramesToBoard(int frames)
    {
        RegisterRepeatedRollsToBoard(0, frames * 2);
    }

    private void RegisterTwoRollsToBoard(int pins1, int pins2)
    {
        _bowlingScoreboard.RegisterRollToBoard(pins1);
        _bowlingScoreboard.RegisterRollToBoard(pins2);
    }

    private void RegisterThreeRollsToBoard(int pins1, int pins2, int pins3)
    {
        _bowlingScoreboard.RegisterRollToBoard(pins1);
        _bowlingScoreboard.RegisterRollToBoard(pins2);
        _bowlingScoreboard.RegisterRollToBoard(pins3);
    }

    private string GetGutterFramesString(int frames)
    {
        string displayString = "";
        for (int i = 0; i < frames; ++i)
            displayString += $"Frame: {i + 1,3}\t Result: {0,3} {0,3}\t Cumulative Frame Score: {0,3}\n";
        return displayString;
    }
}