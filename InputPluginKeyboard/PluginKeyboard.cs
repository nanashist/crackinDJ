using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public class PluginKeyboard : IInput
{
    public int Initialize(string DeviceName)
    {
        return 0;
    }
    public InputState Update()
    {
        InputState rtn = new InputState();
        if (KeyInput.GetKeyState((int)Keys.Left))
        {
            rtn.FaderValue = 0;
        }
        else if (KeyInput.GetKeyState((int)Keys.Right))
        {
            rtn.FaderValue = 127;
        }
        else
        {
            rtn.FaderValue = 64;
        }
        if (KeyInput.GetKeyState((int)Keys.Z))
        {
            rtn.Left.Speed = 1.0;
        }
        if (KeyInput.GetKeyState((int)Keys.X))
        {
            rtn.Left.Speed = -1.0;
        }
        //rtn.Left.Angle = 360 / 50;

        return rtn;
    }
    public void Dispose()
    {
    }
}
