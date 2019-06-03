/*
MIT License
Copyright (c) 2019 Patrick72762
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

package PortTunnelDesktop;

import PortTunnelLib.*;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;

public class Gui extends JFrame {
    private ButtonGroup rbGroup;
    private JRadioButton selectSC, selectCC, selectSS;
    private JLabel lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, txtActiveConnections;
    private JCheckBox cbHttp, cbSsl, cbStreamLog;
    private JTextArea txtHost1, txtHost2, intPort1, intPort2, intCP, intTimeout, txtLog;
    private JButton btnStart, btnStop;
    private KeyAdapter tabAdapter;

    private Color bgColor = new Color(192,192,192);
    private Color bgMain = new Color(64,64,64);
    private Color foreColor = Color.white;

    private PortTunnel pt;

    public Gui(){
        setLayout(null);

        createItems();

        txtHost1.setText("illumina-chemie.de");
        cbHttp.setSelected(true);
        intPort1.setText("80");
        intPort2.setText("80");
        intTimeout.setText("3000");

        setMinimumSize(new Dimension(419, 450));
        setMaximumSize(new Dimension(420, 451));
        setTitle("PortTunnel");
        setSize(419, 450);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setPreferredSize(new Dimension(480, 300));
        getContentPane().setBackground(bgMain);

        setVisible(true);
    }
    
    private void createItems(){
        tabAdapter = new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if(e.getKeyCode() == KeyEvent.VK_TAB) {
                    System.out.println(e.getModifiers());
                    if (e.getModifiers() > 0) e.getComponent().transferFocusBackward();
                    else e.getComponent().transferFocus();
                    e.consume();
                }
            }
        };

        //Connection 1

        lbl1 = new JLabel("Connection 1:");
        lbl1.setBounds(10, 40, 73, 15);

        Font f = new Font(lbl1.getFont().getName(), Font.PLAIN, 11);

        lbl1.setFont(f);
        lbl1.setForeground(foreColor);
        this.add(lbl1);

        txtHost1 = new JTextArea();
        txtHost1.setBounds(91, 40, 137, 17);
        txtHost1.setRows(1);
        txtHost1.addKeyListener(tabAdapter);
        txtHost1.setBackground(bgColor);
        txtHost1.setBorder(BorderFactory.createLineBorder(Color.gray));
        txtHost1.setFont(f);
        txtHost1.setForeground(Color.black);
        this.add(txtHost1);

        lbl2 = new JLabel("Port:");
        lbl2.setBounds(234 , 40, 49, 15);
        lbl2.setFont(f);
        lbl2.setForeground(foreColor);
        this.add(lbl2);

        intPort1 = new JTextArea();
        intPort1.setBounds(269, 40, 67, 17);
        intPort1.setRows(1);
        intPort1.addKeyListener(tabAdapter);
        intPort1.setBackground(bgColor);
        intPort1.setBorder(BorderFactory.createLineBorder(Color.gray));
        intPort1.setFont(f);
        intPort1.setForeground(Color.black);
        this.add(intPort1);

        //Connection 2

        lbl3 = new JLabel("Connection 2:");
        lbl3.setBounds(10, 70, 73, 15);
        lbl3.setFont(f);
        lbl3.setForeground(foreColor);
        this.add(lbl3);

        txtHost2 = new JTextArea();
        txtHost2.setBounds(91, 70, 137, 17);
        txtHost2.setRows(1);
        txtHost2.addKeyListener(tabAdapter);
        txtHost2.setBackground(bgColor);
        txtHost2.setBorder(BorderFactory.createLineBorder(Color.gray));
        txtHost2.setFont(f);
        txtHost2.setVisible(false);
        txtHost2.setForeground(Color.black);
        this.add(txtHost2);

        lbl4 = new JLabel("Port:");
        lbl4.setBounds(234 , 70, 49, 15);
        lbl4.setFont(f);
        lbl4.setForeground(foreColor);
        this.add(lbl4);

        intPort2 = new JTextArea();
        intPort2.setBounds(269, 70, 67, 17);
        intPort2.setRows(1);
        intPort2.addKeyListener(tabAdapter);
        intPort2.setBackground(bgColor);
        intPort2.setBorder(BorderFactory.createLineBorder(Color.gray));
        intPort2.setFont(f);
        intPort2.setForeground(Color.black);
        this.add(intPort2);

        //Radio Buttons

        rbGroup = new ButtonGroup();

        selectSC = new JRadioButton("Server-Client-Mode");
        selectSC.setBounds(10, 10, 120, 20);
        selectSC.setBackground(bgMain);
        selectSC.setFont(f);
        selectSC.setSelected(true);
        selectSC.setForeground(Color.white);
        selectSC.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                txtHost1.setVisible(true);
                txtHost2.setVisible(false);
                intCP.setVisible(false);
            }
        });
        this.add(selectSC);
        rbGroup.add(selectSC);

        selectCC = new JRadioButton("Client-Client-Mode");
        selectCC.setBounds(135, 10, 120, 20);
        selectCC.setBackground(bgMain);
        selectCC.setFont(f);
        selectCC.setForeground(Color.white);
        selectCC.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                txtHost1.setVisible(true);
                txtHost2.setVisible(true);
                intCP.setVisible(true);
            }
        });
        this.add(selectCC);
        rbGroup.add(selectCC);

        selectSS = new JRadioButton("Server-Server-Mode");
        selectSS.setBounds(260, 10, 130, 20);
        selectSS.setBackground(bgMain);
        selectSS.setFont(f);
        selectSS.setForeground(Color.white);
        selectSS.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                txtHost1.setVisible(false);
                txtHost2.setVisible(false);
                intCP.setVisible(true);
            }
        });
        this.add(selectSS);
        rbGroup.add(selectSS);

        //Check Boxes

        cbHttp = new JCheckBox("HTTP");
        cbHttp.setBounds(342, 40, 55, 17);
        cbHttp.setBackground(bgMain);
        cbHttp.setFont(f);
        cbHttp.setForeground(Color.white);
        this.add(cbHttp);

        cbSsl = new JCheckBox("SSL");
        cbSsl.setBounds(342, 70, 55, 17);
        cbSsl.setBackground(bgMain);
        cbSsl.setFont(f);
        cbSsl.setForeground(Color.white);
        this.add(cbSsl);

        cbStreamLog = new JCheckBox("Log");
        cbStreamLog.setBounds(342, 100, 55, 17);
        cbStreamLog.setBackground(bgMain);
        cbStreamLog.setFont(f);
        cbStreamLog.setForeground(Color.white);
        this.add(cbStreamLog);

        //Control Port

        lbl5 = new JLabel("Control-Port: ");
        lbl5.setBounds(10, 100, 65, 15);
        lbl5.setFont(f);
        lbl5.setForeground(foreColor);
        this.add(lbl5);

        intCP = new JTextArea();
        intCP.setBounds(91, 100, 67, 17);
        intCP.setRows(1);
        intCP.addKeyListener(tabAdapter);
        intCP.setBackground(bgColor);
        intCP.setBorder(BorderFactory.createLineBorder(Color.gray));
        intCP.setFont(f);
        intCP.setVisible(false);
        intCP.setForeground(Color.black);
        this.add(intCP);

        //Active Connections Label

        txtActiveConnections = new JLabel("0");
        txtActiveConnections.setBounds(174, 100, 25, 15);
        txtActiveConnections.setFont(f);
        txtActiveConnections.setForeground(foreColor);
        this.add(txtActiveConnections);

        //Timeout

        lbl6 = new JLabel("Timeout: ");
        lbl6.setBounds(215, 100, 48, 15);
        lbl6.setFont(f);
        lbl6.setForeground(foreColor);
        this.add(lbl6);

        intTimeout = new JTextArea();
        intTimeout.setBounds(269, 100, 67, 17);
        intTimeout.setRows(1);
        intTimeout.addKeyListener(tabAdapter);
        intTimeout.setBackground(bgColor);
        intTimeout.setBorder(BorderFactory.createLineBorder(Color.gray));
        intTimeout.setFont(f);
        intTimeout.setForeground(Color.black);
        this.add(intTimeout);

        //Buttons

        btnStart = new JButton();
        btnStart.setText("Start");
        btnStart.setBounds(12, 130, 175, 25);
        btnStart.setBackground(bgColor);
        btnStart.setForeground(Color.black);
        btnStart.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                startPortTunnel();
            }
        });
        this.add(btnStart);

        btnStop = new JButton();
        btnStop.setText("Stop");
        btnStop.setBounds(216, 130, 175, 25);
        btnStop.setBackground(bgColor);
        btnStop.setForeground(Color.black);
        btnStop.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                try{
                    pt.Stop();
                }
                catch (Exception ex){
                }
                System.exit(0);
            }
        });
        this.add(btnStop);

        //Log Output

        txtLog = new JTextArea();
        txtLog.setLineWrap(true);
        txtLog.setEditable(false);
        txtLog.setFont(f);
        txtLog.setBackground(bgColor);
        txtLog.setForeground(Color.black);
        JScrollPane scrollPane = new JScrollPane(txtLog);
        scrollPane.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
        scrollPane.setBounds( 12, 170, 379, 225);
        this.add(scrollPane);
    }

    private void startPortTunnel(){
        if(!intPort1.getText().equals("") && !intPort2.getText().equals("")){

            Connection c1 = null, c2 = null;

            try {
                int p1 = Integer.parseInt(intPort1.getText());
                int p2 = Integer.parseInt(intPort2.getText());

                int cp = 0;

                if (selectSC.isSelected())
                {
                    String h1 = txtHost1.getText();

                    c1 = new Connection(h1, p1);
                    c2 = new Connection(p2);

                    this.setTitle("PortTunnel ServerClient");
                }
                else if (selectCC.isSelected())
                {
                    String h1 = txtHost1.getText();
                    String h2 = txtHost2.getText();

                    c1 = new Connection(h1, p1);
                    c2 = new Connection(h2, p2);

                    cp = Integer.parseInt(intCP.getText());

                    this.setTitle("PortTunnel Client");
                }
                else if (selectSS.isSelected())
                {
                    c1 = new Connection(p1);
                    c2 = new Connection(p2);

                    cp = Integer.parseInt(intCP.getText());

                    this.setTitle("PortTunnel Server");
                }

                pt = new PortTunnel(c1, c2, Integer.parseInt(intTimeout.getText()), cp);

                pt.AddListener(new LogListener() {
                    @Override
                    public void NewLogOutput(String s) {
                        txtLog.setText(txtLog.getText() + s);

                        System.out.print(s);

                        txtActiveConnections.setText(Integer.toString(
                                pt.getActiveConnections()));
                    }
                });

                if(cbStreamLog.isSelected()){
                    pt.AddListener(new StreamLogListener() {
                        @Override
                        public void NewStreamLog(String s) {
                            String filename = s.split("\r")[0]
                                    .replace(' ', '-')
                                    .replace(':', '-')+ ".txt";

                            try {
                                new File("Log/").mkdirs();

                                BufferedWriter writer = new BufferedWriter(new FileWriter("Log/" + filename));
                                writer.write(s);

                                writer.close();
                            }catch (Exception e){
                                e.printStackTrace();
                            }
                        }
                    });
                }

                pt.setHttpProtocol(cbHttp.isSelected());
                pt.setDestIsSsl(cbSsl.isSelected());

                blockInputs();

                pt.setTicksBetweenBytes(0);

                pt.Start();
            }
            catch (Exception e){
            }
        }
    }

    private void blockInputs(){
        selectCC.setEnabled(false);
        selectSS.setEnabled(false);
        selectSC.setEnabled(false);
        btnStart.setEnabled(false);
        txtHost1.setEnabled(false);
        txtHost2.setEnabled(false);
        intPort1.setEnabled(false);
        intPort2.setEnabled(false);
        intCP.setEnabled(false);
        intTimeout.setEnabled(false);
        cbHttp.setEnabled(false);
        cbSsl.setEnabled(false);
        cbStreamLog.setEnabled(false);
    }
}
