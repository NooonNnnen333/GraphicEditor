namespace GraphicApp;






public partial class Graphics : GraphicsView, IDrawable
{
    public List<Figure> ShapesE { get; } = new(); // Список трапеций для отрисовки
    public List<PointF> ControlPoints;

    public short? mode; // Режим для класс draw

//======================================================================================================================
    public Graphics() // Конструктор
    {
        Drawable = this;
        StartInteraction += (sender, args) => ClickedOnCanvas?.Invoke(sender, args);
        Console.WriteLine("Возобновление");
        flag = false;

    }

    /* Делегат и триггер для передачи области во viewmodel */
    public delegate void
        CanvasInspectionHeandler(object? sender, TouchEventArgs eventArgs); // Делегат для принятия нажатия

    public event CanvasInspectionHeandler? ClickedOnCanvas;

    /* Изменение цвета */
    public void Cvet()
    {
        ShapesE[ShapesE.Count - 1].fillColor = Colors.Blue;
        Invalidate();
    }

//----------------------------------------------------------------------------------------------------------------------    

    public void Draw(ICanvas canvas, RectF dirtyRect) // Встроенный метод для отрисовки
    {

        switch (mode)
        {
            case 0: // Создание прямоугольника
                canvas.StrokeColor = Colors.Blue;
                canvas.StrokeSize = 5;
                DrawFigure(canvas);
                break;
            case 1:
                FillDraw(canvas, ShapesE[ShapesE.Count - 1]);
                break;
            case 3: // Создание сплайна 
                canvas.StrokeColor = Colors.Blue;
                canvas.StrokeSize = 5;
                DrawFigure(canvas);
                break;

        }


    }

    public void DrawFigure(ICanvas canvas)
    {

        foreach (Figure shape in ShapesE)
        {
            var path = new PathF(); // Создаём новый линию фигуры
            var values = shape.coordinates; // Список из ключей словаря координат фигуры 

            /* Перебор точек и соединение их линиями */
            path.MoveTo(values[0].X, values[0].Y);
            for (int i = 1; i < values.Count; i++)
                path.LineTo(values[i].X, values[i].Y);

            // Закрываем и заливаем **только** не-сплайны
            if (shape is not Spline)
            {
                path.Close();
                FillDraw(canvas, shape);
            }

            canvas.DrawPath(path);

        }

    }

    private void FillDraw(ICanvas canvas, Figure figureFill)
    {

        if (figureFill.fillColor == null)
        {
            Console.WriteLine("Нет цвета");
            return;
        }

        SimpleScanlineFill(canvas, figureFill.coordinates, figureFill.fillColor);
        Console.WriteLine($"Id {figureFill.id}");

        mode = 0;
    }



    // private void Spline(ICanvas canvas) 
    // {
    //     canvas.DrawPath();
    // }

//----------------------------------------------------------------------------------------------------------------------    

    private PointF? firstPoint;

    public bool CreateRectangleWithTouch(PointF p, int id)
    {
        //_controlPoints = null;

        if (firstPoint is null)
        {
            // первый клик — запоминаем
            firstPoint = p;
            return false;
        }
        else
        {
            // второй клик — строим по двум точкам новую фигуру
            PointF secondPoint = p;
            var shape = new Rectangle(
                (int)firstPoint.X,
                (int)firstPoint.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)secondPoint.X,
                (int)firstPoint.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)secondPoint.X,
                (int)secondPoint.Y,
//----------------------------------------------------------------------------------------------------------------------
                (int)firstPoint.X,
                (int)secondPoint.Y,
                id
            );
            ShapesE.Add(shape);
            firstPoint = null;
            Invalidate();
        }

        return true;
    }

    private List<PointF>? _controlPoints;
    private bool flag;
    public bool CreateSpline(PointF p, int id)
    {
        if ((_controlPoints != null && ShapesE.Count > 0 && ShapesE[ShapesE.Count - 1] is Spline) || flag == true)
            _controlPoints.Add(p);
        else
        {
            _controlPoints = null;
            _controlPoints ??= new List<PointF>();  // Создаём новый список
            _controlPoints.Add(p);
            flag = true;
        }

        // строим, только если точек ≥ 2
        if (_controlPoints.Count >= 2)
        {
            if (ShapesE.Count > 0 && ShapesE[ShapesE.Count-1] is Spline oldSpline)
            {
                ShapesE[^1] = new Spline(_controlPoints, oldSpline.id);
                Invalidate();
                flag = false;

                return false;
            }

            flag = false;
            // первая готовая дуга
            ShapesE.Add(new Spline(_controlPoints, id));
            Console.WriteLine("новый");
            Invalidate();
            return true; // сразу увеличиваем Id


        }

        return false;
    }






// Метод добавления фигуры
    public void AddShape(Rectangle s)
    {
        ShapesE.Add(s);
        Invalidate();
    }
    
    // Параллельный перенос: смещает каждую вершину на dx по X и dy по Y
    public void Translate(double dx, double dy)
    {
        foreach (var shape in ShapesE)
        {
            for (int i = 0; i < shape.coordinates.Count; i++)
            {
                shape.coordinates[i].X += (int)dx;
                shape.coordinates[i].Y += (int)dy;
            }
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
            var verts = shape.coordinates;
            double centerX = verts.Average(p => p.X);
            double centerY = verts.Average(p => p.Y);

            for (int i = 0; i < verts.Count; i++)
            {
                double dx = verts[i].X - centerX;
                double dy = verts[i].Y - centerY;
                int newX = (int)(centerX + (dx * cos - dy * sin));
                int newY = (int)(centerY + (dx * sin + dy * cos));
                verts[i].X = newX;
                verts[i].Y = newY;
            }
        }
        Invalidate();
    }

    
    // Масштабирование: умножаем координаты каждой вершины на коэффициенты scaleX и scaleY
    public void Scale(double sx, double sy)
    {
        foreach (var shape in ShapesE)
        {
            var verts = shape.coordinates;
            double centerX = verts.Average(p => p.X);
            double centerY = verts.Average(p => p.Y);

            for (int i = 0; i < verts.Count; i++)
            {
                double dx = verts[i].X - centerX;
                double dy = verts[i].Y - centerY;
                verts[i].X = (int)(centerX + dx * sx);
                verts[i].Y = (int)(centerY + dy * sy);
            }
        }
        Invalidate();
    }

    

    
}
