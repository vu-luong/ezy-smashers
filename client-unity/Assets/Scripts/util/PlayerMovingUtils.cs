using UnityEngine;

public class PlayerMovingUtils
{
	// todo vu: Convert these fields to configs
	private static float velocity = 6f;
	private static float desiredRotationSpeed = 0.2f;

	public static PlayerStateModel GetPlayerStateOfNextFrame(
		PlayerInputModel playerInput,
		PlayerStateModel currentPlayerState)
	{
		bool upInput = playerInput.KeyInputs[0];
		bool leftInput = playerInput.KeyInputs[1];
		bool downInput = playerInput.KeyInputs[2];
		bool rightInput = playerInput.KeyInputs[3];

		// Calculate the rotation based on inputs
		var movement = InputUtils.ComputeMovementFromInput(upInput, leftInput, downInput, rightInput);

		var desiredMoveDirection = Vector3.forward * movement.z + Vector3.right * movement.x;

		// Rotate transform without sending info to the server
		var currentRotation = currentPlayerState.Rotation;
		var currentPosition = currentPlayerState.Position;
		var nextRotation = Quaternion.Slerp(currentRotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);

		// Move transform
		var moveVector = desiredMoveDirection * Time.fixedDeltaTime * velocity;
		var nextPosition = currentPosition + moveVector;

		return new PlayerStateModel(nextPosition, nextRotation);
	}
}
