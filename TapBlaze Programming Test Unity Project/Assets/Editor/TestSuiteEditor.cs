using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestSuite))]
public class TestSuiteEditor : Editor
{
    public static string bonusWheelTestResults = null;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("Recommended minimum value of 1000.");

        TestSuite testSuiteScript = (TestSuite)target;
        if (GUILayout.Button("Test Bonus Wheel"))
        {
            TestSuiteEditor.bonusWheelTestResults = testSuiteScript.TestBonusWheel();
        }
        if (TestSuiteEditor.bonusWheelTestResults != null)
        {
            EditorGUILayout.HelpBox(TestSuiteEditor.bonusWheelTestResults, MessageType.None);
        }
    }

}
