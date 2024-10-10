using FirstPersonController;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    private bool _isHoldingAlt;
    private FirstPersonMovement _fpm;
    private FirstPersonLook _fpl;

    private void Start()
    {
        _fpm = FindObjectOfType<FirstPersonMovement>();
        _fpl = FindObjectOfType<FirstPersonLook>();
    }
    
    private void Update()
    {
        HoldAltForUnlockCursor();
    }

    public void DenyPlayerMovement()
    {
        _fpm.playerCanMove = false;
        _fpl.canRotate = false;
    }

    public void AllowPlayerMovement()
    { 
        if (inventory.activeSelf)
        {
            _fpm.playerCanMove = false;
            _fpl.canRotate = false;
        }
        else
        {
            _fpm.playerCanMove = true;
            _fpl.canRotate = true;
        }
    }
    public void LockCursor()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HoldAltForUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !inventory.activeSelf)
        {
            _isHoldingAlt = true;
        }
        if (_isHoldingAlt && Input.GetKey(KeyCode.LeftAlt) && !inventory.activeSelf)
        {
            UnlockCursor();
            DenyPlayerMovement();
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && !inventory.activeSelf)
        {
            _isHoldingAlt = false;
            LockCursor();
            AllowPlayerMovement();
        }
        
    }
}
