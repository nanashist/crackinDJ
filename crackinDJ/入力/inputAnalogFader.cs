using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

public class inputAnalogFader
{
    /// <summary>
    /// 最初に１回だけ呼ぶ
    /// </summary>
    public void Initial()
    {
        //アナログ入力の入力範囲等の設定(-127～127の範囲で入力するのでセンターの範囲をセット)
        inputFader.SetFaderInitialize( -110, 110);
    }
    /// <summary>
    /// 入力情報更新
    /// </summary>
    public void Update()
    {
        //アナログ入力の値を読み込む。(analogValueに(-127～127)の値を渡す
        int analogValue = 100;
        DX.DINPUT_JOYSTATE dINPUT_JOYSTATE;
        DX.GetJoypadDirectInputState(DX.DX_INPUT_PAD1, out dINPUT_JOYSTATE);
        analogValue = (dINPUT_JOYSTATE.Ry - dINPUT_JOYSTATE.Rx) * 127 / 2000;


        inputFader.SetFaderValue(analogValue);
    }
}
