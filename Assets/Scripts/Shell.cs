using UnityEngine;

public class Shell : MonoBehaviour
{
    private Rigidbody rb;
    private Transform hunter;
    private Vector3 shotDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hunter = GameObject.Find("Hunter").transform;
        hunter.SetParent(this.transform);
        hunter.localPosition = Constant.SHOT_CAM_POS;
        shotDirection = hunter.forward;
    }

    private void Update()
    {
        rb.AddForce(shotDirection * 3f,ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Citizen")){
            var isCriminal = col.GetComponent<Citizen>().isCriminal;
        }
    }
}
