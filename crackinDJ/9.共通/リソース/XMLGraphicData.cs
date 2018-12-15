using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 汎用(要GraphicObject)
/// 使用するグラフィックをXMLで一括管理
/// </summary>
public class XMLGraphicData
{
    public List<RECORD> LIST;

    public class RECORD
    {
        public EnumGraphic Name;
        public string FileName;
        public EnumAlign Align;
        public int Xdiv;
        public int Ydiv;
        public int AnimeFrame;
    }

    public XMLGraphicData()
    {
        LIST = new List<RECORD>();
    }
    /// <summary>
    /// GraphicObjectの追加
    /// デフォルトは縦分割数でアニメーションする
    /// アニメーション速度はAnimeFrameで指定
    /// </summary>
    /// <param name="name">Enumの名前</param>
    /// <param name="filename">ファイル名</param>
    /// <param name="align">Drawしたときの表示位置</param>
    /// <param name="xdiv">横分割数</param>
    /// <param name="ydiv">縦分割数</param>
    /// <param name="AnimeFrame"></param>
    public void Add(EnumGraphic name, string filename, EnumAlign align, int xdiv, int ydiv,int animeframe)
    {
        RECORD record = new RECORD();
        record.Name = name;
        record.FileName = filename;
        record.Align = align;
        record.Xdiv = xdiv;
        record.Ydiv = ydiv;
        record.AnimeFrame = animeframe;
        LIST.Add(record);
    }
    ///// <summary>
    ///// EnumGraphic指定で取り出せるように辞書化
    ///// </summary>
    ///// <returns></returns>
    //public Dictionary<EnumGraphic, RECORD> ToDictionary()
    //{
    //    Dictionary<EnumGraphic, RECORD> rtn = new Dictionary<EnumGraphic, RECORD>();
    //    foreach (RECORD rec in LIST)
    //    {
    //        rtn.Add(rec.Name, rec);
    //    }
    //    return rtn;
    //}

    public Dictionary<EnumGraphic, GraphicObject> MakeGraphicObjectDictionary()
    {
        Dictionary<EnumGraphic, GraphicObject> rtn = new Dictionary<EnumGraphic, GraphicObject>();
        foreach (RECORD rec in LIST)
        {
            GraphicObject go = new GraphicObject(rec.Name,rec.FileName, rec.Align, rec.Xdiv, rec.Ydiv,rec.AnimeFrame);
            rtn.Add(rec.Name, go);
        }
        return rtn;
    }

}

