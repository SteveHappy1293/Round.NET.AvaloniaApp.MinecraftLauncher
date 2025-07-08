package Windows;

import javax.swing.*;
import java.awt.*;

public class MainWindow {
    private JFrame mainframe = new JFrame("RMCL 配置工具");

    public MainWindow() {
        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception e) {
            e.printStackTrace();
        }

        mainframe.setSize(900, 600);
        mainframe.setMinimumSize(new Dimension(900, 600));
        mainframe.setLocationRelativeTo(null);
        mainframe.setLayout(null);
        mainframe.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mainframe.addWindowListener(new java.awt.event.WindowAdapter() {
            @Override
            public void windowClosing(java.awt.event.WindowEvent windowEvent) {
                System.exit(0);
            }
        });

        setView();
    }

    private void setView(){

    }

    public void setShow() {
        mainframe.setVisible(true);
    }
}