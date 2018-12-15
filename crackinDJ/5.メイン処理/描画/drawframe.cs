using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
/// <summary>
/// 画面下のフレームの描画
/// </summary>
public static class DrawLowFrame
{
    private static int rcount = 0;
    public static void Draw()
    {
        MyDraw.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        MyDraw.Draw(EnumGraphic.FRAME,new MyDraw.DrawParam(0, datacalc.JudgeLineY, EnumPriority.Frame));
        MyDraw.DrawModi(EnumGraphic.LINEJUDGE,new MyDraw.DrawModiParam (0,datacalc.JudgeLineY,datacalc.ScreenWidth,2,EnumPriority.Frame));
        //_frame.Draw(0, datacalc.JudgeLineY);
        ////_rainbow.DrawModi(0, datacalc.JudgeLineY,datacalc.ScreenWidth,2,rcount++);
        //rcount = rcount % 8;
        //DX.DrawGraph(0, datacalc.JudgeLineY, GHframe, DX.TRUE);
    }
}
