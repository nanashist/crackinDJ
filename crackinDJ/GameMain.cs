using DxLibDLL;
using NAudio.Wave;
using System;
using System.Drawing;

/// <summary>
/// 曲選択後のメイン処理
/// </summary>
public static class GameMain 
{
    private static MIDIinput midiinput;

    public static void SetDrawObjects()
    {
        // 画像の読み込み
        DrawObject scr = new DrawObject("scr.png", DrawObject.EnumAlign.RIGHTCENTER, 2, 1);//スクラッチの左右向き緑
        DrawObject scrbad = new DrawObject("scrbad.png", DrawObject.EnumAlign.RIGHTCENTER, 2, 1);//スクラッチの左右向き緑
        DrawObject scrcore = new DrawObject("scrcore.png", DrawObject.EnumAlign.CENTERCENTER);//スクラッチの左右向き緑
        DrawObject bar = new DrawObject("bar.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject baroff = new DrawObject("baroff.png", DrawObject.EnumAlign.CENTERBOTTOM);

        DrawObject disk = new DrawObject("disk.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject light = new DrawObject("disklight.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject NGXMark = new DrawObject("xDisk.png", DrawObject.EnumAlign.CENTERCENTER);

        DrawObject disklightl = new DrawObject("disklightl.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject disklightr = new DrawObject("disklightr.png", DrawObject.EnumAlign.CENTERCENTER);

        DrawObject cutinL = new DrawObject("cutinL.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject cutinR = new DrawObject("cutinR.png", DrawObject.EnumAlign.CENTERBOTTOM);

        DrawObject diskshadowL = new DrawObject("diskshadowL.png", DrawObject.EnumAlign.CENTERBOTTOM, 1, 2);//アクティブ皿の裏ののびーる赤と紫
        DrawObject diskshadowR = new DrawObject("diskshadowR.png", DrawObject.EnumAlign.CENTERBOTTOM, 1, 2);//アクティブ皿の裏ののびーる赤と紫

        DrawObject barL = new DrawObject("playlinel.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject barR = new DrawObject("playliner.png", DrawObject.EnumAlign.CENTERBOTTOM);

        DrawObject cue = new DrawObject("cue.png", DrawObject.EnumAlign.CENTERCENTER, 1, 36);
        DrawObject cueL = new DrawObject("cueL.png", DrawObject.EnumAlign.CENTERCENTER, 1, 37);
        DrawObject cueR = new DrawObject("cueR.png", DrawObject.EnumAlign.CENTERCENTER, 1, 37);

        DrawObject fader = new DrawObject("fader.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject faderC = new DrawObject("faderC.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject faderL = new DrawObject("faderL.png", DrawObject.EnumAlign.CENTERBOTTOM);
        DrawObject faderR = new DrawObject("faderR.png", DrawObject.EnumAlign.CENTERBOTTOM);

        DrawObject particle = new DrawObject("star.png", DrawObject.EnumAlign.CENTERCENTER, 1, 3);

        DrawObject frame = new DrawObject("frame.png",DrawObject.EnumAlign.LEFTTOP);
        DrawObject rainbow = new DrawObject("Rainbow.png", DrawObject.EnumAlign.LEFTTOP,1,8);

        DrawObject barBeat = new DrawObject("barBeat.png", DrawObject.EnumAlign.LEFTTOP);
        DrawObject barMeasure = new DrawObject("barMeasure.png", DrawObject.EnumAlign.LEFTTOP);

        DrawObject COOL =new DrawObject("!!!COOL!!!.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject PERFECT = new DrawObject("PERFECT.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject GREAT = new DrawObject("GREAT.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject GOOD = new DrawObject("GOOD.png", DrawObject.EnumAlign.CENTERCENTER);
        DrawObject BAD = new DrawObject("BAD.png", DrawObject.EnumAlign.CENTERCENTER);

        DiscQueCutData.SetDrawObject(disk, cue, null, light,NGXMark);
        DiscQueCutData.SetLDrawObject(cueL, diskshadowL, cutinL, barL);
        DiscQueCutData.SetRDrawObject(cueR, diskshadowR, cutinR, barR);
        ScratchUnit.SetDrawObject(scr, scrbad, scrcore, bar, baroff);

        EffectParticle.SetDrawObject(particle);
        EffectJudge.SetDrawObject(COOL,PERFECT,GREAT,GOOD,BAD);
        EffectRecord.SetDrawObject(disklightl, disklightr);

        drawfader.setdrawobject(fader, faderC, faderL, faderR);
        DrawLowFrame.SetDrawObject(frame, rainbow);
        DrawBeatline.SetDrawObject(barBeat, barMeasure);

    }
    public static void DoInit()
    {
        midiinput = new MIDIinput("Traktor Kontrol S2 MK2 MIDI");
        midiinput.SetConfig(Config.ReadXML("crackinDJ.config"));
        midiinput.StartMonitoring();
    }

    public static void DoXFader()
    {
        while (true)
        {
            if (midiinput.xfader == 64) break;
        }
    }

    public static void DoMain(CdjData cdjdata)
    {


        //using NAudio.Wave;
        //シンプル再生(２つ作れば多重再生可能）

        IWaveProvider FloatStereo44100Provider;
        AudioFileReader reader;
        reader = new AudioFileReader(cdjdata.SOUND);
        reader.Volume = 0.1F;



        IWaveProvider stereo;
        if (reader.WaveFormat.Channels == 1)
        {
            if (reader.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
            {
                //NAudio.Wave.SampleProviders.MonoToStereoSampleProvider s = new NAudio.Wave.SampleProviders.MonoToStereoSampleProvider(reader);
                stereo = new Wave16ToFloatProvider(new MonoToStereoProvider16(new WaveFloatTo16Provider(reader)));
                WaveFormatConversionProvider conv = new WaveFormatConversionProvider(new WaveFormat(44100, 2), stereo);
            }
            else if (reader.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
            {
                stereo = new Wave16ToFloatProvider(new MonoToStereoProvider16(reader));
            }
            else
            {
                return;
            }

        }
        else
        {
            stereo = reader;
        }

        FloatStereo44100Provider = stereo;//最終的にこの形式に統一44100にするかどうかは検討の余地あり

        SoundDriver.AddWaveProvider(FloatStereo44100Provider, "main");
        SoundDriver.Play();



        //while (waveOut.PlaybackState == PlaybackState.Playing)
        //{
        //    Application.DoEvents();
        //    this.Text = reader.CurrentTime.ToString();
        //} // 再生の終了を待つ
        //  // 再生の終了を待たずにWaveOutのインスタンスが破棄されると、その時点で再生が停止する

        //inputAnalogFader inpfader = new inputAnalogFader();
        inputMIDIFader inpfader = new inputMIDIFader(midiinput);
        inputMIDIRecord RecordL = new inputMIDIRecord(CdjData.Left, midiinput);
        inputMIDIRecord RecordR = new inputMIDIRecord(CdjData.Right, midiinput);

        inpfader.Initial();

        SetDrawObjects();


        //GHplaylineL = DX.LoadGraph("playline.png");
        //GHplaylineR = DX.LoadGraph("playline2.png");

        int MovieGraphHandle;
        int MovieGraphHandle1, MovieGraphHandle2;

        MovieGraphHandle1 = DX.LoadGraph("B3_TYPE42.avi");
        MovieGraphHandle2 = DX.LoadGraph("E_Map_TYPE01a.avi");

        // 描画先を裏画面に変更
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        // 画像を左右に動かす処理のための変数を初期化

        now.set_startbpm(0, cdjdata.BPM);
        //now.set_startbpm(DX.GetNowCount(), music.BPM);
        Random random = new Random();


        int dbg = 0;
        // メインループ
        while (DX.ProcessMessage() != -1)
        {
            //再生終了で抜ける
/*
            if ((DX.GetJoypadInputState(DX.DX_INPUT_KEY_PAD1) & DX.PAD_INPUT_9) != 0)
            {
                waveOut.Dispose();
                break;
            }
            if(waveOut.PlaybackState == PlaybackState.Stopped)
            {
                waveOut.Dispose();
                break;
            }
*/
            // 画面をクリア
            DX.ClearDrawScreen();

            now.settime((int)reader.CurrentTime.TotalMilliseconds);
            cdjdata.SetStep(now.judgementlinestep);

            //ムービー処理
            {
                // 画像を描画する座標を更新
                if ((DX.GetJoypadInputState(DX.DX_INPUT_KEY_PAD1) & DX.PAD_INPUT_RIGHT) != 0)
                {
                    MovieGraphHandle = MovieGraphHandle1;
                }
                else
                {
                    MovieGraphHandle = MovieGraphHandle2;
                }
                // 画像を描画
                //ムービー
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
                //ムービー調整
                if (DX.GetMovieStateToGraph(MovieGraphHandle1) != 1)
                {
                    DX.SeekMovieToGraph(MovieGraphHandle1, 0);
                    DX.PlayMovieToGraph(MovieGraphHandle1);
                }
                if (DX.GetMovieStateToGraph(MovieGraphHandle2) != 1)
                {
                    DX.SeekMovieToGraph(MovieGraphHandle2, 0);
                    DX.PlayMovieToGraph(MovieGraphHandle2);
                }

            }

            //キューイングのディスクとか再生ラインとかカットイン矢印とかを描く
            foreach (DiscQueCutData o in cdjdata.lstquedata)
            {
                o.Cutin(inputFader.GetCutInState());
                if (o.ActiveState == EnumActiveState.NEXT)
                {
                    if (o.lr == CdjData.Left)
                    {
                        o.Queing(RecordL.DeltaAngle);
                    }
                    else
                    {
                        o.Queing(RecordR.DeltaAngle);
                    }
                }
            }

            if (cdjdata.nowlr == 1 && inputFader.GetFaderState()== EnumFaderState.RIGHT)
            {
                //waveOut.Volume = 0;
            }
            else if (cdjdata.nowlr == -1 && inputFader.GetFaderState() == EnumFaderState.LEFT)
            {
                //waveOut.Volume = 0;
            }
            else
            {
                //waveOut.Volume = DEF_VOLUME;
            }


            RecordL.Update();
            RecordR.Update();
            inpfader.Update();
            midiinput.update(DateTime.Now);
            //inputFader.GetFaderState();
            //----------------------------------------------------------------------------------------
            //デバッグ用

            DX.DrawString(0, 0, "fader" + inputFader.GetFaderValue(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 20, "cutin" + inputFader.GetCutInState().ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 40, "angle" + RecordR.DeltaAngle.ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 60, "Pos " + midiinput.Pos(-1).ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 80, "Spd " + midiinput.Speed(-1).ToString(), DX.GetColor(255, 255, 255));

            //----------------------------------------------------------------------------------------

            //描画処理
            DoDraw(cdjdata);


        }

    }

    /// <summary>
    /// 描画処理
    /// </summary>
    /// <param name="cdjdata"></param>
    private static void DoDraw(CdjData cdjdata)
    {


        //４拍子の線
        DrawBeatline.Draw();

        //判定ラインの下側の絵
        DrawLowFrame.Draw();

        //DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 228);

        drawfader.draw();

        //キューイングのディスクとか再生ラインとかカットイン矢印とかを描く
        foreach (DiscQueCutData o in cdjdata.lstquedata)
        {
            o.Draw();
        }
        //スクラッチユニット(シャカシャカで１つ)を1個ずつ描く
        foreach (ScratchUnit scunit in cdjdata.lstscratchunit)
        {
            scunit.Draw();
        }

        DrawEffect.Draw();

        // 裏画面の内容を表画面に反映する
        DX.ScreenFlip();
    }




}
