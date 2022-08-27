using UnityEngine;
using UnityEngine.SceneManagement;

public class GameM : MonoBehaviour
{
	bool gameHasEnded = false;
	bool gameWin = false;

	public float restartDelay = 2f;
	public GameObject gameOverPanel;
	public GameObject winPanel;

   public ImaginationCharacterFollow ImaginationCamera;

	void Awake() {
		gameOverPanel.SetActive(false);
		winPanel.SetActive(false);
	}

	public void EndGame() {
		if (gameHasEnded == false) {
			gameHasEnded = true;
			gameOverPanel.SetActive(true);
			ImaginationCamera.Camera.enabled = false; 
			Invoke("Restart", restartDelay);
		}
	}

	public void GameWin() {
		if (gameWin == false) {		
			gameWin = true;
			winPanel.SetActive(true);
			ImaginationCamera.Camera.enabled = false;
			Invoke("Restart", restartDelay);
		}
	}

	public bool getGameWin() {
		return gameWin;
	}

	public bool getGameEnded() {
		return gameHasEnded;
	}

	void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}