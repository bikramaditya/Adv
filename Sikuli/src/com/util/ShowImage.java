package com.util;

import java.awt.Graphics;
import java.awt.Panel;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.JFrame;

public class ShowImage extends Panel {
  private static final long serialVersionUID = 1L;
  private static JFrame frame = null;
  private BufferedImage image;

  public ShowImage(String filename) {
    try {
      image = ImageIO.read(new File(filename));
    } catch (IOException ie) {
      ie.printStackTrace();
    }
  }

  public void paint(Graphics g) {
    g.drawImage(image, 0, 0, null);
  }

  static public void show(String file) throws Exception {
    frame = new JFrame("Calibrating...please wait");
    Panel panel = new ShowImage(file);
    frame.getContentPane().add(panel);
    frame.setAlwaysOnTop(true);
    frame.setSize(800, 800);
    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    frame.setVisible(true);
  }

public static void close() {
		if(frame != null)
		{
			frame.dispose();
		}
	}
}
