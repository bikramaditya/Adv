package com.util;

import java.awt.AWTException;
import java.awt.GraphicsDevice;
import java.awt.GraphicsEnvironment;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.StringSelection;
import java.awt.event.KeyEvent;

public class KeyBoard {

	public static void type(String textToType) throws AWTException, InterruptedException {
		StringSelection stringSelection = new StringSelection(textToType);
		Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();
		clipboard.setContents(stringSelection, stringSelection);

		Robot robot = new Robot();
		robot.keyPress(KeyEvent.VK_CONTROL);
		Thread.sleep(50);
		robot.keyPress(KeyEvent.VK_V);
		Thread.sleep(50);
		robot.keyRelease(KeyEvent.VK_V);
		Thread.sleep(50);
		robot.keyRelease(KeyEvent.VK_CONTROL);
	}

	public static void pressEnter() throws AWTException, InterruptedException {
		Robot robot;
		robot = new Robot();
		robot.keyPress(KeyEvent.VK_ENTER); // press Enter
		Thread.sleep(10);
		robot.keyRelease(KeyEvent.VK_ENTER); // release Enter
	}

	public static void switchFocus() {
		try {
			GraphicsEnvironment ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
			GraphicsDevice defaultScreen = ge.getDefaultScreenDevice();
			Rectangle rect = defaultScreen.getDefaultConfiguration().getBounds();

			Mouse.moveMouse((int) rect.getMaxX() / 2, 15);
			Mouse.click();
		} catch (Exception e) {
		}
	}

	private static void pressEscape() throws AWTException, InterruptedException {
		Robot robot = new Robot();
		Toolkit.getDefaultToolkit().setLockingKeyState(KeyEvent.VK_NUM_LOCK, false);

		robot.keyPress(KeyEvent.VK_ESCAPE);
		Thread.sleep(500);
	}
}
