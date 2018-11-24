using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public enum EnumFaderState
{
    NONE,
    LEFT,
    CENTER,
    RIGHT
}
public static class inputFader
{
    /// <summary>
    /// 今のフェーダー位置
    /// </summary>
    private static EnumFaderState _fader_state;
    private static EnumFaderState _fader_cutin_state;

    /// <summary>
    /// アナログ値仮に-127～127としえおく
    /// </summary>
    private static int _fader_value;
    private static int _fader_min;
    private static int _fader_max;
    private static int _fader_centermin;
    private static int _fader_centermax;
    /// <summary>
    /// 今のフェーダー位置
    /// </summary>
    /// <returns></returns>
    public static EnumFaderState GetFaderState()
    {
        return _fader_state;
    }
    /// <summary>
    /// 0～1が緑、-は左、1以上は右
    /// </summary>
    /// <returns></returns>
    public static double FaderPercent()
    {
        //return (double)((double)(_fader_value - _fader_min) / (_fader_max - _fader_min));
        return (double)((double)(_fader_value - _fader_centermin) / (_fader_centermax - _fader_centermin));
    }
    public static double FaderPercentCenterMin() { return (double)((double)(_fader_centermin - _fader_centermin) / (_fader_centermax - _fader_centermin)); }
    public static double FaderPercentCenterMax() { return (double)((double)(_fader_centermax - _fader_centermin) / (_fader_centermax - _fader_centermin)); }
    public static double FaderPercentMin() { return (double)((double)(_fader_min - _fader_centermin) / (_fader_centermax - _fader_centermin)); }
    public static double FaderPercentMax() { return (double)((double)(_fader_max - _fader_centermin) / (_fader_centermax - _fader_centermin)); }

    //public static double FaderPercentCenterMin() { return (double)((double)(_fader_centermin - _fader_min) / (_fader_max - _fader_min)); }
    //public static double FaderPercentCenterMax() { return (double)((double)(_fader_centermax - _fader_min) / (_fader_max - _fader_min)); }
    //public static double FaderPercentMin() { return (double)((double)(_fader_min - _fader_min) / (_fader_max - _fader_min)); }
    //public static double FaderPercentMax() { return (double)((double)(_fader_max - _fader_min) / (_fader_max - _fader_min)); }

    /// <summary>
    /// カットインタイミング（真ん中カットインは使わないけど返せる
    /// </summary>
    /// <returns></returns>
    public static EnumFaderState GetCutInState()
    {
        return _fader_cutin_state;
    }
    /// <summary>
    /// フェーダーの数値
    /// </summary>
    /// <returns></returns>
    public static int GetFaderValue()
    {
        return _fader_value;
    }

    public static void SetFaderInitialize( int mincenter, int maxcenter)
    {
        _fader_min = -127;
        _fader_max = 127;
        _fader_centermin = mincenter;
        _fader_centermax = maxcenter;
    }

    public static void SetFaderValue(int value)
    {
        _fader_value = value;
        if(value<_fader_min)_fader_value = _fader_min;
        if(value>_fader_max)_fader_value = _fader_max;

        _fader_cutin_state = EnumFaderState.NONE;
        if(_fader_value < _fader_centermin)
        {
            if (_fader_state != EnumFaderState.LEFT) _fader_cutin_state = EnumFaderState.LEFT;
            _fader_state = EnumFaderState.LEFT;
        }
        else if(_fader_value > _fader_centermax)
        {
            if (_fader_state != EnumFaderState.RIGHT) _fader_cutin_state = EnumFaderState.RIGHT;
            _fader_state = EnumFaderState.RIGHT;
        }else
        {
            if (_fader_state != EnumFaderState.CENTER) _fader_cutin_state = EnumFaderState.CENTER;
            _fader_state = EnumFaderState.CENTER;
        }
    }
}
