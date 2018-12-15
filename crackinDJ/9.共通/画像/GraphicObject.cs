using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;

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
    RIGHTBOTTOM,
    NONE
}

/// <summary>
/// Alignment指定可能な描画オブジェクト（配列も可能）
/// </summary>
public class GraphicObject
{
    #region 共有
    private static Dictionary<EnumGraphic, GraphicObject> _dctGraphicObject;

    public static void SetDictionary(Dictionary<EnumGraphic, GraphicObject> dctGraphicObject)
    {
        _dctGraphicObject = dctGraphicObject;
    }

    /// <summary>
    /// XMLから読み込んだGraphicObjectをEnumGraphic指定で取得
    /// ここで実際の画像ファイルを読み込んでインスタンス化
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static GraphicObject GetGraphicObject(EnumGraphic g)
    {
        if (_dctGraphicObject.ContainsKey(g))
        {
            return _dctGraphicObject[g];
        }
        else
        {
            return null;
        }
    }
    #endregion


    public EnumGraphic name;
    /// <summary>
    /// テンキーの位置に基準座標をずらす。初期は左上=7
    /// </summary>
    public EnumAlign align = EnumAlign.LEFTTOP;
    /// <summary>
    /// グラフィックハンドル
    /// </summary>
    private int gh;
    /// <summary>
    /// グラフィックハンドル配列
    /// </summary>
    private int[] gharray;
    /// <summary>
    /// 画像幅、高さ
    /// </summary>
    public int width, height;
    /// <summary>
    /// Alignによる補正
    /// </summary>
    public int dx, dy;
    /// <summary>
    /// 配列でアニメーションする(animepatternコマ数)
    /// </summary>
    public int animepattern;
    /// <summary>
    /// 何フレームごとにパターンが変わるか
    /// </summary>
    public int animeframe;

    public int GHandle(long frame,int num)
    {
        if (animeframe == -1 || animepattern ==0)
        {
            return gh;
        }
        else
        {
            if (animepattern == 1)
            {
                return gharray[num];
            }
            else
            {
                int ptn = (int)((long)(frame / animeframe) % animepattern);
                return gharray[num + animepattern * ptn];
            }
        }
    }

    public int GHandle(long frame)
    {
        if (animeframe == -1 || animepattern == 0)
        {
            return gh;
        }
        else
        {
            int ptn = (int)((long)(frame / animeframe) % animepattern);
            return gharray[ptn];
        }
    }

    /// <summary>
    /// コンストラクタ
    /// ファイル名と位置を指定
    /// xの分割数、yの分割数を指定して配列で使用可能に
    /// デフォルトはy分割数でアニメーションする
    /// </summary>
    /// <param name="_filename"></param>
    /// <param name="_align"></param>
    /// <param name="xdiv"></param>
    /// <param name="ydiv"></param>
    public GraphicObject(EnumGraphic enumgraphic, string _filename, EnumAlign _align, int xdiv, int ydiv, int _animeframe)
    {
        name = enumgraphic;
        gh = DX.LoadGraph(_filename);
        align = _align;
        DX.GetGraphSize(gh, out width, out height);
        SetAlign(align);
        animepattern = ydiv;
        animeframe = -1;
        if (xdiv * ydiv == 0)
        {
            return;
        }

        gharray = new int[xdiv * ydiv];
        DX.LoadDivGraph(_filename, xdiv * ydiv, xdiv, ydiv, width / xdiv, height / ydiv, gharray);
        DX.GetGraphSize(gharray[0], out width, out height);
        SetAlign(align);
        animeframe = _animeframe;
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

    public void GetAlignDeltaXY(EnumAlign align, out int deltax, out int deltay)
    {
        switch (align)
        {
            case EnumAlign.LEFTTOP:
                deltax = 0; deltay = 0;
                break;
            case EnumAlign.CENTERTOP:
                deltax = -width / 2; deltay = 0;
                break;
            case EnumAlign.RIGHTTOP:
                deltax = -width; deltay = 0;
                break;
            case EnumAlign.LEFTCENTER:
                deltax = 0; deltay = -height / 2;
                break;
            case EnumAlign.CENTERCENTER:
                deltax = -width / 2; deltay = -height / 2;
                break;
            case EnumAlign.RIGHTCENTER:
                deltax = -width; deltay = -height / 2;
                break;
            case EnumAlign.LEFTBOTTOM:
                deltax = 0; deltay = -height;
                break;
            case EnumAlign.CENTERBOTTOM:
                deltax = -width / 2; deltay = -height;
                break;
            case EnumAlign.RIGHTBOTTOM:
                deltax = -width; deltay = -height;
                break;
            default:
                deltax = 0; deltay = 0;
                break;
        }
    }
}
