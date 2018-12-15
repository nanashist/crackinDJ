using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
/// <summary>
/// 計算用
/// </summary>
public class datacalc
{
    public const double PI = 3.1415926535897932384626433832795;
    /// <summary>
    /// スクリーン幅
    /// </summary>
    public static int ScreenWidth = 1280;
    /// <summary>
    /// スクリーン幅
    /// </summary>
    public static int ScreenHeight = 720;

    /// <summary>
    /// センターライン
    /// </summary>
    public static int ScreenCenterLine = 640;
    /// <summary>
    /// 左ラインの中心座標
    /// </summary>
    public　static int ScreenLineL_X = 640 - 400;
    /// <summary>
    /// 右ラインの中心座標
    /// </summary>
    public　static int ScreenLineR_X = 640 + 400;
    /// <summary>
    /// 左皿の中心座標
    /// </summary>
    public　static int DiscLineL_X = 640 - 260;
    /// <summary>
    /// 右皿の中心座標
    /// </summary>
    public　static int DiscLineR_X = 640 + 260;
    /// <summary>
    /// 判定ラインのＹ座標
    /// </summary>
    public static int JudgeLineY = 540;
    /// <summary>
    /// 1小節の分解能(192で全音符 96で2分音符)
    /// </summary>
    public const int resolution = 192;

    public static int beatresolution { get { return resolution / 4; } }

    /// <summary>
    /// BPM計算のベース。
    /// BPMで割ると1小節の時間(msec)が得られる
    /// </summary>
    const Int64 BPMBASE = 60 * 4 * 1000;
    /// <summary>
    /// msec per step(1step辺りのミリ秒計算用の固定定数。1minutes4beatをmsec換算してstep数で割る。
    /// 単位はmsec。これをBPMで割ると1小節192step換算で1step辺りのmsecが計算出来る。
    /// </summary>
    const double MPS = BPMBASE / resolution;

    const double MPD = BPMBASE / 540;
    /// <summary>
    /// </summary>
    /// <param name="BPM"></param>
    /// <param name="millis"></param>
    /// <returns></returns>
    public static int millis2step(double BPM, int millis)
    {
        return (int)(millis * BPM / MPS);
    }

    public static int step2millis(double BPM, int step)
    {
        return (int)(step * MPS / BPM);

    }

    public static int millis2dot(double BPM, int millis)
    {
        return (int)(millis * BPM / MPD);
    }

    public static int step2dot(int step)
    {
        return (int)(step * MPS / MPD);
    }

    /// <summary>
    /// 今の進行dot数と曲データのstepからY座標(下)を返却
    /// </summary>
    /// <param name="nowdot"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static int realtimey(int nowdot, int step)
    {
        return (int)(540 - (datacalc.step2dot(step) - nowdot));
    }
    /// <summary>
    /// 左ラインの中心線と右ラインの中心線を返す
    /// </summary>
    /// <param name="lr"></param>
    /// <returns></returns>
    public static int realtimelinex(int lr)
    {
        if (lr > 0)
        {
            return ScreenLineL_X;
        }
        else
        {
            return ScreenLineR_X;
        }
    }
    public static int realtimerecordx(int lr)
    {
        if (lr > 0)
        {
            return DiscLineL_X;
        }
        else
        {
            return DiscLineR_X;
        }
    }

}
