public class PlayerInputModel
{
	// 0 = UP, 1 = LEFT, 2 = DOWN, 3 = RIGHT, 4 = SPACE
	public bool[] KeyInputs { get; }
	public int Time { get; }

	public PlayerInputModel(bool[] keyInputs, int time)
	{
		KeyInputs = keyInputs;
		Time = time;
	}
}
