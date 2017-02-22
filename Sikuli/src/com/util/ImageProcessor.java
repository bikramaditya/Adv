package com.util;

import java.awt.AWTException;
import java.awt.Dimension;
import java.awt.Graphics2D;
import java.awt.GraphicsDevice;
import java.awt.GraphicsEnvironment;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.RenderingHints;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.Transparency;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;

import javax.imageio.ImageIO;

import org.sikuli.script.Image;
import org.sikuli.script.Match;
import org.sikuli.script.Screen;
import java.io.ByteArrayOutputStream;

public class ImageProcessor 
{
	private static int originalW = 1920; 
	private static int originalH = 1080;
	public static Rectangle getMatchedPoint(String imageName ) 
	{
		String screenImagePath = getScreenImgPath();
		MatchValidator.setScreen(screenImagePath);
		Rectangle rect = MatchValidator.tryAgainWithAllCombo(imageName);
		
		return rect;
		
	}

	private static String getScreenImgPath() {
		BufferedImage originalImage = takeScreenShot(); 
	    byte[] imageInByte = null;
	    String outFile = null ;
		try {
	        outFile = System.getProperty("user.home")+"\\Capture.png";
	        ImageIO.write(originalImage, "png", new File(outFile));
	        
			ByteArrayOutputStream baos = new ByteArrayOutputStream();
			
			ImageIO.write(originalImage, "png", baos);
			baos.flush();
			imageInByte = baos.toByteArray();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return outFile;
	}

	private static BufferedImage takeScreenShot() 
	{
		BufferedImage screenFullImage = null;
		 try {
	            Robot robot = new Robot();
	            Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
	            
	            dim = new Dimension((int)dim.getWidth(),(int)(dim.getHeight()));
	            
	            Rectangle screenRect = new Rectangle(dim);
	            screenFullImage = robot.createScreenCapture(screenRect);
	        } catch (AWTException ex) {
	            System.err.println(ex);
	        }
		return screenFullImage;
	}
	private static BufferedImage getScaledInstance(BufferedImage img, int targetWidth, int targetHeight, Object hint, boolean higherQuality) 
	{
		int type = (img.getTransparency() == Transparency.OPAQUE) ? BufferedImage.TYPE_INT_RGB : BufferedImage.TYPE_INT_ARGB;
		BufferedImage ret = (BufferedImage) img;
		int w, h;
		if (higherQuality) {
			// Use multi-step technique: start with original size, then
			// scale down in multiple passes with drawImage()
			// until the target size is reached
			w = img.getWidth();
			h = img.getHeight();
		} else {
			// Use one-step technique: scale directly from original
			// size to target size with a single drawImage() call
			w = targetWidth;
			h = targetHeight;
		}

		do {
			if (higherQuality && w > targetWidth) {
				w /= 2;
				if (w < targetWidth) {
					w = targetWidth;
				}
			}

			if (higherQuality && h > targetHeight) {
				h /= 2;
				if (h < targetHeight) {
					h = targetHeight;
				}
			}

			BufferedImage tmp = new BufferedImage(w, h, type);
			Graphics2D g2 = tmp.createGraphics();
			g2.setRenderingHint(RenderingHints.KEY_RENDERING, hint);
			g2.drawImage(ret, 0, 0, w, h, null);
			g2.dispose();

			ret = tmp;
		} while (w != targetWidth || h != targetHeight);

		return ret;
	}

	public static ArrayList<String> createAllSizes(String imageName) 
	{
		GraphicsDevice gd = GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice();
		
		int w = gd.getDisplayMode().getWidth();
		int h = gd.getDisplayMode().getHeight();
				
		double xRatio = (double) w / originalW;
		double yRatio = (double) h / originalH;

		return resizeLoop(10, imageName, xRatio, yRatio);	
	}

	private static ArrayList<String> resizeLoop(int N, String imageName, double xRatio, double yRatio) 
	{
		BufferedImage img = null;
		ArrayList<String> all_files = new ArrayList<String>();
		for(int i = (int) (-N*(1.5)) ; i < N * (1.5) ; i++)
		{
			try 
			{
			    img = ImageIO.read(new File(imageName));
			    	
			    int targetWidth = (int) (img.getWidth() * yRatio * (1+(double)i/(double)(3*N)));
				int targetHeight = (int) (img.getHeight() * yRatio * (1+(double)i/(double)(3*N)));
				BufferedImage scaledImg = getScaledInstance(img, targetWidth, targetHeight, RenderingHints.VALUE_RENDER_QUALITY, false);
				File outputfile = new File("resized/"+ i+new File(imageName).getName());
				ImageIO.write(scaledImg, "png", outputfile);
				all_files.add(outputfile.getPath()); 
			} 
			catch (IOException e) 
			{
				e.printStackTrace();
			}
		}
		return all_files;
	}
}
