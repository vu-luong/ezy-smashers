package com.vuluong.common.repo;

import com.tvd12.ezydata.mongodb.EzyMongoRepository;
import com.tvd12.ezyfox.database.annotation.EzyRepository;
import com.vuluong.common.entity.User;

@EzyRepository("userRepo")
public interface UserRepo extends EzyMongoRepository<Long, User> {
}

