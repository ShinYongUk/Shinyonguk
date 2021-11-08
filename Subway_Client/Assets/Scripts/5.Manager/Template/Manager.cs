using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Manager<T> : Singleton<T>, IManager where T : new()
{

    public object ExecMethod(string bzMethod, object[] _object)
    {

        MethodInfo method = this.GetType().GetMethod(bzMethod, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (method != null)
        {
            object obj = method.Invoke(this, _object);
            return obj;
        }

        return null;
    }


}