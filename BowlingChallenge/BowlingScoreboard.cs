namespace BowlingChallenge;

/// <summary>
/// Class <c>BowlingScoreboard</c> models a scoreboard for a ten-pin bowling game.
/// </summary>
public class BowlingScoreboard
{
    private const int MaxFrames = 10; // max number of frames in a game, in this case 10
    private List<Frame> _frames = new(); // record all frames in a game
    private List<int> _cmlFrameScores = new(); // record cumulative frame scores

    public BowlingScoreboard()
    {
        // argument of Frame constructor indicates if it is the last frame
        _frames.Add(new Frame(_frames.Count == MaxFrames - 1));
    }

    /// <summary>    
    /// This method registers a roll to the scoreboard and updates the cumulative score list if needed.
    /// </summary>
    /// <param name="pins"> The number of pins knocked down of the roll. </param>
    /// <exception cref="ArgumentException"> Thrown when registering a roll to an already complete game. </exception>
    public void RegisterRollToBoard(int pins)
    {
        if (_frames.Count == MaxFrames && _frames.Last().IsComplete)
            throw new ArgumentException($"Invalid roll: at most {MaxFrames} frames per game.");

        _frames.Last().RegisterRollToFrame(pins);

        if (_frames.Count < MaxFrames && _frames.Last().IsComplete)
            _frames.Add(new Frame(_frames.Count == MaxFrames - 1));

        UpdateCmlFrameScores(); // update cumulative frame score list
    }

    /// <summary>
    /// This method returns the current total score.
    /// It is the sum of the scores of frames whose scores have been determined
    /// and the pins knocked down in the remaining frames.
    /// </summary>
    public int GetTotalScore()
    {
        // total score of frames whose scores have been determined
        int totalScore = _cmlFrameScores.Count == 0 ? 0 : _cmlFrameScores.Last();

        // pins knocked down in frames whose scores have not been determined yet
        for (int i = _cmlFrameScores.Count; i < _frames.Count; ++i)
            totalScore += _frames[i].FrameRolls.Sum();

        return totalScore;
    }

    public void ClearScoreboard()
    {
        _frames = new();
        _cmlFrameScores = new();
        _frames.Add(new Frame(_frames.Count() == MaxFrames - 1));
    }

    /// <summary>
    /// This method overrides the <c>ToString()</c> method for displaying the scoreboard.
    /// </summary>
    /// <returns> A string representation of the scoreboard. </returns>
    public override string ToString()
    {
        string displayString = "";

        for (int i = 0; i < _frames.Count; ++i)
        {
            Frame curFrame = _frames[i];
            int? displayScore = i < _cmlFrameScores.Count ? _cmlFrameScores[i] : null;
            switch (curFrame.FrameRolls.Count)
            {
                case 1:
                    displayString += string.Format("Frame: {0, 3}\t Result: {1, 3}\t\t Cumulative Frame Score: {2, 3}\n",
                        i + 1, GetRollDisplayString(curFrame, 0), displayScore);
                    break;
                case 2:
                    displayString += string.Format("Frame: {0, 3}\t Result: {1, 3} {2, 3}\t Cumulative Frame Score: {3, 3}\n",
                        i + 1, GetRollDisplayString(curFrame, 0), GetRollDisplayString(curFrame, 1), displayScore);
                    break;
                case 3:
                    displayString += string.Format("Frame: {0, 3}\t Result: {1, 3} {2, 3} {3, 3}\t Cumulative Frame Score: {4, 3}\n",
                        i + 1, GetRollDisplayString(curFrame, 0), GetRollDisplayString(curFrame, 1), GetRollDisplayString(curFrame, 2), displayScore);
                    break;
            }
        }

        return displayString;
    }

    private string GetRollDisplayString(Frame frame, int i)
    {
        string rollDisplayString = frame.FrameRolls[i].ToString();
        
        if (frame.FrameRolls[i] == 10) rollDisplayString = "X";
        if (i > 0 && frame.FrameRolls[i-1] < 10 && frame.FrameRolls[i] + frame.FrameRolls[i-1] == 10)
            rollDisplayString = "/";
        
        return rollDisplayString;
    }

    private void UpdateCmlFrameScores()
    {
        int frameToEvalIdx = _cmlFrameScores.Count; // index of the frame to evaluate score
        Frame frameToEval = _frames[frameToEvalIdx];
        int curSum = _cmlFrameScores.Count == 0 ? 0 : _cmlFrameScores.Last();

        // if frame not complete, do not need to update score
        if (!frameToEval.IsComplete) return;

        // if open frame or the last frame, frame score is the sum of pins knocked down in that frame
        if (frameToEval.FrameType == FrameType.Open || frameToEvalIdx == MaxFrames - 1)
        {
            _cmlFrameScores.Add(curSum + _frames[frameToEvalIdx].FrameRolls.Sum());
            return; // in both cases no need to recurse
        }

        // handle non-last strike/spare frames
        int? frameScore = null;
        switch (frameToEval.FrameType)
        {
            case FrameType.Spare:
                frameScore = GetNonLastSpareFrameScore(frameToEvalIdx);
                break;
            case FrameType.Strike:
                frameScore = GetNonLastStrikeFrameScore(frameToEvalIdx);
                break;
        }
        if (frameScore == null) return; // bonus rolls not thrown yet, just return
        _cmlFrameScores.Add(curSum + (int)frameScore);

        // cascading update if there are frames left
        if (_cmlFrameScores.Count < _frames.Count)
            UpdateCmlFrameScores();
    }

    private int? GetNonLastSpareFrameScore(int frameIdx)
    {
        // next frame is available and has at least 1 roll registered
        if (frameIdx + 1 < _frames.Count && _frames[frameIdx + 1].FrameRolls.Count >= 1)
            return 10 + _frames[frameIdx + 1].FrameRolls[0];

        return null;
    }

    private int? GetNonLastStrikeFrameScore(int frameIdx)
    {
        // next frame is available and has at least 2 rolls registered
        if (frameIdx + 1 < _frames.Count && _frames[frameIdx + 1].FrameRolls.Count >= 2)
            return 10 + _frames[frameIdx + 1].FrameRolls.Take(2).Sum();

        // the frame after the next is available and has at least 1 roll registered
        if (frameIdx + 2 < _frames.Count && _frames[frameIdx + 2].FrameRolls.Count >= 1)
            return 20 + _frames[frameIdx + 2].FrameRolls[0];

        return null;
    }
}