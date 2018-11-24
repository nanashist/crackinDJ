using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Midi;

public class MIDIinput
{
    Config _config;

    private class MIDITurnTable
    {
        public List<Param> ParamList;
        public bool On;
        public double Speed;
        public int PlusCount;
        public int MinusCount;
        public int Pos;

        public MIDITurnTable()
        {
            ParamList = new List<Param>();
        }
    }
    private MIDITurnTable[] turntable;
    /// <summary>
    /// -1:L,1:R
    /// </summary>
    /// <param name="LR"></param>
    /// <returns></returns>
    public double Speed(int LR)
    {
        if (LR == CdjData.Left)
        {
            return turntable[0].Speed;
        }
        else
        {
            return turntable[1].Speed;
        }
    }

    public int Pos(int LR)
    {
        if (LR == CdjData.Left)
        {
            return turntable[0].Pos;
        }
        else
        {
            return turntable[1].Pos;
        }
    }

    public int xfader;

    MidiIn midiIn;
    List<string> MIDIInList;
    int _index;
    bool monitoring;

    private class Param
    {
        public long ticks;
        public int Timestamp;
        public int value;
    }
    public MIDIinput(string devicename)
    {
        _index = -1;
        turntable =new MIDITurnTable[2];
        MIDIInList = new List<string>();
        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            MIDIInList.Add(MidiIn.DeviceInfo(device).ProductName);
            if(MidiIn.DeviceInfo(device).ProductName == devicename)
            {
                _index = device;
            }
        }
    }
    public MIDIinput(int index)
    {
        MIDIinput_initialize(index);
    }

    public void MIDIinput_initialize(int index){
        MIDIInList = new List<string>();
        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            MIDIInList.Add(MidiIn.DeviceInfo(device).ProductName);
        }
        if (index < MIDIInList.Count)
            _index = index;
        else
            _index = -1;
    }

    /// <summary>
    /// 割り込みスタート
    /// MIDIキー操作があるとmidiIn_MessageRecievedが呼ばれる
    /// </summary>
    public void StartMonitoring()
    {

        if (MIDIInList.Count == 0 || _index == -1)
        {
            MessageBox.Show("No MIDI input devices available");
            return;
        }
        if (midiIn != null)
        {
            midiIn.Dispose();
            midiIn.MessageReceived -= midiIn_MessageReceived;
            midiIn.ErrorReceived -= midiIn_ErrorReceived;
            midiIn = null;
        }
        if (midiIn == null)
        {
            midiIn = new MidiIn(_index);
            midiIn.MessageReceived += midiIn_MessageReceived;
            midiIn.ErrorReceived += midiIn_ErrorReceived;
        }
        midiIn.Start();
        monitoring = true;
        turntable[0] = new MIDITurnTable();
        turntable[1] = new MIDITurnTable();

    }

    void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
    {
        //progressLog1.LogMessage(Color.Red, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }

    public void StopMonitoring()
    {
        if (monitoring)
        {
            midiIn.Stop();
            monitoring = false;
        }
    }

    public void SetConfig(Config config)
    {
        _config = config;
    }

        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
    {
        //if (checkBoxFilterAutoSensing.Checked && e.MidiEvent != null && e.MidiEvent.CommandCode == MidiCommandCode.AutoSensing)
        if(e.MidiEvent != null && e.MidiEvent.CommandCode == MidiCommandCode.AutoSensing)
        {
            return;
        }

        if (e.MidiEvent.CommandCode == MidiCommandCode.ControlChange)
        {
            ControlChangeEvent cc = (ControlChangeEvent)e.MidiEvent;
            switch (_config.CheckMidiEvent(cc))
            {
                case EnumMidiResult.L_TABLE_LEFT:
                    SetTurnTable(0, 1, e.Timestamp);
                    break;
                case EnumMidiResult.L_TABLE_RIGHT:
                    SetTurnTable(0, -1, e.Timestamp);
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_ON:
                    turntable[0].On = true;
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_OFF:
                    turntable[0].On = false;
                    break;
                case EnumMidiResult.R_TABLE_LEFT:
                    SetTurnTable(1, 1, e.Timestamp);
                    break;
                case EnumMidiResult.R_TABLE_RIGHT:
                    SetTurnTable(1, -1, e.Timestamp);
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_ON:
                    turntable[1].On = true;
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_OFF:
                    turntable[1].On = false;
                    break;
                case EnumMidiResult.X_FADER:
                    xfader = cc.ControllerValue;
                    break;
                default:
                    break;
            }
            /*
            //5chのパンがクロスフェーダー
            if (CheckControl(cc, 5, (int)MidiController.Pan))
            {
                xfader = cc.ControllerValue;
            }
            //1chの3が左皿on,off
            if (CheckControl(cc, 1, 3))
            {
                turntable[0].On = (cc.ControllerValue == 127);//127ならtrue
            }
            if (CheckControl(cc, 1, (int)MidiController.BreathController))
            {
                SetTurnTable(0, cc.ControllerValue, e.Timestamp);
            }
            //2chの3が右皿on,off
            if (CheckControl(cc, 2, 3))
            {
                turntable[0].On = (cc.ControllerValue == 127);//127ならtrue
            }
            if (CheckControl(cc, 2, (int)MidiController.BreathController))
            {
                SetTurnTable(1, cc.ControllerValue, e.Timestamp);
            }
            */
        }
        //progressLog1.LogMessage(Color.Blue, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }
    private void SetTurnTable(int tuntableLR,int value,int Timestamp)
    {
        Param p = new Param();
        p.Timestamp = Timestamp;
        p.ticks = DateTime.Now.Ticks;
        p.value = value;
        turntable[tuntableLR].Pos += p.value;
        turntable[tuntableLR].ParamList.Add(p);
        if (turntable[tuntableLR].ParamList.Count > 2)
        {
            turntable[tuntableLR].ParamList.RemoveAt(0);
        }
        if (turntable[tuntableLR].ParamList.Count == 2)
        {
            turntable[tuntableLR].Speed = (double)36 * p.value / (turntable[tuntableLR].ParamList[1].Timestamp - turntable[tuntableLR].ParamList[0].Timestamp);
        }

    }

    public void update(DateTime now)
    {
        for(int i = 0; i < 2; i++)
        {
            if (turntable[i].ParamList.Count == 2)
            {
                long ticks = (long)((turntable[i].ParamList[1].ticks - turntable[i].ParamList[0].ticks) * 1.5);
                if (ticks < 360000) ticks = 360000;
                if ((now.Ticks - turntable[i].ParamList.Last().ticks) > ticks)
                {
                    turntable[i].Speed = 0;
                }
            }

        }
    }

    bool CheckControl(ControlChangeEvent e ,int channel,int controller)
    {
        if (e.Channel == channel && (int)e.Controller == controller)
        {
            return true;
        }
        return false;
    }
}

public class inputMIDIFader
{
    MIDIinput _midi;
    public inputMIDIFader(MIDIinput midi)
    {
        _midi = midi;
    }
    /// <summary>
    /// 最初に１回だけ呼ぶ
    /// </summary>
    public void Initial()
    {
        //アナログ入力の入力範囲等の設定(-127～127の範囲で入力するのでセンターの範囲をセット)
        inputFader.SetFaderInitialize(-110, 110);
    }
    /// <summary>
    /// 入力情報更新
    /// </summary>
    public void Update()
    {
        //アナログ入力の値を読み込む。(analogValueに(-127～127)の値を渡す
        inputFader.SetFaderValue(_midi.xfader * 2 - 127);
    }
}

public class inputMIDIRecord
{
    private int preangle;
    private double deltaangle;
    private int _LR;
    private MIDIinput _midi;

    private int prePos;
    private int deltaPos;

    public inputMIDIRecord(int LR, MIDIinput midi)
    {
        _LR = LR;
        _midi = midi;
        prePos = midi.Pos(LR);
    }

    public double DeltaAngle { get { return deltaangle; } }

    public void Update()
    {
        deltaPos = _midi.Pos(_LR) - prePos;
        prePos = _midi.Pos(_LR);
        deltaangle = (double)deltaPos * 360 / 50;

        //deltaangle = _midi.Speed(_LR) * 3.33;//1倍速で60/1秒なら200/60度
        //; preangle = angle;
    }
}
