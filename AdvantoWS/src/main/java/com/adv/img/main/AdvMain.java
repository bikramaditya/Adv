package com.adv.img.main;

import org.sikuli.script.Match;
import com.adv.img.util.*;

public class AdvMain
{	
	Match match; 
	public String getCoords(String subImage, String mainImage)
	{		
		AdvProcessor advProc = new AdvProcessor(subImage, mainImage);
		try {
			match = advProc.getMatch();
		} catch (Exception e) {
			e.printStackTrace();
			return "Coords Not Found";
		}
		
		return match.x+"-"+match.y+"-"+match.w+"-"+match.h;
	}
}