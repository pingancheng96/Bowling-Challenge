namespace BowlingChallengeUnitTests;

using BowlingChallenge;

[TestClass]
public class FrameTest
{
    private readonly Frame _nonLastFrame = new(false);
    private readonly Frame _lastFrame = new(true);

    // ---------- Part 1: Tests for a non-last frame
    // The next 2 tests test for RegisterRollToFrame with a roll of pins < 0 and > 10
    [TestMethod]
    public void RegisterRollToFrame_NonLastFrameRollNeg1_ThrowsOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _nonLastFrame.RegisterRollToFrame(-1));
    }

    [TestMethod]
    public void RegisterRollToFrame_NonLastFrameRoll11_ThrowsArgumentOutOfRange()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _nonLastFrame.RegisterRollToFrame(11));
    }

    // The next test tests for RegisterRollToFrame with 0 <= pins <= 10
    [TestMethod]
    public void RegisterRollToFrame_NonLastFrameRoll4_GetsIncompleteFrameWith4()
    {
        _nonLastFrame.RegisterRollToFrame(4);

        Assert.AreEqual(false, _nonLastFrame.IsComplete);
        CollectionAssert.AreEqual(new List<int> { 4 }, _nonLastFrame.FrameRolls);
    }

    // The next 4 tests test for RegisterRollToFrame with rolls that give a strike, spare, and open non-last frame.
    [TestMethod] // strike non-last frame
    public void RegisterRollToFrame_NonLastFrameRoll10_GetsCompleteStrikeFrameWith10()
    {
        _nonLastFrame.RegisterRollToFrame(10);

        Assert.AreEqual(true, _nonLastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _nonLastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10 }, _nonLastFrame.FrameRolls);
    }

    [TestMethod] // spare non-last frame, 2nd roll != 10
    public void RegisterRollToFrame_NonLastFrameRoll4And6_GetsCompleteSpareFrameWith4And6()
    {
        RegisterTwoRollsToFrame(_nonLastFrame, 4, 6);

        Assert.AreEqual(true, _nonLastFrame.IsComplete);
        Assert.AreEqual(FrameType.Spare, _nonLastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 4, 6 }, _nonLastFrame.FrameRolls);
    }

    [TestMethod] // spare non-last frame, 2nd roll = 10
    public void RegisterRollToFrame_NonLastFrameRoll0And10_GetsCompleteSpareFrameWith0And10()
    {
        RegisterTwoRollsToFrame(_nonLastFrame, 0, 10);

        Assert.AreEqual(true, _nonLastFrame.IsComplete);
        Assert.AreEqual(FrameType.Spare, _nonLastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 0, 10 }, _nonLastFrame.FrameRolls);
    }

    [TestMethod] // open non-last frame
    public void RegisterRollToFrame_NonLastFrameRoll4And5_GetsCompleteOpenFrameWith9()
    {
        RegisterTwoRollsToFrame(_nonLastFrame, 4, 5);

        Assert.AreEqual(true, _nonLastFrame.IsComplete);
        Assert.AreEqual(FrameType.Open, _nonLastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 4, 5 }, _nonLastFrame.FrameRolls);
    }

    // The next 2 tests test for RegisterRollToFrame with an invalid 2rd roll to a non-last frame.
    [TestMethod] // non-last frame roll 1 + roll 2 > 10 illegal
    public void RegisterRollToFrame_NonLastFrameRoll4And7_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterTwoRollsToFrame(_nonLastFrame, 4, 7));
    }

    [TestMethod] // strike non-last frame no 2nd roll
    public void RegisterRollToFrame_NonLastFrameRoll10And7_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterTwoRollsToFrame(_nonLastFrame, 10, 7));
    }

    // the next 2 tests test for RegisterRollToFrame with an invalid 3rd roll to a non-last frame
    [TestMethod] // spare non-last frame no 3rd roll
    public void RegisterRollToFrame_NonLastFrameRoll1And9And7_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterThreeRollsToFrame(_nonLastFrame, 1, 9, 7));
    }

    [TestMethod] // open non-last frame no 3rd roll
    public void RegisterRollToFrame_NonLastFrameRoll2And3And8_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterThreeRollsToFrame(_nonLastFrame, 2, 3, 8));
    }

    // ---------- Part 2: Tests for a last frame
    // The next test tests for RegisterRollToFrame with a roll of 0 <= pins <= 10 to a last frame
    [TestMethod]
    public void RegisterRollToFrame_LastFrameRoll5_GetsIncompleteFrameWith5()
    {
        _lastFrame.RegisterRollToFrame(5);

        Assert.AreEqual(false, _lastFrame.IsComplete);
        CollectionAssert.AreEqual(new List<int> { 5 }, _lastFrame.FrameRolls);
    }

    // The next 3 tests test for RegisterRollToFrame with rolls that give a strike, spare, and open last frame
    [TestMethod] // strike last frame
    public void RegisterRollToFrame_LastFrameRoll10_GetsIncompleteStrikeFrameWith10()
    {
        _lastFrame.RegisterRollToFrame(10);

        Assert.AreEqual(false, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10 }, _lastFrame.FrameRolls);
    }

    [TestMethod] // spare last frame
    public void RegisterRollToFrame_LastFrameRoll2And8_GetsIncompleteSpareFrameWith2And8()
    {
        RegisterTwoRollsToFrame(_lastFrame, 2, 8);

        Assert.AreEqual(false, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Spare, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 2, 8 }, _lastFrame.FrameRolls);
    }

    [TestMethod] // open last frame
    public void RegisterRollToFrame_LastFrameRoll2And7_GetsCompleteOpenFrameWith2And7()
    {
        RegisterTwoRollsToFrame(_lastFrame, 2, 7);

        Assert.AreEqual(true, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Open, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 2, 7 }, _lastFrame.FrameRolls);
    }

    // The next 2 tests test for RegisterRollToFrame with a 2nd roll to a strike last frame
    [TestMethod] // strike last frame, 1st bonus != 10
    public void RegisterRollToFrame_LastFrameRoll10And7_GetsIncompleteStrikeFrameWith10And7()
    {
        RegisterTwoRollsToFrame(_lastFrame, 10, 7);

        Assert.AreEqual(false, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10, 7 }, _lastFrame.FrameRolls);
    }

    [TestMethod] // strike last frame, 1st bonus = 10
    public void RegisterRollToFrame_LastFrameRoll10And10_GetsIncompleteStrikeFrameWith10And10()
    {
        RegisterTwoRollsToFrame(_lastFrame, 10, 10);

        Assert.AreEqual(false, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10, 10 }, _lastFrame.FrameRolls);
    }

    // The next 3 tests test for RegisterRollToFrame with a 3rd roll to a strike last frame
    [TestMethod] // strike last frame, 0 <= 1st, 2nd bonus < 10
    public void RegisterRollToFrame_LastFrameRoll10And3And2_GetsCompleteStrikeFrameWith10And3And2()
    {
        RegisterThreeRollsToFrame(_lastFrame, 10, 3, 2);

        Assert.AreEqual(true, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10, 3, 2 }, _lastFrame.FrameRolls);
    }

    [TestMethod] // strike last frame, 1st bonus = 10 and 0 <= 2nd bonus < 10
    public void RegisterRollToFrame_LastFrameRoll10And10And6_GetsCompleteStrikeFrameWith10And10And6()
    {
        RegisterThreeRollsToFrame(_lastFrame, 10, 10, 6);

        Assert.AreEqual(true, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10, 10, 6 }, _lastFrame.FrameRolls);
    }

    [TestMethod] // strike last frame, 1st bonus = 2nd bonus = 10
    public void RegisterRollToFrame_LastFrameRoll10And10And10_GetsCompleteStrikeFrameWith10And10And10()
    {
        RegisterThreeRollsToFrame(_lastFrame, 10, 10, 10);

        Assert.AreEqual(true, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Strike, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 10, 10, 10 }, _lastFrame.FrameRolls);
    }

    // The next test tests for registering a 3rd roll to a spare last frame
    [TestMethod]
    public void RegisterRollToFrame_LastFrameRoll3And7And6_GetsIncompleteSpareFrameWith3And7And6()
    {
        RegisterThreeRollsToFrame(_lastFrame, 3, 7, 6);

        Assert.AreEqual(true, _lastFrame.IsComplete);
        Assert.AreEqual(FrameType.Spare, _lastFrame.FrameType);
        CollectionAssert.AreEqual(new List<int> { 3, 7, 6 }, _lastFrame.FrameRolls);
    }

    // The next 4 tests test for registering invalid 2nd, 3rd, and 4th rolls
    [TestMethod] // open last frame, 1st roll + 2nd roll > 10 illegal
    public void RegisterRollToFrame_LastFrameRoll8And6_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterTwoRollsToFrame(_lastFrame, 8, 6));
    }

    [TestMethod] // strike last frame 2nd roll != 10, 2nd roll + 3rd roll > 10 illegal
    public void RegisterRollToFrame_LastFrameRoll10And7And8_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterThreeRollsToFrame(_lastFrame, 10, 7, 8));
    }

    [TestMethod] // open last frame no 3rd roll
    public void RegisterRollToFrame_LastFrameRoll4And1And7_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => RegisterThreeRollsToFrame(_lastFrame, 4, 1, 7));
    }

    [TestMethod] // no 4rd roll
    public void RegisterRollToFrame_LastFrameRoll5And5And6And7_ThrowsArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            RegisterThreeRollsToFrame(_lastFrame, 5, 5, 6);
            _lastFrame.RegisterRollToFrame(7);
        });
    }

    // 2 helper functions
    private void RegisterTwoRollsToFrame(Frame frame, int pins1, int pins2)
    {
        frame.RegisterRollToFrame(pins1);
        frame.RegisterRollToFrame(pins2);
    }

    private void RegisterThreeRollsToFrame(Frame frame, int pins1, int pins2, int pins3)
    {
        frame.RegisterRollToFrame(pins1);
        frame.RegisterRollToFrame(pins2);
        frame.RegisterRollToFrame(pins3);
    }
}