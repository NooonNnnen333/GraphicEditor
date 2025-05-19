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
    public int id;
    public string name { get; set; }
    
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

    //public string name { get; set; } // Название, которое будет отображаться в пользовательском интерфейсе
    
}
