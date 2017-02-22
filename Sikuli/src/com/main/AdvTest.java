package com.main;

import java.awt.AWTException;
import java.awt.Rectangle;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import org.sikuli.script.Screen;

import com.util.AdvProcessor;
import com.util.ImageProcessor;
import com.util.Mouse;

public class AdvTest 
{
	public static Map<String, Rectangle> imgSourceDest = new HashMap<String, Rectangle>();
	public static Screen s = new Screen();
	public static void main(String[] args) throws InterruptedException, IOException 
	{
		String mainImage = "C:\\Users\\bipadh\\Capture.png";
		AdvProcessor advProc = new AdvProcessor("imgs/" + "sales_main.PNG", mainImage);
		try {
			advProc.MatchAndClick();
		} catch (AWTException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		mainImage = "C:\\Users\\bipadh\\Capture1.png";
		advProc = new AdvProcessor("imgs/" + "accounts.PNG", mainImage);
		try {
			advProc.MatchAndClick();
		} catch (AWTException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
//		click("imgs/" + "account_form.PNG");
//		click("imgs/" + "sales_main.PNG");
	}
}