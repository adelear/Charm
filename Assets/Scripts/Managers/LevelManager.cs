using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        GameManager.Instance.SwitchState(GameState.GAME);
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() != GameState.GAME) return;
    }

}
