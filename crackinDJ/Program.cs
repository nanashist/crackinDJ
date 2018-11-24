using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxLibDLL;

namespace crackindj
{
    
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {


            // ウインドウモードで起動
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetGraphMode(datacalc.ScreenWidth, datacalc.ScreenHeight, 16);
            // ＤＸライブラリの初期化
            if (DX.DxLib_Init() < 0)
            {
                return;
            }

            CdjXmlData cdjXML;
            cdjXML = new CdjXmlData(120, "test", 5);
            cdjXML.SOUND = "Crackin'DJ_02.mp3";
            //cdjXML.AddQue(192, 360);
            //cdjXML.addscratch(192 + 24, new List<int> { 12, -12, 12, -12 });
            //cdjXML.addscratch(192 + 96, new List<int> { 24,  12, -12 });
            //cdjXML.AddQue(384, 30);
            //cdjXML.AddQue(384 + 24, 10);
            //cdjXML.AddQue(384 + 48, 20);
            cdjXML.AddQue(1,0, 0);
            cdjXML.AddScratch(192 + 24, new List<int> { 12, -12, 12, -12 });
            cdjXML.AddScratch(192 + 96, new List<int> { 24, 12, -12 });
            cdjXML.AddQue(2, 0, 0);
            cdjXML.AddQue(2, 24, 0);
            cdjXML.AddQue(2, 48, 0);
            cdjXML.AddQue(3,0, 180);
            cdjXML.AddQue(4,0, 360);

            cdjXML.DataFinalize();
            CdjData cdj = new CdjData(cdjXML);
            /*
            Config config = new Config();
            config.ASIO.DriverName = "Traktor Kontrol S2 MK2";
            config.ASIO.Channeloffset = 2;
            config.WriteXML("config.XML");
            */
            Config config = Config.ReadXML("crackinDJ.config");

            SoundDriver.Init(config.ASIO.DriverName,config.ASIO.Channeloffset);

            GameMain.DoInit();
            GameMain.DoXFader();

            GameMain.DoMain(cdj);

            // ＤＸライブラリの後始末
            DX.DxLib_End();
        }
    }
}
