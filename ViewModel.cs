using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Platform;

namespace GraphicApp;

public partial class ViewModel : ObservableObject
{
    private readonly Graphics _graphics;
    private int _id; // Id фигуры, к которую мы создаём, для реализации работы со слоями

    [ObservableProperty] 
    public ObservableCollection<Figure> figures; // Папка для хранения созданных фигур
    
//======================================================================================================================    
    
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
        _graphics = graphics; // Для взаимодействия с классом "Graphics" (а значит и с холстом)
        _id = 0; // Изначально номер фигуры - 0
        _graphics.ClickedOnCanvas += OnTouchStarted; // Для взаимодействие через мышь / палец (если на мобильном устройстве) с холстом
    }

    // Создание трапеции в точке (X, Y)
    [RelayCommand]
    public void CreateFigure()
    {
        var shape = new Rectangle(
            X + 30, // XLV
            Y, // YLV
            X + 80, // XRV
            Y, // YRV
            X + 110, // XRN
            Y + 55, // YNR
            X, // XLN
            Y + 55, // YLN
            _id++
        );

        _graphics.AddShape(shape);
        Figures = new ObservableCollection<Figure>(_graphics.ShapesE);
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

//======================================================================================================================    

    private PointF? _firstPoint;

    private void OnTouchStarted(object? sender, TouchEventArgs eventArgs)
    {
        var p = eventArgs.Touches[0]; // всегда первая точка

        if (_firstPoint is null)
        {
            // первый клик — запоминаем
            _firstPoint = new PointF(Convert.ToInt32(p.X), Convert.ToInt32(p.Y));
        }
        else
        {
            // второй клик — строим по двум точкам новую фигуру
            var second = p;
            var shape = new Rectangle(
                (int)_firstPoint.X,
                (int)_firstPoint.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)second.X,
                (int)_firstPoint.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)second.X,
                (int)second.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)_firstPoint.X,
                (int)second.Y,
                _id++
            );
            
            // если нужно — добавляй параметры тут
            _graphics.AddShape(shape); 

            // сбрасываем для новой пары кликов
            _firstPoint = null;
            
            Figures = new ObservableCollection<Figure>(_graphics.ShapesE);
        }
    }
    
    
}