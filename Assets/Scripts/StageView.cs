using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageView : MonoBehaviour
{
    public Button startButton;
    public Button nextStageButton;
    public Button titleButton;
    public Button gameClearTitleButton;
    public Image startPanel;
    public Image inGamePanel;
    public Image clearPanel;
    public Image gameOverPanel;
    public Image gameClearPanel;
    [SerializeField] private Text countDownLabel;
    [SerializeField] private Text timerLabel;
    [SerializeField] private Image timeGage;
    [SerializeField] private Text startLevelLabel;
    [SerializeField] private Text startTimeLabel;
    [SerializeField] private Text inGameLevelLabel;

    public IEnumerator CountDown(){
        countDownLabel.gameObject.SetActive(true);
        int second = 3;
        countDownLabel.text = second.ToString ();
        while (second > 1) {
            yield return new WaitForSeconds (0.8f);
            second -= 1;
            countDownLabel.text = second.ToString ();
        }
        yield return new WaitForSeconds (0.8f);
        countDownLabel.gameObject.SetActive(false);
        StagePresenter.Instance.SetPhase(StagePhase.PLAY);
    }

    public void OnTimeChanged(int time){
        timerLabel.text = time.ToString();
        timeGage.fillAmount = (float)time / Constant.TIME_LIMIT_ARRAY[GameManager.Instance.CurrentLevel - 1];
    }

    public void SetStartLabel(){
        startLevelLabel.text = "Level " + GameManager.Instance.CurrentLevel.ToString();
        startTimeLabel.text = "制限時間 : " + Constant.TIME_LIMIT_ARRAY[GameManager.Instance.CurrentLevel - 1] + "s";
        inGameLevelLabel.text = "Level " + GameManager.Instance.CurrentLevel.ToString();
    }
}
