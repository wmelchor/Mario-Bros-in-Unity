using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* This is a Singleton */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }

    public int stage { get; private set; }

    public int lives { get; private set; }
    public int coins {get; private set; }

    private void Awake() {
        if(Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy() {
        if(Instance == this) {
            Instance = null;
        }
    }

    private void Start() {
        Application.targetFrameRate = 60;
        NewGame();
    }

    private void NewGame() {
        lives = 5;
        coins = 0;
        LoadLevel(1, 1);
    }

    public void LoadLevel(int world, int stage) {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}"); // This is loading our scenes, like 1-1
    }

    public void NextLevel() {
        if(stage == 4) {
            LoadLevel(world + 1, 1);
        } else {
            LoadLevel(world, stage + 1);
        }
    }

    public void ResetLevel(float delay) {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel() { // This for when Mario dies
        lives--;
        if(lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }
    }

    private void GameOver() {
        Invoke(nameof(NewGame), 2f); // Start new game after two seconds
        // SceneManager.LoadScene("GameOver"); // If I want to create a game over scene I could add it here...
    }

    public void AddCoin() {
        coins++;
        if (coins == 100) {
            AddLife();
            coins = 0;
        }
    }

    public void AddLife() {
        lives++;
    }
}
