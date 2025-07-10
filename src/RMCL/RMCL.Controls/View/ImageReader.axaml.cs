using Avalonia.Controls;
using Avalonia.Media;

namespace RMCL.Controls.View;

public partial class ImageReader : UserControl, IDisposable
{
    private bool _disposed;
    private IImage _image;
    private double _maxWidth = double.PositiveInfinity;
    private double _maxHeight = double.PositiveInfinity;

    public IImage ImageSource
    {
        get => _image;
        set
        {
            if (_image != value)
            {
                (_image as IDisposable)?.Dispose();
                _image = value;
                Update();
            }
        }
    }

    public double MaxWidth
    {
        get => _maxWidth;
        set
        {
            _maxWidth = value;
            UpdateSizeConstraints();
        }
    }

    public double MaxHeight
    {
        get => _maxHeight;
        set
        {
            _maxHeight = value;
            UpdateSizeConstraints();
        }
    }

    public ImageReader()
    {
        InitializeComponent();
    }

    public void Update()
    {
        if (MainImage == null || _image == null) return;

        if (MainImage.Background is ImageBrush existingBrush)
        {
            existingBrush.Source = _image as IImageBrushSource;
            existingBrush.Stretch = Stretch.UniformToFill;
        }
        else
        {
            MainImage.Background = new ImageBrush()
            {
                Source = _image as IImageBrushSource,
                Stretch = Stretch.UniformToFill
            };
        }
    }

    private void UpdateSizeConstraints()
    {
        if (MainImage != null)
        {
            MainImage.MaxWidth = _maxWidth;
            MainImage.MaxHeight = _maxHeight;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            (_image as IDisposable)?.Dispose();
            MainImage.Background = null;
        }
        
        _disposed = true;
    }

    ~ImageReader()
    {
        Dispose(false);
    }
}