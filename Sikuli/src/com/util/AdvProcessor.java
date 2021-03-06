package com.util;

import java.awt.AWTException;
import java.io.IOException;
import java.util.ArrayList;
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
}
