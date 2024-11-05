using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectStage : MonoBehaviour
{
    [SerializeField] public Stage stage;

    public void ButtonPress()
    {
        StageManager.Instance.SetCurrentIndex(stage.stageID);
        StageManager.Instance.SetCurrentStage(stage);
    }
}
