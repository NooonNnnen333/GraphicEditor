namespace GraphicApp;

public partial class MainPage : ContentPage
{
    private PointF? _firstPoint;
    public ViewModel _viewModel;
    
    public MainPage()
    {
        InitializeComponent();

        // Создаём ViewModel и передаём ей ссылку на CanvasControl из XAML
        _viewModel = new ViewModel(CanvasControl);

        // Назначаем ViewModel в качестве BindingContext (если нужно биндинги)
        BindingContext = _viewModel;
    }

    // вызывается при первом касании / клике
    private void OnCanvasStart(object sender, TouchEventArgs e)
    {
        var p = e.Touches[0];   // первая точка
        // тут ваша логика для первого нажатия
    }

    // вызывается при движении пальца / мыши
    private void OnCanvasDrag(object sender, TouchEventArgs e)
    {
        var p = e.Touches[0];
        // тут можете, например, динамически показывать линию
    }

    // вызывается при отпускании
    private void OnCanvasEnd(object sender, TouchEventArgs e)
    {
        var p = e.Touches[0];
        // тут финализируете фигуру
    }

}