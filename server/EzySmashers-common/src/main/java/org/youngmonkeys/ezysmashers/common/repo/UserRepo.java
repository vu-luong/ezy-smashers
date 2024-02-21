package org.youngmonkeys.ezysmashers.common.repo;

import com.tvd12.ezydata.database.EzyDatabaseRepository;
import com.tvd12.ezyfox.database.annotation.EzyRepository;
import org.youngmonkeys.ezysmashers.common.entity.User;

@EzyRepository("userRepo")
public interface UserRepo extends EzyDatabaseRepository<Long, User> {
}

