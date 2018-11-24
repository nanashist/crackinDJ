using DxLibDLL;
using NAudio.Wave;
using System;
using System.Collections.Generic;


/// <summary>
/// 縦棒でつながったスクラッチ一連
/// </summary>
public class ScratchUnit
{
    /// <summary>
    /// 表示用ステップ情報はここから
    /// </summary>
    public int startstep;
    public int endstep;
    public int lr;
    public List<ScratchChip> lstscratch;
    #region 描画オブジェクト
    private static DrawObject _scratch_frame;//緑の三角。ラインより下で赤
    private static DrawObject _scratch_frame_bad;//赤の三角。
    private static DrawObject _scratch_core;

    private static DrawObject _bar;//緑で少し大きい。ラインで消える
    private static DrawObject _bar_off;//グレーの四角。ラインより下でも表示
    /// <summary>
    /// 描画オブジェクトの設定
    /// </summary>
    /// <param name="scr"></param>
    /// <param name="scrcore"></param>
    /// <param name="bar"></param>
    /// <param name="barcore"></param>
    public static void SetDrawObject(DrawObject scr ,DrawObject scrbad,DrawObject scrcore,DrawObject bar,DrawObject barcore)
    {
        _scratch_frame_bad = scrbad;
        _scratch_frame = scr;
        _scratch_core = scrcore;
        _bar = bar;
        _bar_off = barcore;
    }

    #endregion
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="o"></param>
    public ScratchUnit(OneData o)
    {
        startstep = o.startstep;
        endstep = o.endstep;
        lr = o.lr;
        lstscratch = new List<ScratchChip>();
        foreach (Tuple<int, int> d in o.StepList)
        {
            ScratchChip s = new ScratchChip
            {
                judge = EnumJudge.NOTYET,
                step = d.Item1,
                data = d.Item2
            };
            lstscratch.Add(s);
        }

    }
    public void Draw()
    {
        DrawScratch();
        DrawScratchLine();
        DrawCore();
    }

    private void DrawScratch()
    {
        //スクラッチの向き描画
        foreach (ScratchChip sc in lstscratch)
        {
            int x, y,h,num;

            
            
            y = datacalc.realtimey(now.dot, sc.step);
            h = datacalc.JudgeLineY - y + _scratch_frame.height / 2;
 
            
            if (sc.data > 0)
            {
                x = datacalc.realtimelinex(lr);
                num = 0;
            }
            else
            {
                x = datacalc.realtimelinex(lr) + _scratch_frame.width;
                num = 1;
            }
            //判定までは赤表示
            if( sc.judge== EnumJudge.NOTYET || sc.judge == EnumJudge.BAD)
                _scratch_frame_bad.Draw(x, y, num);
            //判定ラインより上まで描画
            if(h>0)
                _scratch_frame.DrawUpperRect(x, y, num,h);

        }

    }
    /// <summary>
    /// スクラッチコアの描画
    /// </summary>
    private void DrawCore()
    {
        //スクラッチコア描画
        foreach (ScratchChip scr in lstscratch)
        {
            _scratch_core.Draw(datacalc.realtimelinex(lr), datacalc.realtimey(now.dot, scr.step));
        }

    }

    private void DrawScratchLine()
    {
        int y3 = -1;
        int y2 = datacalc.realtimey(now.dot, startstep);//上の端
        int y1 = datacalc.realtimey(now.dot, endstep);// 540 - (endstep - nowdot);//下の端
        if (y1 < datacalc.ScreenHeight && y2 > 0)
        {
            //描画範囲内なら描く
            int width = 12;

            int x = datacalc.realtimelinex(lr);
            int gh,ghoff;
            gh = _bar.gh;
            ghoff = _bar_off.gh;
            if (y2 > datacalc.JudgeLineY)
            {
                y3 = y2;
                y2 = datacalc.JudgeLineY;
            }
            if (y1 < datacalc.JudgeLineY)
            {
                DX.DrawModiGraph(x - (width / 2), y1, x + (width / 2), y1, x + (width / 2), y2, x - (width / 2), y2, gh, DX.TRUE);
            }
            else
            {
                y2 = y1;
            }
            if (y3 != -1)
            {
                DX.DrawModiGraph(x - (width / 2), y2, x + (width / 2), y2, x + (width / 2), y3, x - (width / 2), y3, ghoff, DX.TRUE);
            }
        }
    }
}

/// <summary>
/// スクラッチチップ１個
/// </summary>
public class ScratchChip
{
    /// <summary>
    /// 表示位置
    /// </summary>
    public int step;

    /// <summary>
    /// 向き
    /// </summary>
    public int data;

    /// <summary>
    /// 判定
    /// NOTYETなら未判定
    /// ライン超えて未判定ならBADに
    /// 向き一致で成功ならPERFECT
    /// 逆向きならGREAT
    /// </summary>
    public EnumJudge judge;
    public ScratchChip()
    {
        judge = EnumJudge.NOTYET;
    }

}