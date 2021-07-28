package com.youngmonkeys.app.config;

import com.tvd12.ezyfox.bean.annotation.EzyPropertiesBean;

import lombok.Data;

@Data
@EzyPropertiesBean(prefix = "application")
public class AppConfig {
	
	private String helloPrefix;
	private String goPrefix;
}

