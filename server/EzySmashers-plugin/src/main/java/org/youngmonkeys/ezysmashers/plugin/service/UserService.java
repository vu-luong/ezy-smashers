package org.youngmonkeys.ezysmashers.plugin.service;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import lombok.AllArgsConstructor;
import lombok.Setter;
import org.youngmonkeys.ezysmashers.common.entity.User;
import org.youngmonkeys.ezysmashers.common.repo.UserRepo;

import java.util.List;

@Setter
@AllArgsConstructor
@EzySingleton("userService")
public class UserService {

    private final UserRepo userRepo;

    public void saveUser(User user) {
        userRepo.save(user);
    }

    public User createUser(String username, String password) {
        User user = new User();
        user.setUsername(username);
        user.setPassword(password);
        userRepo.save(user);
        return user;
    }

    public User getUser(String username) {
        return userRepo.findByField("username", username);
    }

    public List<User> getAllUsers() {
        return userRepo.findAll();
    }
}
