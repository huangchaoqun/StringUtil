using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 给UI.Text使用的double buffer string
/// </summary>
public class DoubleVString
{   
    private int index;
    private VString stringA;
    private VString stringB;

    public DoubleVString(int maxCount)
    {
        stringA = new VString(maxCount);
        stringB = new VString(maxCount);
    }
    
    public VString GetCurrentVString()
    {
        return (index % 2) == 0 ? stringA : stringB;
    }
    
    public VString GetNextVString()
    {
        return (index % 2) == 0 ? stringB : stringA;
    }


    //交换current 和Next string对象
    public void SwapVString()
    {
        index = (index + 1) % 2;
    }
}