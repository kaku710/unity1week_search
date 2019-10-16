using UnityEngine;

public class StagePresenter : MonoBehaviour {
    private Citizen criminal; //犯人を格納

    private void Start(){
        CreateCharacter();
    }

    private void CreateCharacter () {
        for (int i = 0; i < 10; i++) {
            var num = Random.Range(0,Constant.CITIZEN_COUNT);
            var obj = (GameObject)Resources.Load("People/Citizen" + num);
            var citizen = Instantiate(obj,Vector3.zero,Quaternion.identity);
            if(i == 0){
                criminal = citizen.GetComponent<Citizen>();
                criminal.isCriminal = true;
            }
        }
    }
}