using System.Text.RegularExpressions;

public static class StringUtil
{
    public static string[] StringToArray(string str, string splitChar)
    {
        if(str == null)
        {
            return null;
        }

        return Regex.Split(str, splitChar);
    }
}
