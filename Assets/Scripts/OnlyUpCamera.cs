using UnityEngine;
public class OnlyUpCamera:MonoBehaviour{
 public Transform target;public float distance=5.2f,height=2.4f,sensitivity=.16f;float yaw,pitch=12;Vector2 look;
 public void AddLook(Vector2 d){look+=d;}
 void LateUpdate(){if(!target)return;yaw+=look.x*sensitivity;pitch=Mathf.Clamp(pitch-look.y*sensitivity,-12,55);look=Vector2.zero;Quaternion rot=Quaternion.Euler(pitch,yaw,0);Vector3 pos=target.position+Vector3.up*height-rot*Vector3.forward*distance;transform.position=Vector3.Lerp(transform.position,pos,12*Time.deltaTime);transform.rotation=Quaternion.LookRotation(target.position+Vector3.up*1.2f-transform.position,Vector3.up);}
}