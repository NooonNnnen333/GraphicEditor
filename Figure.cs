using Microsoft.Maui.Graphics;

namespace GraphicApp;

public abstract class Figure // Абстрактный базовый класс на основе кторого булут создаваться фигуры 
{
    public int id
    {
        set
        {
            name = value.ToString();
        }
    }
    
    
    public string name { get; set; } // Название, которое будет отображаться в пользовательском интерфейсе

//---------Характеристики Canvas (кисти)--------------------------------------------------------------------------------
    
    public Color _color;
    public int _strokeSize;
    
//----------------------------------------------------------------------------------------------------------------------

    public Dictionary<string, int> coordinates; // Тут хранятся координаты опорных точек создаваемой фигуры

//======================================================================================================================

}

public class Rectangle : Figure
{
    public Rectangle(int XLV, int YLV, int XRV, int YRV, int XRN, int YRN, int XLN, int YLN, int _id)
    {
        
        coordinates = new Dictionary<string, int> // инициализируем словарь координат
        {
            { "XLV", XLV },
            { "YLV", YLV },
            { "XRV", XRV },
            { "YRV", YRV },
            { "XRN", XRN },
            { "YRN", YRN },
            { "XLN", XLN },
            { "YLN", YLN }
        };

        id = _id;

        name = "Прямоугольник " + name;


    }

}
