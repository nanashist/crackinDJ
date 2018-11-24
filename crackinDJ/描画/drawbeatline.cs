using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DrawBeatline
{
    private static DrawObject _beat;
    private static DrawObject _measure;
    public static void SetDrawObject(DrawObject beat, DrawObject measure)
    {
        _beat = beat;
        _measure = measure;
    }

    public static void Draw()
    {
        int y = (int)(now.judgementlinestep / datacalc.beatresolution);
        for (int n = 0; n < 5; n++)
        {
            if ((y + n) % 4 == 0)
            {
                if (now.dot % 2 == 1)
                    _measure.DrawModi(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2);
            }
            else
            {
                if(now.dot % 2 == 0)
                    _beat.DrawModi(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2);
            }
        }
        
    }
}

