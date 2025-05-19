
namespace GraphicApp;

public partial class Graphics
{
    void SimpleScanlineFill(ICanvas canvas,
        List<PointF> vertices,
        Color fillColor)
    {
        if (fillColor == Colors.Transparent) return;

        int ymin = (int)vertices.Min(p => p.Y);
        int ymax = (int)vertices.Max(p => p.Y);

        canvas.FillColor = fillColor;

        for (int y = ymin; y <= ymax; y++)
        {
            var nodes = new List<int>();

            for (int i = 0; i < vertices.Count; i++)
            {
                int j  = (i == 0) ? vertices.Count - 1 : i - 1;
                int x1 = (int)vertices[j].X, y1 = (int)vertices[j].Y;
                int x2 = (int)vertices[i].X, y2 = (int)vertices[i].Y;

                bool crosses = (y1 <= y && y2 > y) || (y2 <= y && y1 > y);
                if (crosses && y1 != y2)
                {
                    int x = x1 + (y - y1) * (x2 - x1) / (y2 - y1);
                    nodes.Add(x);
                }
            }

            nodes.Sort();

            for (int k = 0; k + 1 < nodes.Count; k += 2)
                canvas.DrawLine(nodes[k], y, nodes[k + 1], y); // горизонтальная полоса
        }
    }


}
