package org.youngmonkeys.ezysmashers.app.exception;

import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.Room;

public class JoinNonWaitingRoomException extends RuntimeException {

    public JoinNonWaitingRoomException(String playerName, Room room) {
        super(
            "player: " + playerName + " join room with status " + room.getStatus() + " instead of "
                + RoomStatus.WAITING);
    }
}
