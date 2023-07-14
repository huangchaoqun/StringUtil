using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
/// </summary>
public class VStringUtil
{
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e, string f)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, f, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e, string f, string g)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, f, g, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e, string f, string g, string h)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, f, g, h, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e, string f, string g, string h, string i)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, f, g, h, i, true);
        return vString.GetString();
    }
    /// <summary>
    /// 只能作为临时字符串使用,代码任何地方使用只能赋值给临时变量,不可保存
    /// </summary>
    public static string Concat(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j)
    {
        VString vString = VStringShareObject.GetShareVString();
        vString.Concat(a, b, c, d, e, f, g, h, i, j, true);
        return vString.GetString();
    }

    /// <summary>
    /// 如果不是共享string,则返回str,如果是共享string则返回copy str
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ConvertToNormalString(string str)
    {
        if(VStringShareObject.UseShareObject(str) || VString.UseShareObject(str))
        {
            return string.Copy(str);
        }
        return str;
    }

}