package org.youngmonkeys.ezysmashers.app.exception;

public class CreateRoomWhenNotInLobbyException extends RuntimeException {

    public CreateRoomWhenNotInLobbyException(String playerName) {
        super("player: " + playerName + " create room when not in lobby");
    }
}
