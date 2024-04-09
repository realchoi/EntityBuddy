using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DBuddy.AppUi.ViewModels;
using DBuddy.AppUi.Views;
using DBuddy.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DBuddy.AppUi;

public partial class App : Application
{
    public IHost Host { get; }

    public App()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
            //UseContentRoot(AppContext.BaseDirectory).
            // 往依赖服务容器中注入服务
            ConfigureServices((context, services) =>
            {
                // Services
                services.AddSingletonCoreServices();
                // services.AddSingleton<IGenerateClassService, GenerateClassService>();

                // Views and ViewModels
                // ...
                // services.AddSingleton<MainWindowViewModel>();
            }).Build();
    }

    /// <summary>
    /// 获取容器中的服务实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns>服务实例</returns>
    /// <exception cref="ArgumentException"></exception>
    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 添加共享资源
        var vmLocator = new VmLocator();
        Resources.Add("VMLocator", vmLocator);

        base.OnFrameworkInitializationCompleted();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
    }
}