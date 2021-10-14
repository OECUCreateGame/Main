using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NovelGameMaker : MonoBehaviour
{
    [SerializeField]
    private Text Nametext;//テキストの場所指定
    [SerializeField]
    private List<string> GenerateNametext = new List<string>();//名前リスト
    [SerializeField]
    private Text Maintext;//メインテキストの場所指定
    [SerializeField]
    private List<string> GenerateMaintext = new List<string>();//メインテキストリスト

    private string WaitGenerateMaintext = null;//リストから抜き取った一つ分のやつ（加工済み）
    private int GenerateMaintextCount = 0;//入力された行数をカウント
    private int NextWordCount = 0;//入力された文字数をカウント（文章中の）
    private string NextWord;//次の単語一つを格納する用
    private string NowWord;//今の文章を一時格納する(スクリプトの橋渡し用）
    private int NextLineCount = 0;//現在の表示文字数を計測。16文字で改行するため。
    private int MaxWord;//最大文字数


    [SerializeField]
    private Image ReftCharacterSprite;//右のキャラの位置指定
    [SerializeField]
    private Image LightCharacterSprite;//左のキャラの位置指定

    AudioSource audioSource; //音楽再生用。スタート時に再生される(SE登録したいならaudioSourceを名前変更してコピペ、保存位置を増やして。）

    [SerializeField]
    private float TextUpSpeed = 1.0f;//IEnumerableのyield速度。次の文字が表示されるまでの速度制御
    [SerializeField]
    private float TextUpSpeedtoFast = 0.1f;//通常速度よりも速くしている。初期はコメントアウトしている。

    private int generateMaintextCount, generateNametextCount;//名前と本文のリスト数を計測
    private int GenerateCount;//リスト数が少ない方の数を入力。

    private bool OpenWord = true;

    public Controller KeyController;

    private void NovelMake()//文章を全て最初にテキストに表示できるように加工する（重くなるから？）
    {
        Debug.Log("NovelGameMake スタート");
        OpenWord = true;
        generateMaintextCount = GenerateMaintext.Count;
        generateNametextCount = GenerateNametext.Count;
        if (generateMaintextCount >= generateNametextCount)
        {
            GenerateCount = generateNametextCount;
        }
        else
        {
            GenerateCount = generateMaintextCount;
        }
        StartCoroutine("PlayNovelGame");
    }

    private IEnumerator PlayNovelGame()//ノベルゲームの中身
    {
        Debug.Log("IEnumerable PlayNovelGame:スタート");
        WaitGenerateMaintext = GenerateMaintext[GenerateMaintextCount];//WaitgenerateMaintextにリストの中身を格納する
        Debug.Log("リスト格納");
        MaxWord = WaitGenerateMaintext.Length;//長さ計測
        Debug.Log("長さ格納");
        for (; NextWordCount < MaxWord;)//文字数最大まで表示を継続させるfor文
        {
            Debug.Log("表示for" + NextWordCount);
            if (NextLineCount == 15)//もし表示文字が16だったら改行するif文
            {
                Debug.Log("改行if開始");
                NextLineCount = 0;//計測をリセットする
                NowWord += ("\n");//改行コードを追加する。使用機種により改行コードが違うらしいのでMac以外でバグったらここ。
                Debug.Log(NowWord);//現在の文字表示
                Debug.Log("改行");//改行完了ログ
            }

            NextWord = WaitGenerateMaintext.Substring(NextWordCount, 1);//次の文字に今入力中の文字（今の入力文字数）を一文字追加する

            NowWord += NextWord;//現在表示しているテキストと直接ひっついてるstringに突っ込む

            //以下「テキスト表示を加速させる場合」
            if(GenerateMaintextCount == 1)
            {
                yield return new WaitForSeconds(TextUpSpeedtoFast);
                Debug.Log("文字遅延" + TextUpSpeedtoFast);//文字遅延起動ログ

            }
            else
            {
            yield return new WaitForSeconds(TextUpSpeed);//文字遅延。※文字速度早くしたいならifでGenerateMaintext[]の数字を指定してコピペ、速度変更をする。
            Debug.Log("文字遅延" + TextUpSpeed);//文字遅延起動ログ
            }


            ++NextWordCount;//文字数カウント。
            ++NextLineCount;//改行用カウント。15になるとリセットされる。
        }
        OpenWord = false;
        Debug.Log("表示完了");//一文終了
        //クリック2回か全文表示後の何かしらの操作で次の文章に移動
        //もし次の文章がない（今のがラストだったら）→何かしらの操作

}

    void Start()
    {
        Debug.Log("スタート");//ゲーム開始
        audioSource = GetComponent<AudioSource>();//BGM開始
        StartCoroutine("NovelMake");
    }

    // Update is called once per frame
    void Update()
    {
        Maintext.text = NowWord;
    }

    private void FixedUpdate()
    {
        if(KeyController.IsClicked)
        {
            if(OpenWord)
            {
                TextUpSpeed = 0.1f;
            }
            else
            {
                ++generateMaintextCount;
                OpenWord = true;
                StartCoroutine("PlayNovelGame");
            }
        }
    }
}
