using UnityEngine;
using TMPro;
using System.Collections;

public static class StaticPopUpHandler
{
    // Método para mostrar o pop-up
    public static void ShowPopUp(TMP_Text popUpText, string text, MonoBehaviour context)
    {
        if (popUpText != null)
        {
            popUpText.text = text;
            popUpText.gameObject.SetActive(true);

            // Cancela a corrotina de ocultação, se houver uma ativa
            context.StopAllCoroutines();
        }
    }

    // Método para ocultar o pop-up após um atraso
    public static void HidePopUpAfterDelay(TMP_Text popUpText, float delay, MonoBehaviour context)
    {
        if (popUpText != null)
        {
            context.StartCoroutine(HidePopUpCoroutine(popUpText, delay));
        }
    }

    // Método para ocultar o pop-up imediatamente
    public static void HidePopUp(TMP_Text popUpText, MonoBehaviour context)
    {
        if (popUpText != null)
        {
            // Cancela a corrotina de ocultação, se houver uma ativa
            context.StopAllCoroutines();
            popUpText.gameObject.SetActive(false);
        }
    }

    // Corrotina para ocultar o pop-up
    private static IEnumerator HidePopUpCoroutine(TMP_Text popUpText, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popUpText != null)
        {
            popUpText.gameObject.SetActive(false);
        }
    }
}
