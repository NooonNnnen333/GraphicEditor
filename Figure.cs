using Microsoft.Maui.Graphics;

namespace GraphicApp;

public class PointF
{
    public int X;
    public int Y;

    public PointF(int _x, int _y)
    {
        (X, Y) = (_x, _y);
    }
    public PointF(){}
}

public abstract class Figure // Абстрактный базовый класс на основе кторого булут создаваться фигуры 
{
    public int id; // Для работы со слоями
    public string name { get; set; } // Название, которое будет отображаться в пользовательском интерфейсе
    
//---------Характеристики Canvas (кисти)--------------------------------------------------------------------------------
    
    public Color colorStrok;
    public Color? fillColor;
    public int _strokeSize;
    
//----------------------------------------------------------------------------------------------------------------------

    public List<PointF> coordinates; // Тут хранятся координаты опорных точек создаваемой фигуры

//======================================================================================================================

}

public class Rectangle : Figure
{
    public Rectangle(int XLV, int YLV, int XRV, int YRV, int XRN, int YRN, int XLN, int YLN, int _id)
    {
        
        coordinates = new List<PointF>
        {
            new PointF { X = XLV, Y = YLV },
            new PointF { X = XRV, Y = YRV },
            new PointF { X = XRN, Y = YRN },
            new PointF { X = XLN, Y = YLN }
        };
        
        // coordinates = new Dictionary<string, int> // инициализируем словарь координат
        // {
        //     { "XLV", XLV },
        //     { "YLV", YLV },
        //     { "XRV", XRV },
        //     { "YRV", YRV },
        //     { "XRN", XRN },
        //     { "YRN", YRN },
        //     { "XLN", XLN },
        //     { "YLN", YLN }
        // };
        
        id = _id;
        name = "Прямоугольник " + (_id + 1); 
        
    }

    //public string name { get; set; } 
}

public class Spline : Figure
{
    public Spline(List<PointF> ctrlPoints, int id, int segments = 20)
    {
        this.id   = id;
        name      = $"Сплайн {id + 1}";
        coordinates = new List<PointF>();

        if (ctrlPoints.Count < 2)
            throw new ArgumentException("Для сплайна нужно минимум 2 точки");

        // Дублируем первую/последнюю для Catmull–Rom
        var pts = new List<PointF> { ctrlPoints[0] };
        pts.AddRange(ctrlPoints);
        pts.Add(ctrlPoints[^1]);

        for (int i = 0; i < pts.Count - 3; i++)
        {
            var p0 = pts[i];     var p1 = pts[i + 1];
            var p2 = pts[i + 2]; var p3 = pts[i + 3];

            for (int j = 0; j <= segments; j++)
            {
                float t  = j / (float)segments;
                float t2 = t * t, t3 = t2 * t;

                float x = 0.5f * ((2 * p1.X) +
                                  (-p0.X + p2.X) * t +
                                  (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t2 +
                                  (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t3);
                float y = 0.5f * ((2 * p1.Y) +
                                  (-p0.Y + p2.Y) * t +
                                  (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t2 +
                                  (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t3);

                coordinates.Add(new PointF((int)x, (int)y));
            }
        }
    }

    
    
}
