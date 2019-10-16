using UnityEngine;

public class StagePresenter : SingletonMonoBehaviour<StagePresenter> {
    private Citizen criminal; //犯人を格納

    private void Start () {
        CreateCharacter ();
    }

    private void CreateCharacter () {
        for (int i = 0; i < 10; i++) {
            var num = Random.Range (0, Constant.CITIZEN_COUNT);
            var obj = (GameObject) Resources.Load ("People/Citizen" + num);
            var pos = GetSuitablePositon();
            var citizen = Instantiate (obj, pos, Quaternion.identity);
            if (i == 0) {
                criminal = citizen.GetComponent<Citizen> ();
                criminal.isCriminal = true;
            }
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
                else if(raycastHit.collider.CompareTag ("Field")) {
                    pos = new Vector3(rndX,0f,rndZ);
                    break;
                }
            }
        }
        return pos;
    }
}