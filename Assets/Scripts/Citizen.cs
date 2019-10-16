using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Citizen : MonoBehaviour
{
    public bool isCriminal;
    private NavMeshAgent agent;
    private Vector3 destination;
    public Citizen(){
        isCriminal = false;
    }

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start(){
        destination = StagePresenter.Instance.GetSuitablePositon();
    }

    private void Update(){
        if(Vector3.Distance(transform.position,destination) <= 5){
            destination = StagePresenter.Instance.GetSuitablePositon();
        }
        else agent.SetDestination(destination);
    }
}
