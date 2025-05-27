#if UNITY_EDITOR_WIN
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
#elif UNITY_EDITOR_OSX
using System.Runtime.InteropServices;
#endif
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TitleBarPathDisplay
{
    private const float UpdateInterval = 1.0f;
    private static float _lastUpdateTime;
    private static string _originalTitle;

#if UNITY_EDITOR_WIN
    private static IntPtr _hwnd;

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern bool SetWindowText(IntPtr hWnd, string lpString);
#elif UNITY_EDITOR_OSX
    [DllImport("objc", EntryPoint = "objc_msgSend")]
    private static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

    [DllImport("objc", EntryPoint = "sel_registerName")]
    private static extern IntPtr GetSelector(string name);
#endif

    static TitleBarPathDisplay()
    {
#if UNITY_EDITOR_WIN
        FindEditorWindowHandle();
        _originalTitle = GetCurrentWindowTitle();
        if (_originalTitle.Contains("[[")) {
            var startIndex = _originalTitle.IndexOf("[[");  
            _originalTitle = _originalTitle.Substring(0, startIndex-1);    
        }
#elif UNITY_EDITOR_OSX
        _originalTitle = GetMacWindowTitle();
#endif

        EditorApplication.update += Update;
        EditorApplication.quitting += RestoreOriginalTitle;
    }

#if UNITY_EDITOR_WIN
    private static void FindEditorWindowHandle()
    {
        int currentProcessId = Process.GetCurrentProcess().Id;
        EnumWindows(delegate(IntPtr hwnd, IntPtr lParam)
        {
            GetWindowThreadProcessId(hwnd, out int processId);
            if (processId == currentProcessId)
            {
                int length = GetWindowTextLength(hwnd);
                if (length > 0)
                {
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(hwnd, sb, sb.Capacity);
                    if (sb.ToString().Contains("Unity"))
                    {
                        _hwnd = hwnd;
                        return false; // 找到窗口后停止枚举
                    }
                }
            }
            return true;
        }, IntPtr.Zero);
    }

    private static string GetCurrentWindowTitle()
    {
        if (_hwnd != IntPtr.Zero)
        {
            int length = GetWindowTextLength(_hwnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(_hwnd, sb, sb.Capacity);
            return sb.ToString();
        }
        return Application.productName;
    }
#endif

    private static void Update()
    {
        if (Time.realtimeSinceStartup - _lastUpdateTime > UpdateInterval)
        {
            UpdateWindowTitle();
            _lastUpdateTime = Time.realtimeSinceStartup;
        }
    }

    private static void UpdateWindowTitle()
    {
        string path = Application.dataPath.Replace("/Assets", "");
        string newTitle = $"{_originalTitle} [[{path}]]";

#if UNITY_EDITOR_WIN
        if (_hwnd != IntPtr.Zero)
        {
            SetWindowText(_hwnd, newTitle);
        }
#elif UNITY_EDITOR_OSX
        SetMacWindowTitle(newTitle);
#endif
    }

#if UNITY_EDITOR_OSX
    private static string GetMacWindowTitle()
    {
        IntPtr nsApplication = IntPtr_objc_msgSend(GetClass("NSApplication"), GetSelector("sharedApplication"));
        IntPtr window = IntPtr_objc_msgSend(nsApplication, GetSelector("mainWindow"));
        IntPtr title = IntPtr_objc_msgSend(window, GetSelector("title"));
        return Marshal.PtrToStringAuto(title);
    }

    private static void SetMacWindowTitle(string title)
    {
        IntPtr nsString = CreateNSString(title);
        IntPtr nsApplication = IntPtr_objc_msgSend(GetClass("NSApplication"), GetSelector("sharedApplication"));
        IntPtr window = IntPtr_objc_msgSend(nsApplication, GetSelector("mainWindow"));
        IntPtr_objc_msgSend(window, GetSelector("setTitle:"), nsString);
    }

    private static IntPtr CreateNSString(string text)
    {
        IntPtr cls = GetClass("NSString");
        IntPtr str = Marshal.StringToHGlobalAuto(text);
        IntPtr result = IntPtr_objc_msgSend(cls, GetSelector("stringWithUTF8String:"), str);
        Marshal.FreeHGlobal(str);
        return result;
    }

    private static IntPtr GetClass(string className)
    {
        return IntPtr_objc_msgSend(GetSelector("class"), GetSelector("class"));
    }
#endif

    private static void RestoreOriginalTitle()
    {
#if UNITY_EDITOR_WIN
        if (_hwnd != IntPtr.Zero)
        {
            SetWindowText(_hwnd, _originalTitle);
        }
#elif UNITY_EDITOR_OSX
        SetMacWindowTitle(_originalTitle);
#endif
    }
}