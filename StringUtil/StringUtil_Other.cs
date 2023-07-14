using UnityEngine;
using System.Collections;
using System.Text;
using System;

public partial class StringUtil
{
    /// <summary>
    /// 计算字符串的长度（包含处理中文字符）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int CalcStringLetterNum(string input)
    {
        int counter = 0;
        for (int i = 0, imax = input.Length; i < imax; ++i)
        {
            if (IsChineseLetter(input, i))
            {
                counter += 2;
            }
            else
            {
                counter += 1;
            }
        }

        return counter;
    }

    /// <summary>
    /// 截断字符串（包含处理中文字符）
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxEnglishLength"></param>
    /// <returns></returns>
    public static string TrancString(string input, int maxEnglishLength)
    {
        int counter = 0;
        for (int i = 0, imax = input.Length; i < imax; ++i)
        {
            if (IsChineseLetter(input, i))
            {
                if (counter <= maxEnglishLength && maxEnglishLength < (counter + 2))
                    return input.Substring(0, i);
                counter += 2;
            }
            else
            {
                if (counter <= maxEnglishLength && maxEnglishLength < (counter + 1))
                    return input.Substring(0, i);
                counter += 1;
            }
        }

        return input;
    }

    /// <summary>
    /// 是不是中文字符
    /// </summary>
    /// <param name="input"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsChineseLetter(string input, int index)
    {
        int code = 0;
        int chfrom = System.Convert.ToInt32("4e00", 16);
        int chend = System.Convert.ToInt32("9fff", 16);
        if (input != "")
        {
            code = System.Char.ConvertToUtf32(input, index);

            if (code >= chfrom && code <= chend)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 缩减字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ShrinkString(string str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0, imax = str.Length; i < imax; i++)
        {
            if (str[i] == '\0')
                break;
            sb.Append(str[i]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 自定义的字符串比较
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool IsStringEqual(string left, string right)
    {
        if (System.Object.ReferenceEquals(left, right))
        {
            return true;
        }

        if (left == null && right == null)
            return true;
        if (left == null)
            return false;
        if (right == null)
            return false;

        int leftLen = left.Length;
        int rightLen = right.Length;
        int count = Mathf.Min(leftLen, rightLen);
        for (int index = 0; index < count; ++index)
        {
            char leftChar = left[index];
            char rightChar = right[index];
            if (leftChar != rightChar)
                return false;
        }

        if (leftLen > count && left[count] == '\0')
            return true;
        if (rightLen > count && right[count] == '\0')
            return true;
        if (leftLen == rightLen)
            return true;

        return false;
    }

    /// <summary>
    /// Bytes数组转utf8字符串
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="fromIndex"></param>
    /// <param name="count"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static string GetUtf8StringFromByteBuffer(ref byte[] buffer, int fromIndex, int count, int bufferSize)
    {
        if (buffer == null)
            return string.Empty;
        if (fromIndex < bufferSize)
        {
            int maxCount = bufferSize - fromIndex;
            count = Mathf.Min(maxCount, count);

            string str = ShrinkString(System.Text.Encoding.UTF8.GetString(buffer, fromIndex, count));
            return str;
        }

        Debug.LogError("fromIndex over flow.");
        return "";
    }

    /// <summary>
    /// Bytes数组转utf8字符串
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static string GetUtf8StringFromByteBuffer(ref byte[] buffer)
    {
        return GetUtf8StringFromByteBuffer(ref buffer, 0, buffer.Length, buffer.Length);
    }

    /// <summary>
    /// Utf8字符串转Byte数组
    /// </summary>
    /// <param name="srcStr"></param>
    /// <param name="srcOffset"></param>
    /// <param name="dstBuffer"></param>
    /// <param name="dstOffset"></param>
    /// <param name="dstBufferSize"></param>
    public static void CopyByteBufferFromUtf8String(string srcStr, int srcOffset, ref byte[] dstBuffer, int dstOffset, int dstBufferSize)
    {
        int copyCount;
        CopyByteBufferFromUtf8String(srcStr, srcOffset, ref dstBuffer, dstOffset, dstBufferSize, out copyCount);
    }

    /// <summary>
    /// Utf8字符串转Byte数组
    /// </summary>
    /// <param name="srcStr"></param>
    /// <param name="srcOffset"></param>
    /// <param name="dstBuffer"></param>
    /// <param name="dstOffset"></param>
    /// <param name="dstBufferSize"></param>
    /// <param name="copyCount"></param>
    public static void CopyByteBufferFromUtf8String(string srcStr, int srcOffset, ref byte[] dstBuffer, int dstOffset, int dstBufferSize, out int copyCount)
    {
        byte[] srcBuffer = System.Text.Encoding.UTF8.GetBytes(srcStr);
        int srcLen = srcBuffer.Length;
        srcLen = Mathf.Max(srcLen - srcOffset, 0);
        int dstMaxCopyCount = dstBufferSize - dstOffset;
        dstMaxCopyCount = Mathf.Max(dstMaxCopyCount, 0);
        dstMaxCopyCount = Mathf.Min(dstMaxCopyCount, srcLen);
        System.Buffer.BlockCopy(srcBuffer, srcOffset, dstBuffer, dstOffset, dstMaxCopyCount);

        copyCount = dstMaxCopyCount;
    }

    /// <summary>
    /// Byte数组复制
    /// </summary>
    /// <param name="srcBuffer"></param>
    /// <param name="srcOffset"></param>
    /// <param name="dstBuffer"></param>
    /// <param name="dstOffset"></param>
    /// <param name="dstBufferSize"></param>
    public static void CopyByteBuffer(ref byte[] srcBuffer, int srcOffset, ref byte[] dstBuffer, int dstOffset, int dstBufferSize)
    {
        int copyCount;
        CopyByteBuffer(ref srcBuffer, srcOffset, ref dstBuffer, dstOffset, dstBufferSize, out copyCount);
    }

    /// <summary>
    /// Byte数组复制
    /// </summary>
    /// <param name="srcBuffer"></param>
    /// <param name="srcOffset"></param>
    /// <param name="dstBuffer"></param>
    /// <param name="dstOffset"></param>
    /// <param name="dstBufferSize"></param>
    /// <param name="copyCount"></param>
    public static void CopyByteBuffer(ref byte[] srcBuffer, int srcOffset, ref byte[] dstBuffer, int dstOffset, int dstBufferSize, out int copyCount)
    {
        int srcLen = srcBuffer.Length;
        srcLen = Mathf.Max(srcLen - srcOffset, 0);
        int dstMaxCopyCount = dstBufferSize - dstOffset;
        dstMaxCopyCount = Mathf.Max(dstMaxCopyCount, 0);
        dstMaxCopyCount = Mathf.Min(dstMaxCopyCount, srcLen);
        System.Buffer.BlockCopy(srcBuffer, srcOffset, dstBuffer, dstOffset, dstMaxCopyCount);

        copyCount = dstMaxCopyCount;
    }

    /// <summary>
    /// Byte数组转十六进制字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string BytesToHex(byte[] bytes)
    {
        char[] c = new char[bytes.Length * 2];

        byte b;

        for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
        {
            b = ((byte)(bytes[bx] >> 4));
            c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

            b = ((byte)(bytes[bx] & 0x0F));
            c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
        }

        return new string(c);
    }

    /// <summary>
    /// 十六进制字符串转Byte数组
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] HexToBytes(string str)
    {
        if (str.Length == 0 || str.Length % 2 != 0)
            return new byte[0];

        byte[] buffer = new byte[str.Length / 2];
        char c;
        for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
        {
            // Convert first half of byte
            c = str[sx];
            buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

            // Convert second half of byte
            c = str[++sx];
            buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
        }

        return buffer;
    }

}