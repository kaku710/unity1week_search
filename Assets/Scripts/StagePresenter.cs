using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StagePresenter : SingletonMonoBehaviour<StagePresenter> {
    [SerializeField] private StageView view;
    public StagePhase Phase { get; private set; }
    private int criminalNumber;
    private GameObject sampleCriminal;
    private Citizen criminal; //犯人を格納
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject rifle;
    [SerializeField] private RuntimeAnimatorController citizenAnimator;

    private void Start () {
        SetPhase(StagePhase.MEMORY);
        criminalNumber = Random.Range (0, Constant.CITIZEN_COUNT);
        var sample = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        sampleCriminal = Instantiate (sample, Constant.CRIMINAL_POS, Quaternion.Euler (0, 180, 0));
        rifle.SetActive(false);
        SetEvents();
    }

    public void SetPhase (StagePhase phase) {
        this.Phase = phase;
        switch (Phase) {
            case StagePhase.MEMORY:
                break;
            case StagePhase.COUNTDOWN:
                view.startPanel.gameObject.SetActive(false);
                mainCam.transform.parent.DOMove(Constant.PLAY_CAM_POS,1f).SetEase(Ease.Linear);
                StartCoroutine(view.CountDown());
                break;
            case StagePhase.PLAY:
                view.inGamePanel.gameObject.SetActive(true);
                mainCam.gameObject.GetComponent<MouseLook>().enabled = true;
                rifle.SetActive(true);
                CreateCharacter();
                break;
            case StagePhase.SHOT:
                view.inGamePanel.gameObject.SetActive(false);
                rifle.SetActive(false);
                var citizens = GameObject.FindGameObjectsWithTag("Citizen");
                foreach(var c in citizens){
                    c.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
                }
                break;
            case StagePhase.CLEAR:
                Debug.Log("CLEAR");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                view.clearPanel.gameObject.SetActive(true);
                break;
            case StagePhase.GAMEOVER:
                Debug.Log("GAMEOVER");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                view.gameOverPanel.gameObject.SetActive(true);
                break;
        }
    }

    private void SetEvents () {
        this.UpdateAsObservable ().
        Where (x => Phase == StagePhase.MEMORY).
        Subscribe (x => {
            if (Input.GetKey (KeyCode.RightArrow)) {
                sampleCriminal.transform.Rotate (0, -150 * Time.deltaTime, 0);
            }
            if (Input.GetKey (KeyCode.LeftArrow)) {
                sampleCriminal.transform.Rotate (0, 150 * Time.deltaTime, 0);
            }
        });
        view.startButton.onClick.AddListener (() => {
            SetPhase(StagePhase.COUNTDOWN);
            Destroy(sampleCriminal);
        });
        view.nextStageButton.onClick.AddListener(() => {
            GameManager.Instance.AddLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        view.titleButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Title");
        });
    }

    private void CreateCharacter () {
        //まず犯人を生成
        var c = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        var cpos = GetSuitablePositon ();
        var crm = Instantiate (c, cpos, Quaternion.identity);
        criminal = crm.GetComponent<Citizen> ();
        criminal.isCriminal = true;
        crm.GetComponent<Animator>().runtimeAnimatorController = citizenAnimator;
        //そのあとにモブを生成
        for (int i = 0; i < 9; i++) {
            var num = GetCharaNumber();
            var obj = (GameObject) Resources.Load ("People/Citizen" + num);
            var pos = GetSuitablePositon ();
            var citizen = Instantiate (obj, pos, Quaternion.identity);
            citizen.GetComponent<Animator>().runtimeAnimatorController = citizenAnimator;
        }
    }

    public Vector3 GetSuitablePositon () {
        Vector3 pos = Vector3.zero;
        while (true) {
            float rndX = Random.Range (Constant.MIN_X, Constant.MAX_X);
            float rndZ = Random.Range (Constant.MIN_Z, Constant.MAX_Z);
            Ray ray = new Ray (new Vector3 (rndX, 20f, rndZ), Vector3.down);
            RaycastHit raycastHit;
            if (Physics.Raycast (ray, out raycastHit, 30f)) {
                if (raycastHit.collider.CompareTag ("Obstacle")) continue;
                else if (raycastHit.collider.CompareTag ("Field")) {
                    pos = new Vector3 (rndX, 0f, rndZ);
                    break;
                }
            }
        }
        return pos;
    }

    private int GetCharaNumber(){
        int num = 0;
        while(true){
            num = Random.Range(0,Constant.CITIZEN_COUNT);
            if(num != criminalNumber) break;
        }
        return num;
    }
}

public enum StagePhase {
    MEMORY,
    COUNTDOWN,
    PLAY,
    SHOT,
    CLEAR,
    GAMEOVER
}