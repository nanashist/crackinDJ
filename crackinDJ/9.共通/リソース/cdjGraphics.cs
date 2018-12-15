using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///crackinDJで使う画像の定義

/// <summary>
/// 描画優先度。上にあるものほど後に描く(優先度高)
/// </summary>
public enum EnumPriority
{
    Particle,
    GameObject,
    JudgeObject,
    Frame,
    Waveform
}

public enum EnumGraphic
{
    //スクラッチ
    SCRATCHGREEN,       //緑の右、左(中心固定)
    SCRATCHRED,         //赤の右、左(中心固定)
    SCRATCHCORE,        //コア(中心固定)
    SCRATCHBARGREEN,    //まとまりの縦線(中心固定縦は変形)
    SCRATCHBARGRAY,     //まとまりの縦線(判定ラインより下)(中心固定縦は変形)

    //レコード
    DISCBASE,           //皿の絵(中心固定)
    DISCBACKLIGHT,      //皿の裏でビートに合わせて光る(中心固定)
    DISCNGMARK,         //カットイン失敗時の×印(中心固定)

    //キューイング
    DISCCUEPOINT,       //キュー位置が白く光る(中心固定)36
    DISCCUELEFT,        //残りキュー表示右(中心固定)37
    DISCCUERIGHT,       //残りキュー表示左(中心固定)37

    //フェーダー
    FADEROBJ,           //フェーダーの絵(中心固定？縦は下固定が良いかも)
    FADERLEFT,          //左の光(自由変形)
    FADERCENTER,        //真ん中の光(自由変形)
    FADERRIGHT,         //右の光(自由変形)

    //判定
    JUDGECOOL,          //(中心固定)
    JUDGEPERFECT,       //(中心固定)
    JUDGEGREAT,         //(中心固定)
    JUDGEGOOD,          //(中心固定)
    JUDGEBAD,           //(中心固定)

    //小節線
    LINEBEAT,           //4拍子の薄い線(自由変形)
    LINEMEASURE,        //ちょっと濃い線(自由変形)
    LINEJUDGE,          //判定ラインキラキラ(自由変形)8枚

    //パーティクル
    PARTICLESTARGREEN,  //ばらまく★(中心固定)3枚
    PARTICLESTARRED,    //ばらまく★(中心固定)
    PARTICLESTARPURPLE, //ばらまく★(中心固定)
    PARTICLEDISCRED,    //皿が消える時のボワ(中心固定)
    PARTICLEDISCPURPLE, //皿が消える時のボワ(中心固定)

    
    //ゲームフレーム（主に下半分
    FRAME,

}