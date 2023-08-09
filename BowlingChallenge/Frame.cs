using System.Collections.ObjectModel;

namespace BowlingChallenge;

/// <summary>
/// Class <c>Frame</c> models a frame in a blowing game.
/// </summary>
public class Frame
{
    public bool IsComplete { get; private set; }
    public FrameType FrameType { get; private set; } // type of the frame, can be open, spare, or strike
    public ReadOnlyCollection<int> FrameRolls => _frameRolls.AsReadOnly();
    private readonly List<int> _frameRolls = new(); // rolls of the frame
    private readonly bool _isLastFrame;
    

    
    /// <summary>
    /// Constructor of <c>Frame</c> class.
    /// </summary>
    /// <param name="isLastFrame"> Parameter <c>isLastFrame</c> indicates if the current frame is the last frame of a game. </param>
    public Frame(bool isLastFrame)
    {
        _isLastFrame = isLastFrame;
    }

    /// <summary>
    /// Add a roll to <c>FrameRolls</c>, update <c>FrameType</c> and <c>IsComplete</c> if necessary.
    /// </summary>
    /// <param name="pins"> Integer parameter <c>pins</c> indicates the number of pins knocked down in the roll to register. </param>
    public void RegisterRollToFrame(int pins)
    {
        if (IsComplete)
            throw new ArgumentException("Invalid roll: cannot register a roll to a complete frame.");
        if (pins is < 0 or > 10)
            throw new ArgumentOutOfRangeException(nameof(pins),
                "Invalid roll: pins knocked down must be between 0 and 10.");

        switch (_frameRolls.Count)
        {
            case 0:
                AddFirstRollAndUpdateFrameType(pins);
                if (!_isLastFrame && pins == 10)
                    IsComplete = true;
                break;
            case 1:
                AddSecondRollAndUpdateFrameType(pins);
                if (!_isLastFrame || FrameType == FrameType.Open)
                    IsComplete = true;
                break;
            case 2:
                AddThirdRollAndUpdateFrameType(pins);
                IsComplete = true;
                break;
        }
    }

    private void AddFirstRollAndUpdateFrameType(int pins)
    {
        _frameRolls.Add(pins);
        if (pins == 10) FrameType = FrameType.Strike;
    }

    private void AddSecondRollAndUpdateFrameType(int pins)
    {
        if (FrameType != FrameType.Strike)
            ValidatePinsSum(_frameRolls[0], pins);

        _frameRolls.Add(pins);

        if (_frameRolls[0] + pins < 10)
            FrameType = FrameType.Open;
        else
            FrameType = _frameRolls[0] == 10 ? FrameType.Strike : FrameType.Spare;
    }

    private void AddThirdRollAndUpdateFrameType(int pins)
    {
        if (FrameType == FrameType.Strike && _frameRolls[1] < 10)
            ValidatePinsSum(_frameRolls[1], pins);
        _frameRolls.Add(pins);
    }

    private void ValidatePinsSum(int prevPins, int pins)
    {
        if (prevPins + pins is < 0 or > 10)
            throw new ArgumentException("Invalid roll: cannot knock down more than 10 pins if not strike",
                nameof(pins));
    }
}