using System;

public static class GameEvents {
    public static event Action<string> OnSelectedPlotPack;

    public static void SendEventOnSelectedPlotPack(string path) {
        OnSelectedPlotPack?.Invoke(path);
    }
}