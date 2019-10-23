using UnityEngine;

public class TitlePresenter : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startButton;

    private void Start(){
        startButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
        });
    }
}
