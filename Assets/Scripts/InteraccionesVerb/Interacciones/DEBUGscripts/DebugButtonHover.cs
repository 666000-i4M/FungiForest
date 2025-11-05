using UnityEngine;
using UnityEngine.EventSystems;

public class DebugButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " cursor entró");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " cursor salió");
    }
}
