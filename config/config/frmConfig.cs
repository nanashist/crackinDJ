using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Midi;
using NAudio.Wave;

public partial class frmConfig : Form
{
    string configFilename = "crackinDJ.config";
    Config _config;
    MidiIn _midiin;

    public bool LOnOff = false;
    public int LCount = 0;
    public bool ROnOff = false;
    public int RCount = 0;
    public int Fader;

    public frmConfig()
    {
        InitializeComponent();


    }
    private void MessageReceived(MidiInMessageEventArgs e){
        ControlChangeEvent cc;

        cc = checked((ControlChangeEvent)e.MidiEvent);
        lstMIDIEvent.Items.Add(new Config.clsMIDIevent(cc.Channel, (int)cc.Controller, cc.ControllerValue));

    }

    private void frmConfig_Load(object sender, EventArgs e)
    {
        List<float> flist = new List<float>();
        flist.Add(0);
        flist.Add(2);
        flist.Add(4);
        flist.Add(6);
        flist.Add(8);
        flist.Add(10);
        DiscDraw.Draw(pictureBox1, flist);
        DiscDraw.Draw(pictureBox2, flist);

        _config = Config.ReadXML(configFilename);

        lstMIDIEvent.Items.Clear();
        //ASIOドライバー一覧取得
        foreach (string device in AsioOut.GetDriverNames())
        {
            cboAsioDriver.Items.Add(device);
        }
        if (cboAsioDriver.Items.Count > 0)
        {
            cboAsioDriver.SelectedIndex = 0;
        }
        //MIDI入力デバイス一覧取得
        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            cboMIDIINDEVICE.Items.Add(MidiIn.DeviceInfo(device).ProductName);
        }
        if (cboMIDIINDEVICE.Items.Count > 0)
        {
            cboMIDIINDEVICE.SelectedIndex = 0;
        }
    }

    private void Config2Form()
    {
        for(int i = 0; i < cboAsioDriver.Items.Count - 1; i++)
        {
            if ((string)cboAsioDriver.Items[i] == _config.ASIO.DriverName)
            {
                cboAsioDriver.SelectedIndex = i;
            }
        }
        txtASIOoffset.Text = _config.ASIO.Channeloffset.ToString();
        for (int i = 0; i < cboMIDIINDEVICE.Items.Count - 1; i++)
        {
            if ((string)cboMIDIINDEVICE.Items[i] == _config.MIDI.DeviceName)
            {
                cboMIDIINDEVICE.SelectedIndex = i;
            }
        }
        SetButtonColor(btnLTurnL, _config.MIDI.LeftTurnTable.LeftTrun.Channel);
        SetButtonColor(btnLTurnR, _config.MIDI.LeftTurnTable.RightTrun.Channel);
        SetButtonColor(btnLTouch, _config.MIDI.LeftTurnTable.TouchOn.Channel);
        SetButtonColor(btnRTurnL, _config.MIDI.RightTurnTable.LeftTrun.Channel);
        SetButtonColor(btnRTurnR, _config.MIDI.RightTurnTable.RightTrun.Channel);
        SetButtonColor(btnRTouch, _config.MIDI.RightTurnTable.TouchOn.Channel);
        SetButtonColor(btnFadeL, _config.MIDI.Xfader.Channel);
        SetButtonColor(btnFadeR, _config.MIDI.Xfader.Channel);


    }

    private void SetButtonColor(Button btn,int Channel)
    {
        if(Channel == -1)
        {
            btn.UseVisualStyleBackColor = true;
        }
        else
        {
            btn.BackColor = Color.Gold;
        }
    }
    private void timer1_Tick(object sender, EventArgs e)
    {
        //this.Text = di.speed.ToString();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void btn_Click(object sender, EventArgs e)
    {

        string name = "";
        System.Windows.Forms.Button button = null;
        try
        {
            button = checked((System.Windows.Forms.Button)sender);
            name = button.Name;
        }
        catch (System.OverflowException err)
        {
            Console.WriteLine(err.ToString());
        }


        if (lstMIDIEvent.SelectedItem == null)
        {
            button.UseVisualStyleBackColor = true;
            toolTip1.SetToolTip(button, "割り当てなし");
            return;
        }

        //リストからEVENTに
        Config.clsMIDIevent MIDIevent;// = (Config.clsMIDIevent)lstMIDIEvent.SelectedItem;
        try
        {
            MIDIevent = (Config.clsMIDIevent)lstMIDIEvent.SelectedItem;
        }
        catch (System.OverflowException err)
        {
            Console.WriteLine(err.ToString());
            return;
        }

        switch (name)
        {
            case "btnLTurnL":
                _config.MIDI.LeftTurnTable.LeftTrun = MIDIevent;
                break;
            case "btnLTouch":
                _config.MIDI.LeftTurnTable.TouchOn = MIDIevent;
                break;
            case "btnLTurnR":
                _config.MIDI.LeftTurnTable.RightTrun = MIDIevent;
                break;
            case "btnRTurnL":
                _config.MIDI.RightTurnTable.LeftTrun = MIDIevent;
                break;
            case "btnRTouch":
                _config.MIDI.RightTurnTable.TouchOn = MIDIevent;
                break;
            case "btnRTurnR":
                _config.MIDI.RightTurnTable.RightTrun = MIDIevent;
                break;
            case "btnFadeL":
                _config.MIDI.SetXFader(MIDIevent,true);
                break;
            case "btnFadeC":
                break;
            case "btnFadeR":
                _config.MIDI.SetXFader(MIDIevent, false);
                break;
            default:
                break;
        }
        switch (name)
        {
            case "btnLTurnL":
            case "btnLTouch":
            case "btnLTurnR":
            case "btnRTurnL":
            case "btnRTouch":
            case "btnRTurnR":
            case "btnFadeL":
            case "btnFadeC":
            case "btnFadeR":
                button.BackColor = Color.Gold;
                toolTip1.SetToolTip(button, MIDIevent.ToolTipText());
                break;
            default:
                break;
        }

        lstMIDIEvent.SelectedItem = null;

    }

    /// <summary>
    /// Configファイルの保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSave_Click(object sender, EventArgs e)
    {
        _config.WriteXML(configFilename);
    }

    private void btnRecord_Click(object sender, EventArgs e)
    {
        timer2start = DateTime.Now;
        timer2.Enabled = !timer2.Enabled;
    }
    private DateTime timer2start;

    private void timer2_Tick(object sender, EventArgs e)
    {
        MIDIinputToGraphic();
        //皿回し(レコードの描画)
        float f = (float)(DateTime.Now.Ticks - timer2start.Ticks) * 200 / 10000000;
        DiscDraw.DrawRecord(pictureBox1, f);
        DiscDraw.DrawRecord(pictureBox2, f);

    }

    private  void MIDIinputToGraphic()
    {
        DiscDraw.Draw(pictureBox1, new List<float>());
        DiscDraw.Draw(pictureBox2, new List<float>());
        if (LOnOff) DiscDraw.DrawTouch(pictureBox1);
        if (ROnOff) DiscDraw.DrawTouch(pictureBox2);
        trkFader.Value = Fader;
        DiscDraw.DrawDiscPos(pictureBox1, LCount);
        DiscDraw.DrawDiscPos(pictureBox2, RCount);
    }

    private void btnMIDIIn_Click(object sender, EventArgs e)
    {
        if (cboMIDIINDEVICE.SelectedItem != null)
        {
            if (monitoring)
                StopMonitoring();
            else
                StartMonitoring();
        }
    }

    bool monitoring = false;
    private void StartMonitoring()
    {
        btnMIDIIn.Text = "MonitorOff";
        if (_midiin != null)
        {
            _midiin.Dispose();
            _midiin.MessageReceived -= midiIn_MessageReceived;
            _midiin.ErrorReceived -= midiIn_ErrorReceived;
            _midiin = null;
        }
        if (_midiin == null)
        {
            _midiin = new MidiIn(cboMIDIINDEVICE.SelectedIndex);
            _midiin.MessageReceived += midiIn_MessageReceived;
            _midiin.ErrorReceived += midiIn_ErrorReceived;
        }
        _midiin.Start();
        monitoring = true;
    }

    private void StopMonitoring()
    {
        btnMIDIIn.Text = "MonitorOn";
        _midiin.Stop();
        monitoring = false;
    }

    void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
    {
        //progressLog1.LogMessage(Color.Red, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }

    delegate void SetTextCallback(Config.clsMIDIevent ev);
    void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
    {
        //if (checkBoxFilterAutoSensing.Checked && e.MidiEvent != null && e.MidiEvent.CommandCode == MidiCommandCode.AutoSensing)
        if (e.MidiEvent != null && e.MidiEvent.CommandCode == MidiCommandCode.AutoSensing)
        {
            return;
        }

        if (e.MidiEvent.CommandCode == MidiCommandCode.ControlChange)
        {
            ControlChangeEvent cc = (ControlChangeEvent)e.MidiEvent;
            Config.clsMIDIevent ev = new Config.clsMIDIevent(cc.Channel, (int)cc.Controller, cc.ControllerValue);
            AddlstMidiEvent(ev);
        }
        //progressLog1.LogMessage(Color.Blue, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }
    void AddlstMidiEvent(Config.clsMIDIevent ev)
    {
        if (lstMIDIEvent.IsDisposed) return;
        if (lstMIDIEvent.InvokeRequired)
        {
            this.Invoke((MethodInvoker)delegate { AddlstMidiEvent(ev); });
        }
        else
        {
            lstMIDIEvent.Items.Add(ev);
            ControlChangeEvent cc = new ControlChangeEvent(0,ev.Channel, (NAudio.Midi.MidiController)ev.ControlChange, ev.value);
            switch (_config.CheckMidiEvent(cc))
            {
                case EnumMidiResult.L_TABLE_LEFT:
                    LCount++;
                    if (LCount == 50)
                        LCount = 0;
                    break;
                case EnumMidiResult.L_TABLE_RIGHT:
                    LCount--;
                    if (LCount == -1)
                        LCount = 49;
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_ON:
                    LOnOff = true;
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_OFF:
                    LOnOff = false;
                    break;
                case EnumMidiResult.R_TABLE_LEFT:
                    RCount++;
                    if (RCount == 50)
                        RCount = 0;
                    break;
                case EnumMidiResult.R_TABLE_RIGHT:
                    RCount--;
                    if (RCount == -1)
                        RCount = 49;
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_ON:
                    ROnOff = true;
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_OFF:
                    ROnOff = false;
                    break;
                case EnumMidiResult.X_FADER:
                    Fader = cc.ControllerValue;
                    break;
                default:
                    break;
            }
            MIDIinputToGraphic();
        }
    }

    private void btnListClear_Click(object sender, EventArgs e)
    {
        lstMIDIEvent.Items.Clear();
    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {

    }

    private void cboAsioDriver_SelectedIndexChanged(object sender, EventArgs e)
    {
        _config.ASIO.DriverName = (string)cboAsioDriver.SelectedItem;
    }

    private void cboMIDIINDEVICE_SelectedIndexChanged(object sender, EventArgs e)
    {
        _config.MIDI.DeviceName = (string)cboMIDIINDEVICE.SelectedItem;
    }

    private void txtASIOoffset_TextChanged(object sender, EventArgs e)
    {
        if (txtASIOoffset.Text == "")
        {
            _config.ASIO.Channeloffset = 0;
        }
        else
        {
            _config.ASIO.Channeloffset =Convert.ToInt16(txtASIOoffset.Text);
        }
    }
}
