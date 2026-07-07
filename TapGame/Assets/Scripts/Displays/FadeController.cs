using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// フェードさせるためのクラス
/// </summary>
public class FadeController : MonoBehaviour
{
    /// <summary>
    /// フェード開始が実行されたときに呼び出す関数
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(float duration)
    {
        float elapsed = 0.0f;
        Image panel = GetComponent<Image>();


        while (elapsed < duration)
        {
            //時間と共に透過度を変更する
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            Color color = panel.color;
            color.a = alpha;
            panel.color = color;
            yield return null;
        }
    }
}