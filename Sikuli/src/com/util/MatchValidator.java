package com.util;

import java.awt.Point;
import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;

import javax.imageio.ImageIO;

import org.sikuli.script.FindFailed;
import org.sikuli.script.Finder;
import org.sikuli.script.Match;
import org.sikuli.script.Region;
import org.sikuli.script.Screen;

public class MatchValidator {
	private static Finder finder = null;
	private static BufferedImage deskTop = null;
	static ArrayList<Match> matchList = null;
	static Region r = null;
	public static void setScreen(String screenImagePath) 
	{		
		try {
			deskTop = ImageIO.read(new File(screenImagePath));
			r = new Region(0, 0, deskTop.getWidth(), deskTop.getHeight());
			finder = new Finder(screenImagePath);
			//finder.findAll("");
		} catch (IOException e) {
		}
	}
	
	static Rectangle tryAgainWithAllCombo(String imageName) {
		matchList = new ArrayList<Match>();
		Date startTime = new Date();
		System.out.println("Start..." + startTime);
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
				//System.out.println("image="+image);
				finder.findAll(image);				
				while(finder.hasNext())
				{
					Match m = finder.next();
					matchList.add(m);
				}
				//matchList.add(image);
				//System.out.println("Matched->" + matchedImage);
			}
		}

		ArrayList<String> imgFiles = ImageProcessor.createAllSizes(imageName);
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

		// Collections.sort(matchList);
		//
		// if(matchList.size() == 0)
		// {
		// return null;
		// }
		// else
		// {
		// int size = matchList.size();
		// double mean = (double)size/(double)2;
		// int index = (int) Math.ceil(mean) - 1;
		// return matchList.get(index).getImageFilename();
		// }
		if(matchList.size() == 0)
		{
			return null;
		}
		else if(matchList.size() == 1)
		{
			Match match = matchList.get(0);
			Rectangle rect = new Rectangle(match.x, match.y, match.w, match.h);
			return rect;
		}
		else if(matchList.size() > 1)
		{
			Match match = getNearestMatch();
			Rectangle rect = new Rectangle(match.x, match.y, match.w, match.h);
			return rect;
		}
		return null;
	}

	private static Match getNearestMatch() 
	{
		Point centroid = centroid();
		Match m = nearestPoint(centroid);
		return m;
	}

	private static Match nearestPoint(Point centroid) 
	{
		double minDistance = 1000000;
		Match finalMatch = null;
		for (Match match : matchList) {
			int x = match.x;
			int y = match.y;
			double xDiff = (centroid.x - x);
			double yDiff = (centroid.y - y);
			
			double disctSqrd = Math.pow(xDiff, 2) + Math.pow(yDiff, 2);
			double dist = Math.sqrt(disctSqrd);
			if(dist < minDistance)
			{
				minDistance = dist;
				finalMatch = match;
			}
		}
		return finalMatch;
	}

	private static Point centroid() {
		double centroidX = 0, centroidY = 0;

		for (Match m : matchList) {
			centroidX += m.getX();
			centroidY += m.getY();
		}
		return new Point((int) centroidX / matchList.size(), (int) centroidY / matchList.size());
	}
}