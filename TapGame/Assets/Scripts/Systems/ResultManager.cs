using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



/// <summary>
/// タイトルシーンに遷移させるためのクラス
/// </summary>
public class ResultManager : MonoBehaviour
{

    // フェードするため
    [SerializeField] private FadeController fadeController; // インスペクターで設定

    // フェードにかける時間
    [SerializeField] private float maxFadeTime = 1.0f;

    //フェード中かどうかの判定
    private bool IsFading = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    ///  ゲーム開始ボタンが押された時の処理
    /// </summary>
    public void OnTapTitleButton()
    {
        if (IsFading) return;

        StartCoroutine(StartTitleWithFade());
    }



    /// <summary>
    /// ボタンが押されたらフェードさせるため
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTitleWithFade()
    {
        IsFading = true;

        yield return StartCoroutine(fadeController.FadeIn(maxFadeTime));

        // リザルト画面からタイトル画面へ遷移させるため
        SceneManager.LoadScene("Title");
    }

}
