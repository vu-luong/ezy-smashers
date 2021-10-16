public class ReconciliationInfo
{
	public ReconciliationInfo(int timeTick, PlayerStateData stateData, PlayerInputData inputData)
	{
		TimeTick = timeTick;
		StateData = stateData;
		InputData = inputData;
	}

	public int TimeTick { get; set; }
	public PlayerStateData StateData { get; set; }
	public PlayerInputData InputData { get; set; }
}
