using UnityEngine;
using UnityEngine.UI;

public class HideStartText : MonoBehaviour
{
    [SerializeField] private Text StartInfoText;

    private void Start()
    {
        StartInfoText.enabled = true;
    }

    public void TurnOfStartText()
    {
        StartInfoText.enabled = false;
    }
}
