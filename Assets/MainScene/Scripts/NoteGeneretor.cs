using UnityEngine;
using System;

public class NotesGenerator : MonoBehaviour
{
    // --- [② 譜面を読み込む] で定義されたクラスと変数 ---
    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        public int num;
        public int block;
        public int LPB;
    }

    [SerializeField]
    private TextAsset scoreData; // インスペクターからJsonファイルをアサインするための変数

    private int[] scoreNum;   // ノーツの番号を順に入れる
    private int[] scoreBlock; // ノーツの種類を順に入れる
    private int BPM;
    private int LPB;


    // --- [③ ノーツを生成する] / [5. 音楽を再生する] で追加された変数 ---
    [SerializeField]
    private GameObject notesPre;

    [SerializeField]
    private AudioSource gameAudio;

    public static bool isAudioPlay = false; // (元のコードの typo "isAudioPlay = false" を修正)

    private float moveSpan = 0.01f;
    private float nowTime;    // 音楽の再生されている時間
    private int beatNum;      // 今の拍数
    private int beatCount;    // json配列用(拍数)のカウント
    private bool isBeat;      // ビートを打っているか(生成のタイミング)


    void Awake()
    {
        // 1. 譜面の読み込み
        MusicReading();

        // 2. moveSpan（0.01秒）ごとに NotesIns を繰り返し実行
        InvokeRepeating("NotesIns", 0f, moveSpan);
    }

    /// <summary>
    /// 譜面の読み込み
    /// </summary>
    void MusicReading()
    {
        if (scoreData == null)
        {
            Debug.LogError("scoreData (Json) がインスペクターで設定されていません。");
            return;
        }

        string inputString = scoreData.text; // (元のコードの scoreData.ToString() から修正)
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        scoreNum = new int[inputJson.notes.Length];
        scoreBlock = new int[inputJson.notes.Length];
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            // ノーツがある場所を入れる
            scoreNum[i] = inputJson.notes[i].num;
            // ノーツの種類を入れる
            scoreBlock[i] = inputJson.notes[i].block;
        }
    }

    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        // 今の音楽の時間の取得
        nowTime += moveSpan; // (1)
        
        // ノーツが無くなったら処理終了
        if (beatCount >= scoreNum.Length) return;

        // 楽譜上でどこかの取得
        beatNum = (int)(nowTime * BPM / 60 * LPB); // (2)
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        // json上でのカウントと楽譜上でのカウントの一致
        if (beatCount < scoreNum.Length)
        { 
            isBeat = (scoreNum[beatCount] == beatNum); // (3)
        }
           
        // 生成のタイミングなら
        if (isBeat)
        {
            // ノーツ0の生成（音再生用ノーツ）
            if (scoreBlock[beatCount] == 0)
            {                     
                // ここで必要ならAudioPlay用のプレハブを生成するなどの処理
                // ※記事の「5.音楽を再生する」では明記されていませんが、
                // タグ "AudioPlay" をつけたノーツプレハブを生成する必要があります。
            }

            // ノーツ1の生成
            if (scoreBlock[beatCount] == 1)
            {      
                Instantiate(notesPre);                 
            }

            beatCount++; // (5)
            isBeat = false;
        }

        // 音楽の再生チェック
        AudioPlay();
    }

    /// <summary>
    /// 音再生開始
    /// </summary>
    void AudioPlay()
    {
        // isAudioPlay が true になったらオーディオを再生する
        if (isAudioPlay && gameAudio != null && !gameAudio.isPlaying)
        {
            gameAudio.Play();
            // 何度も Play() が呼ばれないように、一度再生したらフラグを戻すか、
            // 記事の通り gameAudio.enabled = isAudioPlay; とするなどの制御が必要です。
            // ここでは簡易的にPlayとしています。
        }
    }
}
