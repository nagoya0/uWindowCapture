﻿using System;
using System.Text;
using System.Runtime.InteropServices;

#pragma warning disable 114, 465

namespace uWindowCapture
{

public enum DebugMode
{
    None = 0,
    File = 1,
    UnityLog = 2, /* currently has bug when app exits. */
}

public enum CaptureMode
{
    None = -1,
    PrintWindow = 0,
    BitBlt = 1,
}

public enum MessageType
{
    None = -1,
    WindowAdded = 0,
    WindowRemoved = 1,
}

[StructLayout(LayoutKind.Sequential)]
public struct Message
{
    [MarshalAs(UnmanagedType.I4)]
    public MessageType type;
    [MarshalAs(UnmanagedType.I4)]
    public int windowId;
    [MarshalAs(UnmanagedType.I8)]
    public System.IntPtr windowHandle;
}

public static class Lib
{
    public const string name = "uWindowCapture";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugLogDelegate(string str);

    [DllImport(name, EntryPoint = "UwcInitialize")]
    public static extern void Initialize();
    [DllImport(name, EntryPoint = "UwcFinalize")]
    public static extern void Finalize();
    [DllImport(name, EntryPoint = "UwcSetDebugMode")]
    public static extern void SetDebugMode(DebugMode mode);
    [DllImport(name, EntryPoint = "UwcSetLogFunc")]
    public static extern void SetLogFunc(DebugLogDelegate func);
    [DllImport(name, EntryPoint = "UwcSetErrorFunc")]
    public static extern void SetErrorFunc(DebugLogDelegate func);
    [DllImport(name, EntryPoint = "UwcGetRenderEventFunc")]
    public static extern IntPtr GetRenderEventFunc();
    [DllImport(name, EntryPoint = "UwcUpdate")]
    public static extern void Update();
    [DllImport(name, EntryPoint = "UwcGetMessageCount")]
    private static extern int GetMessageCount();
    [DllImport(name, EntryPoint = "UwcGetMessages")]
    private static extern IntPtr GetMessages_Internal();
    [DllImport(name, EntryPoint = "UwcGetWindowHandle")]
    public static extern IntPtr GetWindowHandle(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowOwner")]
    public static extern IntPtr GetWindowOwner(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowX")]
    public static extern int GetWindowX(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowY")]
    public static extern int GetWindowY(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowWidth")]
    public static extern int GetWindowWidth(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowHeight")]
    public static extern int GetWindowHeight(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowZOrder")]
    public static extern int GetWindowZOrder(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTitleLength")]
    private static extern int GetWindowTitleLength(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTitle", CharSet = CharSet.Unicode)]
    private static extern IntPtr GetWindowTitle_Internal(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTexturePtr")]
    public static extern IntPtr GetWindowTexturePtr(int id);
    [DllImport(name, EntryPoint = "UwcSetWindowTexturePtr")]
    public static extern void SetWindowTexturePtr(int id, IntPtr texturePtr);
    [DllImport(name, EntryPoint = "UwcGetWindowCaptureMode")]
    public static extern CaptureMode GetWindowCaptureMode(int id);
    [DllImport(name, EntryPoint = "UwcSetWindowCaptureMode")]
    public static extern void SetWindowCaptureMode(int id, CaptureMode mode);
    [DllImport(name, EntryPoint = "UwcIsWindow")]
    public static extern bool IsWindow(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowVisible")]
    public static extern bool IsWindowVisible(int id);
    [DllImport(name, EntryPoint = "UwcIsAltTabWindow")]
    public static extern bool IsAltTabWindow(int id);
    [DllImport(name, EntryPoint = "UwcIsDesktop")]
    public static extern bool IsDesktop(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowEnabled")]
    public static extern bool IsWindowEnabled(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowUnicode")]
    public static extern bool IsWindowUnicode(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowZoomed")]
    public static extern bool IsWindowZoomed(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowIconic")]
    public static extern bool IsWindowIconic(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowHungUp")]
    public static extern bool IsWindowHungUp(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowTouchable")]
    public static extern bool IsWindowTouchable(int id);

    public static Message[] GetMessages()
    {
        var count = GetMessageCount();
        var ptr = GetMessages_Internal();
        var size = Marshal.SizeOf(typeof(Message));

        var messages = new Message[count];
        for (int i = 0; i < count; ++i) {
            var data = new System.IntPtr(ptr.ToInt64() + (size * i));
            messages[i] = (Message)Marshal.PtrToStructure(data, typeof(Message));
        }

        return messages;
    }

    public static string GetWindowTitle(int id)
    {
        var len = GetWindowTitleLength(id);
        var ptr = GetWindowTitle_Internal(id);
        return Marshal.PtrToStringUni(ptr, len);
    }
}

}