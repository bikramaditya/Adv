package com.adv.controller;

import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/qa")
public class QAController {
	@RequestMapping(value = "/{name}", method = RequestMethod.GET)
	public String hello1(@PathVariable String name) {
		String result = "id=" + name;
		return result;
	}
}
