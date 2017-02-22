package com.xyz.util;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Cursor;
import java.awt.Image;
import java.awt.Point;
import java.awt.Toolkit;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import javax.swing.ImageIcon;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingConstants;
import javax.swing.TransferHandler;

public class DragLabel {
  public static void main(String args[]) {
    final JFrame frame = new JFrame("Drag Label");
    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

    JLabel label = new JLabel("Drag Me", SwingConstants.CENTER);
    label.setIcon(new ImageIcon("images/pin.png"));
    label.setTransferHandler(new TransferHandler("foreground"));
    MouseListener listener = new MouseAdapter() {
      public void mousePressed(MouseEvent me) {
        JComponent comp = (JComponent) me.getSource();
        TransferHandler handler = comp.getTransferHandler();
        handler.exportAsDrag(comp, me, TransferHandler.COPY);
        
      }
    };
    label.addMouseListener(listener);
    frame.add(label, BorderLayout.CENTER);
    frame.getContentPane().setBackground(Color.WHITE);
    frame.setSize(300, 150);
    frame.setVisible(true);
  }
}