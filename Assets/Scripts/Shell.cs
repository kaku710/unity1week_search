using UnityEngine;

public class Shell : MonoBehaviour
{
    private Rigidbody rb;
    private Transform hunter;
    private Transform camParent;
    private Vector3 shotDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hunter = GameObject.Find("Hunter").transform;
        camParent = GameObject.Find("CamParent").transform;
        hunter.SetParent(this.transform);
        hunter.localPosition = Constant.SHOT_CAM_POS;
        shotDirection = hunter.forward;
    }

    private void Update(){
        rb.AddForce(shotDirection * 3f,ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Citizen")){
            var isCriminal = col.GetComponent<Citizen>().isCriminal;
            if(isCriminal) StagePresenter.Instance.SetPhase(StagePhase.CLEAR);
            else StagePresenter.Instance.SetPhase(StagePhase.GAMEOVER);
            hunter.SetParent(camParent);
            Destroy(this.gameObject);
        }
        else if(col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Field")){ 
            StagePresenter.Instance.SetPhase(StagePhase.GAMEOVER);
            hunter.SetParent(camParent);
            Destroy(this.gameObject);
        }
    }
}
