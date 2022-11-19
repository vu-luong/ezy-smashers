package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

import java.util.List;

@Getter
@Builder
public class JoinedMMORoomModel {
    private long roomId;
    private List<String> playerNames;
}
