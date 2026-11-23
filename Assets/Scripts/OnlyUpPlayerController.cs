using UnityEngine;
public class OnlyUpPlayerController:MonoBehaviour{
 public float walkSpeed=5.2f,jumpHeight=1.65f,gravity=-22f,turnSpeed=14f;CharacterController cc;Vector3 vertical;Vector2 moveInput;bool jumpQueued;public static OnlyUpPlayerController Instance;
 void Awake(){Instance=this;cc=GetComponent<CharacterController>();}
 public void SetMove(Vector2 v){moveInput=Vector2.ClampMagnitude(v,1);}public void Jump(){jumpQueued=true;}
 void Update(){Vector2 input=moveInput;
#if UNITY_EDITOR || UNITY_STANDALONE
 if(input.sqrMagnitude<.01f)input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));if(Input.GetButtonDown("Jump"))jumpQueued=true;
#endif
 var cam=Camera.main;Vector3 f=cam?cam.transform.forward:Vector3.forward;f.y=0;f.Normalize();Vector3 r=cam?cam.transform.right:Vector3.right;r.y=0;r.Normalize();Vector3 d=f*input.y+r*input.x;
 if(d.sqrMagnitude>.01f)transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(d),turnSpeed*Time.deltaTime);
 cc.Move(d.normalized*walkSpeed*Time.deltaTime);if(cc.isGrounded&&vertical.y<0)vertical.y=-2f;if(jumpQueued&&cc.isGrounded)vertical.y=Mathf.Sqrt(jumpHeight*-2f*gravity);jumpQueued=false;vertical.y+=gravity*Time.deltaTime;cc.Move(vertical*Time.deltaTime);
}}