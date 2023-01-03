public class RoomRepository
{
	private static readonly RoomRepository INSTANCE = new();
	private long playingRoomId;
	
	public static RoomRepository GetInstance()
	{
		return INSTANCE;
	}

	public void UpdatePlayingRoomId(long roomId)
	{
		playingRoomId = roomId;
	}

	public long GetPlayingRoomId()
	{
		return playingRoomId;
	}
}
