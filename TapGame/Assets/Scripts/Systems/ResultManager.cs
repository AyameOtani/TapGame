using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
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
    /// タイトルへ戻るボタンが押された時の処理
    /// </summary>
    public void OnTapTitleButton()
    {
        // リザルト画面からタイトル画面へ遷移させるため
        SceneManager.LoadScene("Title");
    }
}
