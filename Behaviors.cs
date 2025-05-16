namespace GraphicApp;

public class BehaviorsClass : Behavior<ScrollView>
{
    
    double _startX, _startY;

    protected override void OnAttachedTo(ScrollView scroll)
    {
        base.OnAttachedTo(scroll);

        var pan = new PanGestureRecognizer();
        pan.PanUpdated += (s, e) =>
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
                        false);
                    break;
            }
        };

        scroll.GestureRecognizers.Add(pan);
        
    }
    
}