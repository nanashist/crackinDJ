using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// cdjdataを作るためのクラス(こいつに依存するクラスの無いように)
/// 曲データメインクラス
/// </summary>
public class CdjXmlData
{
    /// <summary>
    /// BPM(小数可)
    /// </summary>
    public double BPM;
    /// <summary>
    /// 曲タイトル(アルファベット大文字限定？)
    /// </summary>
    public string TITLE;
    /// <summary>
    /// レベル
    /// </summary>
    public int LEVEL;
    /// <summary>
    /// 曲ファイル名
    /// </summary>
    public string SOUND;
    /// <summary>
    /// 開始位置(msec単位)
    /// </summary>
    public int GAP;
    /// <summary>
    /// ジャケットのデザイン
    /// </summary>
    public string JACKET;
    /// <summary>
    /// レコードの表面の丸い部分のデザイン
    /// </summary>
    public string LABEL;
    /// <summary>
    /// データ。使用時はquedataとscarachdataに分割して使う
    /// </summary>
    public List<RECORD> DATALIST;
    
    ////このメンバの値はシリアル化しないようにする
    //[System.Xml.Serialization.XmlIgnore]
    //public string Message;

    [System.Xml.Serialization.XmlIgnore]
    public List<RECORD> quedata;
    [System.Xml.Serialization.XmlIgnore]
    public List<RECORD> scratchdata;



    /// <summary>
    /// 引数なしのコンストラクタ（がないとシリアライズ出来ないので）
    /// </summary>
    public CdjXmlData()
    {
        BPM = 0;
        TITLE = "";
        LEVEL = 0;
        quedata = new List<RECORD>();
        scratchdata = new List<RECORD>();
    }

    public CdjXmlData(double bpm, string title, int level)
    {
        BPM = bpm;
        TITLE = title;
        LEVEL = level;
        quedata = new List<RECORD>();
        scratchdata = new List<RECORD>();

    }
    /// <summary>
    /// キューイングデータ追加
    /// </summary>
    /// <param name="_step"></param>
    /// <param name="value"></param>
    public void AddQue(int _step, int value)
    {
        RECORD o;
        o = new RECORD();
        o.SetQue(_step, value);
        quedata.Add(o);
        quedata.Sort();
    }
    /// <summary>
    /// キューイングデータ追加
    /// </summary>
    /// <param name="_step"></param>
    /// <param name="value"></param>
    public void AddQue(int _measno,int _step, int value)
    {
        RECORD o;
        o = new RECORD();
        o.SetQue(_measno * datacalc.resolution + _step, value);
        quedata.Add(o);
        quedata.Sort();
    }

    /// <summary>
    /// スクラッチデータ追加
    /// </summary>
    /// <param name="_step"></param>
    /// <param name="values"></param>
    public void AddScratch(int _step, List<int> values)
    {
        RECORD o;
        o = new RECORD();
        o.SetScratch(_step, values);
        scratchdata.Add(o);
    }
    /// <summary>
    /// データを使用可能な状態に整形する
    /// 曲データ完成後に一度呼ぶ
    /// </summary>
    public void DataFinalize()
    {
        quedata.Sort();
        RECORD pre = null;
        Int16 lr = CdjData.Left;
        foreach (RECORD o in quedata)
        {
            o.lr = lr;
            o.datatype = 0;
            if (pre != null)
            {
                pre.endstep = o.startstep;
            }
            pre = o;
            lr = (Int16)(-lr);
        }
        pre.endstep = 99999;
        foreach (RECORD o in scratchdata)
        {
            foreach (RECORD q in quedata)
            {
                if(q.startstep <= o.startstep && o.endstep <= q.endstep)
                {
                    o.lr = (Int16)(-q.lr);
                }
            }
            o.datatype = 1;
            o.SetScratchEndStep();
        }

    }

    public static CdjData Read(string filename)
    {
        CdjXmlData cdjXML;
        Object o = new CdjXmlData();
        MyUtil.XML.Read(filename,ref o);
        cdjXML = (CdjXmlData)o;
        CdjData t = cdjXML.MakeCdjData();

        cdjXML.quedata = new List<RECORD>();
        cdjXML.scratchdata = new List<RECORD>();
        foreach (RECORD r in cdjXML.DATALIST)
        {
            if (r.datatype == 0)
                cdjXML.quedata.Add(r);
            else
                cdjXML.scratchdata.Add(r);
        }
        cdjXML.DataFinalize();
        return cdjXML.MakeCdjData();
    }

    public void Write(string filename)
    {
        DATALIST = new List<RECORD>();
        DATALIST.AddRange(quedata);
        DATALIST.AddRange(scratchdata);
        DATALIST.Sort();

        MyUtil.XML.Write(filename, this);
    }

    /// <summary>
    /// XML読み込み後実際の曲データに変換して返す
    /// </summary>
    /// <returns></returns>
    public CdjData MakeCdjData()
    {
        CdjData rtn = new CdjData();
        rtn.BPM = this.BPM;
        rtn.TITLE = this.TITLE;
        rtn.LEVEL = this.LEVEL;
        rtn.SOUND = this.SOUND;
        rtn.GAP = this.GAP;
        rtn.JACKET = this.JACKET;
        rtn.LABEL = this.LABEL;
        rtn.lstquedata = new List<DiscQueCutData>();
        DiscQueCutData preque = new DiscQueCutData
        {
            startstep = 0
        };
        bool bfirst = true;
        foreach (RECORD o in this.quedata)
        {
            DiscQueCutData q = new DiscQueCutData();
            if (bfirst)
                q.ActiveState = EnumActiveState.NEXT;
            else
                q.ActiveState = EnumActiveState.INACTIVE;
            q.startstep = o.startstep;
            q.endstep = o.endstep;
            q.lr = o.lr;
            q.quecount = o.data[0];
            if (q.quecount < 360)
            {
                if (preque.endstep - preque.startstep > datacalc.resolution)
                {
                    q.activemillis = datacalc.step2millis(BPM, preque.endstep - datacalc.resolution);
                }
                else
                {
                    q.activemillis = datacalc.step2millis(BPM, preque.startstep);
                }
            }
            else
            {
                q.activemillis = datacalc.step2millis(BPM, preque.startstep);

            }

            q.judge = EnumJudge.NOTYET;
            rtn.lstquedata.Add(q);
            preque = q;
            bfirst = false;
        }
        rtn.lstscratchunit = new List<ScratchUnit>();
        foreach (RECORD o in this.scratchdata)
        {
            ScratchUnit scunit = MakeScratchUnit(o);
            rtn.lstscratchunit.Add(scunit);

        }
        return rtn;
    }

    public ScratchUnit MakeScratchUnit(RECORD o)
    {
        ScratchUnit rtn = new ScratchUnit();
        rtn.startstep = o.startstep;
        rtn.endstep = o.endstep;
        rtn.lr = o.lr;
        rtn.lstscratch = new List<ScratchChip>();
        foreach (StepData d in o.StepList)
        {
            ScratchChip s = new ScratchChip
            {
                judge = EnumJudge.NOTYET,
                step = d.Step,
                data = d.Data
            };
            rtn.lstscratch.Add(s);
        }
        return rtn;
    }


    //#region XML読み書き
    ///// <summary>
    ///// 曲データをファイルに保存(パス区切りを/で書かないとエラーが出る)
    ///// </summary>
    ///// <param name="fileName"></param>
    //public void XmlSerialize(string fileName)
    //{
    //    //XMLファイルに保存
    //    XmlSerializer serializer =
    //        new XmlSerializer(typeof(CdjXmlData));
    //    StreamWriter sw =
    //        new StreamWriter(fileName, false, new UTF8Encoding(false));
    //    serializer.Serialize(sw, this);
    //    sw.Close();
    //}

    ///// <summary>
    ///// 曲データをファイルから読み込み(パス区切りを/で書かないとエラーが出る)
    ///// </summary>
    ///// <param name="fileName"></param>
    //public void XmlDeserialize(string fileName)
    //{
    //    //XMLファイルから復元
    //    XmlSerializer serializer =
    //        new XmlSerializer(typeof(CdjXmlData));
    //    StreamReader sr = new StreamReader(fileName, new UTF8Encoding(false));
    //    CdjXmlData obj =
    //        (CdjXmlData)serializer.Deserialize(sr);
    //    sr.Close();

    //    //Dictionaryに戻す
    //    this.BPM = obj.BPM;
    //    this.LEVEL = obj.LEVEL;
    //    this.TITLE = obj.TITLE;
    //    this.quedata = obj.quedata;
    //    this.scratchdata = obj.scratchdata;
    //    this.DataFinalize();
    //}
    //#endregion
}
public class StepData
{
    public int Step;
    public int Data;
    public StepData(int step, int data)
    {
        Step = step;
        Data = data;
    }

}
/// <summary>
/// オブジェ1個分のクラス
/// </summary>
public class RECORD : IComparable<RECORD>
{
    public int MEASURE { get { return measureno; } set { measureno = value; } }
    public int STEP { get { return step; } set { step = value; } }
    public string TYPE
    {
        get
        {
            if (datatype == 0)
                return "QUE";
            else
                return "SCRATCH";
        }
        set
        {
            if (value == "SCRATCH")
                datatype = 1;
            else
                datatype = 0;
        }
    }
    public string DATA
    {
        get { return DataString(); }
        set
        {
            string[] datas = value.Split(new string[] { "," }, System.StringSplitOptions.None);
            data = new List<int>();
            foreach (string s in datas)
            {
                data.Add(Convert.ToInt16(s));
            }
        }
    }

    private int measureno;
    private int step;
    [System.Xml.Serialization.XmlIgnore]
    public int startstep
    {
        get
        {
            return measureno * datacalc.resolution + step;
        }
        set
        {
            measureno = (int)(value / datacalc.resolution);
            step = value % datacalc.resolution;
        }
    }
    [System.Xml.Serialization.XmlIgnore]
    public int endstep;//後で計算で出す
    [System.Xml.Serialization.XmlIgnore]
    public Int16 lr;//後で計算で出す1:l -1:r

    [System.Xml.Serialization.XmlIgnore]
    public Int16 datatype;//表示用

    /// <summary>
    /// cue時回転角度0でカットイン 
    /// scr時＋が左、－が右で次のスクラッチまでの音符長を指定。普通の8分4回なら24,-24,24,-24となる。（最後の数字は0なんでもいい）
    /// </summary>
    [System.Xml.Serialization.XmlIgnore]
    public List<int> data;

    /// <summary>
    /// 表示実ステップと向き
    /// </summary>
    [System.Xml.Serialization.XmlIgnore]
    public List<StepData> StepList = new List<StepData>();

    public RECORD()
    {
        startstep = 0;
        data = new List<int>();
    }
    public void SetQue(int _step, int value)
    {
        startstep = _step;
        data = new List<int> { value };
    }
    public void SetScratch(int _step, List<int> values)
    {
        startstep = _step;
        data = values;
    }

    /// <summary>
    /// スクラッチデータの末尾調整
    /// </summary>
    public void SetScratchEndStep()
    {
        int sum = 0;
        int last = 0;
        StepList = new List<StepData>();
        foreach (int i in data)
        {
            StepList.Add(new StepData(startstep + sum, i));
            sum += (int)System.Math.Abs(i);
            last = (int)System.Math.Abs(i);
        }
        sum -= last;
        endstep = startstep + sum;
    }
    // Implement the CompareTo method. For the parameter type, Use 
    // the type specified for the type parameter of the generic 
    // IComparable interface. 
    //
    public int CompareTo(RECORD other)
    {
        // The temperature comparison depends on the comparison of the
        // the underlying Double values. Because the CompareTo method is
        // strongly typed, it is not necessary to test for the correct
        // object type.
        return startstep.CompareTo(other.startstep);
    }

    public override string ToString()
    {
        if (datatype == 0)
        {
            return string.Format("{0:D3}.{1:D3}({2,5:D}) {3,3:D}", startstep / datacalc.resolution, startstep % datacalc.resolution, startstep, data[0]);
        }
        else
        {

            return string.Format("{0:D3}.{1:D3}({2,5:D}) {3}", startstep / datacalc.resolution, startstep % datacalc.resolution, startstep, DataString());
        }

    }
    public string DataString()
    {
        string s = "";
        bool first = true;
        foreach (int sdata in data)
        {
            if (!first) s += ",";
            s += sdata.ToString();
            first = false;
        }
        return s;
    }
}

