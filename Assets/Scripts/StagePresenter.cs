using UniRx;
using UniRx.Triggers;
using UnityEngine;
using DG.Tweening;

public class StagePresenter : SingletonMonoBehaviour<StagePresenter> {
    [SerializeField] private StageView view;
    public StagePhase Phase { get; private set; }
    private int criminalNumber;
    private GameObject sampleCriminal;
    private Citizen criminal; //犯人を格納
    [SerializeField] private Camera mainCam;

    private void Start () {
        SetPhase(StagePhase.MEMORY);
        criminalNumber = Random.Range (0, Constant.CITIZEN_COUNT);
        var sample = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        sampleCriminal = Instantiate (sample, Constant.CRIMINAL_POS, Quaternion.Euler (0, 180, 0));
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
                mainCam.gameObject.GetComponent<MouseLook>().enabled = true;
                CreateCharacter();
                break;
            case StagePhase.CLEAR:
                break;
            case StagePhase.GAMEOVER:
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
    }

    private void CreateCharacter () {
        //まず犯人を生成
        var c = (GameObject) Resources.Load ("People/Citizen" + criminalNumber);
        var cpos = GetSuitablePositon ();
        var crm = Instantiate (c, cpos, Quaternion.identity);
        criminal = crm.GetComponent<Citizen> ();
        criminal.isCriminal = true;
        //そのあとにモブを生成
        for (int i = 0; i < 9; i++) {
            var num = GetCharaNumber();
            var obj = (GameObject) Resources.Load ("People/Citizen" + num);
            var pos = GetSuitablePositon ();
            var citizen = Instantiate (obj, pos, Quaternion.identity);
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
    CLEAR,
    GAMEOVER
}