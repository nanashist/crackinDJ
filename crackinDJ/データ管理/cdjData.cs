using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// 曲データメインクラス
/// </summary>
public class CdjData
{
    /// <summary>
    /// 左の固定値
    /// </summary>
    public static Int16 Left = 1;
    /// <summary>
    /// 右の固定値
    /// </summary>
    public static Int16 Right = -1;
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

    public List<DiscQueCutData> lstquedata;
    public List<ScratchUnit> lstscratchunit;
    /// <summary>
    /// XMLからゲーム用クラスに変換
    /// </summary>
    public CdjData(CdjXmlData data)
    {
        BPM = data.BPM;
        TITLE = data.TITLE;
        LEVEL = data.LEVEL;
        SOUND = data.SOUND;
        GAP = data.GAP;
        JACKET = data.JACKET;
        LABEL = data.LABEL;
        lstquedata = new List<DiscQueCutData>();
        DiscQueCutData preque = new DiscQueCutData
        {
            startstep = 0
        };
        bool bfirst = true;
        foreach (OneData o in data.quedata)
        {
            DiscQueCutData q = new DiscQueCutData();
            if(bfirst)
                q.ActiveState = EnumActiveState.NEXT;
            else
                q.ActiveState = EnumActiveState.INACTIVE;
            q.startstep = o.startstep;
            q.endstep = o.endstep;
            q.lr = o.lr;
            q.quecount = o.data[0];
            if (q.quecount < 360)
            {
                if(preque.endstep - preque.startstep> datacalc.resolution)
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
            lstquedata.Add(q);
            preque = q;
            bfirst = false;
        }
        lstscratchunit = new List<ScratchUnit>();
        foreach( OneData o in data.scratchdata)
        {
            ScratchUnit scunit = new ScratchUnit(o);
            lstscratchunit.Add(scunit);

        }
    }
    /// <summary>
    /// 今のステップからアクティブなキューデータ(縦棒が太くなるところ)と次のキューデータをセット
    /// </summary>
    /// <param name="nowstep"></param>
    public void SetStep(long nowstep)
    {
        bool nowactiveflg = false;
        foreach (DiscQueCutData q in lstquedata)
        {
            if (nowactiveflg)
            {
                q.ActiveState = EnumActiveState.NEXT;
                nowactiveflg = false;
            }

            if (q.startstep <= nowstep && nowstep <= q.endstep)
            {
                nowactiveflg = true;
                _nowlr = q.lr;
                q.ActiveState = EnumActiveState.ACTIVE;
                
            }
            else
            {
                if (q.ActiveState == EnumActiveState.ACTIVE)
                {
                    //前までアクティブだった
                    q.ActiveState = EnumActiveState.PAST;
                    if (q.judge == EnumJudge.NOTYET)
                        q.judge = EnumJudge.BAD;
                }
            }
        }
    }

    private int _nowlr;
    public int nowlr { get { return _nowlr; } }
}

