namespace PoznejHrademMaui.Components;

public partial class TittleLayout : ContentView
{
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(TittleLayout), default(string));

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TittleLayout), default(string));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public TittleLayout()
    {
        InitializeComponent();
        BindingContext = this;

    }

    private void Image_SizeChanged(object sender, EventArgs e)
    {
        frameLay.Padding =new Thickness(10, 10, ((Image)sender).Width, 10);

        var size = textLabel.Measure(frameLay.Width, double.PositiveInfinity).Request;
        // Set the minimum height for the frame based on the measured height
        frameLay.HeightRequest = size.Height*2.3;
        this.MinimumHeightRequest = (size.Height*2.3)*2;
    }

    private void Grid_SizeChanged(object sender, EventArgs e)
    {
        imageL.MaximumHeightRequest = ((Grid)sender).Height;
    }
}