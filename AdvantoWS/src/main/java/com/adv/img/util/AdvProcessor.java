package com.adv.img.util;

import java.io.IOException;
import java.util.ArrayList;
import org.sikuli.script.Match;

import com.adv.img.util.ClosestPair.Point;

public class AdvProcessor 
{
	private String subImage;
	private String mainImage;
	public AdvProcessor(String subImage, String mainImage) {
		this.subImage = subImage;
		this.mainImage = mainImage;
	}
	
	public Point getMatch() throws IOException
	{
		AdvImageProcessor advImgProc = new AdvImageProcessor();
		
		ArrayList<String> imgFiles = advImgProc.createAllSizes(this.subImage);
		
		Point point = advImgProc.getAllMatches(this.mainImage ,imgFiles);
		
		return point;		
	}
}
