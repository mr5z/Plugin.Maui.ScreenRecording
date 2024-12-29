using Microsoft.Maui.LifecycleEvents;

namespace Plugin.Maui.ScreenRecording;

public static class AppBuilderExtensions
{
	/// <summary>
	/// Initializes the .NET MAUI Screen Recording Library
	/// </summary>
	/// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/>.</param>
	/// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="ScreenRecording"/>.</returns>
	public static MauiAppBuilder UseScreenRecording(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<IScreenRecording>(ScreenRecording.Default);

#if ANDROID
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddAndroid(android =>
            {
                android.OnActivityResult((activity, requestCode, resultCode, data) =>
                {
                    if (requestCode == ScreenRecordingImplementation.RequestMediaProjectionCode)
                    {
                        var instance = (ScreenRecordingImplementation)ScreenRecording.Default;
                        switch (resultCode)
                        {
                            case Android.App.Result.Ok:
                                instance.OnScreenCapturePermissionGranted((int)resultCode, data);
                                break;
                            case Android.App.Result.Canceled:
                                instance.OnScreenCapturePermissionDenied();
                                break;
                        }
                    }
                });
            });
        });
#endif

		return builder;
	}
}