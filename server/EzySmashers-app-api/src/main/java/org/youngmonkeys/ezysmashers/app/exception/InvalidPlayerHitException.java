package org.youngmonkeys.ezysmashers.app.exception;

public class InvalidPlayerHitException extends RuntimeException {

    public InvalidPlayerHitException(String playerName) {
        super("player: " + playerName + " send invalid hit");
    }
}
