using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InputState
{
    public int FaderValue;
    public EnumFaderState FaderState;
    public EnumFaderState CutinState;

    public Disc Left;
    public Disc Right;

    public InputState()
    {
        Left = new Disc();
        Right = new Disc();
    }
    public class Disc
    {
        public bool Touch;
        public double Speed;
        public double Angle;

        public Disc Clone()
        {
            Disc rtn = new Disc();
            rtn.Touch = Touch ;
            rtn.Speed =Speed;
            rtn.Angle = Angle;
            return rtn;
        }
    }

    public InputState Clone()
    {
        InputState rtn = new InputState();
        rtn.FaderValue = FaderValue;
        rtn.FaderState = FaderState;
        rtn.CutinState = CutinState;
        rtn.Left = Left.Clone();
        rtn.Right = Right.Clone();
        return rtn;
    }

    public void SetCutin(InputState prestate)
    {
        if (FaderState != prestate.FaderState)
        {
            CutinState = FaderState;
        }
        else
        {
            CutinState = EnumFaderState.NONE;
        }
    }
}
