using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class StringExtend
{
    private static volatile object lockThis = new object();

    public static string ToTempString(this int i)
    {
        lock (lockThis)
        {
            return VString.IntToString(i);
        }
    }

    public static string ToTempString(this float f, int digits = 2)
    {
        lock (lockThis)
        {
            return VString.FloatToString(f, digits);
        }
    }

    public static string ToTempString(this long l)
    {
        lock (lockThis)
        {
            return VString.LongToString(l);
        }
    }

    public static string ToTempStringLower(this string str)
    {
        lock (lockThis)
        {
            return VString.ToLower(str);
        }
    }

    public static string ToTempStringUpper(this string str)
    {
        lock (lockThis)
        {
            return VString.ToUpper(str);
        }
    }

    public static string ToTempSubString(this string str, int index, int count)
    {
        lock (lockThis)
        {
            return VString.ToTempSubString(str, index, count);
        }
    }



    #region 转美式数字
    public static string ToStringUS(this float f)
    {
        return StringUtil.Num2US(f);
    }
    public static string ToStringUS(this int i)
    {
        return StringUtil.Num2US(i);
    }
    public static string ToStringUS(this long i)
    {
        return StringUtil.Num2US(i);
    }
    #endregion
}






/// <summary>
/// 内容可变的字符串
/// !!!只能作为临时变量使用,绝对不可以在逻辑中存储引用,包含VString和返回的string对象
/// </summary>
public class VString
{
    private string _data;
    private int maxCount;

    private static int _internalVsIndex;
    private static VString[] _internalVSArray = new VString[]
    {
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64),
        new VString(64)
    };
    private static string[] digitalNumberArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


    public VString(int maxCount = 1024)
    {
        this.maxCount = maxCount + 1; //多加一个，用于留1个给字符串结束符
        _data = new string('\0', this.maxCount);
        Clear();
    }

    public string GetString()
    {
        return _data;
    }



    /// <summary>
    /// int转string,无GC,注意生成的string一定不能进行存贮
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string IntToString(int val)
    {
        return LongToString(val);
    }

    /// <summary>
    /// long转string,无GC,注意生成的string一定不能进行存贮
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string LongToString(long val)
    {
        if (val == 0)
        {
            return "0";
        }

        VString tempVS = GetInternalVString();
        bool isNegative = false;
        if (val < 0)
        {
            val = -val;
            isNegative = true;
        }

        while (val != 0)
        {
            long mod = val % 10;
            val = val / 10;
            tempVS.Push(digitalNumberArray[mod]);
        }

        if (isNegative)
        {
            tempVS.Push("-");
        }

        tempVS.ReverseString();
        return tempVS.GetString();
    }

    /// <summary>
    /// float转string,无GC,注意生成的string一定不能进行存贮
    /// </summary>
    /// <param name="f"></param>
    /// <param name="digits">小数的位数</param>
    /// <returns></returns>
    public static string FloatToString(float f, int digits = 2)
    {
        bool isNegative = false;
        if (f < 0)
        {
            f = -f;
            isNegative = true;
        }

        int iPart = Mathf.FloorToInt(f);
        float fPart = f - iPart;

        VString tempVS0 = GetInternalVString();


        if (iPart != 0)
        {
            while (iPart != 0)
            {
                long mod = iPart % 10;
                iPart = iPart / 10;
                tempVS0.Push(digitalNumberArray[mod]);
            }
        }
        else
        {
            tempVS0.Push("0");
        }

        if (isNegative)
        {
            tempVS0.Push("-");
        }
        tempVS0.ReverseString();


        if (digits != 0)
        {
            VString tempVS1 = GetInternalVString();
            fPart = fPart * Mathf.Pow(10, digits);
            int iPart2 = Mathf.RoundToInt(fPart);

            int i = 0;
            while (iPart2 != 0 && i < digits)
            {
                long mod = iPart2 % 10;
                iPart2 = iPart2 / 10;
                i++;
                tempVS1.Push(digitalNumberArray[mod]);
            }
            tempVS1.ReverseString();

            tempVS0.Push(".");
            tempVS0.Push(tempVS1.GetString());
            while (i < digits)
            {
                i++;
                tempVS0.Push("0");
            }
        }
        else
        {
            tempVS0.Push(".");
            for (int i = 0; i < digits; ++i)
            {
                tempVS0.Push("0");
            }
        }

        return tempVS0.GetString();
    }


    /// <summary>
    /// 把一个字符串拷贝后,转换为lower case,,注意生成的string一定不能进行存贮
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToLower(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            VString tempVS = VStringShareObject.GetShareVString();
            tempVS.Push(str);
            tempVS.ToLower();
            return tempVS.GetString();
        }
        return str;
    }

    /// <summary>
    /// 把一个字符串拷贝后,转换为upper case,,注意生成的string一定不能进行存贮
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUpper(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            VString tempVS = VStringShareObject.GetShareVString();
            tempVS.Push(str);
            tempVS.ToUpper();
            return tempVS.GetString();
        }
        return str;
    }

    public static string ToTempSubString(string str, int index, int count)
    {
        if (string.IsNullOrEmpty(str) || count <= 0 || index < 0)
        {
            Debug.LogError(VStringUtil.Concat("ToTempSubString IsNullOrEmpty ", index.ToTempString(), "/", count.ToTempString()));
            return str;
        }

        if (index + count > str.Length)
        {
            Debug.LogError(VStringUtil.Concat("ToTempSubString ", str, index.ToTempString(), "/", count.ToTempString()));
            return str;
        }

        VString tempVS1 = VStringShareObject.GetShareVString();
        tempVS1.Push(str);
        VString tempVS2 = VStringShareObject.GetShareVString();
        tempVS2.CopyFrom(tempVS1, index, count);
        return tempVS2.GetString();
    }


    /// <summary>
    /// 拼接两个字符串
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public string Concat(string a, string b, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        return _data;
    }



    public string Concat(string a, string b, string c, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, string f, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        Push(f);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, string f, string g, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        Push(f);
        Push(g);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, string f, string g, string h, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        Push(f);
        Push(g);
        Push(h);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, string f, string g, string h, string i, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        Push(f);
        Push(g);
        Push(h);
        Push(i);
        return _data;
    }
    public string Concat(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }

        Push(a);
        Push(b);
        Push(c);
        Push(d);
        Push(e);
        Push(f);
        Push(g);
        Push(h);
        Push(i);
        Push(j);
        return _data;
    }


    public static bool UseShareObject(string str)
    {
        for (int i = 0; i < _internalVSArray.Length; ++i)
        {
            if (string.ReferenceEquals(str, _internalVSArray[i].GetString()))
            {
                return true;
            }
        }
        return false;
    }


    //往当前的字符串中添加字符串
    public unsafe void Push(string newStr)
    {
        if (string.IsNullOrEmpty(newStr))
        {
            return;
        }

        int copyLen = newStr.Length;
        int newLen = _data.Length + copyLen;
        if ((newLen + 1) > maxCount) //留1个给字符串结束符
        {
            int len = newLen;
            copyLen = maxCount - _data.Length - 1;
            newLen = maxCount - 1; //设置新的长度
            //这个地方不使用VstringUtil.Concat避免死循环
            Debug.LogError(StringUtil.Concat("超过了最大添加长度 ", maxCount.ToTempString(), " ", len.ToTempString()));
        }

        if (copyLen <= 0)
        {
            return;
        }

        fixed (char* src = newStr)
        {
            fixed (char* dst = _data)
            {
                UnsafeFunction.memcpyimpl((byte*)src, (byte*)(dst + _data.Length), copyLen * 2); //system.string的存储每个元素两个字节

                int* iDst = (int*)dst;
                iDst = iDst - 1;    //字符串的长度在第一个元素的前面4个字节
                *iDst = newLen;

                char* iEnd = (char*)(dst + newLen);
                *iEnd = (char)0;//设置字符串结束符
            }
        }
    }


    public unsafe void Clear()
    {
        fixed (char* p = _data)
        {
            int* pSize = (int*)p;
            pSize = pSize - 1;
            *pSize = 0;
        }
    }

    public unsafe void CopyFrom(VString srcVstring, int startIndex, int count)
    {
        if ((count + 1) > maxCount) //留1个给字符串结束符
        {
            throw new ArgumentException(VStringUtil.Concat("copy count is larger then maxCount ",
                count.ToTempString(), " ", maxCount.ToTempString()));
        }

        string srcStr = srcVstring.GetString();
        if (startIndex + count > srcStr.Length)
        {
            throw new ArgumentException(VStringUtil.Concat("copy count is larger then srcString len ",
                count.ToTempString(), " ", srcStr.Length.ToTempString(), " ", startIndex.ToTempString()));
        }

        Clear();

        fixed (char* src = srcStr)
        {
            fixed (char* dst = _data)
            {
                UnsafeFunction.memcpyimpl((byte*)(src + startIndex), (byte*)dst, count * 2); //system.string的存储每个元素两个字节

                int* iDst = (int*)dst;
                iDst = iDst - 1;    //字符串的长度在第一个元素的前面4个字节
                *iDst = count;

                char* iEnd = (char*)(dst + _data.Length);
                *iEnd = (char)0;//设置字符串结束符
            }
        }
    }

    public unsafe void ToLower()
    {
        int index = 0;
        int len = _data.Length;
        fixed (char* dst = _data)
        {
            while (index < len)
            {
                char tempChar = *(dst + index);
                *(dst + index) = char.ToLower(tempChar);
                ++index;
            }
        }
    }

    public unsafe void ToUpper()
    {
        int index = 0;
        int len = _data.Length;
        fixed (char* dst = _data)
        {
            while (index < len)
            {
                char tempChar = *(dst + index);
                *(dst + index) = char.ToUpper(tempChar);
                ++index;
            }
        }
    }

    //反转字符串的内容
    private unsafe string ReverseString()
    {
        int len = _data.Length;
        if (len > 0)
        {
            fixed (char* pHead = _data)
            {
                int count = len / 2;
                for (int i = 0; i < count; ++i)
                {
                    char temp = pHead[i];
                    pHead[i] = pHead[len - 1 - i];
                    pHead[len - 1 - i] = temp;
                }
            }
        }
        return _data;
    }


    private static VString GetInternalVString()
    {
        _internalVsIndex = (_internalVsIndex + 1) % _internalVSArray.Length;
        VString vString = _internalVSArray[_internalVsIndex];
        vString.Clear();
        return vString;
    }

}