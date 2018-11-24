using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class inputDigitalFader
{
    private int[] faderValue = new int[3];

    private int[] preKey = new int[3];//0,1,2左中右の入力状態
    private int lastKeyIndex;//最後に押されたキーを0,1,2で残す

    /// <summary>
    /// 最初に１回だけ呼ぶ
    /// </summary>
    public void Initial()
    {
        //デジタルだからここは固定で良い
        inputFader.SetFaderInitialize(-100, 100);
        preKey[0] = 0;
        preKey[1] = 0;
        preKey[2] = 0;
        faderValue[0] = -127;
        faderValue[1] = 0;
        faderValue[2] = 127;
    }

    /// <summary>
    /// 入力情報更新
    /// </summary>
    public void Update()
    {
        ConsoleKeyInfo c = Console.ReadKey();
        
        int[] nowKey = new int[3];
        nowKey[0] = 0;
        nowKey[1] = 0;
        nowKey[2] = 0;
        //入力を読み込む。ここに処理を書く============
        if (c.Key == ConsoleKey.LeftArrow) nowKey[0] = 1;
        if (c.Key == ConsoleKey.DownArrow) nowKey[1] = 1;
        if (c.Key == ConsoleKey.RightArrow) nowKey[2] = 1;
        //============================================
        if ((nowKey[0] + nowKey[1] + nowKey[2]) == 0)
        {
            //何も押されていない場合は前回値をそのままキープ
        }
        else if ((nowKey[0] + nowKey[1] + nowKey[2]) == 1)
        {
            //１つしか押されていないのでそれを渡す
            if (nowKey[0] == 1) lastKeyIndex = 0;
            if (nowKey[1] == 1) lastKeyIndex = 1;
            if (nowKey[2] == 1) lastKeyIndex = 2;
        }
        else
        {
            //複数押された場合はエッジ検出
            int[] trigKey = new int[3];//今回エッジ検出したキー
            int trigsum = 0;
            for (int i = 0; i < 3; i++)
            {
                trigKey[i] = 0;
                if (nowKey[i] == 1 && nowKey[i] != preKey[i])
                {
                    trigsum++;
                    trigKey[i] = 1;
                }
            }
            if (trigsum == 1)
            {
                if (trigKey[0] == 1) lastKeyIndex = 0;
                if (trigKey[1] == 1) lastKeyIndex = 1;
                if (trigKey[2] == 1) lastKeyIndex = 2;
            }
            else
            {
                //複数完全同時に押された場合は真ん中で
                lastKeyIndex = 1;
            }
        }
        inputFader.SetFaderValue(faderValue[lastKeyIndex]);
        preKey[0] = nowKey[0];
        preKey[1] = nowKey[1];
        preKey[2] = nowKey[2];
    }
}

