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
            var values = shape.coordinates; // Список из ключей словаря координат фигуры 
            
            /* Перебор точек и соединение их линиями */
            path.MoveTo(values[0].X, values[0].Y);
            for (int i = 1; i < values.Count; i ++)
            {
                path.LineTo(values[i].X, values[i].Y);
            }
            
            path.Close();
            canvas.DrawPath(path);
            

        }
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
