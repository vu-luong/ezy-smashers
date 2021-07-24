package com.vuluong.plugin.service;

import com.vuluong.common.entity.User;

import java.util.List;

public interface UserService {
	
	void saveUser(User user);
	
	User createUser(String username, String password);
	
	User getUser(String username);
	
	List<User> getAllUsers();
}

