using Crolow.Designer.Common.Application;
using Crolow.Designer.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Crolow.Designer.Wpf.App;

public partial class App : Application
{
    public static IHost Host { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()

            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json",
                                   optional: false,
                                   reloadOnChange: true);
            })

            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })

            .Build();

        DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        await Host.StartAsync();

        var window = Host.Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }

    private void App_DispatcherUnhandledException(
        object sender,
        System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            e.Exception.ToString(),
            "Dispatcher Exception",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        e.Handled = true;
    }

    private void CurrentDomain_UnhandledException(
        object sender,
        UnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            e.ExceptionObject.ToString(),
            "Unhandled Exception",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
    protected override async void OnExit(ExitEventArgs e)
    {
        await Host.StopAsync();
        Host.Dispose();

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RuntimeController>();
        services.AddSingleton<DocumentsController>();
        services.AddSingleton<MainWindow>();

        services.Configure<ApplicationOptions>(
            configuration.GetSection("Application"));
    }
}
