using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    public int CurrentLevel { get; private set; }
    public float clearTime;
    public float[] clearTimeArray;

    private void Start () {
        CurrentLevel = 1;
        clearTimeArray = new float[Constant.MAX_LEVEL];
    }

    public void AddLevel () {
        this.CurrentLevel++;
    }

    public void SetClearTime (int level, float time) {
        clearTimeArray[level] = time;
    }

    public void CalculateClearTime () {
        clearTime = clearTimeArray.Sum();
        UnityEngine.Debug.Log(clearTime);
    }
}