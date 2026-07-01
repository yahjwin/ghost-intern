using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText;

    void Update()
    {
        bool studentDone =
            PlayerPrefs.GetInt("StudentAlreadyCleared", 0) == 1;

        bool workerDone =
            PlayerPrefs.GetInt("WorkerAlreadyCleared", 0) == 1;

        bool guardDone =
            PlayerPrefs.GetInt("GuardAlreadyCleared", 0) == 1;

        questText.text =
            "오늘의 업무\n\n" +

            (studentDone ?
            "[완료] 학생 놀래키기\n" :
            "[ ] 학생 놀래키기\n") +

            (workerDone ?
            "[완료] 직장인 정전시키기\n" :
            "[ ] 직장인 정전시키기\n") +

            (guardDone ?
            "[완료] 경비원 따돌리기" :
            "[ ] 경비원 따돌리기");
    }
}