#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdsService))]
public class AdsServiceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var ads = (AdsService)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Show Banner"))
            ads.ShowBanner();

        if (GUILayout.Button("Hide Banner"))
            ads.HideBanner();

        if (GUILayout.Button("Show Rewarded"))
            ads.ShowRewarded();
    }
}
#endif