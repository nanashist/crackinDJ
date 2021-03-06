﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
/// <summary>
/// 汎用(要MyUtil)
/// 拡張DateTime
/// DateTimeEx.Init()を呼ぶとそこからの経過ミリ秒をMillisで取得可能
/// </summary>
[DebuggerDisplay("{Millis}msec")] 
public class DateTimeEx
{
    private static DateTime _basedate=DateTime.Now;
    private DateTime _datetime;

    public DateTimeEx(DateTime d)
    {
        _datetime = d;
    }

    public DateTimeEx(long ticks)
    {
        _datetime = new DateTime(ticks);
    }

    public DateTimeEx AddMilliseconds(double value)
    {
        return new DateTimeEx(_datetime.AddMilliseconds(value));
    }
    public long Ticks { get { return _datetime.Ticks; } }
    public long Millis { get { return (long)(_datetime.Ticks - _basedate.Ticks) / MyUtil.Ticks.ticks2msec; } }
    /// <summary>
    /// 基準時間の設定(呼んだときからの経過時間を計算可能になる）
    /// </summary>
    public static void Init(){_basedate=DateTime.Now;}

    public static DateTimeEx Now { get { return new DateTimeEx(DateTime.Now); } }

    public static bool operator ==(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime == c2._datetime); }
    public static bool operator !=(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime != c2._datetime); }
    public static bool operator >=(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime >= c2._datetime); }
    public static bool operator <=(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime <= c2._datetime); }
    public static bool operator >(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime > c2._datetime); }
    public static bool operator <(DateTimeEx c1, DateTimeEx c2) { return (c1._datetime < c2._datetime); }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        return (this==(DateTimeEx)obj);
    }
}