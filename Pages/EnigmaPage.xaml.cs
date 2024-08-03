using Camera.MAUI;
using Camera.MAUI.ZXing;
using PoznejHrademMaui.Components;
using PoznejHrademMaui.DataManager;
using SkiaSharp;

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

        formColection.ItemsSource = formList;
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


    //Formul��
    List<string> formList = new List<string>() {
    "�sp�n� jsi vylu�til/a tajenku! Vypln�n�m a odesl�n�n formul��e se m��e� p�ihl�sit do slosov�n� o ceny.",
    "Nezapome� je�t� poslat foto vstupenky �i raz�tka z nav�t�ven�ho objektu na e-mail info@jh.cz",
    "Vypln�n�m a odesl�n�m formul��e pro slosov�n� v r�mci interaktivn�ho pr�vodce Poznej Hradec d�v�m souhlas se zpracov�n�m sv�ch osobn�ch �daj� ve smyslu na��zen� Evropsk�ho parlamentu a Rady (EU) 2016/679 (GDPR\"). Spr�vce �daj� m�sto Jind�ich�v Hradec tyto �daje zpracuje pouze k ��elu vyhodnocen� sout�e a bude je uchov�vat pouze po nezbytn� nutnou dobu. Sv�j souhlas m��ete kdykoliv odvolat bez jak�chkoliv sankc�, rovn� tak po��v�te pr�v ve smyslu �l. 15 a� 22 GDPR."
    };
    private async void SendForm(object sender, EventArgs e)
    {
        if (NameInput.Text != null & LastNameInput.Text != null & Streedinput.Text != null & CityInput != null & PScInput.Text != null & EmailInput.Text != null & PhoneInput.Text != null)
        {
            if (!EmailInput.Text.Contains("@"))
            {
                Backdata.Text = "Zadejte platn� email";
                Backdata.TextColor = Colors.Red;

            }
            else
            {
                try
                {

                    Backdata.Text = ""; //await response.Content.ReadAsStringAsync();
                    Backdata.TextColor = Colors.Black;
                    var file = await MediaPicker.PickPhotoAsync();
                    sendNV.IsEnabled = false;
                    sendVV.IsEnabled = false;
                    using (var fileStream = await file.OpenReadAsync())
                    {
                        var originalBitmap = SKBitmap.Decode(fileStream);

                        int originalWidth = originalBitmap.Width;
                        int originalHeight = originalBitmap.Height;

                        int maxWidth = 900;
                        int newWidth, newHeight;

                        if (originalWidth > maxWidth)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)((float)originalHeight * maxWidth / originalWidth);
                        }
                        else
                        {
                            newWidth = originalWidth;
                            newHeight = originalHeight;
                        }

                        var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);

                        using (var resizedStream = new MemoryStream())
                        {
                            resizedBitmap.Encode(resizedStream, SKEncodedImageFormat.Jpeg, 100);
                            var resizedBytes = resizedStream.ToArray();
                            var base64Image = Convert.ToBase64String(resizedBytes);

                            SendData(base64Image);
                        }
                    }
                }
                catch (Exception)
                {

                    Backdata.Text = "Nepovedlo se nahr�t Fotografii";
                    Backdata.TextColor = Colors.Red;
                }
            }


        }
        else
        {
            if (NameInput.Text == null) NameInput.PlaceholderColor = Colors.Red;
            if (LastNameInput.Text == null) LastNameInput.PlaceholderColor = Colors.Red;
            if (Streedinput.Text == null) Streedinput.PlaceholderColor = Colors.Red;
            if (CityInput.Text == null) CityInput.PlaceholderColor = Colors.Red;
            if (PScInput.Text == null) PScInput.PlaceholderColor = Colors.Red;
            if (EmailInput.Text == null) EmailInput.PlaceholderColor = Colors.Red;


            if (PhoneInput.Text == null) PhoneInput.PlaceholderColor = Colors.Red;
        }

    }
    private async void TakePhotoSendForm(object sender, EventArgs e)
    {
        if (NameInput.Text != null & LastNameInput.Text != null & Streedinput.Text != null & CityInput != null & PScInput.Text != null & EmailInput.Text != null & EmailInput.Text.Contains("@") & PhoneInput.Text != null)
        {
            try
            {

                Backdata.Text = ""; //await response.Content.ReadAsStringAsync();
                Backdata.TextColor = Colors.Black;
                var file = await MediaPicker.CapturePhotoAsync();
                sendNV.IsEnabled = false;
                sendVV.IsEnabled = false;
                using (var fileStream = await file.OpenReadAsync())
                {
                    var originalBitmap = SKBitmap.Decode(fileStream);

                    int originalWidth = originalBitmap.Width;
                    int originalHeight = originalBitmap.Height;

                    int maxWidth = 900;
                    int newWidth, newHeight;

                    if (originalWidth > maxWidth)
                    {
                        newWidth = maxWidth;
                        newHeight = (int)((float)originalHeight * maxWidth / originalWidth);
                    }
                    else
                    {
                        newWidth = originalWidth;
                        newHeight = originalHeight;
                    }

                    var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);

                    using (var resizedStream = new MemoryStream())
                    {
                        resizedBitmap.Encode(resizedStream, SKEncodedImageFormat.Jpeg, 100);
                        var resizedBytes = resizedStream.ToArray();
                        var base64Image = Convert.ToBase64String(resizedBytes);

                        SendData(base64Image);
                    }
                }

            }
            catch (Exception)
            {

                Backdata.Text = "Nepovedlo se nahr�t Fotografii";
                Backdata.TextColor = Colors.Red;
            }

        }
        else
        {
            if (NameInput.Text == null) NameInput.PlaceholderColor = Colors.Red;
            if (LastNameInput.Text == null) LastNameInput.PlaceholderColor = Colors.Red;
            if (Streedinput.Text == null) Streedinput.PlaceholderColor = Colors.Red;
            if (CityInput.Text == null) CityInput.PlaceholderColor = Colors.Red;
            if (PScInput.Text == null) PScInput.PlaceholderColor = Colors.Red;
            if (EmailInput.Text == null) EmailInput.PlaceholderColor = Colors.Red;
            if (PhoneInput.Text == null) PhoneInput.PlaceholderColor = Colors.Red;
        }

    }
    public async void SendData(string photo)
    {
        var url = "https://infocentrum.jh.cz/redakce/json.php?akce=data_to_form";


        var client = new HttpClient();


        //var content = new StringContent(JsonConvert.SerializeObject(data));

        client.BaseAddress = new Uri(url);
        //client.DefaultRequestHeaders.Add("contentType", "application/x-www-form-urlencoded");
        // var content = new StringContent(content1, Encoding.UTF8, "application/json");
        MultipartFormDataContent form = new MultipartFormDataContent();

        form.Add(new StringContent(NameInput.Text), "name");
        form.Add(new StringContent(LastNameInput.Text), "surname");
        form.Add(new StringContent(Streedinput.Text), "street");
        form.Add(new StringContent(CityInput.Text), "town");
        form.Add(new StringContent(PScInput.Text), "psc");
        form.Add(new StringContent(EmailInput.Text), "email");
        form.Add(new StringContent(PhoneInput.Text), "phone");
        form.Add(new StringContent(photo), "vstupenka");
        var content = form;
        HttpResponseMessage response = await client.PostAsync(url, content);
        // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
        var result = await response.Content.ReadAsStringAsync();
        Backdata.TextColor = Colors.Red;
        if (result.Contains("{\"response\":\"1\"}"))
        {
            Backdata.BackgroundColor = Colors.Green;
            Backdata.Text = "V�e �sp�sn� odeslalo.";
        }
        else Backdata.Text = "Omlouv�me se. vyskytla se chyba. zkuste to pros�m pozd�ko.";

        Backdata.TextColor = Colors.Black;
        //}
        //catch (Exception exep)
        //{
        //    Backdata.Text = "error:   " + exep.Message;
        //}

        Backdata.TextColor = Colors.Black;
    }


}
