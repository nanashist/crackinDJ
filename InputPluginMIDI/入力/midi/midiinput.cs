using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using NAudio.Midi;

public class MIDIinput : IInput
{

    private class Param
    {
        public long ticks;
        public int Timestamp;
        public int value;
    }

    Config _config;
    int xfader,LPos,RPos;

    MidiIn midiIn;
    List<string> MIDIInList;
    int _index;
    bool monitoring;

    private class MIDITurnTable
    {
        public List<Param> ParamList;
        public bool Touch;
        public double Speed;
        public int Pos;

        public MIDITurnTable()
        {
            ParamList = new List<Param>();
        }
    }
    private MIDITurnTable[] turntable;
    
    /// <summary>
    /// 名前渡しで初期化。失敗なら-1を返す
    /// </summary>
    /// <param name="devicename"></param>
    /// <returns></returns>
    public int Initialize(string devicename)
    {
        xfader = 64;
        LPos = 0; RPos = 0;
        _index = -1;

        _config = Config.ReadXML("crackinDJ.config");
        turntable = new MIDITurnTable[2];
        MIDIInList = new List<string>();
        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            MIDIInList.Add(MidiIn.DeviceInfo(device).ProductName);
            if (MidiIn.DeviceInfo(device).ProductName == devicename)
            {
                _index = device;
            }
        }
        if (_index == -1)
        {
            return -1;
        }
        return StartMonitoring();
    }

    /// <summary>
    /// 毎フレーム呼ばれて速度チェックする
    /// </summary>
    /// <param name="now"></param>
    public InputState Update()
    {
        if (monitoring)
        {
            DateTime now = DateTime.Now;
            for (int i = 0; i < 2; i++)
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

            InputState rtn = new InputState();
            rtn.FaderValue = xfader;
            rtn.Left.Speed = turntable[0].Speed;
            rtn.Left.Touch = turntable[0].Touch;
            rtn.Left.Angle = (turntable[0].Pos - LPos) * 360 / 50;

            rtn.Right.Speed = turntable[1].Speed;
            rtn.Right.Touch = turntable[1].Touch;
            rtn.Right.Angle = (turntable[1].Pos - LPos) * 360 / 50;

            LPos = turntable[0].Pos;
            RPos = turntable[1].Pos;
            return rtn;
        }
        else
        {
            return new InputState();
        }

    }

    public void Dispose()
    {
        StopMonitoring();
    }


    /// <summary>
    /// 割り込みスタート
    /// MIDIキー操作があるとmidiIn_MessageRecievedが呼ばれる
    /// </summary>
    /// <returns>-1:No MIDI INPUT DEVICE 0:OK</returns>
    public int StartMonitoring()
    {

        if (MIDIInList.Count == 0 || _index == -1)
        {
            //MessageBox.Show("No MIDI input devices available");
            return -1;
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
        return 0;
    }

    /// <summary>
    /// エラー受信イベントとりあえず無視
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
    {
        //progressLog1.LogMessage(Color.Red, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }

    /// <summary>
    /// MIDI入力やめ
    /// </summary>
    public void StopMonitoring()
    {
        if (monitoring)
        {
            midiIn.Stop();
            monitoring = false;
        }
    }
    /// <summary>
    /// 左回転、右回転、タッチ、フェーダーのイベントをセット
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            switch (_config.CheckMidiEvent(cc))
            {
                case EnumMidiResult.L_TABLE_LEFT:
                    SetTurnTable(0, 1, e.Timestamp);
                    break;
                case EnumMidiResult.L_TABLE_RIGHT:
                    SetTurnTable(0, -1, e.Timestamp);
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_ON:
                    turntable[0].Touch = true;
                    break;
                case EnumMidiResult.L_TABLE_TOUCH_OFF:
                    turntable[0].Touch = false;
                    break;
                case EnumMidiResult.R_TABLE_LEFT:
                    SetTurnTable(1, 1, e.Timestamp);
                    break;
                case EnumMidiResult.R_TABLE_RIGHT:
                    SetTurnTable(1, -1, e.Timestamp);
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_ON:
                    turntable[1].Touch = true;
                    break;
                case EnumMidiResult.R_TABLE_TOUCH_OFF:
                    turntable[1].Touch = false;
                    break;
                case EnumMidiResult.X_FADER:
                    xfader = cc.ControllerValue;
                    break;
                default:
                    break;
            }

        }
        //progressLog1.LogMessage(Color.Blue, String.Format("Time {0} Message 0x{1:X8} Event {2}", e.Timestamp, e.RawMessage, e.MidiEvent));
    }

    /// <summary>
    /// 皿の左右回転の計算(イベント受信ごとに計算する)
    /// 停止はイベントが発生しないので注意
    /// </summary>
    /// <param name="tuntableLR"></param>
    /// <param name="value"></param>
    /// <param name="Timestamp"></param>
    private void SetTurnTable(int tuntableLR, int value, int Timestamp)
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



    /// <summary>
    /// 入力されたイベントが自分が求めているchannel,coltrollerのものか
    /// </summary>
    /// <param name="e"></param>
    /// <param name="channel"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    bool CheckControl(ControlChangeEvent e, int channel, int controller)
    {
        if (e.Channel == channel && (int)e.Controller == controller)
        {
            return true;
        }
        return false;
    }

}


//public class MIDIinputorg
//{
//    ///// <summary>
//    ///// -1:L,1:R
//    ///// </summary>
//    ///// <param name="LR"></param>
//    ///// <returns></returns>
//    //public double Speed(int LR)
//    //{
//    //    if (LR == CdjData.Left)
//    //    {
//    //        return turntable[0].Speed;
//    //    }
//    //    else
//    //    {
//    //        return turntable[1].Speed;
//    //    }
//    //}

//    //public int Pos(int LR)
//    //{
//    //    if (LR == CdjData.Left)
//    //    {
//    //        return turntable[0].Pos;
//    //    }
//    //    else
//    //    {
//    //        return turntable[1].Pos;
//    //    }
//    //}



////    public MIDIinputorg(string devicename)
////    {

////    }
////    public MIDIinputorg(int index)
////    {
////        MIDIinput_initialize(index);
////    }

////    public void MIDIinput_initialize(int index){
////        MIDIInList = new List<string>();
////        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
////        {
////            MIDIInList.Add(MidiIn.DeviceInfo(device).ProductName);
////        }
////        if (index < MIDIInList.Count)
////            _index = index;
////        else
////            _index = -1;
////    }




////    public void SetConfig(Config config)
////    {
////        _config = config;
////    }

////}

////public class inputMIDIFader
////{
////    MIDIinput _midi;
////    public inputMIDIFader(MIDIinput midi)
////    {
////        _midi = midi;
////    }
////    /// <summary>
////    /// 最初に１回だけ呼ぶ
////    /// </summary>
////    public void Initial()
////    {
////        //アナログ入力の入力範囲等の設定(-127～127の範囲で入力するのでセンターの範囲をセット)
////        inputFader.SetFaderInitialize(-110, 110);
////    }
////    /// <summary>
////    /// 入力情報更新
////    /// </summary>
////    public void Update()
////    {
////        //アナログ入力の値を読み込む。(analogValueに(-127～127)の値を渡す
////        inputFader.SetFaderValue(_midi.xfader * 2 - 127);
////    }
////}

////public class inputMIDIRecord
////{
////    private int preangle;
////    private double deltaangle;
////    private int _LR;
////    private MIDIinput _midi;

////    private int prePos;
////    private int deltaPos;

////    public inputMIDIRecord(int LR, MIDIinput midi)
////    {
////        _LR = LR;
////        _midi = midi;
////        prePos = midi.Pos(LR);
////    }

////    public double DeltaAngle { get { return deltaangle; } }

////    public void Update()
////    {
////        deltaPos = _midi.Pos(_LR) - prePos;
////        prePos = _midi.Pos(_LR);
////        deltaangle = (double)deltaPos * 360 / 50;

////        //deltaangle = _midi.Speed(_LR) * 3.33;//1倍速で60/1秒なら200/60度
////        //; preangle = angle;
////    }
////}
