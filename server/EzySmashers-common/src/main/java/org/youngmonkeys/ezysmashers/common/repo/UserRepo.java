package org.youngmonkeys.ezysmashers.common.repo;

import com.tvd12.ezydata.mongodb.EzyMongoRepository;
import com.tvd12.ezyfox.database.annotation.EzyRepository;
import org.youngmonkeys.ezysmashers.common.entity.User;

@EzyRepository("userRepo")
public interface UserRepo extends EzyMongoRepository<Long, User> {
}

