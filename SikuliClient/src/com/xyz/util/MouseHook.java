package com.xyz.util;

import java.util.Date;
import java.util.logging.Handler;
import java.util.logging.Level;
import java.util.logging.Logger;

import org.jnativehook.GlobalScreen;
import org.jnativehook.NativeHookException;
import org.jnativehook.mouse.NativeMouseEvent;
import org.jnativehook.mouse.NativeMouseInputListener;

public class MouseHook implements NativeMouseInputListener 
{
	private static int clickX;
	private static int clickY;
	private static boolean isDraging = false;
	
    public void nativeMouseClicked(NativeMouseEvent e) {
    	clickX = e.getX();
    	clickY = e.getY();
    	//String fileName = PartialScreenCapturer.capture(clickX, clickY);
    }

    public void nativeMousePressed(NativeMouseEvent e) {
        //System.out.println("Mouse Pressed: " + e.getButton());
    }

    public void nativeMouseReleased(NativeMouseEvent e) 
    {   
       if(isDraging)
       {
    	   isDraging = false;
    	   clickX = e.getX();
           clickY = e.getY();
    	   System.out.println("Item dropped at "+clickX+":"+clickY);
       }
    }

    public void nativeMouseMoved(NativeMouseEvent e) {
        //System.out.println("Mouse Moved: " + e.getX() + ", " + e.getY());
    }

    public void nativeMouseDragged(NativeMouseEvent e) {
        
        isDraging = true;
    }

    public static void main(String[] args) {
        try {
        	// Get the logger for "org.jnativehook" and set the level to off.
        	Logger logger = Logger.getLogger(GlobalScreen.class.getPackage().getName());
        	logger.setLevel(Level.OFF);

        	// Change the level for all handlers attached to the default logger.
        	Handler[] handlers = Logger.getLogger("").getHandlers();
        	for (int i = 0; i < handlers.length; i++) {
        	    handlers[i].setLevel(Level.OFF);
        	}
        	
            GlobalScreen.registerNativeHook();
        }
        catch (NativeHookException ex) {
            System.err.println("There was a problem registering the native hook.");
            System.err.println(ex.getMessage());

            System.exit(1);
        }

        // Construct the example object.
        MouseHook example = new MouseHook();

        // Add the appropriate listeners.
        GlobalScreen.addNativeMouseListener(example);
        GlobalScreen.addNativeMouseMotionListener(example);
    }
}