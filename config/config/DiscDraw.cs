using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

class DiscDraw
{
    public static void Draw(PictureBox pct,List<float> lstvalue)
    {
        int n = pct.Width;
        if (pct.Height < n) n = pct.Height;
        //bitmap作成
        Bitmap bmp = new Bitmap(n, n);
        Graphics g = Graphics.FromImage(bmp);
        //とりあえず円描く
        Pen p = new Pen(Color.Black, 1);
        g.DrawArc(p, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), 0, 360);

        //弧を描く
        SolidBrush brush;
        p = new Pen(Color.Red, 3);
        foreach(float f in lstvalue)
        {
            brush = new SolidBrush(Color.FromArgb(20, Color.Red));
            g.FillPie(brush, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), (float)(f*7.2), (float)(7.2));
        }

        pct.Image = bmp;
        //pct.Refresh();
    }

    public static void DrawRecord(PictureBox pct,float f)
    {
        Bitmap bmp = new Bitmap(pct.Image);
        //float f = (float)(DateTime.Now.Ticks - timer2start.Ticks) * 200 / 10000000;
        SolidBrush brush;
        Graphics g = Graphics.FromImage(bmp);
        for (int i = 0; i < 10; i++)
        {
            brush = new SolidBrush(Color.FromArgb(20, Color.Red));
            g.FillPie(brush, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), (float)(f - i * 3.33), (float)(5 + i * 3.33));
        }
        brush = new SolidBrush(Color.Red);
        g.FillPie(brush, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), f, 5);

        pct.Image = bmp;
        //pct.Refresh();
    }

    public static void DrawDiscPos(PictureBox pct, int Count)
    {
        Bitmap bmp = new Bitmap(pct.Image);
        //float f = (float)(DateTime.Now.Ticks - timer2start.Ticks) * 200 / 10000000;

        SolidBrush brush;
        Graphics g = Graphics.FromImage(bmp);
        brush = new SolidBrush(Color.Blue);
        g.FillPie(brush, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), Count * 360 / 50, 7.2f);

        pct.Image = bmp;
        //pct.Refresh();
    }

    public static void DrawTouch(PictureBox pct)
    {
        Bitmap bmp = new Bitmap(pct.Image);
        SolidBrush brush;
        Graphics g = Graphics.FromImage(bmp);
        brush = new SolidBrush(Color.FromArgb(100,Color.Blue));
        g.FillPie(brush, new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1), 0, 360);

        pct.Image = bmp;
        //pct.Refresh();

    }

}
