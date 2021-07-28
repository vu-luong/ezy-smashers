package com.youngmonkeys.plugin.service;


import com.youngmonkeys.common.entity.User;

import java.util.List;

public interface UserService {
	
	void saveUser(User user);
	
	User createUser(String username, String password);
	
	User getUser(String username);
	
	List<User> getAllUsers();
}

