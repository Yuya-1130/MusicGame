using UnityEngine;
using UnityEngine.InputSystem;
public class Light : MonoBehaviour
{
    [SerializeField] float Speed = 3;
    [SerializeField] private int num = 0;
    private Renderer rend;
    float alfa = 0;
   
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 現在接続されているキーボードを取得
        var keyboard = Keyboard.current;
        if (keyboard == null) return; // キーボードがなければ処理しない

        // レーン1：Dキー
        if (num == 1)
        {
            if (keyboard.dKey.wasPressedThisFrame)
            {
                colorChange();
            }
        }

        // レーン2：Fキー または Gキー
        if (num == 2)
        {
            if (keyboard.fKey.wasPressedThisFrame)
            {
                colorChange();
            }
        }

        // レーン3：Jキー または Hキー
        if (num == 3)
        {
            if (keyboard.gKey.wasPressedThisFrame || keyboard.hKey.wasPressedThisFrame)
            {
                colorChange();
            }
        }

        if (num == 4)
        {
            if (keyboard.jKey.wasPressedThisFrame)
            {
                colorChange();
            }
        }
        // レーン4：Kキー
        if (num == 5)
        {
            if (keyboard.kKey.wasPressedThisFrame)
            {
                colorChange();
            }
        }
        alfa -= Speed * Time.deltaTime;
        if (rend != null)
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, alfa);
        }
    
}
    void colorChange()
    {
        alfa = 0.3f;
        rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, alfa);
    }
}