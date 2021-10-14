using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//キャラクターの変更とヌメレーターがエラー吐くから後回し。
public class NovelController : MonoBehaviour
{
    //テキスト位置を指定
    [SerializeField]
    private Text Nametext;
    [SerializeField]
    private Text Maintext;
    //キャラクター位置。必要であればコピペで名前だけ変えて増やして。
    [SerializeField]
    private Sprite CharacterSprite;


    [SerializeField]
    private bool SceneSwitch;
    //もしボタンによる選択肢をしたい場合に使う、ボタン指定
    [SerializeField]
    GameObject ButtonPrefab;
    [SerializeField]
    Transform ButtonPanel;
    [SerializeField]
    Sprite ButtonSprite;

    //次に読み込むシーンの名前。stringなので名前直接ぶち込んで。
    public string NextSceneName;

    //テキストを外部から編集できるようにするためのリスト
    public List<string> generatedMaintext = new List<string>();
    public List<string> generatedNametext = new List<string>();




    //現在再生中のテキストを判断
    private int textCount;

    //表示完了の判断も兼ねている
    private bool textAnimationPlay = false;

    //テキストの長さを図るための宣言。
    int MaintextListCount,NametextListCount,MinListCount;


    //多分クリックを一瞬だけ検知するための部屋
    public bool IsClicked()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        textCount = 0;
        MaintextListCount = generatedMaintext.Count;
        NametextListCount = generatedNametext.Count;

        //小さい方のテキストを代入（エラー）
        if (MaintextListCount <= NametextListCount)
        {
            MinListCount = MaintextListCount;
        }
        else
        {
            MinListCount = NametextListCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Maintext.text = string.Format("{0}", generatedMaintext[textCount]);
        Nametext.text = string.Format("{0}", generatedNametext[textCount]);
    }

    private void FixedUpdate()
    {

        //クリックかエンター押されたらテキストを一気に表示する（未実装）
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || IsClicked())
        {
            textAnimationPlay = true;
        }

        //テキストが終了したら次のテキストを指定
        {
            if (textAnimationPlay == true)
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || IsClicked())
                {
                        textCount += 1;
                        textAnimationPlay = false;
                    if (textCount == MinListCount)
                    {
                        if(SceneSwitch)
                        SceneManager.LoadScene(NextSceneName);
                        else
                        {
                         //未実装、ボタンによる選択肢分岐。   
                         GameObject button = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity);
                          button.transform.SetParent(ButtonPanel);
                            button.GetComponent<Image>().sprite = ButtonSprite;
                        }
                    }
                }
        }
    }
}
