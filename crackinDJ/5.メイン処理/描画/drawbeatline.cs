using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DrawBeatline
{

    public static void Draw()
    {
        int y = (int)(now.judgementlinestep / datacalc.beatresolution);
        for (int n = 0; n < 5; n++)
        {
            if ((y + n) % 4 == 0)
            {
                if (now.dot % 2 == 1)
                    MyDraw.DrawModi(EnumGraphic.LINEMEASURE, new MyDraw.DrawModiParam(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2, EnumPriority.Frame));
                    //_measure.DrawModi(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2);
            }
            else
            {
                if (now.dot % 2 == 0)
                    MyDraw.DrawModi(EnumGraphic.LINEBEAT, new MyDraw.DrawModiParam(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2, EnumPriority.Frame));
                    //_beat.DrawModi(0, datacalc.realtimey(now.dot, (y + n) * datacalc.beatresolution), datacalc.ScreenWidth, 2);
            }
        }
        
    }
}

