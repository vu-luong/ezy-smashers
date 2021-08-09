package com.youngmonkeys.app.game;

import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.SynchronizedPlayerManager;
import lombok.Getter;

public class GameRoom extends MMORoom {
    @Getter
    private MMOPlayer master;
    
    public GameRoom(Builder builder) {
        super(builder);
    }
    
    public void addPlayer(MMOPlayer player) {
        PlayerManager<MMOPlayer> playerManager = this.getPlayerManager();
        if(playerManager.containsPlayer(player)) {
            return;
        }
        synchronized (this) {
            if(playerManager.isEmpty()) {
                master = player;
            }
            playerManager.addPlayer(player);
        }
    }

    public void removePlayer(MMOPlayer player) {
        PlayerManager<MMOPlayer> playerManager = this.getPlayerManager();
        synchronized (this) {
            playerManager.removePlayer(player);
            if(master == player && !playerManager.isEmpty()) {
                master = playerManager.getPlayerByIndex(0);
            }
        }
    }

    public boolean isEmpty() {
        return this.getPlayerManager().isEmpty();
    }
    
    public static Builder builder() {
        return new Builder();
    }
    
    public static class Builder extends MMORoom.Builder {
        
        public Builder addRoomUpdatedHandler(MMORoomUpdatedHandler handler) {
            return (Builder) super.addRoomUpdatedHandler(handler);
        }
    
        public Builder distanceOfInterest(double distance) {
            return (Builder) super.distanceOfInterest(distance);
        }
    
        public Builder defaultPlayerManager(int maxPlayer) {
            return (Builder) super.defaultPlayerManager(maxPlayer);
        }
    
        public Builder playerManager(PlayerManager playerManager) {
            return (Builder) super.playerManager(playerManager);
        }
    
        @Override
        protected GameRoom newProduct() {
            return (GameRoom) super.newProduct();
        }
    
        @Override
        public GameRoom build() {
            return (GameRoom) super.build();
        }
    }
}
