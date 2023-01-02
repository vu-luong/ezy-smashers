public class ReconciliationInfo
{
	public int TimeTick { get; }
	public PlayerStateModel PlayerState { get; }
	public PlayerInputData InputData { get; }
	
	public ReconciliationInfo(int timeTick, PlayerStateModel playerState, PlayerInputData inputData)
	{
		TimeTick = timeTick;
		PlayerState = playerState;
		InputData = inputData;
	}
}
