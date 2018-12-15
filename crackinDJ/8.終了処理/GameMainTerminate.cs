using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

/// <summary>
/// 終了処理
/// </summary>
public static partial class GameManager
{
    public static void DoTerminate()
    {
        SoundDriver.Stop();
        SoundDriver.Dispose();

        // ＤＸライブラリの後始末
        DX.DxLib_End();

    }

}
