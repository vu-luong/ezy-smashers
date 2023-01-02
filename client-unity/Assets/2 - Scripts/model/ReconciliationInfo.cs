public class ReconciliationInfo
{
	public int TimeTick { get; }
	public PlayerStateModel PlayerState { get; }
	public PlayerInputModel PlayerInput { get; }
	
	public ReconciliationInfo(int timeTick, PlayerStateModel playerState, PlayerInputModel playerInput)
	{
		TimeTick = timeTick;
		PlayerState = playerState;
		PlayerInput = playerInput;
	}
}
