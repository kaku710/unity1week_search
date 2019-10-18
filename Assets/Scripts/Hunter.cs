using UnityEngine;

public class Hunter : MonoBehaviour {
    private Camera mainCam;
    [SerializeField] private Transform shellTransform;

    private void Start () {
        mainCam = GetComponent<Camera> ();
    }

    private void Update () {
        if (StagePresenter.Instance.Phase != StagePhase.PLAY) return;

        Zoom ();
        if (Input.GetKeyDown (KeyCode.Space)) Shot ();
    }

    private void Zoom () {
        if (Input.GetKey (KeyCode.W)) mainCam.fieldOfView -= 0.5f;
        if (Input.GetKey (KeyCode.S)) mainCam.fieldOfView += 0.5f;
        mainCam.fieldOfView = Mathf.Clamp (mainCam.fieldOfView, 30, 60);
    }

    private void Shot () {
        StagePresenter.Instance.SetPhase(StagePhase.SHOT);
        var shell = (GameObject) Resources.Load ("Shell");
        var obj = Instantiate (shell, shellTransform.position, shellTransform.rotation);
    }
}