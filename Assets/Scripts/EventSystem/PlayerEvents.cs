using System;

public class PlayerEvents
{
    public event Action OnInteractPressed;
    public void InteractPressed()
    {
        OnInteractPressed?.Invoke();
    }
}
