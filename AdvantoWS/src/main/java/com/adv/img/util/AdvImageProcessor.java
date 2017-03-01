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
import java.util.List;

import javax.imageio.ImageIO;

import org.sikuli.script.Finder;
import org.sikuli.script.Match;
import org.sikuli.script.Pattern;

import com.adv.img.util.ClosestPair.Pair;
import com.adv.img.util.ClosestPair.Point;

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

	public Point getAllMatches(String mainImage, ArrayList<String> imgFiles) throws IOException 
	{
		ArrayList<Match> matchList = tryAgainWithAllCombo(mainImage, imgFiles);
		
		
		if(matchList == null || matchList.size()==0) return null;
		else if(matchList.size()==1)
			{
				Match match = matchList.get(0);
				return new Point(match.x, match.y, match.w, match.h);
			}
		else
		{
			print(matchList);
			//Match bestMatch = get_max_similarity(matchList);
			List<Point> pointsList = new ArrayList<Point>();
			for (Match match : matchList) {
				pointsList.add(new Point(match.x,match.y, match.w, match.h));
			}
			
			Pair dqClosestPair = ClosestPair.divideAndConquer(pointsList);
			
			//Point p = pointFromSmallestCircle(pointsList);
			
			return dqClosestPair.point1;
		}
	}
	
	
	
	private Point pointFromSmallestCircle(List<Point> pointsList) 
	{
		int radius = Integer.MAX_VALUE;
		
		// TODO Auto-generated method stub
		return null;
	}

	private void print(ArrayList<Match> matchList) 
	{
		for (Match match : matchList) {
			System.out.println(match.x+","+match.y);
		}
	}

	private Match get_max_similarity(ArrayList<Match> matchList) 
	{
		double similarity = 0.0;
		Match fMatch = null;
		for (Match match : matchList) {
			if(match.getScore() > similarity)
			{
				similarity = match.getScore();
				fMatch = match;
			}
		}
		
		return fMatch;
	}
	private Match getBestMatch(ArrayList<Match> matchList) 
	{
		double minDist = Double.MAX_VALUE;
		Match m1 = null;
		Match m2 = null;
		
		for (int i = 0 ; i < matchList.size() ; i++) {
			for (int j = i; j < matchList.size() ; j++) {				
				Match match1 = matchList.get(i);
				Match match2 = matchList.get(j);
				if(match1 != match2)
				{
					double dist = getDist(match1, match2);
					if(dist < minDist)
					{
						m1 = match1;
						m2 = match2;
					}
				}
			}
		}
		m1.x = (m1.x+m2.x)/2;
		m1.y = (m1.y+m2.y)/2;
		m1.w = (m1.w+m2.w)/2;
		m1.h = (m1.h+m2.h)/2;
		return m1;
	}

	private double getDist(Match match1, Match match2) 
	{
		try
		{
			double sqr = (match1.x-match2.x)*(match1.x-match2.x) + (match1.y-match2.y)*(match1.y-match2.y);
			return Math.sqrt(sqr);
		}
		catch(Exception e)
		{
			return Double.MAX_VALUE;
		}
	}

	private ArrayList<Match> tryAgainWithAllCombo(String mainImage, ArrayList<String> imgFiles) 
	{
		Date startTime = new Date();
		System.out.println("Start..." + startTime);
		
		final ArrayList<Match> matchList = new ArrayList<Match>();

		class OneShotTask implements Runnable {
			String image = "";
			Finder localFinder;
			String localMain;
			OneShotTask(String str, String mainImage) {
				image = str;
				localMain = mainImage;
				try {
					localFinder = new Finder(localMain);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}				
			}

			public void run() {
				System.out.println("Finding for : "+image);
				if(image==null)
				{
					System.out.println();
				}

				localFinder.find(new Pattern(image).similar((float) 0.85));	//Pattern("captured.png").similar(0.95)			
				while(localFinder.hasNext())
				{
					Match m = localFinder.next();
					matchList.add(m);
				}
				localFinder.destroy();
			}
		}

		ArrayList<Thread> allThreads = new ArrayList<Thread>();

		for (String image : imgFiles) {
			Thread t = new Thread(new OneShotTask(image, mainImage));
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
