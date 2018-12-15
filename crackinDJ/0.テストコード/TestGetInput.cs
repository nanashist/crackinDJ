using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestGetInput
{
    public static IInput GetInput()
    {
        //インストールされているプラグインを取得する
        IInput input = (IInput)Plugin.PluginInfo.GetPluginByName("IInput");//インターフェース名を文字列で渡す。結果をそのままキャスト取得できなければNullが返る

        return input;
    }
}
