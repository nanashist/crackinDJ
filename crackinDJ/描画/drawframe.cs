using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
/// <summary>
/// 画面下のフレームの描画
/// </summary>
public static class DrawLowFrame
{
    private static DrawObject _frame;
    private static DrawObject _rainbow;
    private static int rcount = 0;
    public static void SetDrawObject(DrawObject frame,DrawObject rainbow)
    {
        _frame = frame;
        _rainbow = rainbow;
    }
    public static void Draw()
    {
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        _frame.Draw(0, datacalc.JudgeLineY);
        _rainbow.DrawModi(0, datacalc.JudgeLineY,datacalc.ScreenWidth,2,rcount++);
        rcount = rcount % 8;
        //DX.DrawGraph(0, datacalc.JudgeLineY, GHframe, DX.TRUE);
    }
}
