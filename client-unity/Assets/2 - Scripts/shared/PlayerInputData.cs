namespace _2___Scripts.shared
{
	public class PlayerInputData
	{
		public PlayerInputData(bool[] keyInputs)
		{
			KeyInputs = keyInputs;
		}

		// 0 = UP, 1 = LEFT, 2 = DOWN, 3 = RIGHT, 4 = SPACE
		public bool[] KeyInputs { get; set; }
	}
}
