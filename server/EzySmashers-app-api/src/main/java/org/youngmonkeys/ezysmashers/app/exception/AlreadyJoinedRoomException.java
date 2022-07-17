package org.youngmonkeys.ezysmashers.app.exception;

import com.tvd12.gamebox.entity.Room;

public class AlreadyJoinedRoomException extends RuntimeException {
	
	public AlreadyJoinedRoomException(String playerName, Room room) {
		super("player: " + playerName + " already joined room " + room.getName());
	}
	
}
