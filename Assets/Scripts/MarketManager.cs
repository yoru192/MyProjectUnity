using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private GameObject marketPanel;
    private CursorController _controlCursor;

    private void Start()
    {
        if (marketPanel)
        {
            marketPanel.SetActive(false);
        }
        _controlCursor = FindObjectOfType<CursorController>();
    }
    
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenPanel();
        }
    }
    
    public void ClosePanel()
    {
        marketPanel.SetActive(false);  // Вимкнути панель
        _controlCursor.LockCursor();
        _controlCursor.AllowPlayerMovement();
    }

    private void OpenPanel()
    {
        marketPanel.SetActive(true);
        _controlCursor.UnlockCursor();
        _controlCursor.DenyPlayerMovement();
    }
}