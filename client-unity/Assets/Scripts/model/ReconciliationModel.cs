public class ReconciliationModel
{
	public int TimeTick { get; }
	public PlayerStateModel PlayerState { get; }
	public PlayerInputModel PlayerInput { get; }
	
	public ReconciliationModel(int timeTick, PlayerStateModel playerState, PlayerInputModel playerInput)
	{
		TimeTick = timeTick;
		PlayerState = playerState;
		PlayerInput = playerInput;
	}
}
