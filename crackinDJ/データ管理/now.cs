using DxLibDLL;

public static class now
{
    /// <summary>
    /// 曲開始からの経過ミリ秒数
    /// </summary>
    public static int millis;
    /// <summary>
    /// 判定ラインに来ているステップをdot数で
    /// </summary>
    public static int dot;
    /// <summary>
    /// 判定ラインに来ているステップ
    /// </summary>
    public static int judgementlinestep;
    /// <summary>
    /// y=0位置のステップ
    /// </summary>
    public static int screentopstep;

    private static int _start;
    private static double _bpm;

    public static void set_startbpm(int start,double bpm)
    {
        _start = start;
        _bpm = bpm;
    }
    public static void settime(int nowmillis)
    {
        millis = nowmillis - _start;
        dot = datacalc.millis2dot(_bpm, millis);
        judgementlinestep = datacalc.millis2step(_bpm, millis);//スクラッチラインに来ているステップ数
        screentopstep = judgementlinestep + datacalc.resolution;//表示可能な一番上のステップ数
    }

}
