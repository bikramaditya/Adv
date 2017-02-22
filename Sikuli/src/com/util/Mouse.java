package com.util;

import java.awt.AWTException;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.event.InputEvent;

import org.sikuli.script.Match;

public class Mouse {

	public static void moveMouse(int x, int y) throws AWTException {
		Point orig = MouseInfo.getPointerInfo().getLocation();

		double xdiff = x - orig.x;
		double ydiff = y - orig.y;
		int maxStep = 100;

		for (int i = 1; i <= maxStep; i++) {
			double xtemp = (i) * xdiff / maxStep;
			double ytemp = (i) * ydiff / maxStep;
			Robot robot = new Robot();
			robot.mouseMove(orig.x + (int) xtemp, orig.y + (int) ytemp);
			robot.delay(5);
		}

	}

	public static void click() throws AWTException {
		Robot robot = new Robot();
		robot.delay(100);
		robot.delay(300);
		robot.mousePress(InputEvent.BUTTON1_MASK);
		robot.delay(200);
		robot.mouseRelease(InputEvent.BUTTON1_MASK);

	}

	public static void clickRect(Rectangle rect) throws AWTException 
	{
		int x = rect.x;
		int y = rect.y;
		int w = rect.width;
		int h = rect.height;
		x = x + w/2;
		y = y + h/2;
		moveMouse(x,y);
		click();
	}

	public static void clickRect(Match match) throws AWTException {
		int x = match.x;
		int y = match.y;
		int w = match.w;
		int h = match.h;
		x = x + w/2;
		y = y + h/2;
		moveMouse(x,y);
		click();
	}
}
