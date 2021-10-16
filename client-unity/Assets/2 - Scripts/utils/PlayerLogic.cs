using UnityEngine;

public class PlayerLogic
{
	public static float velocity = 6f;
	public static float desiredRotationSpeed = 0.2f;

	public static PlayerStateData GetNextFrameData(PlayerInputData inputData, PlayerStateData currentStateData)
	{
		bool upInput = inputData.KeyInputs[0];
		bool leftInput = inputData.KeyInputs[1];
		bool downInput = inputData.KeyInputs[2];
		bool rightInput = inputData.KeyInputs[3];

		// Calculate the rotation based on inputs
		var movement = InputUtils.ComputeMovementFromInput(upInput, leftInput, downInput, rightInput);

		var desiredMoveDirection = Vector3.forward * movement.z + Vector3.right * movement.x;

		// Rotate transform without sending info to the server
		var currentRotation = currentStateData.Rotation;
		var currentPosition = currentStateData.Position;
		var nextRotation = Quaternion.Slerp(currentRotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);

		// Move transform and send info the the server
		var moveVector = desiredMoveDirection * Time.fixedDeltaTime * velocity;
		var nextPosition = currentPosition + moveVector;

		return new PlayerStateData(nextPosition, nextRotation);
	}

}
