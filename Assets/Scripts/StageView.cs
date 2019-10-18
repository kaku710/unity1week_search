using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageView : MonoBehaviour
{
    public Button startButton;
    public Button nextStageButton;
    public Button titleButton;
    public Image startPanel;
    public Image inGamePanel;
    public Image clearPanel;
    public Image gameOverPanel;
    [SerializeField] private Text countDownLabel;

    public IEnumerator CountDown(){
        countDownLabel.gameObject.SetActive(true);
        int second = 3;
        countDownLabel.text = second.ToString ();
        while (second > 1) {
            yield return new WaitForSeconds (1f);
            second -= 1;
            countDownLabel.text = second.ToString ();
        }
        yield return new WaitForSeconds (1f);
        countDownLabel.gameObject.SetActive(false);
        StagePresenter.Instance.SetPhase(StagePhase.PLAY);
    }
}
