namespace GraphicApp;





    
public class Graphics : GraphicsView, IDrawable
{
    public List<Figure> ShapesE { get; } = new(); // Список трапеций для отрисовки
    
//======================================================================================================================    
    public Graphics() // Конструктор
    {
        Drawable = this;
        
        StartInteraction += (sender, args) => ClickedOnCanvas?.Invoke(sender, args);

    }
    /* Делегат и триггер для передачи области во viewmodel */
    public delegate void CanvasInspectionHeandler(object? sender, TouchEventArgs eventArgs); // Делегат для принятия нажатия

    public event CanvasInspectionHeandler? ClickedOnCanvas;
    
    /* Изменение цвета */
    public void Cvet() 
    {
        Invalidate();
    }

    
    public void Draw(ICanvas canvas, RectF dirtyRect) // Встроенный метод для отрисовки
    {

        canvas.StrokeColor = Colors.Blue;
        canvas.StrokeSize = 5;
        DrawFigure(canvas);
        

        
    }

    public void DrawFigure(ICanvas canvas) 
    {
        
        foreach (Figure shape in ShapesE)
        {
            var path = new PathF(); // Создаём новый линию фигуры
            var values = shape.coordinates.Values.ToList(); // Список из ключей словаря координат фигуры 
            
            /* Перебор точек и соединение их линиями */
            path.MoveTo(values[0], values[1]);
            for (int i = 2; i < values.Count-1; i += 2)
            {
                path.LineTo(values[i], values[i + 1]);
            }
            
            path.Close();
            canvas.DrawPath(path);
            
            /* Предыдущая версия
             foreach (var shape in ShapesE)
               {
                   var path = new PathF();
                   path.MoveTo(shape.XLV, shape.YVL);         
                   path.LineTo(shape.XRV, shape.YVR);         
                   path.LineTo(shape.XRN, shape.YNR);         
                   path.LineTo(shape.XLN, shape.YNL);         
                   path.Close();
                   canvas.DrawPath(path);
               }*/
        }
    }

    // Метод добавления трапеции
    public void AddShape(Rectangle s)
    {
        ShapesE.Add(s);
        Invalidate();
    }
    
    // Параллельный перенос: смещает каждую вершину на dx по X и dy по Y
    public void Translate(double dx, double dy)
    {
        // Ключи для X и Y координат
        var xKeys = new[] { "XLV", "XRV", "XRN", "XLN" };
        var yKeys = new[] { "YLV", "YVR", "YNR", "YLN" };

        foreach (var shape in ShapesE)
        {
            foreach (var key in xKeys)
                shape.coordinates[key] += (int)dx;
            foreach (var key in yKeys)
                shape.coordinates[key] += (int)dy;
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
            // Собираем текущие координаты в массивы
            var Xs = new[] { shape.coordinates["XLV"], shape.coordinates["XRV"], shape.coordinates["XRN"], shape.coordinates["XLN"] };
            var Ys = new[] { shape.coordinates["YLV"], shape.coordinates["YVR"], shape.coordinates["YNR"], shape.coordinates["YLN"] };

            // Центр фигуры
            double centerX = Xs.Average();
            double centerY = Ys.Average();

            // Ключи в том же порядке
            string[] keysX = { "XLV", "XRV", "XRN", "XLN" };
            string[] keysY = { "YLV", "YVR", "YNR", "YLN" };

            for (int i = 0; i < 4; i++)
            {
                double x = Xs[i] - centerX;
                double y = Ys[i] - centerY;
                int newX = (int)(centerX + (x * cos - y * sin));
                int newY = (int)(centerY + (x * sin + y * cos));
                shape.coordinates[keysX[i]] = newX;
                shape.coordinates[keysY[i]] = newY;
            }
        }
        Invalidate();
    }
    
    // Масштабирование: умножаем координаты каждой вершины на коэффициенты scaleX и scaleY
    public void Scale(double scaleX, double scaleY)
    {
        foreach (var shape in ShapesE)
        {
            // Текущие координаты
            var Xs = new[] { shape.coordinates["XLV"], shape.coordinates["XRV"], shape.coordinates["XRN"], shape.coordinates["XLN"] };
            var Ys = new[] { shape.coordinates["YLV"], shape.coordinates["YVR"], shape.coordinates["YNR"], shape.coordinates["YLN"] };

            // Центр фигуры
            double centerX = Xs.Average();
            double centerY = Ys.Average();

            string[] keysX = { "XLV", "XRV", "XRN", "XLN" };
            string[] keysY = { "YLV", "YVR", "YNR", "YLN" };

            for (int i = 0; i < 4; i++)
            {
                double dx = Xs[i] - centerX;
                double dy = Ys[i] - centerY;
                shape.coordinates[keysX[i]] = (int)(centerX + dx * scaleX);
                shape.coordinates[keysY[i]] = (int)(centerY + dy * scaleY);
            }
        }
        Invalidate();
    }
    

    
}
