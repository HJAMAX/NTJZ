using System;
public class TimeUtil
{
    /// <summary>
    /// 根据时间戳获取当前时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime GetTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static string GetFormatedTime(string timeFormat, string timeStamp)
    {
        DateTime dateTime = GetTime(timeStamp);
        return dateTime.ToString(timeFormat);
    }

    public static string GetCurrentTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    public static long GetCurrentTimeStampLong()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    /// <summary>
    /// 获得当前时间与指定时间的时间差
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static TimeSpan GetTimeDiff(string timeStamp)
    {
        return DateTime.Now - GetTime(timeStamp);
    }

    /// <summary>
    /// 获得指定时间与当前时间的时间差
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static TimeSpan GetTimeFromNow(string timeStamp)
    {
        return GetTime(timeStamp) - DateTime.Now;
    }
}
