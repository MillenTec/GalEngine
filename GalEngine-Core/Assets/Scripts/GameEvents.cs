using System;
using JetBrains.Annotations;
using UnityEngine;

public static class GameEvents {
    public static string GamingPackPath = null;
    public static event Action<string> OnSelectedPlotPack;
    [CanBeNull] public static event Action<string> OnObjectClicked;
    public static event Action<Sprite> OnGamingBackgroundChanged;
    public static event Action<AudioClip> OnGamingVoiceChanged;

    public static void SendEventOnSelectedPlotPack(string path) {
        OnSelectedPlotPack?.Invoke(path);
    }
    
    public static void SendEventOnObjectClicked(string id = null) {
        OnObjectClicked?.Invoke(id);
    }

    public static void SendEventOnGamingBackgroundChanged(Sprite sprite) {
        OnGamingBackgroundChanged?.Invoke(sprite);
    }

    public static void SendEventOnGamingVoiceChanged(AudioClip audioClip) {
        OnGamingVoiceChanged?.Invoke(audioClip);
    }
}