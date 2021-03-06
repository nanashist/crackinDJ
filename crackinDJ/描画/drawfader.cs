﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DxLibDLL;
/// <summary>
/// フェーダーの光とフェーダーの描画
/// </summary>
public static class drawfader
{
    static DrawObject _fader,_faderC, _faderL, _faderR;
    public static void setdrawobject(DrawObject fader,DrawObject faderC,DrawObject faderL,DrawObject faderR)
    {
        _fader = fader;
        _faderC = faderC;
        _faderL = faderL;
        _faderR = faderR;
    }
    /// <summary>
    /// フェーダーの光とフェーダーの描画
    /// </summary>
    /// <param name="faderValue">フェーダーの値</param>
    public static void draw()
    {
        int Blenddelta = 0;// (int)((nowmillis / 10) % 40)*2; 
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 168 + Blenddelta);
        DrawObject _faderLR = _faderC;
        int width = 16 * 6;//緑、赤、紫、それぞれのフェーダーの幅(dot)
        int faderx = (int)(width * inputFader.FaderPercent());
        int faderLeftX = datacalc.ScreenWidth / 2 - 16 * 3;//フェーダーの光の左端
        double deltadelta = 0.05;
        double deltaLeft = (inputFader.FaderPercentCenterMin() - 0.5) * deltadelta;
        double deltaRight = (inputFader.FaderPercentCenterMax() - 0.5) * deltadelta;
        double deltaCLeft = (inputFader.FaderPercent() - 0.5 - 0.01) * deltadelta;
        double deltaCRight = (inputFader.FaderPercent() - 0.5 + 0.01) * deltadelta;
        //int deltax = 10
        if (inputFader.GetFaderState() == EnumFaderState.LEFT)
        {
            deltaLeft = (inputFader.FaderPercentMin() - 0.5) * deltadelta;
            //deltaCRight = -deltaCRight;
            deltaRight = -deltaRight;
            _faderLR = _faderL;
            faderLeftX -= width;//フェーダーの光の左端
            faderx += width;
        }
        else if (inputFader.GetFaderState() == EnumFaderState.RIGHT)
        {
            deltaRight = (inputFader.FaderPercentMax() - 0.5) * deltadelta;
            deltaLeft = -deltaLeft;
            _faderLR = _faderR;
            faderLeftX += width;//フェーダーの光の左端
            faderx -= width;

        }
        if (faderx > 8)
        {
            _faderLR.DrawModi(
                new Point((int)(faderLeftX * (1 + deltaLeft)), datacalc.JudgeLineY - 60),//左上
                new Point((int)(faderLeftX * (1 + deltaCLeft)) + faderx - 7, datacalc.JudgeLineY - 60),//右上
                new Point(faderLeftX + faderx - 7, datacalc.JudgeLineY + 16),//右下
                new Point(faderLeftX, datacalc.JudgeLineY + 16) //左下
                );
        }
        //if (faderx + 8 < width)
        {
            _faderLR.DrawModi(
                new Point((int)(faderLeftX * (1 + deltaCRight) + faderx + 7), datacalc.JudgeLineY - 60),//左上
                new Point((int)(faderLeftX * (1 + deltaRight) + width), datacalc.JudgeLineY - 60),//右上
                new Point(faderLeftX + width, datacalc.JudgeLineY + 16),//右下
                new Point(faderLeftX + faderx + 7, datacalc.JudgeLineY + 16) //左下
                );
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        _fader.Draw((int)(faderLeftX + faderx), datacalc.JudgeLineY + 16);
    }

}
