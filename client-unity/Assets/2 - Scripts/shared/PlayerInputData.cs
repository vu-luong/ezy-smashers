public class PlayerInputData
{

	public PlayerInputData(bool[] keyInputs, int time)
	{
		KeyInputs = keyInputs;
		Time = time;
	}

	// 0 = UP, 1 = LEFT, 2 = DOWN, 3 = RIGHT, 4 = SPACE
	public bool[] KeyInputs { get; set; }

	public int Time { get; set; }
}
