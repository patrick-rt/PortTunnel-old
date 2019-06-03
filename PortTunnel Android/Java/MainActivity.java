package de.patricklutz.porttunnelandroid;

import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;

import PortTunnelLib.Connection;
import PortTunnelLib.LogListener;
import PortTunnelLib.PortTunnel;

public class MainActivity extends AppCompatActivity {
    EditText etH1, etH2, etP1, etP2, etCP, etTo;
    TextView log, activeConnections;
    Button btnStart, btnStop;
    CheckBox cbHttp, cbSsl;
    private PortTunnel tunnel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        getItems();
        setActions();
    }

    void getItems()
    {
        etH1 = findViewById(R.id.etHost1);
        etH2 = findViewById(R.id.etHost2);
        etP1 = findViewById(R.id.etPort1);
        etP2 = findViewById(R.id.etPort2);
        etCP = findViewById(R.id.etControlPort);
        etTo = findViewById(R.id.etTimeout);

        log = findViewById(R.id.txtLog);
        activeConnections = findViewById(R.id.txtActiveConnections);

        btnStart = findViewById(R.id.btnStart);
        btnStop = findViewById(R.id.btnStop);

        cbHttp = findViewById(R.id.cbHttp);
        cbSsl = findViewById(R.id.cbSsl);

        SharedPreferences preferences = getSharedPreferences("SETUP", MODE_PRIVATE);

        etH1.setText(preferences.getString("H1", "illumina-chemie.de"));
        etH2.setText(preferences.getString("H2", ""));

        etP1.setText(Integer.toString(preferences.getInt("P1", 80)));
        etP2.setText(Integer.toString(preferences.getInt("P2", 8080)));

        etCP.setText(Integer.toString(preferences.getInt("CP", 0)));
        etTo.setText(Integer.toString(preferences.getInt("TO", 1000)));

        cbHttp.setChecked(preferences.getBoolean("HTTP", false));
        cbSsl.setChecked(preferences.getBoolean("SSL", false));
    }

    void setActions(){
        btnStart.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startPortTunnel();
            }
        });

        btnStop.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stopPortTunnel();
            }
        });
    }

    private void stopPortTunnel(){
        try
        {
            tunnel.Stop();
        }
        catch (Exception e)
        { }

        try
        {
            PortTunnel.ExitEnvironment();
        }
        catch (Exception e)
        { }
    }

    private void startPortTunnel(){
        try
        {
            String h1 = etH1.getText().toString();
            String h2 = etH2.getText().toString();

            int p1 = Integer.parseInt(etP1.getText().toString());
            int p2 = Integer.parseInt(etP2.getText().toString());

            int cp = Integer.parseInt(etCP.getText().toString());
            int to = Integer.parseInt(etTo.getText().toString());

            SharedPreferences preferences = getSharedPreferences("SETUP", MODE_PRIVATE);
            SharedPreferences.Editor editor = preferences.edit();

            editor.putString("H1", h1);
            editor.putString("H2", h2);

            editor.putInt("P1", p1);
            editor.putInt("P2", p2);
            editor.putInt("CP", cp);
            editor.putInt("TO", to);

            editor.putBoolean("HTTP", cbHttp.isChecked());
            editor.putBoolean("SSL", cbSsl.isChecked());

            editor.commit();

            Connection c1 = new Connection(h1, p1);
            Connection c2 = new Connection(h2, p2);

            tunnel = new PortTunnel(c1, c2, to, cp);

            tunnel.setHttpProtocol(cbHttp.isChecked());
            tunnel.setDestIsSsl(cbSsl.isChecked());

            tunnel.AddListener(new LogListener() {
                @Override
                public void NewLogOutput(String s) {
                    final String text = s;
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            try
                            {
                                log.setText(log.getText().toString() + text);

                                activeConnections.setText(((PortTunnel)tunnel).getActiveConnections());
                            }
                            catch (Exception ex)
                            { }
                        }
                    });
                }
            });

            blockInputs();

            tunnel.Start();
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }

    private void blockInputs(){
        btnStart.setEnabled(false);
        etH1.setEnabled(false);
        etH2.setEnabled(false);
        etP1.setEnabled(false);
        etP2.setEnabled(false);
        etCP.setEnabled(false);
        etTo.setEnabled(false);
        cbHttp.setEnabled(false);
        cbSsl.setEnabled(false);
    }
}
