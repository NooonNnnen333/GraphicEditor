namespace GraphicApp;

using Microsoft.Maui.Controls;


public class BehaviorsClass : Behavior<ScrollView>
{
    double _startX, _startY;
    double _currentScale = 1;

    protected override void OnAttachedTo(ScrollView scroll)
    {
        base.OnAttachedTo(scroll);

        // Панорамирование (уже было)
        var pan = new PanGestureRecognizer(); // Иницилизируем объект, который считывает свайпы
        pan.PanUpdated += Pan_PanUpdated;

        void Pan_PanUpdated(object s, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = scroll.ScrollX;
                    _startY = scroll.ScrollY;
                    break;
                case GestureStatus.Running:
                    scroll.ScrollToAsync(
                        _startX - e.TotalX,
                        _startY - e.TotalY,
                        true);
                    break;
            }
        };
        scroll.GestureRecognizers.Add(pan);

        // Зум (приближение/отдаление)
        var pinch = new PinchGestureRecognizer();
        pinch.PinchUpdated += Pinch_Updated;
        
        void Pinch_Updated(object? s, PinchGestureUpdatedEventArgs e)
        {
            // scroll.Content — это твой Graphics
            if (scroll.Content == null)
                return;

            if (e.Status == GestureStatus.Running)
            {
                double scale = _currentScale * e.Scale;
                scroll.Content.Scale = scale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                if (scroll.Content != null)
                    _currentScale = scroll.Content.Scale;
            }
        };
        scroll.GestureRecognizers.Add(pinch);
    }

}
