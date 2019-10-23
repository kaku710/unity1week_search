using UnityEngine;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startButton;

    private void Start(){
        AudioManager.Instance.PlayBGM("game_bgm");
        GameManager.Instance.clearTime = 0;
        startButton.onClick.AddListener(() => {
            AudioManager.Instance.PlaySE("button");
            UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
        });
    }
}
