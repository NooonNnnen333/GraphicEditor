using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Platform;

namespace GraphicApp;

public partial class ViewModel : ObservableObject
{
    private readonly Graphics _graphics;
    private int _id; // Id фигуры, к которую мы создаём, для реализации работы со слоями

    // Координаты для добавления трапеции
    [ObservableProperty] private int x;
    [ObservableProperty] private int y;

    // Параметры параллельного переноса
    [ObservableProperty] private double translateX;
    [ObservableProperty] private double translateY;

    // Параметры поворота (в градусах)
    [ObservableProperty] private double rotationAngle;

    // Параметры масштабирования
    [ObservableProperty] private double scaleX = 1;
    [ObservableProperty] private double scaleY = 1;

    public ViewModel(Graphics graphics)
    {
        _graphics = graphics;
        _id = 0;
    }

    // Создание трапеции в точке (X, Y)
    [RelayCommand]
    public void CreateTrapezoid()
    {
        _graphics.AddShape(new Shape { XLV = X + 30, YVL = Y, XRV = X + 80, YVR = Y, XLN = X, YNL = Y+55, XRN = X+110, YNR = Y+55, canvas = _graphics.canvasSvoistvo, ID = _id++});
    }

    // Параллельный перенос
    [RelayCommand]
    public void Translate()
    {
        _graphics.Translate(TranslateX, TranslateY);
    }
    
    // Поворот
    [RelayCommand]
    public void Rotate()
    {
        _graphics.Rotate(RotationAngle);
    }
    
    // Масштабирование
    [RelayCommand]
    public void Scale()
    {
        _graphics.Scale(ScaleX, ScaleY);
    }

    [RelayCommand]
    public void Cvet()
    {
        _graphics.Cvet();
    } 
}