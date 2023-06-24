using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LevelUpPopup : MonoBehaviour
{
    public float popupDuration = 2.0f;
    private TextMeshProUGUI popupText;

    void Start()
    {
        popupText = this.GetComponent<TextMeshProUGUI>();
        this.gameObject.SetActive(false);
    }

    public void ShowPopup(int level)
    {
        popupText.text = "Subiste de nivel! Ahora estas en el nivel " + level.ToString() + "!";
        this.gameObject.SetActive(true);
        StartCoroutine(HidePopup());
    }

    IEnumerator HidePopup()
    {
        yield return new WaitForSeconds(popupDuration);
        this.gameObject.SetActive(false);
    }
}
