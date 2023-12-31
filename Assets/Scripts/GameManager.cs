using UnityEngine;
using System;

public static class GameManager
{
    public static event Action OnPause;
    public static event Action OnUnPause;

    public static event Action OnEnterBook;
    public static event Action OnExitBook;

    public static event Action OnFoodEaten;

    public static event Action OnFuriousChange;

    public static event Action OnWin;
    public static event Action OnLose;

    public const int MaxFuriousFrogs = 5;
    public const int FrogWinCount  = 12;
    public static int FuriousFrogCount;
    public static int FrogCount;

    public static bool IsPaused;

    public static bool ShowMenuOnPause;
    public static bool GameRunning = true;

    public static bool TutorialShown;

    public static void Pause()
    {
        IsPaused = true;
        OnPause?.Invoke();
    }

    public static void UnPause()
    {
        IsPaused = false;
        OnUnPause?.Invoke();
    }



    public static void EnterBook()
    {
        OnEnterBook?.Invoke();
    }

    public static void ExitBook()
    {
        OnExitBook?.Invoke();
    }



    public static void EatFood()
    {
        OnFoodEaten?.Invoke();
    }


    public static void IncreaseFrogCount()
    {
        FrogCount++;
        if (FrogCount >= FrogWinCount)
        {
            ShowMenuOnPause = false;
            GameRunning = false;

            Pause();
            OnWin?.Invoke();
        }
    }

    public static void FrogEnterFurious()
    {
        FuriousFrogCount++;

        if (FuriousFrogCount >= MaxFuriousFrogs)
        {
            ShowMenuOnPause = false;
            GameRunning = false;

            FuriousFrogCount = 0;
            Pause();
            OnLose?.Invoke();
        }

        OnFuriousChange?.Invoke();
    }

    public static void FrogExitFurious()
    {
        FuriousFrogCount--;

        OnFuriousChange?.Invoke();
    }

    public static bool TryGetFrog(Collider coll, out Frog frog)
    {
        if (coll.TryGetComponent(out GrabBranch branch))
        {
            frog = branch.RootObject.GetComponent<Frog>();
            return true;
        }
        else
        {
            frog = null;
            return false;
        }
    }
}
