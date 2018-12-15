using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

/// <summary>
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
    ////このメンバの値はシリアル化しないようにする
    //[System.Xml.Serialization.XmlIgnore]
    //public string Message;

    public List<OneData> quedata;
    public List<OneData> scratchdata;
    /// <summary>
    /// 引数なしのコンストラクタ（がないとシリアライズ出来ないので）
    /// </summary>
    public CdjXmlData()
    {
        BPM = 0;
        TITLE = "";
        LEVEL = 0;
        quedata = new List<OneData>();
        scratchdata = new List<OneData>();
    }

    public CdjXmlData(double bpm, string title, int level)
    {
        BPM = bpm;
        TITLE = title;
        LEVEL = level;
        quedata = new List<OneData>();
        scratchdata = new List<OneData>();

    }
    /// <summary>
    /// キューイングデータ追加
    /// </summary>
    /// <param name="_step"></param>
    /// <param name="value"></param>
    public void AddQue(int _step, int value)
    {
        OneData o;
        o = new OneData();
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
        OneData o;
        o = new OneData();
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
        OneData o;
        o = new OneData();
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
        OneData pre = null;
        Int16 lr = CdjData.Left;
        foreach (OneData o in quedata)
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
        foreach (OneData o in scratchdata)
        {
            foreach (OneData q in quedata)
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


    #region XML読み書き
    /// <summary>
    /// 曲データをファイルに保存(パス区切りを/で書かないとエラーが出る)
    /// </summary>
    /// <param name="fileName"></param>
    public void XmlSerialize(string fileName)
    {
        //XMLファイルに保存
        XmlSerializer serializer =
            new XmlSerializer(typeof(CdjXmlData));
        StreamWriter sw =
            new StreamWriter(fileName, false, new UTF8Encoding(false));
        serializer.Serialize(sw, this);
        sw.Close();
    }

    /// <summary>
    /// 曲データをファイルから読み込み(パス区切りを/で書かないとエラーが出る)
    /// </summary>
    /// <param name="fileName"></param>
    public void XmlDeserialize(string fileName)
    {
        //XMLファイルから復元
        XmlSerializer serializer =
            new XmlSerializer(typeof(CdjXmlData));
        StreamReader sr = new StreamReader(fileName, new UTF8Encoding(false));
        CdjXmlData obj =
            (CdjXmlData)serializer.Deserialize(sr);
        sr.Close();

        //Dictionaryに戻す
        this.BPM = obj.BPM;
        this.LEVEL = obj.LEVEL;
        this.TITLE = obj.TITLE;
        this.quedata = obj.quedata;
        this.scratchdata = obj.scratchdata;
        this.DataFinalize();
    }
    #endregion
}
public class TupleIntInt
{
    public int Item1;
    public int Item2;
    public TupleIntInt(int i1, int i2)
    {
        Item1 = i1;
        Item2 = i2;
    }

}
/// <summary>
/// オブジェ1個分のクラス
/// </summary>
public class OneData : IComparable<OneData>
{
    public int startstep;
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
    public List<int> data;

    /// <summary>
    /// 表示実ステップと向き
    /// </summary>
    [System.Xml.Serialization.XmlIgnore]
    public List<TupleIntInt> StepList = new List<TupleIntInt>();

    public OneData()
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
        StepList = new List<TupleIntInt>();
        foreach (int i in data)
        {
            StepList.Add(new TupleIntInt(startstep + sum, i));
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
    public int CompareTo(OneData other)
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

