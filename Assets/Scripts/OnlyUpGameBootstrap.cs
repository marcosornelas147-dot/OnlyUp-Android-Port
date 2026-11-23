using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OnlyUpGameBootstrap : MonoBehaviour {
 void Start() {
  if (FindObjectOfType<OnlyUpPlayerController>() != null) return;
  RenderSettings.ambientLight=new Color(.58f,.62f,.7f);
  var sun=new GameObject("Sun");var l=sun.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.15f;sun.transform.rotation=Quaternion.Euler(48,-32,0);
  CreateCourse();CreatePlayer();CreateUI();
 }
 Material Mat(Color c){var m=new Material(Shader.Find("Standard"));m.color=c;return m;}
 void CreateCourse(){
  var colors=new[]{new Color(.25f,.48f,.68f),new Color(.32f,.58f,.48f),new Color(.72f,.48f,.27f),new Color(.48f,.39f,.68f)};
  for(int i=0;i<64;i++){float h=i*2.15f,a=i*1.17f,r=3.1f+(i%5)*.28f;Vector3 p=new Vector3(Mathf.Cos(a)*r,h,Mathf.Sin(a)*r);
   var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name="ClimbPlatform_"+i;o.transform.position=p;o.transform.localScale=new Vector3(3.2f,.45f,3.2f);o.transform.rotation=Quaternion.Euler(0,(i*31)%360,0);o.GetComponent<Renderer>().material=Mat(colors[i%4]);
   if(i%8==7){var post=GameObject.CreatePrimitive(PrimitiveType.Cylinder);post.name="CheckpointMarker_"+i;post.transform.position=p+Vector3.up*1.4f;post.transform.localScale=new Vector3(.22f,1.4f,.22f);post.GetComponent<Renderer>().material=Mat(Color.yellow);}
  }
  var ground=GameObject.CreatePrimitive(PrimitiveType.Cube);ground.name="StartGround";ground.transform.position=new Vector3(0,-.55f,0);ground.transform.localScale=new Vector3(18,1,18);ground.GetComponent<Renderer>().material=Mat(new Color(.22f,.28f,.32f));
 }
 void CreatePlayer(){
  var p=GameObject.CreatePrimitive(PrimitiveType.Capsule);p.name="Player";p.transform.position=new Vector3(0,1.2f,0);p.GetComponent<Renderer>().material=Mat(new Color(.9f,.42f,.18f));Destroy(p.GetComponent<CapsuleCollider>());
  var cc=p.AddComponent<CharacterController>();cc.height=1.8f;cc.radius=.38f;cc.center=new Vector3(0,.9f,0);p.AddComponent<OnlyUpPlayerController>();
  var cam=new GameObject("Main Camera");cam.tag="MainCamera";cam.AddComponent<Camera>();cam.AddComponent<AudioListener>();var rig=cam.AddComponent<OnlyUpCamera>();rig.target=p.transform;
 }
 void CreateUI(){
  var co=new GameObject("MobileControls");var canvas=co.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;co.AddComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;co.AddComponent<GraphicRaycaster>();
  if(FindObjectOfType<EventSystem>()==null){var es=new GameObject("EventSystem");es.AddComponent<EventSystem>();es.AddComponent<StandaloneInputModule>();}
  var joy=UIBox(canvas.transform,"JoystickBase",new Vector2(140,140),new Vector2(0,0),new Vector2(0,0),new Vector2(110,110));joy.GetComponent<Image>().color=new Color(1,1,1,.22f);var j=joy.AddComponent<OnlyUpVirtualJoystick>();
  var knob=UIBox(joy.transform,"JoystickKnob",new Vector2(58,58),Vector2.zero,Vector2.zero,Vector2.zero);knob.GetComponent<Image>().color=new Color(1,1,1,.65f);j.knob=knob.GetComponent<RectTransform>();
  var jump=UIBox(canvas.transform,"JumpButton",new Vector2(110,110),new Vector2(1,0),new Vector2(1,0),new Vector2(-110,110));jump.GetComponent<Image>().color=new Color(.95f,.45f,.12f,.85f);jump.AddComponent<OnlyUpJumpButton>();
  var txt=new GameObject("Label");txt.transform.SetParent(jump.transform,false);var tr=txt.AddComponent<RectTransform>();tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=Vector2.zero;tr.offsetMax=Vector2.zero;var text=txt.AddComponent<Text>();text.text="PULAR";text.alignment=TextAnchor.MiddleCenter;text.color=Color.white;text.font=Resources.GetBuiltinResource<Font>("Arial.ttf");
  var look=UIBox(canvas.transform,"LookArea",Vector2.zero,new Vector2(.35f,0),new Vector2(1,1),Vector2.zero);look.GetComponent<Image>().color=new Color(1,1,1,0);look.AddComponent<OnlyUpLookArea>();
 }
 GameObject UIBox(Transform parent,string name,Vector2 size,Vector2 amin,Vector2 amax,Vector2 pos){var o=new GameObject(name);o.transform.SetParent(parent,false);var r=o.AddComponent<RectTransform>();r.anchorMin=amin;r.anchorMax=amax;r.sizeDelta=size;r.anchoredPosition=pos;o.AddComponent<Image>();return o;}
}