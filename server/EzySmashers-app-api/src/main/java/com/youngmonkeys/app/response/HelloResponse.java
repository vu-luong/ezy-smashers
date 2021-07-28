package com.youngmonkeys.app.response;


import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
@EzyObjectBinding
public class HelloResponse {

	private String message; 
	
}
