using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePresenter : SingletonMonoBehaviour<StagePresenter> {
    [SerializeField] private StageView view;
    public StagePhase Phase { get; private set; }
    private int criminalNumber;
    private GameObject sampleCriminal;
    private Citizen criminal; //犯人を格納
    [SerializeField] private GameObject rifle;
    [SerializeField] private RuntimeAnimatorController citizenAnimator;
    public ReactiveProperty<int> stageTimer = new IntReactiveProperty();

    private void Start () {
        SetPhase (StagePhase.MEMORY);
        Init ();
        SetEvents ();
        Bind ();
    }

    public void SetPhase (StagePhase phase) {
        this.Phase = phase;
        switch (Phase) {
            case StagePhase.MEMORY:
                break;
            case StagePhase.COUNTDOWN:
                view.startPanel.gameObject.SetActive (false);
                Camera.main.transform.parent.DOMove (Constant.PLAY_CAM_POS, 1f).SetEase (Ease.Linear);
                StartCoroutine (view.CountDown ());
                break;
            case StagePhase.PLAY:
                view.inGamePanel.gameObject.SetActive (true);
                Camera.main.gameObject.GetComponent<MouseLook> ().enabled = true;
                rifle.SetActive (true);
                CreateCharacter ();
                break;
            case StagePhase.SHOT:
                view.inGamePanel.gameObject.SetActive (false);
                rifle.SetActive (false);
                var citizens = GameObject.FindGameObjectsWithTag ("Citizen");
                foreach (var c in citizens) {
                    c.GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = 0;
                }
                break;
            case StagePhase.CLEAR:
                Debug.Log ("CLEAR");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if(GameManager.Instance.CurrentLevel <= 2) view.clearPanel.gameObject.SetActive (true);
                else {
                    GameManager.Instance.CalculateClearTime();
                    view.gameClearPanel.gameObject.SetActive(true);
                }
                break;
            case StagePhase.GAMEOVER:
                Debug.Log ("GAMEOVER");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                view.gameOverPanel.gameObject.SetActive (true);
                break;
        }
    }

    private void Init () {
        stageTimer.Value = Constant.TIME_LIMIT_ARRAY[GameManager.Instance.CurrentLevel - 1];
        criminalNumber = UnityEngine.Random.Range (0, Constant.CITIZEN_COUNT);
        var sample = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        sampleCriminal = Instantiate (sample, Constant.CRIMINAL_POS, Quaternion.Euler (0, 180, 0));
        rifle.SetActive (false);
        view.SetStartLabel();
    }

    private void Bind () {
        this.stageTimer
            .Subscribe (view.OnTimeChanged)
            .AddTo (this.gameObject);
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

        Observable.Interval (TimeSpan.FromSeconds (1)).
        Where (x => this.Phase == StagePhase.PLAY).
        Subscribe (x => {
            stageTimer.Value--;
            if(stageTimer.Value <= 0) SetPhase(StagePhase.GAMEOVER); 
        }).AddTo (this.gameObject);

        view.startButton.onClick.AddListener (() => {
            SetPhase (StagePhase.COUNTDOWN);
            Destroy (sampleCriminal);
        });
        view.nextStageButton.onClick.AddListener (() => {
            GameManager.Instance.AddLevel ();
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        });
        view.titleButton.onClick.AddListener (() => {
            SceneManager.LoadScene ("Title");
        });
        view.gameClearTitleButton.onClick.AddListener (() => {
            SceneManager.LoadScene ("Title");
        });
    }

    private void CreateCharacter () {
        //まず犯人を生成
        var c = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        var cpos = GetSuitablePositon ();
        var crm = Instantiate (c, cpos, Quaternion.identity);
        criminal = crm.GetComponent<Citizen> ();
        criminal.isCriminal = true;
        crm.GetComponent<Animator> ().runtimeAnimatorController = citizenAnimator;
        //そのあとにモブを生成
        for (int i = 0; i < Constant.MAKE_CITIZEN_COUNT[GameManager.Instance.CurrentLevel - 1] - 1; i++) {
            var num = GetCharaNumber ();
            var obj = (GameObject) Resources.Load ("People/Citizen" + num);
            var pos = GetSuitablePositon ();
            var citizen = Instantiate (obj, pos, Quaternion.identity);
            citizen.GetComponent<Animator> ().runtimeAnimatorController = citizenAnimator;
        }
    }

    public Vector3 GetSuitablePositon () {
        Vector3 pos = Vector3.zero;
        while (true) {
            float rndX = UnityEngine.Random.Range (Constant.MIN_X, Constant.MAX_X);
            float rndZ = UnityEngine.Random.Range (Constant.MIN_Z, Constant.MAX_Z);
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

    private int GetCharaNumber () {
        int num = 0;
        while (true) {
            num = UnityEngine.Random.Range (0, Constant.CITIZEN_COUNT);
            if (num != criminalNumber) break;
        }
        return num;
    }

    public void SetDeadCameraPos (Transform trn) {
        Camera.main.transform.DOMove (trn.position + new Vector3 (0, 4f, 0), 1f);
        Camera.main.transform.DORotate (new Vector3 (90f, -60f, 0), 1f);
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