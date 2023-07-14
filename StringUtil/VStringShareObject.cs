using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class VStringShareObject
{

    private static volatile object lockThis = new object();
    private static int _internalVsIndex;
    private static VString[] _internalVSArray = new VString[]
    {
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048),
        new VString(2048)
    };   


    public static VString GetShareVString()
    {
        lock(lockThis)
        {
            _internalVsIndex = (_internalVsIndex + 1) % _internalVSArray.Length;
            VString vString = _internalVSArray[_internalVsIndex];
            vString.Clear();
            return vString;
        }
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
}