using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tabControl : MonoBehaviour
{
    EventSystem system;

    void Start ()
    {
        system = EventSystem.current;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = null;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            else
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next == null)
                next = system.firstSelectedGameObject.GetComponent<Selectable>();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
        }
    }
}
