using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // 音楽スピーカー
    private bool isMusicStarted = false;              // 再生中フラグ

    public bool IsMusicStarted => isMusicStarted;

    void Start()
    {
        isMusicStarted = false; // フラグの初期化
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 【停止】Tabキーで音楽をストップ
        if (keyboard.tabKey.wasPressedThisFrame)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                isMusicStarted = false; // 再生可能状態に戻す
            }
        }

        // 既に再生中ならこれ以降（再生処理）はスキップ
        if (isMusicStarted) return;

        // 【再生】DFJKGHのどれかが押されたら音楽スタート
        if (keyboard.dKey.wasPressedThisFrame || keyboard.fKey.wasPressedThisFrame ||
            keyboard.gKey.wasPressedThisFrame || keyboard.jKey.wasPressedThisFrame ||
            keyboard.hKey.wasPressedThisFrame || keyboard.kKey.wasPressedThisFrame)
        {
            if (audioSource != null)
            {
                audioSource.Play();
                isMusicStarted = true; // 再生中フラグをON
            }
        }
    }
}