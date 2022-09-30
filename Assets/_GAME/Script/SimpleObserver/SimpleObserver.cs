using System.Collections.Generic;
using System.Linq;

namespace SimpleUnityObserver
{
    public delegate void CallBack();
    public class SimpleObserver
    {
        private static Dictionary<CallBack, string> m_CallBackDict = new Dictionary<CallBack, string>();
        public static int GetCallBackCount => m_CallBackDict.Count;

        /// <summary>Add a method with void type to call in MD/SimpleObserver window</summary>
        /// <param name="callBack">Method()</param>
        public static void AddCallback(CallBack callBack)
        {
            if (!m_CallBackDict.ContainsKey(callBack))
            {
                m_CallBackDict.Add(callBack, callBack.Method.Name);
            }
        }

        /// <summary>Get method with name</summary>
        /// <param name="index">Desired callback index</param>
        /// <param name="methodName">Desired callback method name</param>
        public static CallBack GetCallback(int index, out string methodName)
        {
            methodName = m_CallBackDict.ElementAt(index).Value;
            return m_CallBackDict.ElementAt(index).Key;
        }
    }
}