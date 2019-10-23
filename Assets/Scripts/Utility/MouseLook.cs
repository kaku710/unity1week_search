using UnityEngine;

public class MouseLook : MonoBehaviour {
    private Transform verRot;
    private Transform horRot;
    private float horizontalSpeed = 200f;
    private float verticalSpeed = 200f;

    private void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        verRot = transform.parent;
        horRot = GetComponent<Transform> ();
    }

    private void Update () {
        if(StagePresenter.Instance.Phase != StagePhase.PLAY) return;
        float X_Rotation = Input.GetAxis ("Mouse X");
        float Y_Rotation = Input.GetAxis ("Mouse Y");
        verRot.transform.Rotate (0, X_Rotation * verticalSpeed * Time.deltaTime, 0);
        horRot.transform.Rotate (-Y_Rotation * horizontalSpeed * Time.deltaTime, 0, 0);
    }
}