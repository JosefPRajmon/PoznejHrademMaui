using Camera.MAUI;
using Camera.MAUI.ZXing;
using PoznejHrademMaui.Components;
using PoznejHrademMaui.DataManager;

namespace PoznejHrademMaui.Pages;

[QueryProperty(nameof(Camera), "camera")]
public partial class EnigmaPage : ContentPage
{
    DatabaseService _databaseService { get; set; }
    private bool _isProcessingBarcode = false;

    private bool _camera;

    public bool Camera
    {
        get => _camera;
        set
        {
            _camera = value;
        }
    }
    public EnigmaPage(/*DatabaseService databaseService*/)
    {

        InitializeComponent();
        ConstructorIdenticfunction();

    }

    private void ConstructorIdenticfunction()
    {
        _databaseService = new DatabaseService();
        cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
        cameraView.BarCodeOptions = new BarcodeDecodeOptions
        {
            AutoRotate = true,
            PossibleFormats = { BarcodeFormat.QR_CODE },
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = true
        };
    }


    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.Camera>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.Camera>();

        return status;
    }

    private void CameraView_Loaded(object sender, EventArgs e)
    {
        try
        {
            cameraView.Camera = cameraView.Cameras[0];
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                /*try
                {
                    //await cameraView.StopCameraAsync();

                }
                catch (Exception)
                {
                }
                //await cameraView.StartCameraAsync();
                */
                if (_camera)
                {
                    SetCameraToQRCode();
                }
                else
                {
                    cameraView.IsVisible = false;
                }

            });

        }
        catch (Exception ex)
        {

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await SetCameraToQRCode();
    }

    private async Task SetCameraToQRCode()
    {
        await CheckAndRequestLocationPermission();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            cameraView.IsVisible = true;
            Enigma.IsVisible=false;
            await cameraView.StartCameraAsync();
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateEnigma();
    }

    private void UpdateEnigma()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            enigmaProgres.ItemsSource = await _databaseService.GetScannedCodesAsync();
        });
    }

    private void ScrollView_SizeChanged(object sender, EventArgs e)
    {
        cameraView.HeightRequest = (double)((ScrollView)sender).Height;
        cameraView.WidthRequest = (double)((ScrollView)sender).WidthRequest;
    }

    private async void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        if (_isProcessingBarcode)
            return;

        _isProcessingBarcode = true;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            cameraView.IsVisible = false;
            Enigma.IsVisible = true;
            await cameraView.StopCameraAsync();
        });

        await Task.Run(async () =>
        {
            ScannedCode scannedCode = await _databaseService.GetScannedCodesAsync(args.Result[0].Text);
            if (scannedCode is null)
            {
                MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert((string)Application.Current.Resources["errorTitle"], (string)Application.Current.Resources["errorMessage"], "OK"));
            }
            else
            {
                scannedCode.Enigma = scannedCode.QRCodeText;
                int i = await _databaseService.UpdateScannedCodeAsync(scannedCode);
                if (i == 1)
                {
                    MainThread.BeginInvokeOnMainThread(() => OnAppearing());
                }
            }

            _isProcessingBarcode = false; // Reset flag after processing
        });
    }

}