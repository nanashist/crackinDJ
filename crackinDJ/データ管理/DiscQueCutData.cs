﻿using DxLibDLL;
using System;

public enum EnumActiveState
{
    INACTIVE,
    ACTIVE,
    NEXT,
    PAST
}

/// <summary>
/// キューイング、カットイン１個分
/// </summary>
public class DiscQueCutData
{
    /// <summary>
    /// bActive=trueで今入力待ち状態
    /// </summary>
    public EnumActiveState ActiveState;
    /// <summary>
    /// 判定
    /// NOTYETなら未判定(過ぎてもNOTYETなら下まで表示しつづける？）
    /// タイミングばっちりで皿回転が一致ならCOOL
    /// タイミングばっちりならPERFECT
    /// ちょいずれならGREAT
    /// もうちょいずれならGOOD
    /// ずれずれはBAD
    /// </summary>
    public EnumJudge judge;

    public int lr;
    public int startstep;
    public int endstep;
    public int activemillis;//このタイミングから皿がアクティブになる(状態はNEXT)
    /// <summary>
    /// キューイングしている現在値(0で完了)
    /// </summary>
    public double quecount;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="que"></param>
    public void Queing(double que)
    {
        if(quecount > 0)
        {
            if (que < 0)
            {
                quecount += que;
                if (quecount < 0)
                {
                    quecount = 0;
                    DrawEffect.AddRecord(lr);
                }
            }

        }
    }
    /// <summary>
    /// カットイン
    /// </summary>
    /// <param name="cutstate"></param>
    public void Cutin(EnumFaderState cutstate)
    {
        if(this.ActiveState== EnumActiveState.NEXT)
        {
            if (quecount <= 0)
            {
                if(judge== EnumJudge.NOTYET)
                {
                    if ((lr == CdjData.Left && cutstate == EnumFaderState.LEFT) || (lr == CdjData.Right && cutstate == EnumFaderState.RIGHT))
                    {
                        if (Math.Abs(now.judgementlinestep - startstep) < 5)
                            judge = EnumJudge.PERFECT;//ここにタイミングで判定分岐を入れる（BADもあり得る）
                        else if (Math.Abs(now.judgementlinestep - startstep) < 7)
                            judge = EnumJudge.GREAT;//ここにタイミングで判定分岐を入れる（BADもあり得る）
                        else if (Math.Abs(now.judgementlinestep - startstep) < 10)
                            judge = EnumJudge.GOOD;//ここにタイミングで判定分岐を入れる（BADもあり得る）
                        else if (Math.Abs(now.judgementlinestep - startstep) > 96)
                            judge = EnumJudge.BAD;

                        Random random = new Random();
                        DrawEffect.AddJudge(judge, lr, 1);
                        for (int i = 0; i < 20; i++)
                            DrawEffect.AddParticle(now.millis + i * 10, (int)(datacalc.realtimelinex(lr) + 20 + random.NextDouble() * 10), (int)(datacalc.JudgeLineY), random.NextDouble() * 20 + 10, -random.NextDouble() * Math.PI * 1);

                    }
                }
            }
        }
    }
    #region 描画オブジェクト
    /// <summary>
    /// 左右別で用意する必要がある描画オブジェクト
    /// </summary>
    private class LRDrawObject
    {
        public DrawObject diskque;//キュー量。quecountに合わせて固定表示

        public DrawObject diskshadow;//半円が伸びたような影 bActive時に上から下まで伸びる

        public DrawObject cutin;//カットイン矢印
        public DrawObject bar;
    }
    private static LRDrawObject _ldraw;
    private static LRDrawObject _rdraw;

    private static DrawObject _disk;//皿bActiveなら回る Activeでないなら上部に固定
    private static DrawObject _arrow;//回る矢印bActiveなら回る Activeでないなら回らない
    private static DrawObject _diskbeat;//ビートに合わせて光る白い光 Activeでないなら描画しない
    private static DrawObject _cue;//キューのアクティブ部分を白くする
    private static DrawObject _X;//ばってん
    /// <summary>
    /// 読み込んだ画像をセット
    /// </summary>
    /// <param name="disk"></param>
    /// <param name="arrow"></param>
    /// <param name="diskbeat"></param>
    public static void SetDrawObject(DrawObject disk,DrawObject cue,DrawObject arrow,DrawObject diskbeat,DrawObject NGXmark)
    {
        _disk = disk;
        _arrow = arrow;
        _diskbeat = diskbeat;
        _cue = cue;
        _X = NGXmark;
    }
    /// <summary>
    /// 読み込んだ画像をセット（左の赤いやつ）
    /// </summary>
    /// <param name="diskque"></param>
    /// <param name="diskshadow"></param>
    /// <param name="cutin"></param>
    /// <param name="bar"></param>
    public static void SetLDrawObject(DrawObject diskque,DrawObject diskshadow,DrawObject cutin ,DrawObject bar)
    {
        _ldraw = new LRDrawObject
        {
            diskque = diskque,
            diskshadow = diskshadow,
            cutin = cutin,
            bar = bar
        };
    }
    /// <summary>
    /// 読み込んだ画像をセット（右の紫のやつ）
    /// </summary>
    /// <param name="diskque"></param>
    /// <param name="diskshadow"></param>
    /// <param name="cutin"></param>
    /// <param name="bar"></param>
    public static void SetRDrawObject(DrawObject diskque, DrawObject diskshadow, DrawObject cutin, DrawObject bar)
    {
        _rdraw = new LRDrawObject
        {
            diskque = diskque,
            diskshadow = diskshadow,
            cutin = cutin,
            bar = bar
        };
    }

    #endregion

    #region 描画処理
    public void Draw()
    {
        //赤と紫の縦帯
        DrawPlayline(now.dot);
        LRDrawObject lrdraw;
        if (lr == CdjData.Left)
        {
            lrdraw = _ldraw;
        }
        else
        {
            lrdraw = _rdraw;
        }

        if (this.judge== EnumJudge.BAD)
        {
            //失敗オブジェを描く（×印付き）
            //まだアクティブになっていない
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
            if (quecount > 0)
            {
                _disk.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep));
                //キューの量
                lrdraw.diskque.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep), (int)(quecount / 10));
                _X.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep));
            }
            //カットイン矢印
            lrdraw.cutin.Draw(datacalc.ScreenCenterLine, datacalc.realtimey(now.dot, startstep));
            _X.Draw(datacalc.ScreenCenterLine, datacalc.realtimey(now.dot, startstep));
        }
        else if(this.judge== EnumJudge.NOTYET)
        {
        //未判定オブジェを描く
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
            if (ActiveState == EnumActiveState.NEXT)//皿が光ったり回ったり
            {
                if (quecount > 0)
                {
                    //皿のびーるの赤と紫
                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 60);
                    lrdraw.diskshadow.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep), 0);
                    lrdraw.diskshadow.DrawModi(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep) + lrdraw.diskshadow.height, lrdraw.diskshadow.width, datacalc.JudgeLineY - datacalc.realtimey(now.dot, startstep), 1);
                    int y;
                    if (now.millis - activemillis < 100)
                    {
                        y = (int)(datacalc.realtimey(now.dot, startstep) + datacalc.JudgeLineY * (now.millis - activemillis) / 100);
                    }
                    else
                    {
                        y = datacalc.JudgeLineY;
                    }
                    if (y > datacalc.JudgeLineY) y = datacalc.JudgeLineY;

                    //皿がビートに合わせて光る
                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 128);
                    //double zoom = 1 - ((nowmillis % 500) * 1f / 1000);
                    double zoom = 1 - ((now.judgementlinestep % (datacalc.resolution / 4)) * 1f / (datacalc.resolution / 2));
                    _diskbeat.DrawZoomRotate(datacalc.realtimerecordx(lr), y, zoom, 0);
                    //皿
                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
                    double angle = (now.millis % 1000) * datacalc.PI / 500;
                    _disk.DrawZoomRotate(datacalc.realtimerecordx(lr), y, 1, angle);
                    double q = (quecount / 10);
                    if (q > 360) q = 360;
                    //キューの量
                    lrdraw.diskque.Draw(datacalc.realtimerecordx(lr), y, (int)q);
                    //キューの今の位置
                    _cue.Draw(datacalc.realtimerecordx(lr), y, (int)((quecount / 10 - 1) % 36));

                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, (int)(96 * (zoom - 0.5)));
                    _diskbeat.DrawZoomRotate(datacalc.realtimerecordx(lr), y, 1, 0);
                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);

                }
            }
            else
            {
                //まだアクティブになっていない
                if (quecount > 0)
                {
                    _disk.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep));
                    //キューの量
                    lrdraw.diskque.Draw(datacalc.realtimerecordx(lr), datacalc.realtimey(now.dot, startstep), (int)(quecount / 10));
                }
            }
            //カットイン矢印
            lrdraw.cutin.Draw(datacalc.ScreenCenterLine, datacalc.realtimey(now.dot, startstep));
        }
    }
    /// <summary>
    /// 縦帯の描画
    /// </summary>
    /// <param name="lr">1:L -1:R</param>
    /// <param name="nowdot">再生曲の現在のドット位置</param>
    /// <param name="startstep"></param>
    /// <param name="endstep"></param>
    private void DrawPlayline(int nowdot)
    {
        long y2 = datacalc.realtimey(nowdot, startstep);//上の端
        long y1 = datacalc.realtimey(nowdot, endstep);// 540 - (endstep - nowdot);//下の端
        if (y1 < datacalc.JudgeLineY && y2 > 0)
        {
            //描画範囲内なら描く
            int width = 40;
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 128);
            if (ActiveState == EnumActiveState.ACTIVE)
            {
                width = 80;
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, 200);
            }
            
            //if (y1 < datacalc.JudgeLineY && y2 > datacalc.JudgeLineY) { width = 80; }

            long x = datacalc.realtimelinex(lr);
            x -= (width / 2);
            int gh;
            if (lr == 1)
            {
                gh = _ldraw.bar.gh;
            }//左ライン
            else
            {
                gh = _rdraw.bar.gh;
            }//右ライン
            if (y1 < -135) y1 = (y2 % 135) - 135;
            if (y2 > datacalc.JudgeLineY) y2 = datacalc.JudgeLineY;
            DX.DrawRectGraph((int)x, (int)y1, 0, 0, width, (int)(y2 - y1), gh, DX.TRUE, DX.FALSE);
        }
    }
    #endregion
}

