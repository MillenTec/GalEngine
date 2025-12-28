using System;
using JetBrains.Annotations;

public static class GameEvents {
    public static string GamingPackPath = null;
    public static event Action<string> OnSelectedPlotPack;
    [CanBeNull] public static event Action<string> OnObjectClicked;

    public static void SendEventOnSelectedPlotPack(string path) {
        OnSelectedPlotPack?.Invoke(path);
    }
    
    public static void SendEventOnObjectClicked(string id = null) {
        OnObjectClicked?.Invoke(id);
    }
}