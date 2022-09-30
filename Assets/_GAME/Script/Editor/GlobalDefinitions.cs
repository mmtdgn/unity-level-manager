// @By Mehmet Doğan Date 2022-09-28 //
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class GlobalDefinitions
{
    private static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup, string defines)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
    }

    private static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup, string[] defines)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
    }

    /// <summary>add preProcessor for all target</summary>
    /// <param name="define">define key</param>
    public static void AddDefinition(string define)
    {
        string _definitionKey = string.Empty;
        foreach (var I in GetUpdatedDefinitions(define))
            _definitionKey += I != GetUpdatedDefinitions(define).Last() ? $"{I};" : I;
        //Alternative usage instead of sending Array param!

        AddForAllPlatforms(_definitionKey);
    }

    public static void AddDefinition(BuildTargetGroup targetGroup, string define)
    {
        string _definitionKey = string.Empty;
        foreach (var I in GetUpdatedDefinitions(define))
            _definitionKey += I != GetUpdatedDefinitions(define).Last() ? $"{I};" : I;
        //Alternative way to set string param

        SetScriptingDefineSymbolsForGroup(targetGroup, _definitionKey);
    }

    public static void AddDefinitions(BuildTargetGroup targetGroup, string[] defines)
    {
        List<String> _defineCollections = new List<string>(GetStaledDefinitions());

        foreach (var I in defines)
            if (!_defineCollections.Contains(I))
                _defineCollections.Add(I);

        SetScriptingDefineSymbolsForGroup(targetGroup, _defineCollections.ToArray());
    }

    public static void RemoveDefinition(BuildTargetGroup targetGroup, string define)
    {
        List<string> _definitions = new List<string>(GetStaledDefinitions());
        _definitions.Remove(define);

        SetScriptingDefineSymbolsForGroup(targetGroup, _definitions.ToArray());
    }

    public static void RemoveAllDefinitions(BuildTargetGroup targetGroup)
    {
        SetScriptingDefineSymbolsForGroup(targetGroup, string.Empty);
    }

    private static List<string> GetUpdatedDefinitions(string[] defines)
    {
        string _staleDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        return _staleDefinitions.Split(';').Where(x => defines.Contains(x)).ToList();
    }

    private static List<string> GetUpdatedDefinitions(string define)
    {
        string _staleDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        List<string> _defineCollection = _staleDefinitions.Split(';').ToList();
        if (!_defineCollection.Contains(define)) _defineCollection.Add(define);
        return _defineCollection;
    }

    private static List<string> GetStaledDefinitions()
    {
        string _staleDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        return _staleDefinitions.Split(';').ToList();
    }
#if DEBUG
    public static void LogPreProcessors()
    {
        foreach (var I in GetStaledDefinitions())
            UnityEngine.Debug.Log(I);
    }
#endif


    private static void AddForAllPlatforms(string define)
    {
        SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, define);
        SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, define);
        SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, define);
    }
}