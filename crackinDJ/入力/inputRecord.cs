using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

public class InputRecord
{
    private int preangle;
    private int deltaangle;
    private int _LR;

    public InputRecord(int LR)
    {
        _LR = LR;
    }

    public int DeltaAngle { get { return deltaangle; } }
    
    public void Update()
    {
        DX.GetJoypadDirectInputState(DX.DX_INPUT_PAD1, out DX.DINPUT_JOYSTATE dINPUT_JOYSTATE);
        int angle;
        int x, y;
        if (_LR == 1)
        {
            y = dINPUT_JOYSTATE.Y;
            x = dINPUT_JOYSTATE.X;
        }
        else
        {
            y = dINPUT_JOYSTATE.Rz;
            x = dINPUT_JOYSTATE.Z;
        }
        if (Math.Abs(y) + Math.Abs(x) > 900)
            angle = (int)(Math.Atan2(y,x) * 180 / Math.PI) + 180;
        else
            angle = -1;
        if(preangle == -1|| angle == -1)
        {
            deltaangle = 0;
        }
        else
        {
            deltaangle = preangle - angle;
            if (deltaangle > 300) deltaangle -= 360;
            if (deltaangle < -300) deltaangle += 360;
        }

        preangle = angle;
    }
}
