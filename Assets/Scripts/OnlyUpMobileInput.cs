using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OnlyUpVirtualJoystick:MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler{
 public RectTransform knob;RectTransform rect;void Awake(){rect=GetComponent<RectTransform>();}
 public void OnPointerDown(PointerEventData e){OnDrag(e);}
 public void OnDrag(PointerEventData e){Vector2 p;if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,e.position,e.pressEventCamera,out p)){Vector2 v=Vector2.ClampMagnitude(p/(rect.sizeDelta*.5f),1);if(knob)knob.anchoredPosition=v*(rect.sizeDelta*.32f);if(OnlyUpPlayerController.Instance)OnlyUpPlayerController.Instance.SetMove(v);}}
 public void OnPointerUp(PointerEventData e){if(knob)knob.anchoredPosition=Vector2.zero;if(OnlyUpPlayerController.Instance)OnlyUpPlayerController.Instance.SetMove(Vector2.zero);}
}
public class OnlyUpJumpButton:MonoBehaviour,IPointerDownHandler{public void OnPointerDown(PointerEventData e){if(OnlyUpPlayerController.Instance)OnlyUpPlayerController.Instance.Jump();}}
public class OnlyUpLookArea:MonoBehaviour,IDragHandler{public void OnDrag(PointerEventData e){var c=Camera.main?Camera.main.GetComponent<OnlyUpCamera>():null;if(c)c.AddLook(e.delta);}}