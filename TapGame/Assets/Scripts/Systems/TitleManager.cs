using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
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
    public void OnTapStartButton()
    {
        // スコアをリセット 累計にしないため
        if (ScoreManager.Instance != null)
        {
            // リセットする関数を呼ぶ
            ScoreManager.Instance.ResetScore();
        }
        // タイトル画面からゲーム画面へ遷移する
        SceneManager.LoadScene("Game");
    }

    
}
