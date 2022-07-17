package org.youngmonkeys.ezysmashers.common.entity;

import com.tvd12.ezyfox.annotation.EzyId;
import com.tvd12.ezyfox.database.annotation.EzyCollection;
import lombok.Data;

@Data
@EzyCollection
public class User {
	@EzyId
	Long id;
	
	String username;
	String password;
}

