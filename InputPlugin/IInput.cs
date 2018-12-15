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

public interface IInput
{
    int Initialize(string devicename);
    /// <summary>
    /// 毎フレーム呼んで現在の状態をInputStateで返す
    /// </summary>
    /// <returns></returns>
    InputState Update();
    void Dispose();

}
