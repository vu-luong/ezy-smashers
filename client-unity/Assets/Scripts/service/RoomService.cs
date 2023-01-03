public class RoomService
{
	private static readonly RoomService INSTANCE = new();
	private long playingRoomId;
	
	public static RoomService GetInstance()
	{
		return INSTANCE;
	}

	public void SetPlayingRoomId(long roomId)
	{
		playingRoomId = roomId;
	}

	public long GetPlayingRoomId()
	{
		return playingRoomId;
	}
}
