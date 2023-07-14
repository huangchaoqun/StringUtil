using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 优化打字功能的字符串
/// </summary>
public class VTypingString
{
    enum Tags
    {
        Color = 0,
        Size,
        B,
        I
    }


    private VString allContentString;
    private DoubleVString partialDisplayString;
    private VString willShowString;

    private int _timerMS;
    private int _factor;
    private int _miniSecondPerWord;

    private static readonly string[] endTags = new string[] { "</color>", "</size>", "</b>", "</i>" };
    private static readonly string[] startTags = new string[] { "<color", "<size", "<b", "<i" };
    
    List<int> endTagCaches = new List<int>();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="miniSecondPerWord">出一个字符需要的毫秒时间</param>
    public VTypingString(string text, int miniSecondPerWord = 240)
    {
        if(string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("text is null or empty");
        }

        _miniSecondPerWord = Mathf.Max(miniSecondPerWord, 10);
        allContentString = new VString(text.Length);
        allContentString.Push(text);
        partialDisplayString = new DoubleVString(text.Length);
        willShowString = new VString(text.Length);
        JumpToBegin();
    }

    public bool IsEnd()
    {
        return _factor > allContentString.GetString().Length;
    }
        
    public string GetString()
    {
        return partialDisplayString.GetCurrentVString().GetString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTimeMS"></param>
    /// <returns>true表示触发了一次打字变化</returns>
    public bool OnUpdate(int deltaTimeMS)
    {
        _timerMS += deltaTimeMS;
        if(_timerMS >= _miniSecondPerWord)
        {
            _timerMS = 0;
            OnTyping();
            return true;
        }
        return false;
    }

    private void OnTyping()
    {
        if (CheckStart(Tags.Color)) { }
        else if (CheckStart(Tags.Size)) { }
        else if (CheckStart(Tags.B)) { }
        else if (CheckStart(Tags.I)) { }
        else
        {
            partialDisplayString.GetCurrentVString().Clear();
            partialDisplayString.SwapVString();
            partialDisplayString.GetCurrentVString().CopyFrom(allContentString, 0, Mathf.Min(_factor, allContentString.GetString().Length));
            for (int i = endTagCaches.Count - 1; i >= 0; --i)
            {
                partialDisplayString.GetCurrentVString().Push(endTags[endTagCaches[i]]);
            }
            _factor++;
        }
    }

    public void JumpToBegin()
    {
        _factor = 1;
        _timerMS = -_miniSecondPerWord;
        endTagCaches.Clear();
        partialDisplayString.GetCurrentVString().Clear();
        partialDisplayString.GetNextVString().Clear();
    }

    public void JumpToEnd()
    {
        _factor = allContentString.GetString().Length;
        endTagCaches.Clear();
        OnTyping();
    }


    bool CheckStart(Tags tag)
    {
        if (_factor >= allContentString.GetString().Length)
        {
            return false;
        }

        int iTag = (int)tag;
        willShowString.CopyFrom(allContentString, _factor, allContentString.GetString().Length - _factor);
        string willShow = willShowString.GetString();
        string endTag = endTags[iTag];
        if (willShow.StartsWith(startTags[iTag]))
        {
            int tagLeng = willShow.IndexOf(">") + 1;
            _factor += tagLeng;
            endTagCaches.Add(iTag);//倒叙

            if (CheckStart(Tags.Color)) { }
            else if (CheckStart(Tags.Size)) { }
            else if (CheckStart(Tags.B)) { }
            else if (CheckStart(Tags.I)) { }
            else
            {
                return false;
            }
            return true;
        }
        else if (willShow.StartsWith(endTag))
        {
            int endleng = endTag.Length;//"</color>"的长度
            _factor += endleng;
            for (int i = endTagCaches.Count - 1; i >= 0; --i)
            {
                if(iTag == endTagCaches[i])
                {
                    endTagCaches.RemoveAt(i);
                }
            }

            if (CheckStart(Tags.Color)) { }
            else if (CheckStart(Tags.Size)) { }
            else if (CheckStart(Tags.B)) { }
            else if (CheckStart(Tags.I)) { }
            else
            {
                return false;
            }
            return true;
        }
        return false;
    }


}
