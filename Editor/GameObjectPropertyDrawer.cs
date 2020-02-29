using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EngineArtist {


public static class GameObjectPropertyDrawerUtils {
    public static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
    public static bool objectPickerMode = false;
    public static int pickedDrawerID = 0;
    public static GameObject pickedObject = null;
    public static bool callbackRegistered = false;
    public static int currentPropertyDrawerID = 0;

    public static void ResetPropertyDrawerID() {
        currentPropertyDrawerID = 0;
    }

    public static void UpdateCallback(SceneView view) {
        if (GameObjectPropertyDrawerUtils.objectPickerMode) {
            var cur = Event.current;
            switch (cur.type) {
                case EventType.Layout: {
                    HandleUtility.AddDefaultControl(0);
                    break;
                }
                case EventType.MouseUp: {
                    GameObjectPropertyDrawerUtils.pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                    GameObjectPropertyDrawerUtils.objectPickerMode = false;
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    break;
                }
            }
        }
    }

    public static Texture2D GetIcon(string name) {
        Texture2D result = null;
        if (!icons.TryGetValue(name, out result)) {
            var guids = AssetDatabase.FindAssets(name);
            if (guids.Length > 0) {
                result = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guids[0]));
                icons[name] = result;
            }
        }
        return result;
    }
}


[CustomPropertyDrawer(typeof(GameObject))]
public class GameObjectPropertyDrawer: PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ++GameObjectPropertyDrawerUtils.currentPropertyDrawerID;
        if (!GameObjectPropertyDrawerUtils.callbackRegistered) {
            GameObjectPropertyDrawerUtils.callbackRegistered = true;
            EditorApplication.update -= GameObjectPropertyDrawerUtils.ResetPropertyDrawerID;
            EditorApplication.update += GameObjectPropertyDrawerUtils.ResetPropertyDrawerID;
            SceneView.duringSceneGui -= GameObjectPropertyDrawerUtils.UpdateCallback;
            SceneView.duringSceneGui += GameObjectPropertyDrawerUtils.UpdateCallback;
        }
        if (!GameObjectPropertyDrawerUtils.objectPickerMode && GameObjectPropertyDrawerUtils.pickedDrawerID == GameObjectPropertyDrawerUtils.currentPropertyDrawerID) {
            property.objectReferenceValue = GameObjectPropertyDrawerUtils.pickedObject;
            GameObjectPropertyDrawerUtils.pickedObject = null;
            GameObjectPropertyDrawerUtils.pickedDrawerID = 0;
        }
        EditorGUI.BeginProperty(position, label, property);
        property.objectReferenceValue = EditorGUI.ObjectField(new Rect(position.x, position.y, position.width - position.height, position.height), label, property.objectReferenceValue, typeof(GameObject), true);
        var pickerRect = new Rect(position.x + position.width - position.height, position.y, position.height, position.height);
        if (GUI.Button(pickerRect, "")) {
            GameObjectPropertyDrawerUtils.objectPickerMode = !GameObjectPropertyDrawerUtils.objectPickerMode;
            if (GameObjectPropertyDrawerUtils.objectPickerMode) {
                GameObjectPropertyDrawerUtils.pickedDrawerID = GameObjectPropertyDrawerUtils.currentPropertyDrawerID;
            }
            else {
                GameObjectPropertyDrawerUtils.pickedDrawerID = 0;
            }
        }
        var pickerOffsetX = pickerRect.width / 8f;
        var pickerOffsetY = pickerRect.height / 8f;
        var pickerIconRect = new Rect(pickerRect.x + pickerOffsetX, pickerRect.y + pickerOffsetY, pickerRect.width - pickerOffsetX*2f, pickerRect.height - pickerOffsetY*2f);
        GUI.DrawTexture(pickerIconRect, GameObjectPropertyDrawerUtils.GetIcon("object_picker"), ScaleMode.ScaleAndCrop, true);
        EditorGUI.EndProperty();
    }
}


}