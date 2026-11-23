#if UNITY_EDITOR
using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;using UnityEngine.SceneManagement;
public static class OnlyUpProjectSetup{
 [MenuItem("Only Up/Rebuild Prototype Scene")]
 public static void Build(){var old=Object.FindObjectOfType<OnlyUpGameBootstrap>();if(old)Object.DestroyImmediate(old.gameObject);var go=new GameObject("OnlyUpGameBootstrap");go.AddComponent<OnlyUpGameBootstrap>();EditorSceneManager.SaveScene(SceneManager.GetActiveScene(),"Assets/Scenes/OnlyUpPrototype.unity");PlayerSettings.companyName="OnlyUpReconstruction";PlayerSettings.productName="Only Up Alpha Mobile Prototype";PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,ScriptingImplementation.IL2CPP);PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARMv7;PlayerSettings.defaultInterfaceOrientation=UIOrientation.LandscapeLeft;EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/OnlyUpPrototype.unity",true)};Debug.Log("Only Up prototype scene prepared for Android ARMv7.");}
}
#endif