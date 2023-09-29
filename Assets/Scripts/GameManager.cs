using System;

public static class GameManager
{
    public static event Action OnPause;
    public static event Action OnUnPause;

    public static event Action OnFoodEaten;

    public static void Pause()
    {
        OnPause?.Invoke();
    }

    public static void UnPause()
    {
        OnUnPause?.Invoke();
    }

    public static void EatFood()
    {
        OnFoodEaten?.Invoke();
    }
}
