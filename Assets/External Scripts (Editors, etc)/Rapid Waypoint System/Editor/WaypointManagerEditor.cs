using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor {

    public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
    {
        return (Component)property.serializedObject.targetObject;
    }

    private ReorderableList nodeList;

    private void OnEnable()
    {
        nodeList = new ReorderableList(serializedObject,
                                   serializedObject.FindProperty("waypointNodes"),
                                   true, true, true, true);

        nodeList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = nodeList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            GUIContent typeContent = new GUIContent("Node " + index + ":", "");
            Rect typeRect = new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(typeRect, typeContent);

            Rect prefabRect = new Rect(rect.x + 55, rect.y, rect.width - 60, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(prefabRect, element.FindPropertyRelative("transform"), GUIContent.none);
        };

        nodeList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Nodes");
        };

        nodeList.onSelectCallback = (ReorderableList l) =>
        {
            var transform_ = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("transform").objectReferenceValue as Transform;
            if (transform_)
            {
                EditorGUIUtility.PingObject(transform_.gameObject);
                //Selection.activeTransform = transform_;
            }
        };

        nodeList.onRemoveCallback = (ReorderableList l) =>
        {
            var transform_ = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("transform").objectReferenceValue as Transform;
            if (transform_)
                DestroyImmediate(transform_.gameObject);
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        };

        nodeList.onAddCallback = (ReorderableList l) =>
        {
            
            Node node = new Node();
            GameObject obj = new GameObject("Node " + l.count);
            obj.transform.parent = GameObject.FindObjectOfType<WaypointManager>().transform;                           
            node.transform = obj.transform;

            int index = nodeList.serializedProperty.arraySize;
            nodeList.serializedProperty.arraySize++;
            nodeList.index = index;
            SerializedProperty element = nodeList.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("transform").objectReferenceValue = node.transform;
            serializedObject.ApplyModifiedProperties();
            
        };

        nodeList.onChangedCallback = (ReorderableList l) =>
        {
            for (int i = 0; i < l.count; ++i)
            {
                var transform_ = l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("transform").objectReferenceValue as Transform;
                transform_.gameObject.name = "Node " + i.ToString();
            }
        };

    }

    public override void OnInspectorGUI()
    {
        WaypointManager waypoints = target as WaypointManager;

        GUIContent looping = new GUIContent("Should Loop", "Should the agents restart from node 0?");
        waypoints.looping = EditorGUILayout.Toggle(looping, waypoints.looping);

        GUIContent updateInterval = new GUIContent("Update interval (/s)", "The amount of times per second the waypoint system should update.");
        waypoints.updateIntervalPerSecond = EditorGUILayout.IntSlider(updateInterval, waypoints.updateIntervalPerSecond, 10, 60);

        GUIContent nodeDistance = new GUIContent("Node Proximity", "The minimum distance from the current node to change to the next.");
        waypoints.nodeProximityDistance = EditorGUILayout.FloatField(nodeDistance, waypoints.nodeProximityDistance);

        EditorGUILayout.Separator();

        //List<WaypointAgent> ra = serializedObject.FindProperty("objectToMove") as List<WaypointAgent>();
        EditorGUILayout.SelectableLabel("Current active objects in system:" + waypoints.AgentQuantity.ToString());

        //GUIContent targetMin = new GUIContent("Target spread", "The amount of randomness to apply to chosing the next point to traverse to.");
        //waypoints.nodeProximityDistance = EditorGUILayout.Slider(targetMin, waypoints.nodeProximityDistance, 0.0f, 10.0f);

        GUIContent rotationMode = new GUIContent("Agent Rotation Mode", "The the agent should look at the next node");
        waypoints.rotationMode = (WaypointRotationMode)EditorGUILayout.EnumPopup(rotationMode, waypoints.rotationMode);

        if (waypoints.rotationMode == WaypointRotationMode.Slerp)
        {
            GUIContent slerpRotationSpeed = new GUIContent("Rotation speed", "The speeed at which the agent looks to face the target");
            waypoints.slerpRotationSpeed = EditorGUILayout.Slider(slerpRotationSpeed, waypoints.slerpRotationSpeed, 1, 100);
        }

        GUILayout.Space(10);
        serializedObject.Update();
        nodeList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);
    }

}
