using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LoggerEx
{
    private static string GetCallerInfo()
    {
        var callStack = new StackTrace();
        var frame = callStack.GetFrame(3);
        var method = frame.GetMethod();
        return $"{method.ReflectedType?.Name}::{method.Name}";
    }

    private static string ColorText(string text, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        var lines = text.Split("\n");
        var coloredLines = lines.Select(line => $"<color=#{hexColor}>{line}</color>");
        return string.Join("\n", coloredLines);
    }

    private static string FormatMessage(string message, Color color)
    {
        string callerInfo = GetCallerInfo();
        string formatMsg = $"{callerInfo} \n{ColorText($"{message}", color)}";
        return formatMsg;
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(string message)
    {
        Debug.Log(FormatMessage(message, Color.white));
    }
    
    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message)
    {
        Debug.LogWarning(FormatMessage(message, Color.yellow));
    }
    
    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message)
    {
        Debug.LogError(FormatMessage(message, Color.red));
    }
}
