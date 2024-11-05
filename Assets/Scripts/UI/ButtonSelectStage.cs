using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectStage : MonoBehaviour
{   
    public Stage stage;

    public void ButtonPress()
    {
        StageManager.Instance.SetCurrentStage(stage);
    }
}
