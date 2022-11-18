package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
public class AuthorizeHitModel {
    private boolean isValidHit;
    private String playerName;
    private PlayerHitModel playerHit;
}
