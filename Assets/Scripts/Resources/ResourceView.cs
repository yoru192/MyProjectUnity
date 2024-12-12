using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodView;
    [SerializeField] private TextMeshProUGUI stoneView;
    [SerializeField] private TextMeshProUGUI steelView;
    [SerializeField] private Resource resource;

    private void OnEnable()
    {
        resource.ResourcesChanged += DisplayResources;
    }

    private void OnDisable()
    {
        resource.ResourcesChanged -= DisplayResources;
    }

    private void DisplayResources()
    {
        woodView.text = $"{resource.Wood}";
        stoneView.text = $"{resource.Stone}";
        steelView.text = $"{resource.Steel}";
    }
}