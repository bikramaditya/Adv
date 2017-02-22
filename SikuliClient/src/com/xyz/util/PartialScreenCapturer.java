package com.xyz.util;

import java.awt.AWTException;
import java.awt.Dimension;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
 
import javax.imageio.ImageIO;

public class PartialScreenCapturer {
	 public static String capture(int x, int y) {
		 String format = "jpg";
		 String fileName = "c:/users/bipadh/desktop/PartialScreenshot." + format;
	        try {
	            Robot robot = new Robot();
	             
	            Rectangle captureRect = new Rectangle(x - 25, y - 25, 50, 50);
	            BufferedImage screenFullImage = robot.createScreenCapture(captureRect);
	            ImageIO.write(screenFullImage, format, new File(fileName));
	             
	            System.out.println("A partial screenshot saved!");
	        } catch (AWTException | IOException ex) {
	            System.err.println(ex);
	        }
			return fileName;
	    }
}
