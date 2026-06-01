using UnityEngine;
using UnityEngine.InputSystem;

public class Notes : MonoBehaviour
{
    [Header("ノーツの設定")]
    [SerializeField] private int notesSpeed = 5;  // 移動速度
    [SerializeField] private int laneNum = 1;     // 担当するレーン（1～4）

    private AudioManager audioManager;
    private bool isInResultZone = false; // 判定ゾーンに入っているか

    void Start()
    {
        // 画面内から AudioManager を自動検索して紐付ける
        audioManager = Object.FindFirstObjectByType<AudioManager>();
    }

    void Update()
    {
        // 音楽がまだ始まっていないなら、その場に待機
        if (audioManager == null || !audioManager.IsMusicStarted) return;

        // 【移動】判定線に向かって進む
        transform.position -= transform.forward * Time.deltaTime * notesSpeed;

        // 【判定】ゾーン内で、かつ自分のレーンのキーが押されたら「即消去」
        if (isInResultZone && IsMyLaneKeyPressed())
        {
            Destroy(gameObject);
        }
    }

    // 自分のレーンのキーが押されたかを判定する関数
    private bool IsMyLaneKeyPressed()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return false;

        return laneNum switch
        {
            1 => keyboard.dKey.wasPressedThisFrame,
            2 => keyboard.fKey.wasPressedThisFrame,
            3 => keyboard.gKey.wasPressedThisFrame || keyboard.hKey.wasPressedThisFrame,
            4 => keyboard.jKey.wasPressedThisFrame,
            5 => keyboard.kKey.wasPressedThisFrame,
            _ => false
        };
    }

    // 判定ゾーン（ResultZone）に入った瞬間
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ResultZone"))
        {
            isInResultZone = true;
        }
    }

    // 判定ゾーン（ResultZone）から完全に出た瞬間
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ResultZone"))
        {
            isInResultZone = false;
        }
    }
}