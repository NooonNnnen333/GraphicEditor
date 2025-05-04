using System;
using Android.App;
using Android.Runtime;
using GraphicApp;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace GraphicApp;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}