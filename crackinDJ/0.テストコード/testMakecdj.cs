using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 適当な曲データを作る。データ解像度は192
/// 8分が24,16分が12,24分が8
/// </summary>
public class testMakecdj
{
    public static CdjData TestMakeCDJData()
    {


        CdjData t = CdjXmlData.Read("c:/cdj.xml");

        CdjXmlData cdjXML;
        cdjXML = new CdjXmlData(120, "test", 5);
        cdjXML.SOUND = "Crackin'DJ_02.mp3";

        cdjXML.AddQue(1, 0, 0);
        cdjXML.AddScratch(192 + 24, new List<int> { 12, -12, 12, -12 });
        cdjXML.AddScratch(192 + 96, new List<int> { 24, 12, -12 });
        cdjXML.AddQue(2, 0, 0);
        cdjXML.AddQue(2, 24, 0);
        cdjXML.AddQue(2, 48, 0);
        cdjXML.AddQue(3, 0, 180);
        cdjXML.AddQue(4, 0, 360);

        cdjXML.DataFinalize();
        cdjXML.Write("c:/cdj.cd");
        
        CdjData cdj = cdjXML.MakeCdjData();
        return cdj;
    }

}