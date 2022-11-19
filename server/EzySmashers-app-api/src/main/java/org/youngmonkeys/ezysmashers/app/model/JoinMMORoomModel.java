package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
public class JoinMMORoomModel {
    private long roomId;
    private String playerName;
}
