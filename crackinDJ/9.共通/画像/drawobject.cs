using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;

#region Enum宣言

public enum EnumBLEND
{
    NOBLEND = 0,
    ALPHA = 1,
    ADD = 2,
    SUB = 3,
    MUL = 4
}
public enum EnumDraw
{
    DrawGraph,
    DrawModiGraph,
    DrawRectGraph,
    DrawRotaGraph
} 
#endregion

/// <summary>
/// 汎用(要GraphicObject,EnumGraphic)
/// 優先順位指定描画
/// GraphicObject指定でDrawしていって
/// Flushで描画内容反映
/// </summary>
public partial class MyDraw
{
    private static List<DrawItem> lstDrawObject =new List<DrawItem>();
    private static EnumBLEND lastblend = EnumBLEND.NOBLEND;
    private static int lastblendparam = 0;
    private static long frame = 0;

    /// <summary>
    /// 描画の際のブレンドモードをセットする
    /// </summary>
    /// <param name="BlendMode">DX_BLENDMODE_NOBLEND:ノーブレンド（デフォルト） 　DX_BLENDMODE_ALPHA:αブレンド　　DX_BLENDMODE_ADD:加算ブレンド　　DX_BLENDMODE_SUB:減算ブレンド　　DX_BLENDMODE_MUL:乗算ブレンド</param>
    /// <param name="BlendParam">描画ブレンドモードのパラメータ（０～２５５）</param>
    public static void SetDrawBlendMode(int BlendMode, int BlendParam)
    {
        lastblend = (EnumBLEND)BlendMode;
        lastblendparam = BlendParam;
    }

    #region 描画ブレンド付き(単発で以降の描画にブレンドの影響を出さない)
    /// <summary>
    /// メモリに読みこんだグラフィックの描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="GrHandle"></param>
    private static void DrawGraph(int x, int y, int GrHandle, EnumPriority priority, EnumBLEND blend, int blendparam)
    {
        DrawItem d = new DrawItem();
        d.xy.X = x;
        d.xy.Y = y;
        d.gh = GrHandle;
        d.priority = priority;
        d.blend = blend;
        d.blendparam = blendparam;
        d.drawmode = EnumDraw.DrawGraph;
        d.index = lstDrawObject.Count();
        lstDrawObject.Add(d);
        
    }
    /// <summary>
    /// メモリに読みこんだグラフィックの回転拡大描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="Zoom"></param>
    /// <param name="Angle"></param>
    /// <param name="GrHandle"></param>
    /// <returns></returns>
    private static void DrawRotaGraph(int x, int y,
          double Zoom, double Angle,
          int GrHandle, EnumPriority priority,
          EnumBLEND blend, int blendparam)
    {
        DrawItem d = new DrawItem();
        d.xy.X = x;
        d.xy.Y = y;
        d.zoom = Zoom;
        d.angle = Angle;
        d.gh = GrHandle;
        d.priority = priority;
        d.blend = blend;
        d.blendparam = blendparam;
        d.drawmode = EnumDraw.DrawRotaGraph;
        d.index = lstDrawObject.Count();
        lstDrawObject.Add(d);
    }
    /// <summary>
    /// メモリに読みこんだグラフィックの自由変形描画
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <param name="x3"></param>
    /// <param name="y3"></param>
    /// <param name="x4"></param>
    /// <param name="y4"></param>
    /// <param name="GrHandle"></param>
    /// <returns></returns>
    private static void DrawModiGraph(int x1, int y1, int x2, int y2,
              int x3, int y3, int x4, int y4,
              int GrHandle, EnumPriority priority,
              EnumBLEND blend, int blendparam)
    {
        DrawItem d = new DrawItem();
        d.xy.X = x1; d.xy.Y = y1;
        d.xy2.X = x2; d.xy2.Y = y2;
        d.xy3.X = x3; d.xy3.Y = y3;
        d.xy4.X = x4; d.xy4.Y = y4;
        d.gh = GrHandle;
        d.priority = priority;
        d.blend = blend;
        d.blendparam = blendparam;
        d.drawmode = EnumDraw.DrawModiGraph;
        d.index = lstDrawObject.Count();
        lstDrawObject.Add(d);
    }
    /// <summary>
    /// グラフィックの指定矩形部分のみを描画
    /// </summary>
    /// <param name="DestX">画面上の位置</param>
    /// <param name="DestY"></param>
    /// <param name="SrcX">グラフィックのどこの部分を描く？(左上座標)</param>
    /// <param name="SrcY"></param>
    /// <param name="Width">描画するグラフィックのサイズ</param>
    /// <param name="Height"></param>
    /// <param name="GraphHandle"></param>
    /// <returns></returns>
    private static void DrawRectGraph(int DestX, int DestY,
          int SrcX, int SrcY, int Width, int Height,
          int GrHandle, EnumPriority priority,
          EnumBLEND blend, int blendparam)
    {
        DrawItem d = new DrawItem();
        d.xy.X = DestX;
        d.xy.Y = DestY;
        d.xy2.X = SrcX;
        d.xy2.Y = SrcY;
        d.wh.X = Width;
        d.wh.Y = Height;
        d.gh = GrHandle;
        d.priority = priority;
        d.blend = blend;
        d.blendparam = blendparam;
        d.drawmode = EnumDraw.DrawRectGraph;
        d.index = lstDrawObject.Count();
        lstDrawObject.Add(d);
    }
    #endregion

    #region 描画GraphicObject(blend付きに迂回するだけ

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumgraphic"></param>
    /// <param name="param"></param>
    public static void Draw(EnumGraphic enumgraphic, DrawParam param)
    {
        if (param.UpperHeight < 0)
        {
            Draw(enumgraphic, param.X, param.Y, param.ArrayNum, param.Align, param.Priority, lastblend, lastblendparam);
        }
        else
        {
            DrawUpperRect(enumgraphic, param.X, param.Y, param.ArrayNum, param.UpperHeight, param.Priority, lastblend, lastblendparam);  
        }
    }
    /// <summary>
    /// 4点指定で描画(左上、右上、右下、左下)
    /// </summary>
    /// <param name="enumgraphic"></param>
    /// <param name="param"></param>
    public static void DrawModi(EnumGraphic enumgraphic, DrawModiParam param)
    {
        DrawModi(enumgraphic, param.P1, param.P2, param.P3, param.P4, param.ArrayNum, param.Priority, lastblend, lastblendparam);
    }
    /// <summary>
    /// 中心座標に対して拡大縮小描画
    /// </summary>
    /// <param name="enumgraphic"></param>
    /// <param name="param"></param>
    public static void DrawZoomRotate(EnumGraphic enumgraphic,DrawZoomParam param)
    {
        DrawZoomRotate(enumgraphic, param.X, param.Y, param.Zoom, param.Angle, param.ArrayNum, param.Priority, lastblend, lastblendparam);
    }

    //public static void Draw(EnumGraphic enumgraphic, int x, int y, EnumPriority priority) { Draw(enumgraphic, x, y, 0, EnumAlign.NONE, priority, lastblend, lastblendparam); }
    //public static void Draw(EnumGraphic enumgraphic, int x, int y, int num, EnumPriority priority) { Draw(enumgraphic, x, y, num, EnumAlign.NONE, priority, lastblend, lastblendparam); }
    //public static void Draw(EnumGraphic enumgraphic, int x, int y, int num, EnumAlign tmpalign, EnumPriority priority) { Draw(enumgraphic, x, y, num, tmpalign, priority, lastblend, lastblendparam); }
    //public static void DrawModi(EnumGraphic enumgraphic, int x, int y, int width, int height, EnumPriority priority) { DrawModi(enumgraphic, x, y, width, height, 0, priority, lastblend, lastblendparam); }
    //public static void DrawModi(EnumGraphic enumgraphic, Point p1, Point p2, Point p3, Point p4, EnumPriority priority) { DrawModi(enumgraphic, p1, p2, p3, p4, 0, priority, lastblend, lastblendparam); }
    //public static void DrawModi(EnumGraphic enumgraphic, int x, int y, int width, int height, int num, EnumPriority priority) { DrawModi(enumgraphic, x, y, width, height, num, priority, lastblend, lastblendparam); }
    //public static void DrawUpperRect(EnumGraphic enumgraphic, int x, int y, int num, int upperheight, EnumPriority priority){   DrawUpperRect(enumgraphic, x, y, num, upperheight, priority, lastblend, lastblendparam);   }
    //public static void DrawZoomRotate(EnumGraphic enumgraphic, int x, int y, double zoom, double angle, EnumPriority priority) { DrawZoomRotate(enumgraphic, x, y, zoom, angle, 0, priority, lastblend, lastblendparam); }
    //public static void DrawZoomRotate(EnumGraphic enumgraphic, int x, int y, double zoom, double angle, int num, EnumPriority priority) { DrawZoomRotate(enumgraphic, x, y, zoom, angle, num, priority, lastblend, lastblendparam); }

    
    /// <summary>
    /// 描画パラメータ
    /// </summary>
    public class DrawParam
    {
        public int X;
        public int Y;
        public int ArrayNum;
        public int UpperHeight;
        public EnumPriority Priority;
        public EnumAlign Align;

        public DrawParam(int x,int y,EnumPriority priority){
            Initialize(x, y, -1, 0, priority, EnumAlign.NONE);
        }
        public DrawParam(int x, int y, int num, EnumPriority priority, EnumAlign align)
        {
            Initialize(x, y, -1, num, priority, align);
        }

        /// <summary>
        /// パラメータフル指定コンストラクタ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="num"></param>
        /// <param name="priority"></param>
        /// <param name="align"></param>
        public DrawParam(int x, int y, int upperheight, int num, EnumPriority priority, EnumAlign align)
        {
            Initialize(x, y, upperheight, num, priority, align);
        }

        public void Initialize(int x, int y,int upperheight, int num, EnumPriority priority, EnumAlign align)
        {
            X = x; Y = y;
            Priority = priority;
            ArrayNum = num;
            Align = EnumAlign.NONE;
            UpperHeight = upperheight;
        }
    }
    /// <summary>
    /// 変形描画パラメータ
    /// </summary>
    public class DrawModiParam
    {
        public Point P1, P2, P3, P4;
        public int ArrayNum;
        public EnumPriority Priority;
        public DrawModiParam(int x, int y, int width, int height, EnumPriority priority)
        {
            Initialize(new Point(x, y), new Point(x + width, y), new Point(x + width, y + height), new Point(x, y + height), 0, priority);
        }
        public DrawModiParam(Point p1, Point p2, Point p3, Point p4, EnumPriority priority)
        {
            Initialize(p1, p2, p3, p4, 0, priority);
        }

        public DrawModiParam(Point p1, Point p2, Point p3, Point p4, int num, EnumPriority priority)
        {
            Initialize(p1, p2, p3, p4, num, priority);
        }

        public void Initialize(Point p1, Point p2, Point p3, Point p4, int num, EnumPriority priority)
        {
            P1 = p1; P2 = p2; P3 = p3; P4 = p4;
            ArrayNum = num;
            Priority = priority;
        }
    }
    /// <summary>
    /// 拡大縮小描画パラメータ
    /// </summary>
    public class DrawZoomParam
    {
        public int X;
        public int Y;
        public double Zoom;
        public double Angle;
        public int ArrayNum;
        public EnumPriority Priority;

        public DrawZoomParam(int x, int y, double zoom, double angle, int num, EnumPriority priority)
        {
            Initialize(x, y, zoom, angle, num, priority);
        }

        public DrawZoomParam(int x, int y, double zoom, double angle, EnumPriority priority)
        {
            Initialize(x, y, zoom, angle, 0, priority);
        }

        public void Initialize(int x, int y, double zoom, double angle, int num, EnumPriority priority){
            X = x; Y = y; Zoom = zoom; Angle = angle; ArrayNum = num;
            Priority = priority;
        }
    }
    #endregion

    #region 描画GraphicObject(フルパラメータ)

    /// <summary>
    /// 左上、右上、右下、左下を指定してそこに変形描画
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    private static void DrawModi(EnumGraphic enumgraphic, Point p1, Point p2, Point p3, Point p4, int num, EnumPriority priority, EnumBLEND blend, int blendparam)
    {
        GraphicObject o = GraphicObject.GetGraphicObject(enumgraphic);
        MyDraw.DrawModiGraph(
                         p1.X, p1.Y,
                         p2.X, p2.Y,
                         p3.X, p3.Y,
                         p4.X, p4.Y,
                        o.GHandle(frame, num), priority, blend, blendparam);

    }

    /// <summary>
    /// 配列版の描画Align指定付き
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="num"></param>
    /// <param name="tmpalign"></param>
    private static void Draw(EnumGraphic enumgraphic, int x, int y, int num, EnumAlign tmpalign, EnumPriority priority, EnumBLEND blend, int blendparam)
    {
        int dx, dy;
        GraphicObject o = GraphicObject.GetGraphicObject(enumgraphic);
        if (tmpalign == EnumAlign.NONE)
        {
            dx = o.dx; dy = o.dy;
        }
        else
        {
            o.GetAlignDeltaXY(tmpalign, out dx, out dy);
        }
        MyDraw.DrawGraph(x + dx, y + dy, o.GHandle(frame, num), priority, blend, blendparam);
    }

    /// <summary>
    /// 配列版の描画(上一部分だけ描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="num">配列番号</param>
    private static void DrawUpperRect(EnumGraphic enumgraphic, int x, int y, int num, int upperheight, EnumPriority priority, EnumBLEND blend, int blendparam)
    {
        GraphicObject o = GraphicObject.GetGraphicObject(enumgraphic);
        MyDraw.DrawRectGraph(x + o.dx, y + o.dy, 0, 0, o.width, upperheight, o.GHandle(frame,num), priority, blend, blendparam);
    }

    /// <summary>
    /// 拡大、回転表示(x,y)が中心座標になるのでalignのDelta(dx,dy)値は無視
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="zoom">1で元のまま</param>
    /// <param name="angle">0で元のままラジアン指定</param>
    private static void DrawZoomRotate(EnumGraphic enumgraphic, int x, int y, double zoom, double angle, int num, EnumPriority priority, EnumBLEND blend, int blendparam)
    {
        GraphicObject o = GraphicObject.GetGraphicObject(enumgraphic);
        MyDraw.DrawRotaGraph(x, y, zoom, angle, o.GHandle(frame, num), priority, blend, blendparam);
    }



    #endregion

    /// <summary>
    /// リストに溜め込んだものを全て描画してリストをクリア
    /// 毎フレーム呼ぶ
    /// </summary>
    public static void Flush()
    {
        DX.ClearDrawScreen();

        EnumBLEND preblend = EnumBLEND.NOBLEND;
        int preblendvalue = -1;
        lstDrawObject.Sort(new DrawItem());

        foreach (DrawItem obj in lstDrawObject)
        {
            if (preblend != obj.blend || preblendvalue != obj.blendparam)
            {
                MyDraw.SetDrawBlendMode((int)obj.blend, obj.blendparam);
            }
            switch (obj.drawmode)
            {
                case EnumDraw.DrawGraph:
                    DX.DrawGraph(obj.xy.X, obj.xy.Y, obj.gh, DX.TRUE);
                    break;
                case EnumDraw.DrawModiGraph:
                    DX.DrawModiGraph(obj.xy.X, obj.xy.Y, obj.xy2.X, obj.xy2.Y, obj.xy3.X, obj.xy3.Y, obj.xy4.X, obj.xy4.Y, obj.gh, DX.TRUE);
                    break;
                case EnumDraw.DrawRectGraph:
                    DX.DrawRectGraph(obj.xy.X, obj.xy.Y, obj.xy2.X, obj.xy2.Y, obj.wh.X, obj.wh.Y, obj.gh, DX.TRUE, DX.FALSE);
                    break;
                case EnumDraw.DrawRotaGraph:
                    DX.DrawRotaGraph(obj.xy.X, obj.xy.Y, obj.zoom, obj.angle, obj.gh, DX.TRUE);
                    break;
            }
        }
        DX.ScreenFlip();
        lstDrawObject.Clear();
        frame++;
        // 画面をクリア

    }
    //DX.DrawGraph(x + dx, y + dy, gh, DX.TRUE);
    //DX.DrawModiGraph(x + dx, y + dy,
    //                     x + dx + width, y + dy,
    //                     x + dx + width, y + dy + height,
    //                     x + dx, y + dy + height,
    //                    gh, DX.TRUE);
    //DX.DrawRectGraph(x + dx, y + dy, 0, 0, width, upperheight, gharray[num], DX.TRUE, DX.FALSE);
    //DX.DrawRotaGraph(x, y, zoom, angle, gh, DX.TRUE);

    /// <summary>
    /// 描画パラメータのみを持つサブクラス
    /// </summary>
    private class DrawItem : IComparer<DrawItem>
    {
        //小さいほど優先度高
        public EnumDraw drawmode;
        public int index;
        public EnumPriority priority;

        public int gh;
        public Point xy;
        public Point xy2;
        public Point xy3;
        public Point xy4;
        public Point wh;

        public double zoom;
        public double angle;

        public EnumBLEND blend;
        public int blendparam;

        public DrawItem()
        {
            gh = -1;
            index = 0;
            xy = new Point(0, 0);
            xy2 = new Point(0, 0);
            xy3 = new Point(0, 0);
            xy4 = new Point(0, 0);
            wh = new Point(0, 0);
            blend = EnumBLEND.NOBLEND;
            blendparam = 0;
        }

        public DrawItem(EnumBLEND _blend, int _blendparam)
        {
            gh = -1;
            index = 0;
            xy = new Point(0, 0);
            xy2 = new Point(0, 0);
            xy3 = new Point(0, 0);
            xy4 = new Point(0, 0);
            wh = new Point(0, 0);
            blend = _blend;
            blendparam = _blendparam;
        }

        public int Compare(DrawItem p1, DrawItem p2)
        {
            if (p1.priority > p2.priority) return -1;
            if (p1.priority < p2.priority) return 1;

            if (p1.index > p2.index) return 1;
            if (p1.index < p2.index) return -1;

            return 0;
        }

    }

}
