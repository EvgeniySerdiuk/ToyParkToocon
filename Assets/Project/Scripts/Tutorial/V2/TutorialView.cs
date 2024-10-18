using TMPro;
using UnityEngine;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private Tutorial tutorial;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject secondPointer;
    [SerializeField] private DirectionIndicator directionIndicator;

    private void Awake()
    {
        tutorial.ChangeTask += ChangeTaskView;
        tutorial.TutorialComplete += FinishTutorial;
    }

    private void ChangeTaskView(TutorialTask tutorialTask)
    {
        this.text.text = tutorialTask.Description;

        if(tutorialTask.Task.ObjectPoint == Vector3.zero)
        {
            pointer.SetActive(false);
            directionIndicator.gameObject.SetActive(false);
        }
        else 
        {
            pointer.SetActive(true);
            pointer.transform.position = tutorialTask.Task.ObjectPoint;
            directionIndicator.gameObject.SetActive(true);
        }

        if(tutorialTask.taskType == TaskType.BuyLocation) 
        {
            Destroy(directionIndicator.gameObject);
        }
    }

    private void OnDisable()
    {
        tutorial.ChangeTask -= ChangeTaskView;
        tutorial.TutorialComplete -= FinishTutorial;
    }

    private void FinishTutorial()
    {
        Destroy(pointer.gameObject);
        Destroy(secondPointer.gameObject);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
