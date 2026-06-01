using UnityEngine;
using System;

public class NotesMove : MonoBehaviour
{
    // --- [4. ノーツのタップ判定を作る] で定義された変数 ---
    [SerializeField]
    private float notesSpeed = 0.1f; // インスペクターから速度を調整できるように初期値を設定

    [SerializeField]
    private Vector2 startPos; // ノーツの開始位置

    [SerializeField]
    private Vector2 judgePos; // 判定したい場所

    public static float moveSpan = 0.01f; // 回すスパン
    private float notesTime;


    void Start()
    {
        // 生成位置から判定位置までにかかる時間を計算
        notesTime = (startPos.x - judgePos.x) / notesSpeed;

        // moveSpan（0.01秒）ごとに NotesMove メソッドを繰り返し実行
        InvokeRepeating("NotesMoveProcess", 0f, moveSpan);
    }

    /// <summary>
    /// ノーツを移動させる処理（元のメソッド名から競合を避けるため変更）
    /// </summary>
    void NotesMoveProcess()
    {
        // ノーツを左に移動させる
        transform.position += new Vector3(-notesSpeed, 0f, 0f);
        notesTime -= moveSpan;

        // 判定処理を呼ぶ
        NotesJudge();
    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    void NotesJudge()
    {
        // --- [5. 音楽を再生する] で追加された音再生用ノーツの処理 ---
        if (this.gameObject.CompareTag("AudioPlay"))
        {
            // 元のコードの「=< 0」というタイポを「<= 0」に修正
            if (notesTime <= 0)
            {
                NotesGenerator.isAudioPlay = true;

                // 音楽が再生されたら、この音再生用オブジェクトは役割を終えるので削除する
                Destroy(gameObject);
            }
            return; // 音再生用ノーツの場合は、下の一般ノーツの判定には進まない
        }

        // --- 通常ノーツのタップ判定処理 ---
        // 判定位置の近くにいて、画面タップ（またはクリック）されたか
        if (Math.Abs(notesTime) < 0.5f)
        {
            if (Input.GetMouseButtonDown(0)) // 画面タップまたは左クリック
            {
                Debug.Log("Hit!");
                // 叩かれたらノーツを消す
                Destroy(gameObject);
            }
        }

        // 判定通り過ぎて画面外に行去ってしまったノーツを削除（自動削除を入れないと重くなります）
        if (notesTime < -0.5f)
        {
            Destroy(gameObject);
        }
    }
}
