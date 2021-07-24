package com.vuluong.plugin.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.vuluong.common.entity.User;
import com.vuluong.common.repo.UserRepo;
import com.vuluong.plugin.service.MaxIdService;
import com.vuluong.plugin.service.UserService;
import lombok.Setter;

import java.util.List;

@Setter
@EzySingleton("userService")
public class UserServiceImpl implements UserService {
	
	@EzyAutoBind
	private UserRepo userRepo;
	
	@EzyAutoBind
	private MaxIdService maxIdService;
	
	@Override
	public void saveUser(User user) {
		userRepo.save(user);
	}
	
	@Override
	public User createUser(String username, String password) {
		User user = new User();
		user.setId(maxIdService.incrementAndGet("user"));
		user.setUsername(username);
		user.setPassword(password);
		userRepo.save(user);
		return user;
	}
	
	@Override
	public User getUser(String username) {
		return userRepo.findByField("username", username);
	}
	
	@Override
	public List<User> getAllUsers() {
		return userRepo.findAll();
	}
}
