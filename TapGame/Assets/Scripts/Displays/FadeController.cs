using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェードさせるためのクラス
/// </summary>
public class FadeController : MonoBehaviour
{
    // フェードのタイプの定義 InかOutかを設定して遷移を自然にするため
    public enum FadeType
    {
        FadeInType,
        FadeOutType
    }

    // 自身のものを変えるため
    private Image panel;
    private void Awake()
    {
        panel = GetComponent<Image>();
    }

    /// <summary>
    /// フェード開始が実行されたときに呼び出す関数
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(FadeType type, float duration)
    {
        float elapsed = 0.0f;

        // typeに応じて開始値と終了値を決定
        float startAlpha = (type == FadeType.FadeInType) ? 0.0f : 1.0f;
        float endAlpha = (type == FadeType.FadeInType) ? 1.0f : 0.0f;

        Color color = panel.color;

        while (elapsed < duration)
        {
            //時間と共に透過度を変更する
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            // startAlphaからendAlphaの間をtの割合で補間する
            color.a = Mathf.Lerp(startAlpha, endAlpha, progress);
            panel.color = color;
            yield return null;
        } 

        // 終了値へ設定
        color.a = endAlpha;
        panel.color = color;
    }
}