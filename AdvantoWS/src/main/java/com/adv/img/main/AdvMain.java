package com.adv.img.main;

import org.sikuli.script.Match;
import com.adv.img.util.*;
import com.adv.img.util.ClosestPair.Point;

public class AdvMain
{	
	Point match; 
	public String getCoords(String subImage, String mainImage)
	{		
		AdvProcessor advProc = new AdvProcessor(subImage, mainImage);
		try {
			match = advProc.getMatch();
		} catch (Exception e) {
			e.printStackTrace();
			return "Coords Not Found";
		}
		if(match == null)
		{
			return "{}";
		}
		else
		{
			return match.x+"-"+match.y+"-"+match.w+"-"+match.h;
		}
	}
}