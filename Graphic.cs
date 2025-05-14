namespace GraphicApp;



public class Shape
{
    public int ID;
    public ICanvas canvas;
    public int XLV, YVL, XRV, YVR, XRN, YNR, XLN, YNL;     
}




    
public class Graphics : GraphicsView, IDrawable
{
    public List<Shape> ShapesE { get; } = new(); // Список трапеций для отрисовки
    
    public ICanvas? canvasSvoistvo; // Оперделяем свойство для хранения и измененя цвета
    public Color сolorStroke { get; private set; }
    
    

    
    
//======================================================================================================================    
    public Graphics() // Конструктор
    {
        Drawable = this;
        canvasSvoistvo = null;
        сolorStroke = Colors.Blue;
        
        StartInteraction += OnTouchStarted;
        DragInteraction += OnTouchMoved;

    }

    public void Cvet() // Изменение цвета
    {
        сolorStroke = Colors.Aqua; // или любой другой цвет
        Invalidate();
    }

    
    public void Draw(ICanvas canvas, RectF dirtyRect) // Встроенный метод для отрисовки
    {
        canvasSvoistvo = canvas;
        canvasSvoistvo.StrokeColor = сolorStroke;
        canvasSvoistvo.StrokeSize = 5;
        DrawFigure(canvas);
    }

    public void DrawFigure(ICanvas canvas) 
    {
        
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
    
//======================================================================================================================    

    private PointF? _firstPoint;

    private void OnTouchStarted(object? sender, TouchEventArgs e)
    {
        var p = e.Touches[0]; // всегда первая точка

        if (_firstPoint is null)
        {
            // первый клик — запоминаем
            _firstPoint = p;
        }
        else
        {
            // второй клик — строим по двум точкам новую фигуру
            var second = p;
            var shape = new Shape
            {
                XLV = (int)_firstPoint.Value.X,
                YVL = (int)_firstPoint.Value.Y,
                XRV = (int)second.X,
                YVR = (int)second.Y,
                // остальные вершины можно жестко задать или вычислить...
                XRN = (int)second.X,
                YNR = (int)second.Y + 50,
                XLN = (int)_firstPoint.Value.X,
                YNL = (int)_firstPoint.Value.Y + 50,
            };
            AddShape(shape);

            // сбрасываем для новой пары кликов
            _firstPoint = null;
        }
    }
    private void OnTouchMoved(object? sender, TouchEventArgs e)
    {
        var p = e.Touches[0]; // всегда первая точка

        if (_firstPoint is null)
        {
            // первый клик — запоминаем
            _firstPoint = p;
        }
        else
        {
            // второй клик — строим по двум точкам новую фигуру
            var second = p;
            var shape = new Shape
            {
                XLV = (int)_firstPoint.Value.X,
                YVL = (int)_firstPoint.Value.Y,
                XRV = (int)second.X,
                YVR = (int)second.Y,
                // остальные вершины можно жестко задать или вычислить...
                XRN = (int)second.X,
                YNR = (int)second.Y + 50,
                XLN = (int)_firstPoint.Value.X,
                YNL = (int)_firstPoint.Value.Y + 50,
            };
            AddShape(shape);

            // сбрасываем для новой пары кликов
            _firstPoint = null;
        }
    }

    private void OnTouchEnded(object? sender, TouchEventArgs e)
    {
        // опциональная логика по отпусканию
    }
}


