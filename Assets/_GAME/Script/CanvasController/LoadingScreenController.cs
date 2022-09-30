using UnityEngine.UI;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Image m_FillBar;
    public void SetReady()
    {
        toggleLoadingScreen(true);
        SetFillBar(0);
    }

    public void OnLoadingComplete()
    {
        toggleLoadingScreen(false);
        SetFillBar(1);
    }
    public void SetFillBar(float fillAmount) => m_FillBar.fillAmount = fillAmount;
    private void toggleLoadingScreen(bool value) => gameObject.SetActive(value);
}
