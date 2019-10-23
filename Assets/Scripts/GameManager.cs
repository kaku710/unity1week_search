using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    public int CurrentLevel { get; private set; }
    public float clearTime { get; private set; }
    private float[] clearTimeArray;

    private void Start () {
        CurrentLevel = 1;
        clearTimeArray = new float[Constant.MAX_LEVEL];
    }

    public void AddLevel () {
        this.CurrentLevel++;
    }

    public void SetClearTime (int level, float time) {
        clearTimeArray[level - 1] = time;
    }

    public void CalculateClearTime () {
        clearTime = clearTimeArray.Sum();
    }
}