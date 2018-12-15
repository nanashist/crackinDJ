using DxLibDLL;
using NAudio.Wave;
using System;
using System.Drawing;

/// <summary>
/// 曲選択後のメイン処理
/// </summary>
public static partial class GameManager 
{

    public static void DoMain(CdjData cdjdata)
    {

        //ファイル読み込み、再生
        SoundDriver.MainFileLoad(cdjdata.SOUND);
        SoundDriver.Play();


        //inpfader.Initial();

        //ムービーファイル
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

        InputState preinputstate = new InputState();
        int dbg = 0;
        // メインループ
        while (DX.ProcessMessage() != -1 && DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 0 )
        {
            InputState inputstate = iinput.Update();//入力プラグインから現在の入力状況を返却
            //前回入力情報を加味
            inputstate.SetCutin(preinputstate);

            //再生終了で抜ける処理を書く

            // 画面をクリア
            DX.ClearDrawScreen();

            now.settime((int)SoundDriver.MainCurrentTime().TotalMilliseconds);
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
                
                o.Cutin(inputstate.CutinState);
                if (o.ActiveState == EnumActiveState.NEXT)
                {
                    if (o.lr == CdjData.Left)
                    {
                        o.Queing(inputstate.Left.Angle);
                    }
                    else
                    {
                        o.Queing(inputstate.Right.Angle);
                    }
                }
            }

            //if (cdjdata.nowlr == 1 && inputFader.GetFaderState()== EnumFaderState.RIGHT)
            //{
            //    //waveOut.Volume = 0;
            //}
            //else if (cdjdata.nowlr == -1 && inputFader.GetFaderState() == EnumFaderState.LEFT)
            //{
            //    //waveOut.Volume = 0;
            //}
            //else
            //{
            //    //waveOut.Volume = DEF_VOLUME;
            //}


            //----------------------------------------------------------------------------------------
            //デバッグ用
            /*
            DX.DrawString(0, 0, "fader" + inputFader.GetFaderValue(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 20, "cutin" + inputFader.GetCutInState().ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 40, "angle" +  RecordR.DeltaAngle.ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 60, "Pos " + midiinput.Pos(-1).ToString(), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 80, "Spd " + midiinput.Speed(-1).ToString(), DX.GetColor(255, 255, 255));
            */
            //----------------------------------------------------------------------------------------

            //描画処理
            DoDraw(cdjdata,inputstate);

            preinputstate = inputstate.Clone();
        }

    }

    /// <summary>
    /// 描画処理
    /// </summary>
    /// <param name="cdjdata"></param>
    private static void DoDraw(CdjData cdjdata,InputState inputstate)
    {


        //４拍子の線
        DrawBeatline.Draw();

        //判定ラインの下側の絵
        DrawLowFrame.Draw();

        //MyDraw.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 228);
        //フェーダーの描画（あとで）
        drawfader.draw(inputstate);

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
