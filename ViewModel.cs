using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GraphicApp;

public class Shape
{
    // Координаты вершин трапеции:
    // Верхняя левая новая версия
    public int XLV { get; set; }
    public int YVL { get; set; }
    // Верхняя правая
    public int XRV { get; set; }
    public int YVR { get; set; }
    // Нижняя правая
    public int XRN { get; set; }
    public int YNR { get; set; }
    // Нижняя левая
    public int XLN { get; set; }
    public int YNL { get; set; }
}

public class Graphics : GraphicsView, IDrawable
{
    // Список трапеций для отрисовки
    public List<Shape> ShapesE { get; } = new();

    public Graphics()
    {
        Drawable = this;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 4;

        foreach (var shape in ShapesE)
        {
            var path = new PathF();
            path.MoveTo(shape.XLV, shape.YVL);         // Верхняя левая
            path.LineTo(shape.XRV, shape.YVR);          // Верхняя правая
            path.LineTo(shape.XRN, shape.YNR);          // Нижняя правая
            path.LineTo(shape.XLN, shape.YNL);          // Нижняя левая
            path.Close();
            canvas.DrawPath(path);
        }
    }

    // Метод добавления трапеции
    public void AddShape(Shape s)
    {
        ShapesE.Add(s);
        Invalidate();
    }
    
    // Параллельный перенос: смещает каждую вершину на dx по X и dy по Y
    public void Translate(double dx, double dy)
    {
        foreach (var shape in ShapesE)
        {
            shape.XLV += (int)dx;
            shape.YVL += (int)dy;
            shape.XRV += (int)dx;
            shape.YVR += (int)dy;
            shape.XRN += (int)dx;
            shape.YNR += (int)dy;
            shape.XLN += (int)dx;
            shape.YNL += (int)dy;
        }
        Invalidate();
    }
    
    // Поворот относительно начала координат: для каждого угла применяем:
    // x' = x*cos(angle) - y*sin(angle)
    // y' = x*sin(angle) + y*cos(angle)
    public void Rotate(double angle)
    {
        double rad = angle * Math.PI / 180.0;
        double cos = Math.Cos(rad);
        double sin = Math.Sin(rad);

        foreach (var shape in ShapesE)
        {
            // Вычисляем центр трапеции
            double centerX = (shape.XLV + shape.XRV + shape.XRN + shape.XLN) / 4.0;
            double centerY = (shape.YVL + shape.YVR + shape.YNR + shape.YNL) / 4.0;

            // Верхняя левая вершина
            double newXLV = centerX + ((shape.XLV - centerX) * cos - (shape.YVL - centerY) * sin);
            double newYVL = centerY + ((shape.XLV - centerX) * sin + (shape.YVL - centerY) * cos);
            shape.XLV = (int)newXLV;
            shape.YVL = (int)newYVL;

            // Верхняя правая вершина
            double newXRV = centerX + ((shape.XRV - centerX) * cos - (shape.YVR - centerY) * sin);
            double newYVR = centerY + ((shape.XRV - centerX) * sin + (shape.YVR - centerY) * cos);
            shape.XRV = (int)newXRV;
            shape.YVR = (int)newYVR;

            // Нижняя правая вершина
            double newXRN = centerX + ((shape.XRN - centerX) * cos - (shape.YNR - centerY) * sin);
            double newYNR = centerY + ((shape.XRN - centerX) * sin + (shape.YNR - centerY) * cos);
            shape.XRN = (int)newXRN;
            shape.YNR = (int)newYNR;

            // Нижняя левая вершина
            double newXLN = centerX + ((shape.XLN - centerX) * cos - (shape.YNL - centerY) * sin);
            double newYNL = centerY + ((shape.XLN - centerX) * sin + (shape.YNL - centerY) * cos);
            shape.XLN = (int)newXLN;
            shape.YNL = (int)newYNL;
        }
        Invalidate();
    }
    
    // Масштабирование: умножаем координаты каждой вершины на коэффициенты scaleX и scaleY
    public void Scale(double scaleX, double scaleY)
    {
        foreach (var shape in ShapesE)
        {
            shape.XLV = (int)(shape.XLV * scaleX);
            shape.YVL = (int)(shape.YVL * scaleY);
            
            shape.XRV = (int)(shape.XRV * scaleX);
            shape.YVR = (int)(shape.YVR * scaleY);
            
            shape.XRN = (int)(shape.XRN * scaleX);
            shape.YNR = (int)(shape.YNR * scaleY);
            
            shape.XLN = (int)(shape.XLN * scaleX);
            shape.YNL = (int)(shape.YNL * scaleY);
        }
        Invalidate();
    }
}

//================================================================================================================================================================    

public partial class ViewModel : ObservableObject
{
    private readonly Graphics _graphics;

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
    }

    // Создание трапеции в точке (X, Y)
    [RelayCommand]
    public void CreateTrapezoid()
    {
        _graphics.AddShape(new Shape { XLV = X + 30, YVL = Y, XRV = X + 80, YVR = Y, XLN = X, YNL = Y+55, XRN = X+110, YNR = Y+55});
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
}