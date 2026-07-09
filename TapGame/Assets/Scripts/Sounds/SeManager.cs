using UnityEngine;

/// <summary>
/// SEの管理を一元管理にするため
/// </summary>
public class SeManager : MonoBehaviour
{
    private static SeManager instance;


    /// <summary>
    /// SEの音源ファイルと再生時の音量をセットで管理するためのクラス
    /// </summary>
    [System.Serializable]
    public class SeSetting
    {
        // 再生する音声データ
        public AudioClip clip;

        // 再生音量を 0.0fから1.0f にインスペクター上で設定出来るようにする処理
        [Range(0, 1)] public float volume = 1.0f;
    }


    /// <summary>
    /// 外部からアクセスするため ゲッターのようなもの
    /// </summary>
    public static SeManager Instance
    {
        get { return instance; }
    }

    private AudioSource audioSource;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // SEを変更したりしないのでシーンを跨いでも消えないようにする
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// SEをほかのシーンから呼べるようにする処理
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySE(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
