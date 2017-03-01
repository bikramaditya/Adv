package com.adv.img.util;

import java.awt.Graphics2D;
import java.awt.GraphicsDevice;
import java.awt.GraphicsEnvironment;
import java.awt.RenderingHints;
import java.awt.Transparency;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;

import javax.imageio.ImageIO;

import org.sikuli.script.Finder;
import org.sikuli.script.Match;

public class AdvImageProcessor {

	public ArrayList<String> createAllSizes(String subImage) 
	{
		GraphicsDevice gd = GraphicsEnvironment.getLocalGraphicsEnvironment().getDefaultScreenDevice();
		
		int w = gd.getDisplayMode().getWidth();
		int h = gd.getDisplayMode().getHeight();
				
		double xRatio = (double) w / 1920;
		double yRatio = (double) h / 1080;

		return resizeLoop(10, subImage, xRatio, yRatio);	
	}
	
	private ArrayList<String> resizeLoop(int N, String imageName, double xRatio, double yRatio) 
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
				File outputfile = new File("c:/Temp/resized/"+ i+new File(imageName).getName());
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
	private BufferedImage getScaledInstance(BufferedImage img, int targetWidth, int targetHeight, Object hint, boolean higherQuality) 
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

	public ArrayList<Match> getAllMatches(String mainImage, ArrayList<String> imgFiles) throws IOException 
	{
		Finder finder = new Finder(mainImage);
		ArrayList<Match> matchList = tryAgainWithAllCombo(finder, imgFiles);
		return matchList;
	}
	
	
	private ArrayList<Match> tryAgainWithAllCombo(final Finder finder, ArrayList<String> imgFiles) 
	{
		Date startTime = new Date();
		System.out.println("Start..." + startTime);
		
		final ArrayList<Match> matchList = new ArrayList<Match>();

		class OneShotTask implements Runnable {
			String image = "";

			OneShotTask(String str) {
				image = str;
			}

			public void run() {
				System.out.println("Finding for : "+image);
				if(image==null)
				{
					System.out.println();
				}

				finder.findAll(image);				
				while(finder.hasNext())
				{
					Match m = finder.next();
					matchList.add(m);
				}
			}
		}

		ArrayList<Thread> allThreads = new ArrayList<Thread>();

		for (String image : imgFiles) {
			Thread t = new Thread(new OneShotTask(image));
			allThreads.add(t);
			t.start();
		}

		for (Thread thread : allThreads) {
			try {
				thread.join();
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
		System.out.println("End..." + new Date());

		System.out.println("Elapsed..." + (new Date().getTime() - startTime.getTime()) / 1000 + "sec. List size=" + matchList.size());

		
		return matchList;
	}
	
}
