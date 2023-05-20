using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System;
using UnityEditor;

namespace Camera360
{

    public enum InvokeMethodType { Static, MonoBehaviour, GameStarted};
    [System.Serializable]
    public class Game3DState
    {
        public string stateName;
        public float cameraSpread;
        public float convergencePointDistance;
        public float changeDelay;
        public float cameraSize;
        [Space(10)]
        public InvokeMethodType invokeMethod = InvokeMethodType.Static;

        [Space(10)]
        [Tooltip("Name of the static class containing the Action to listen to")]
        public string staticTypeName;
        [Tooltip("Name of the Action to listen to ")]
        public string triggerActionName;

        [Space(10)]
        public UnityEvent monoBehaviourScript;
        [Space(10)]
        public string monoTriggerAction;

        private Action currentAction = null;

        public Game3DState(float cameraSpread, float convergencePointDistance, float changeDelay, float cameraSize)
        {
            this.cameraSpread = cameraSpread;
            this.convergencePointDistance = convergencePointDistance; 
            this.changeDelay = changeDelay;
            this.cameraSize = cameraSize;
        }
        public void Subscribe(Action stateChange)
        {
            if (invokeMethod == InvokeMethodType.Static)
            {
                StaticSubscribe(stateChange);
            }
            else
            if (invokeMethod == InvokeMethodType.MonoBehaviour)
            {
                UnityEventSubscribe(stateChange);
            }
        }
        public void Unsubscribe(Action stateChange)
        {
            if (invokeMethod == InvokeMethodType.Static)
            {
                StaticUnsubscribe(stateChange);
            }
            else
            if (invokeMethod == InvokeMethodType.MonoBehaviour)
            {
                UnityEventUnsubscribe(stateChange);
            }
        }
        void StaticSubscribe(Action stateChange)
        {
            if(triggerActionName=="")
            {
                return;
            }

            Type type = Type.GetType(staticTypeName);

            FieldInfo fi = type.GetField(triggerActionName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            Action actionVal = (Action)fi.GetValue(null);
            actionVal += stateChange;
            currentAction = stateChange;
            fi.SetValue(Type.GetType(staticTypeName), actionVal);
        }
        void UnityEventSubscribe(Action stateChange)
        {
            object o = monoBehaviourScript.GetPersistentTarget(0);
            FieldInfo fi = o.GetType().GetField(monoTriggerAction, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            Action actionVal = (Action)fi.GetValue(o);
            actionVal += stateChange;
            currentAction = stateChange;
            fi.SetValue(o, actionVal);
        }
        void StaticUnsubscribe(Action stateChange)
        {
            if (triggerActionName == "")
            {
                return;
            }


            Type type = Type.GetType(staticTypeName);

            FieldInfo fi = type.GetField(triggerActionName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            Action actionVal = (Action)fi.GetValue(null);
            actionVal -= currentAction;
            fi.SetValue(Type.GetType(staticTypeName), actionVal);
        }
        void UnityEventUnsubscribe(Action stateChange)
        {
            object o = monoBehaviourScript.GetPersistentTarget(0);
            FieldInfo fi = o.GetType().GetField(monoTriggerAction, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            Action actionVal = (Action)fi.GetValue(o);
            actionVal -= currentAction;
            fi.SetValue(o, actionVal);
        }
        void TestDebug()
        {
            Debug.Log("Trigger fired! " + triggerActionName);
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Game3DState))]
    public class Game3DStateEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);


            position.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, false, EditorStyles.foldout);

            if (property.isExpanded)
            {
                string[] propertyNames = new string[] { "stateName", "cameraSpread", "convergencePointDistance", "changeDelay", "cameraSize", "invokeMethod" };
                float propSpace = 18f;
                float propHeight = 25f;
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    string prop = propertyNames[i];

                    var amountRect = new Rect(position.x, position.y + propSpace*(i+1), position.width, propHeight);
                    EditorGUI.PropertyField(amountRect, property.FindPropertyRelative(prop));
                }
                if (property.FindPropertyRelative("invokeMethod").enumValueIndex == 0)
                {
                    int i = propertyNames.Length;

                    var amountRect = new Rect(position.x, position.y + (propSpace) * (i + 1) + 5, position.width, propHeight);
                    EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("staticTypeName"));
                    i++;

                    var amountRect1 = new Rect(position.x, position.y + (propSpace) * (i + 2), position.width, propHeight);
                    EditorGUI.PropertyField(amountRect1, property.FindPropertyRelative("triggerActionName"));
                }
                else
                if(property.FindPropertyRelative("invokeMethod").enumValueIndex == 1)
                {
                    int i = propertyNames.Length;

                    var amountRect = new Rect(position.x, position.y + (propSpace) * (i + 1) + 5, position.width, propHeight*4);
                    EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("monoBehaviourScript"));
                    amountRect = new Rect(position.x, position.y + (propSpace) * (i + 1) + 90, position.width, propHeight);
                    EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("monoTriggerAction"));
                }
            }
            EditorGUI.EndProperty();

        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(!property.isExpanded)
            {
                return EditorGUI.GetPropertyHeight(property);
            }
            else
            {
                if(property.FindPropertyRelative("invokeMethod").enumValueIndex == 0)
                {
                    return EditorGUI.GetPropertyHeight(property) - 70f;
                }
                else
                if (property.FindPropertyRelative("invokeMethod").enumValueIndex == 1)
                {
                    return EditorGUI.GetPropertyHeight(property) - 50f;
                }
                else
                {
                    return EditorGUI.GetPropertyHeight(property) - 160;
                }
            }
        }
    }
#endif
}