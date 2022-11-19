package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

import java.util.List;

@Getter
@Builder
public class MMORoomPlayerNamesModel {
    private List<String> playerNames;
    private String masterPlayerName;
}
