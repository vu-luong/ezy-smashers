package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

import java.util.List;

@Getter
@Builder
public class StartGameModel {
    private List<String> playerNames;
    private List<PlayerSpawnModel> playerSpawns;
}
