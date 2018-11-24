using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using NAudio.Midi;
using NAudio.Wave;

public enum EnumMidiResult
{
    L_TABLE_LEFT,
    L_TABLE_RIGHT,
    L_TABLE_TOUCH_ON,
    L_TABLE_TOUCH_OFF,
    R_TABLE_LEFT,
    R_TABLE_RIGHT,
    R_TABLE_TOUCH_ON,
    R_TABLE_TOUCH_OFF,
    X_FADER,
    NONE
}

public class Config
{


    public clsASIO ASIO;
    public clsMIDIINPUT MIDI;

    private List<MIDIConfigParam> configParams = new List<MIDIConfigParam>();

    /// <summary>
    /// ASIO周りの設定をまとめたクラス
    /// </summary>
    public class clsASIO
    {
        public string DriverName;
        public int Channeloffset;
    }
    public class clsMIDIevent
    {
        public int Channel;
        public int ControlChange;
        public int value;

        public override string ToString()
        {
            return "Ch:" + Channel.ToString() + " CC:" + ControlChange.ToString() + " Value:" + value.ToString();
        }

        public string ToolTipText()
        {
            return "Ch:" + Channel.ToString() + " CC:" + ControlChange.ToString() + " Value:" + value.ToString();
        }

        public clsMIDIevent()
        {
            Channel = -1;
            ControlChange = -1;
            value = -1;
        }

        public clsMIDIevent(int _channel, int _cc, int _valule)
        {
            Channel = _channel;
            ControlChange = _cc;
            value = _valule;
        }
    }
    public class clsMIDIINPUT
    {
        public string DeviceName;

        public TurnTable LeftTurnTable;
        public TurnTable RightTurnTable;
        public XFader Xfader;
        /// <summary>
        /// ターンテーブルの左右回転、タッチのMIDIイベント用クラス
        /// </summary>
        public class TurnTable
        {
            public clsMIDIevent LeftTrun;
            public clsMIDIevent RightTrun;
            public clsMIDIevent TouchOn;

            public TurnTable()
            {
                LeftTrun = new clsMIDIevent();
                RightTrun = new clsMIDIevent();
                TouchOn = new clsMIDIevent();

            }
        }
        /// <summary>
        /// クロスフェーダー用クラス
        /// </summary>
        public class XFader
        {
            public int Channel;
            public int ControlChange;
            public int Min;
            public int Max;
            
            public XFader()
            {
                Channel = -1;
                ControlChange = -1;
            }
            public clsMIDIevent MIDIevent()
            {
                return (new clsMIDIevent(Channel, ControlChange, Max));
            }
        }
        public clsMIDIINPUT()
        {
            LeftTurnTable = new TurnTable();
            RightTurnTable = new TurnTable();
            Xfader = new XFader();
        }
        /// <summary>
        /// クロスフェーダーのセット
        /// bLeft:true=mix ,false=max
        /// </summary>
        /// <param name="MIDIevent"></param>
        /// <param name="bMax"></param>
        public void SetXFader(clsMIDIevent MIDIevent,bool bLeft)
        {
            if (Xfader.Channel != MIDIevent.Channel && Xfader.ControlChange != MIDIevent.ControlChange)
            {
                Xfader.Channel = MIDIevent.Channel;
                Xfader.ControlChange = MIDIevent.ControlChange;
            }
            if (bLeft)
                Xfader.Min = MIDIevent.value;
            else
                Xfader.Max = MIDIevent.value;
        }
    }

    
    private  void SetConfigParams()
    {
        configParams.Add(new MIDIConfigParam(EnumMidiResult.L_TABLE_LEFT, MIDI.LeftTurnTable.LeftTrun));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.L_TABLE_RIGHT, MIDI.LeftTurnTable.RightTrun));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.L_TABLE_TOUCH_ON, MIDI.LeftTurnTable.TouchOn));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.R_TABLE_LEFT, MIDI.RightTurnTable.LeftTrun));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.R_TABLE_RIGHT, MIDI.RightTurnTable.RightTrun));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.R_TABLE_TOUCH_ON, MIDI.RightTurnTable.TouchOn));
        configParams.Add(new MIDIConfigParam(EnumMidiResult.X_FADER, MIDI.Xfader.MIDIevent()));
    }

    public EnumMidiResult CheckMidiEvent(ControlChangeEvent cc)
    {
        foreach (MIDIConfigParam param in configParams)
        {
            EnumMidiResult result = param.Check(cc);
            {
                if (result != EnumMidiResult.NONE)
                {
                    return result;
                }
            }
        }
        return EnumMidiResult.NONE;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Config()
    {
        ASIO = new clsASIO();
        MIDI = new clsMIDIINPUT();//null;// 
    }

    #region "チェック用ローカルクラス"
    private class MIDIConfigParam
    {
        EnumMidiResult Result;
        int Ch;
        int Control;
        int Value;
        public MIDIConfigParam(EnumMidiResult result, Config.clsMIDIevent ev)
        {
            Ch = ev.Channel;
            Control = ev.ControlChange;
            Value = ev.value;
            Result = result;
        }
        public EnumMidiResult Check(ControlChangeEvent cc)
        {
            if (Ch == cc.Channel && Control == (int)cc.Controller)
            {
                if (Value == cc.ControllerValue)
                {
                    return Result;
                }
                else
                {
                    switch (Result)
                    {
                        case EnumMidiResult.L_TABLE_TOUCH_ON:
                            return EnumMidiResult.L_TABLE_TOUCH_OFF;
                        case EnumMidiResult.R_TABLE_TOUCH_ON:
                            return EnumMidiResult.R_TABLE_TOUCH_OFF;
                        case EnumMidiResult.X_FADER:
                            return EnumMidiResult.X_FADER;
                        default:
                            return EnumMidiResult.NONE;
                    }
                }
            }
            else
            {
                return EnumMidiResult.NONE;
            }
        }
    }
    #endregion

    #region "XML読み書き"
    /// <summary>
    /// XML保存
    /// </summary>
    /// <param name="xmlFilename"></param>
    public void WriteXML(string xmlFilename)
    {

        //XMLファイルに保存
        System.Xml.Serialization.XmlSerializer serializer =
            new System.Xml.Serialization.XmlSerializer(typeof(Config));
        StreamWriter sw =
            new StreamWriter(xmlFilename, false, new UTF8Encoding(false));
        serializer.Serialize(sw, this);
        sw.Close();
    }

    /// <summary>
    /// XML読み込み
    /// </summary>
    /// <param name="xmlFilename"></param>
    /// <returns></returns>
    public static Config ReadXML(string xmlFilename)
    {
        Config rtn = null;
        if (System.IO.File.Exists(xmlFilename))
        {
            try
            {
                //XMLファイルから復元
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(Config));
                StreamReader sr = new StreamReader(xmlFilename, new UTF8Encoding(false));
                rtn = (Config)serializer.Deserialize(sr);
                sr.Close();
            }
            catch (System.OverflowException err)
            {
            }
        }
        else
        {
            rtn = new Config();
        }
        rtn.SetConfigParams();
        return rtn;

    }

    #endregion
}
