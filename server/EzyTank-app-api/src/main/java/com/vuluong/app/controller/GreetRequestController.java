package com.vuluong.app.controller;

import com.vuluong.app.constant.Commands;
import com.vuluong.app.constant.Errors;
import com.vuluong.app.request.GoRequest;
import com.vuluong.app.request.HelloRequest;
import com.vuluong.app.response.HelloResponse;
import com.vuluong.app.service.GreetingService;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.core.exception.EzyBadRequestException;
import com.tvd12.ezyfox.io.EzyStrings;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;

@EzyRequestController
public class GreetRequestController {

	@EzyAutoBind
	private EzyResponseFactory responseFactory;
	
	@EzyAutoBind
	private GreetingService greetingService;
	
	@EzyDoHandle(Commands.HELLO)
	public void sayHello(EzyUser user, HelloRequest request) {
		if(EzyStrings.isNoContent(request.getNickName())) {
			throw new EzyBadRequestException(Errors.UNKNOWN, "invalid nick name");
		}
		responseFactory.newObjectResponse()
			.command(Commands.HELLO)
			.data(new HelloResponse(greetingService.hello(request.getNickName())))
			.user(user)
			.execute();
	}
	
	@EzyDoHandle(Commands.GO)
	public void sayGo(EzyUser user, GoRequest request) {
		if(EzyStrings.isNoContent(request.getNickName())) {
			throw new EzyBadRequestException(Errors.UNKNOWN, "invalid nick name");
		}
		responseFactory.newObjectResponse()
		.command(Commands.GO)
		.data(new HelloResponse(greetingService.go(request.getNickName())))
		.user(user)
		.execute();
	}
}
