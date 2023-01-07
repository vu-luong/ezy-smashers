using UnityEngine;

public class InputUtils
{
	public static Vector3 ComputeMovementFromInput(bool upInput, bool leftInput, bool downInput, bool rightInput)
	{
		Vector3 answer = new Vector3();
		if (upInput)
		{
			answer += Vector3.forward;
		}
		if (leftInput)
		{
			answer += Vector3.left;
		}
		if (downInput)
		{
			answer += Vector3.back;
		}
		if (rightInput)
		{
			answer += Vector3.right;
		}
		return answer;
	}
}
