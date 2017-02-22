package com.util;

import java.awt.AWTException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;

import org.sikuli.script.Match;

public class AdvProcessor 
{
	private String subImage;
	private String mainImage;
	public AdvProcessor(String subImage, String mainImage) {
		this.subImage = subImage;
		this.mainImage = mainImage;
	}
	
	public void MatchAndClick() throws IOException, AWTException
	{
		Match match = getMatch();
		Mouse.clickRect(match);
	}
	
	public Match getMatch() throws IOException
	{
		AdvImageProcessor advImgProc = new AdvImageProcessor();
		
		ArrayList<String> imgFiles = advImgProc.createAllSizes(this.subImage);
		
		ArrayList<Match> matchList = advImgProc.getAllMatches(this.mainImage ,imgFiles);
		
		if(matchList == null)
		{
			return null;
		}
		if(matchList.size() == 1)
		{
			return matchList.get(0);
		}
		if(matchList.size() > 1)
		{
			Match match = get_max_similarity(matchList);
			return match;
		}
		return null;		
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

	private ArrayList<Distance> find_min_three(ArrayList<Distance> distance_matrix) 
	{
		Collections.sort(distance_matrix, new Comparator<Distance>() {

			@Override
			public int compare(Distance p1, Distance p2) {
				// TODO Auto-generated method stub
				return p1.dist - p2.dist;
			}

	    });
		return distance_matrix;
	}

	private ArrayList<Distance> getDistanceMatrix(ArrayList<Match> matchList) 
	{
		ArrayList<Distance> distance_matrix = new ArrayList<Distance>();
		
		for (Match match : matchList) {
			int x = match.x;
			int y = match.y;
			int w = match.w;
			int h = match.h;
			
			int mX = x+w/2;
			int mY = y+h/2;
			
			for (Match subMatch : matchList)
			{
				x = subMatch.x;
				y = subMatch.y;
				w = subMatch.w;
				h = subMatch.h;
				
				int sX = x+w/2;
				int sY = y+h/2;
				
				if(subMatch != match)
				{
					int distance = getDistance(mX, mY, sX, sY);
					Distance dist = new Distance();
					dist.from = match;
					dist.to = subMatch;
					dist.dist = distance;
					
					distance_matrix.add(dist);
				}
			}
		}
		return distance_matrix;
	}

	private int getDistance(int mX, int mY, int sX, int sY) 
	{
		int sq = (mX-sX)*(mX-sX) + (mY - mY)*(mY - mY);
		int sqrt = (int) Math.sqrt(sq);
		
		return sqrt;
	}
}
