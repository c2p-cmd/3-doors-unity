using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scrips {
    public class TakingInput : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI doorText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject[] buttonBg;
        
        private static int correctDoorNumber;

        private static GameObject[] doors;

        private static int score;

        private void Start() {
            doors = GameObject.FindGameObjectsWithTag("DoorBtn");

            correctDoorNumber = Random.Range(1, 4);
            
            Debug.Log("correct: " + " " + correctDoorNumber);
            scoreText.text = "" + PlayerPrefs.GetInt("score", 0);
            doorText.text = "Choose a door";
            foreach (var go in buttonBg) {
                go.SetActive(false);
            }
            // buttonBg[0].SetActive(true);
        }

        private void Update() {
            updateScore();
            // getNames();
        }

        public void openDoor1() {
            doorText.text = "You chose door 1";
            isValidNumber(correctDoorNumber == 1);
            disableAllDoors();
        }

        public void openDoor2() {
            doorText.text = "You chose door 2";
            isValidNumber(correctDoorNumber == 2);
            disableAllDoors();
        }

        public void openDoor3() {
            doorText.text = "You chose door 3";
            isValidNumber(correctDoorNumber == 3);
            disableAllDoors();
        }

        /*private void getNames() {
            foreach (var selectableUI in Selectable.allSelectablesArray) {
                Debug.Log(selectableUI.name);
                switch (selectableUI.name) {
                    case "Door1":
                        buttonBg[0].SetActive(true);
                        buttonBg[1].SetActive(false);
                        buttonBg[2].SetActive(false);
                        break;
                    case "Door2":
                        buttonBg[0].SetActive(false);
                        buttonBg[1].SetActive(true);
                        buttonBg[2].SetActive(false);
                        break;
                    case "Door3":
                        buttonBg[0].SetActive(false);
                        buttonBg[1].SetActive(false);
                        buttonBg[2].SetActive(true);
                        break;
                }
            }
        }*/
        
        private static void disableAllDoors() {
            foreach (var door in doors) {
                door.GetComponent<Button>().interactable = false;
                // door.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        private void isValidNumber(bool condition) {
            if (condition) {
                StartCoroutine(nameof(typeGoodChoice));
                score++;
            } else {
                StartCoroutine(nameof(typeBetterLuckNextTime));
                score = 0;
            }
            StartCoroutine(nameof(waitToReloadScene));
        }

        private IEnumerator typeGoodChoice() {
            const string goodChoice = "\nGood Choice";
            foreach (var ch in goodChoice) {
                yield return new WaitForSecondsRealtime(0.1f);
                doorText.text += ch;
            }
        }

        private IEnumerator typeBetterLuckNextTime() {
            const string msg = "\nOops Better luck next time!";
            foreach (var ch in msg) {
                yield return new WaitForSecondsRealtime(0.1f);
                doorText.text += ch;
            }
        }

        private IEnumerator waitToReloadScene() {
            yield return new WaitForSecondsRealtime(3f);
            for (var i = 1f; i > 0f; i -= 0.1f) {
                yield return new WaitForSecondsRealtime(0.05f);
                doorText.color = new Color(1f, 1f, 1f, i);
            }
            for (var i = 0f; i <= 1f; i += 0.2f) {
                yield return new WaitForSecondsRealtime(0.05f);
                doorText.color = new Color(1f, 1f, 1f, i);
                doorText.text = i switch {
                    0 when doorText.text.Contains("Oops") => $"Score: {score}\nReloading Scene..",
                    0 => $"Score: {score}",
                    _ => doorText.text
                };
            }
            yield return new WaitForSecondsRealtime(1f);
            SceneManager.LoadScene("GameScene");
        }

        private void updateScore() {
            scoreText.text = "" + score;
            PlayerPrefs.SetInt("score", score);
        }
    }
}
