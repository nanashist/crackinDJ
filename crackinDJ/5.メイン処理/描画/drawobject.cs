using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;

public class DrawObject
{
    public enum EnumAlign
    {
        LEFTTOP,
        LEFTCENTER,
        LEFTBOTTOM,
        CENTERTOP,
        CENTERCENTER,
        CENTERBOTTOM,
        RIGHTTOP,
        RIGHTCENTER,
        RIGHTBOTTOM
    }
    /// <summary>
    /// テンキーの位置に基準座標をずらす。初期は左上=7
    /// </summary>
    public EnumAlign align = EnumAlign.LEFTTOP;
    public int gh;
    public int[] gharray;
    public int width, height;
    public int dx, dy;

    /// <summary>
    /// x,yの位置にAlign指定で描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Draw(int x, int y)
    {
        if(gh == -1)
        {
            Console.Write("a");
        }
        DX.DrawGraph(x + dx, y + dy, gh, DX.TRUE);
    }
    /// <summary>
    /// 横幅、縦幅を指定して拡大縮小描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void DrawModi(int x, int y,int width,int height)
    {
        if (gh == -1)
        {
            Console.Write("a");
        }
        DX.DrawModiGraph(x + dx, y + dy,
                         x + dx + width, y + dy,
                         x + dx + width, y + dy + height,
                         x + dx, y + dy + height,
                        gh, DX.TRUE);
    }
    /// <summary>
    /// 左上、右上、右下、左下を指定してそこに変形描画
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    public void DrawModi(Point p1,Point p2,Point p3,Point p4)
    {
        if (gh == -1)
        {
            Console.Write("a");
        }
        DX.DrawModiGraph(
                         p1.X, p1.Y,
                         p2.X, p2.Y,
                         p3.X, p3.Y,
                         p4.X, p4.Y,
                        gh, DX.TRUE);

    }
    /// <summary>
    /// 横幅、縦幅を指定して拡大縮小描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="num"></param>
    public void DrawModi(int x, int y, int width, int height,int num)
    {
        if (gh == -1)
        {
            Console.Write("a");
        }
        DX.DrawModiGraph(x + dx, y + dy,
                         x + dx + width, y + dy,
                         x + dx + width, y + dy + height,
                         x + dx, y + dy + height,
                        gharray[num], DX.TRUE);
    }

    /// <summary>
    /// 配列版の描画Align指定付き
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="num"></param>
    /// <param name="tmpalign"></param>
    public void Draw(int x,int y,int num,EnumAlign tmpalign)
    {
        SetAlign(tmpalign);
        DX.DrawGraph(x + dx, y + dy, gharray[num], DX.TRUE);
        SetAlign(align);
    }
    /// <summary>
    /// 配列版の描画(上一部分だけ描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="num">配列番号</param>
    public void DrawUpperRect(int x, int y, int num,int upperheight)
    {
        DX.DrawRectGraph(x + dx, y + dy, 0, 0, width, upperheight, gharray[num], DX.TRUE, DX.FALSE);
    }

    /// <summary>
    /// 配列版の描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="num">配列番号</param>
    public void Draw(int x, int y, int num)
    {
        if(num>=0)
            DX.DrawGraph(x + dx, y + dy, gharray[num], DX.TRUE);
    }

    /// <summary>
    /// 拡大、回転表示(x,y)が中心座標になるのでalignのDelta値は無視
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="zoom">1で元のまま</param>
    /// <param name="angle">0で元のままラジアン指定</param>
    public void DrawZoomRotate(int x,int y,double zoom,double angle)
    {
        DX.DrawRotaGraph(x, y, zoom, angle, gh, DX.TRUE);
    }

    /// <summary>
    /// 拡大、回転表示(x,y)が中心座標になるのでalignのDelta値は無視
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="zoom">1で元のまま</param>
    /// <param name="angle">0で元のままラジアン指定</param>
    public void DrawZoomRotate(int x, int y, double zoom, double angle,int num)
    {
        DX.DrawRotaGraph(x, y, zoom, angle, gharray[num], DX.TRUE);
    }

    /// <summary>
    /// 未使用
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void DrawMirror(int x,int y)
    {
        switch (align)
        {
            case EnumAlign.LEFTTOP:
            case EnumAlign.LEFTCENTER:
            case EnumAlign.LEFTBOTTOM:
                DX.DrawTurnGraph(x - width, y + dy, gh, DX.TRUE);
                break;
            case EnumAlign.CENTERTOP:
            case EnumAlign.CENTERCENTER:
            case EnumAlign.CENTERBOTTOM:
                DX.DrawTurnGraph(x + dx, y + dy, gh, DX.TRUE);
                break;
            case EnumAlign.RIGHTTOP:
            case EnumAlign.RIGHTCENTER:
            case EnumAlign.RIGHTBOTTOM:
                DX.DrawTurnGraph(x , y + dy, gh, DX.TRUE);
                break;
        }

    }

    /// <summary>
    /// コンストラクタ
    /// ファイル名と位置を指定
    /// </summary>
    /// <param name="_filename"></param>
    /// <param name="_align"></param>
    public DrawObject(string _filename,EnumAlign _align)
    {
        gh = DX.LoadGraph(_filename);
        align = _align;
        DX.GetGraphSize(gh,out width, out height);
        SetAlign(align);
    }
    /// <summary>
    /// コンストラクタ
    /// ファイル名と位置を指定
    /// xの分割数、yの分割数を指定して配列で使用可能に
    /// </summary>
    /// <param name="_filename"></param>
    /// <param name="_align"></param>
    /// <param name="xdiv"></param>
    /// <param name="ydiv"></param>
    public DrawObject(string _filename, EnumAlign _align,int xdiv,int ydiv)
    {
        gh = DX.LoadGraph(_filename);
        DX.GetGraphSize(gh, out width, out height);


        gharray = new int[xdiv * ydiv];
        DX.LoadDivGraph(_filename, xdiv * ydiv, xdiv, ydiv, width / xdiv, height / ydiv, gharray);
        align = _align;
        DX.GetGraphSize(gharray[0], out width, out height);
        SetAlign(align);
    }

    private void SetAlign(EnumAlign align)
    {
        switch (align)
        {
            case EnumAlign.LEFTTOP:
                dx = 0; dy = 0;
                break;
            case EnumAlign.CENTERTOP:
                dx = -width / 2; dy = 0;
                break;
            case EnumAlign.RIGHTTOP:
                dx = -width; dy = 0;
                break;
            case EnumAlign.LEFTCENTER:
                dx = 0; dy = -height / 2;
                break;
            case EnumAlign.CENTERCENTER:
                dx = -width / 2; dy = -height / 2;
                break;
            case EnumAlign.RIGHTCENTER:
                dx = -width; dy = -height / 2;
                break;
            case EnumAlign.LEFTBOTTOM:
                dx = 0; dy = -height;
                break;
            case EnumAlign.CENTERBOTTOM:
                dx = -width / 2; dy = -height;
                break;
            case EnumAlign.RIGHTBOTTOM:
                dx = -width; dy = -height;
                break;
            default:
                dx = 0; dy = 0;
                break;
        }
    }
}
