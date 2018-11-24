using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
/*
            DrawEffect.Init();
            int num = Convert.ToInt16(txtNum.Text);
            double delay = Convert.ToDouble(txtDelay.Text);
            double v = Convert.ToDouble(txtV.Text);
            for (int i = 0; i < num; i++)
            {
                DrawEffect.AddParticle((long)(getmillis() + i * delay), (int)(100 + rnd.NextDouble() * 10), (int)(30 + rnd.NextDouble() * 10), rnd.NextDouble() * v, -rnd.NextDouble() * Math.PI * 1);
            }
            pct.Image = DrawEffect.Draw(getmillis());
            pct.Refresh();
*/
#region 効果クラス
/// <summary>
/// 画面効果ベースクラス
/// </summary>
abstract class EffectBase
{
    /// <summary>
    /// 描画
    /// 描画不要になった時点でfalseを返す
    /// </summary>
    /// <param name="nowmillis"></param>
    /// <returns></returns>
    abstract public bool Draw();
    protected long _startmillis;
    protected long _endmillis;

}

/*
class effectsample : EffectBase
{
    public override bool Draw()
    {

        return (this._startmillis > nowmillis);
    }
}
*/

/// <summary>
/// 判定
/// </summary>
class EffectJudge : EffectBase
{
    private static DrawObject _cool;
    private static DrawObject _perfect;
    private static DrawObject _great;
    private static DrawObject _good;
    private static DrawObject _bad;

    private readonly EnumJudge _judge;
    private DrawObject drawobj;
    private double _zoom;
    private int _lr;

    public double Zoom { get { return _zoom; } }

    public void AddZoom(double addz)
    {
        _zoom += addz;
    }

    public static void SetDrawObject(DrawObject cool,DrawObject perfect,DrawObject great,DrawObject good,DrawObject bad)
    {
        _cool = cool;
        _perfect = perfect;
        _great = great;
        _good = good;
        _bad = bad;
    }

    public EffectJudge(EnumJudge judge, int lr,double zoom)
    {
        _startmillis = now.millis;
        _endmillis = now.millis + 1000;
        _judge = judge;
        _zoom = zoom;
        _lr = lr;
        switch (_judge)
        {
            case EnumJudge.COOL:
                drawobj = _cool;
                break;
            case EnumJudge.PERFECT:
                drawobj = _perfect;
                break;
            case EnumJudge.GREAT:
                drawobj = _great;
                break;
            case EnumJudge.GOOD:
                drawobj = _good;
                break;
            case EnumJudge.BAD:
                drawobj = _bad;
                break;
        }

    }
    /// <summary>
    /// 判定の描画
    /// </summary>
    /// <returns></returns>
    public override bool Draw()
    {
        if (_endmillis < now.millis)
        {
            return false;
        }
        int x= datacalc.ScreenCenterLine;
        if (now.millis - _startmillis < 100)
        {
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, (int)((now.millis - _startmillis) * 2.5));
            if (_lr == CdjData.Left)
            {
                x -= (int)((100 - now.millis + _startmillis) * 2);
            }
            else
            {
                x += (int)((100 - now.millis + _startmillis) * 2);
            }
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        if (_endmillis - now.millis < 250)
        {
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, (int)((_endmillis - now.millis) ));

        }
        if(drawobj != null)
        {
            drawobj.DrawZoomRotate(x, (int)(datacalc.JudgeLineY - 20 - drawobj.height * _zoom), _zoom, 0);
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        return true;
    }
}

class EffectRecord : EffectBase
{
    private static DrawObject _recordl;
    private static DrawObject _recordr;

    private DrawObject drawobj;
    private int _lr;
    private int _x, _y;

    public static void SetDrawObject(DrawObject recordl, DrawObject recordr)
    {
        _recordl = recordl;
        _recordr = recordr;
    }

    public EffectRecord(int lr)
    {
        _startmillis = now.millis;
        _endmillis = now.millis + 300;
        _lr = lr;
        if (lr == CdjData.Left)
        {
            drawobj = _recordl;
        }
        else
        {
            drawobj = _recordr;
        }
        _y = datacalc.JudgeLineY;
        _x = datacalc.realtimerecordx(lr);

    }
    /// <summary>
    /// 判定の描画
    /// </summary>
    /// <returns></returns>
    public override bool Draw()
    {
        if (_endmillis < now.millis)
        {
            return false;
        }
        double zoom = (1 - ((double)(now.millis - _startmillis) / (_endmillis - _startmillis)));
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD,(int)(zoom*180 ));
        drawobj.DrawZoomRotate(_x, _y, zoom * 2.5, 0);
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        return true;
    }
}

/// <summary>
/// パーティクル
/// </summary>
class EffectParticle : EffectBase
{
    private static DrawObject particle;
    //y=vt+1/2gt^2
    private readonly double G = 1.5;
    private readonly  double _vx = 0;
    private readonly double _vy = 0;
    private readonly double _x;
    private readonly double _y;
    private readonly double _angle;
    public static void SetDrawObject(DrawObject _particle)
    {
        particle = _particle;
    }

    public override bool Draw()
    {
        //double deltasec = -0.1;
        double sec = (double)(now.millis - this._startmillis) / 100;
        double millis = now.millis - this._startmillis;
        if (now.millis > this._endmillis)
        {
            return false;
        }

        {
            if (sec > 0)
            {

                int y = (int)(_y + sec * _vy + G * sec * sec);
                int x = (int)(_x + sec * _vx);
                if (x > 0 && x < datacalc.ScreenWidth && y > 0 && y < datacalc.ScreenHeight)
                {
                    int toumei = (int)((this._endmillis - now.millis) * 2 );
                    if (toumei > 250)
                    {
                        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
                    }
                    else
                    {
                        DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, (int)(toumei ));
                    }
                    
                    particle.DrawZoomRotate(x, y, 1, (int)((millis * _angle / 60) % 360), (int)((millis / 50) % 3));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

    }

    public EffectParticle(long startmillis, int x, int y, double v, double radAngle)
    {
        this._startmillis = startmillis;
        this._endmillis = _startmillis + 1500;
        _x = x; _y = y;
        _angle = radAngle;
        _vx = v * Math.Cos(_angle);
        _vy = v * Math.Sin(_angle);
    }

}
#endregion


static class DrawEffect
{

    private static List<EffectBase> effectlist = new List<EffectBase>();
    public static void Draw()
    {
        List<EffectBase> removelist = new List<EffectBase>();
        foreach (EffectBase e in effectlist)
        {
            if (e.Draw() == false)
            {
                removelist.Add(e);
            }
        }
        foreach (EffectBase e in removelist)
            effectlist.Remove(e);
    }
    public static int Count { get { return effectlist.Count; } }

    public static void Init()
    {
    }
    /// <summary>
    /// 星追加
    /// </summary>
    /// <param name="startmillis">発生時刻</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="v"></param>
    /// <param name="radAngle"></param>
    public static void AddParticle(long startmillis, int x, int y, double v, double radAngle)
    {
        effectlist.Add(new EffectParticle(startmillis, x, y, v, radAngle));
    }
    /// <summary>
    /// 判定文字表示エフェクトの追加
    /// </summary>
    /// <param name="judge"></param>
    /// <param name="lr"></param>
    /// <param name="zoom"></param>
    public static void AddJudge(EnumJudge judge,int lr,double zoom)
    {
        EffectJudge deleffect=null;
        foreach(EffectBase e in effectlist)
        {

            if (e is EffectJudge ej)
            {
                zoom = ej.Zoom + 0.2;
                deleffect = ej;
            }
        }
        if (deleffect != null)
        {
            effectlist.Remove(deleffect);
        }
        effectlist.Add(new EffectJudge(judge, lr,zoom));
    }

    public static void AddRecord(int lr)
    {
        effectlist.Add(new EffectRecord(lr));
    }
}
