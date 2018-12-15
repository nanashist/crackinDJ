using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

/// <summary>
/// 初期化処理
/// </summary>
public static partial class GameManager
{

    public static void DoInit()
    {
        
        // ウインドウモードで起動
        DX.ChangeWindowMode(DX.TRUE);
        DX.SetGraphMode(datacalc.ScreenWidth, datacalc.ScreenHeight, 16);
        // ＤＸライブラリの初期化
        if (DX.DxLib_Init() < 0)
        {
            return;
        }

        SetGraphicObjects();

        Config config = Config.ReadXML("crackinDJ.config");
        //ここDLL読み込みに変える
        iinput = TestGetInput.GetInput();

        iinput.Initialize("");

        SoundDriver.Init(config.ASIO.DriverName, config.ASIO.Channeloffset);

        

        //midiinput = new MIDIinput("Traktor Kontrol S2 MK2 MIDI");
        //midiinput.SetConfig(Config.ReadXML("crackinDJ.config"));
        //midiinput.StartMonitoring();
    }

    public static void SetGraphicObjects()
    {

        object obj = new XMLGraphicData();
        MyUtil.XML.Read("gd.xml", ref obj);
        XMLGraphicData readxml = (XMLGraphicData)obj;
        //XMLから辞書にセット
        GraphicObject.SetDictionary(readxml.MakeGraphicObjectDictionary());

        // 画像の読み込み
        //GraphicObject scr = new GraphicObject("scr.png", GraphicObject.EnumAlign.RIGHTCENTER, 2, 1);//スクラッチの左右向き緑
        //GraphicObject scrbad = new GraphicObject("scrbad.png", GraphicObject.EnumAlign.RIGHTCENTER, 2, 1);//スクラッチの左右向き緑
        //GraphicObject scrcore = new GraphicObject("scrcore.png", GraphicObject.EnumAlign.CENTERCENTER);//スクラッチの左右向き緑
        //GraphicObject bar = new GraphicObject("bar.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject baroff = new GraphicObject("baroff.png", GraphicObject.EnumAlign.CENTERBOTTOM);

        //GraphicObject disk = new GraphicObject("disk.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject light = new GraphicObject("disklight.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject NGXMark = new GraphicObject("xDisk.png", GraphicObject.EnumAlign.CENTERCENTER);

        //GraphicObject disklightl = new GraphicObject("disklightl.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject disklightr = new GraphicObject("disklightr.png", GraphicObject.EnumAlign.CENTERCENTER);

        //GraphicObject cutinL = new GraphicObject("cutinL.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject cutinR = new GraphicObject("cutinR.png", GraphicObject.EnumAlign.CENTERBOTTOM);

        //GraphicObject diskshadowL = new GraphicObject("diskshadowL.png", GraphicObject.EnumAlign.CENTERBOTTOM, 1, 2);//アクティブ皿の裏ののびーる紫
        //GraphicObject diskshadowR = new GraphicObject("diskshadowR.png", GraphicObject.EnumAlign.CENTERBOTTOM, 1, 2);//アクティブ皿の裏ののびーる赤

        //GraphicObject barL = new GraphicObject("playlinel.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject barR = new GraphicObject("playliner.png", GraphicObject.EnumAlign.CENTERBOTTOM);

        //GraphicObject cue = new GraphicObject("cue.png", GraphicObject.EnumAlign.CENTERCENTER, 1, 36);
        //GraphicObject cueL = new GraphicObject("cueL.png", GraphicObject.EnumAlign.CENTERCENTER, 1, 37);
        //GraphicObject cueR = new GraphicObject("cueR.png", GraphicObject.EnumAlign.CENTERCENTER, 1, 37);

        //GraphicObject fader = new GraphicObject("fader.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject faderC = new GraphicObject("faderC.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject faderL = new GraphicObject("faderL.png", GraphicObject.EnumAlign.CENTERBOTTOM);
        //GraphicObject faderR = new GraphicObject("faderR.png", GraphicObject.EnumAlign.CENTERBOTTOM);

        //GraphicObject particle = new GraphicObject("star.png", GraphicObject.EnumAlign.CENTERCENTER, 1, 3);

        //GraphicObject frame = new GraphicObject("frame.png", GraphicObject.EnumAlign.LEFTTOP);
        //GraphicObject rainbow = new GraphicObject("Rainbow.png", GraphicObject.EnumAlign.LEFTTOP, 1, 8);

        //GraphicObject barBeat = new GraphicObject("barBeat.png", GraphicObject.EnumAlign.LEFTTOP);
        //GraphicObject barMeasure = new GraphicObject("barMeasure.png", GraphicObject.EnumAlign.LEFTTOP);

        //GraphicObject COOL = new GraphicObject("!!!COOL!!!.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject PERFECT = new GraphicObject("PERFECT.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject GREAT = new GraphicObject("GREAT.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject GOOD = new GraphicObject("GOOD.png", GraphicObject.EnumAlign.CENTERCENTER);
        //GraphicObject BAD = new GraphicObject("BAD.png", GraphicObject.EnumAlign.CENTERCENTER);

        //DiscQueCutData.SetGraphicObject(disk, cue, null, light, NGXMark);
        //DiscQueCutData.SetLGraphicObject(cueL, diskshadowL, cutinL, barL);
        //DiscQueCutData.SetRGraphicObject(cueR, diskshadowR, cutinR, barR);
        //ScratchUnit.SetGraphicObject(scr, scrbad, scrcore, bar, baroff);

        //EffectParticle.SetGraphicObject(particle);
        //EffectJudge.SetGraphicObject(COOL, PERFECT, GREAT, GOOD, BAD);
        //EffectRecord.SetGraphicObject(disklightl, disklightr);

        //drawfader.setGraphicObject(fader, faderC, faderL, faderR);
        //DrawLowFrame.SetGraphicObject(frame, rainbow);
        //DrawBeatline.SetGraphicObject(barBeat, barMeasure);

    }

    public static void DoXFader()
    {
        //while (true)
        //{
        //    if (midiinput.xfader == 64) break;
        //}
    }

}