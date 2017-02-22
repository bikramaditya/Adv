package com.main;

import java.awt.Rectangle;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import org.sikuli.script.Screen;

import com.util.ImageProcessor;
import com.util.Mouse;

public class TestSikuli 
{
	public static Map<String, Rectangle> imgSourceDest = new HashMap<String, Rectangle>();
	public static Screen s = new Screen();
	public static void main(String[] args) throws InterruptedException, IOException {
		//click("imgs/" + "sales_main.PNG");
		click("imgs/" + "accounts.PNG");
		click("imgs/" + "account_form.PNG");
		click("imgs/" + "sales_main.PNG");
	}

	private static void click(String srcName) {
		try {
			Rectangle rect = null;
			Thread.sleep(3000);
			if(imgSourceDest.containsKey(srcName))
			{
				rect = imgSourceDest.get(srcName); 
			}
			else
			{
				rect = ImageProcessor.getMatchedPoint(srcName);
				imgSourceDest.put(srcName, rect);
			}
			Mouse.clickRect(rect);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}