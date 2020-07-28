﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ADBRuntime
{
    using Mono;
    public class ADBEditorColliderEditor : Editor
    {
        [CustomEditor(typeof(ADBEditorCollider))]
        public class ADBRuntimeEditor : Editor
        {
            ADBEditorCollider controller;
            private bool isDeleteCollider;
            private CollideFuncZh collideFuncZh;
            private enum CollideFuncZh
            {
                冻结在表面=0,
                碰撞体_向外排斥=1,
                碰撞体_约束在内=2,
                力场_向外排斥=3,
                力场_约束在内=4,
            }
            public void OnEnable()
            {
                controller = (target as ADBEditorCollider);

            }
            public override void OnInspectorGUI()
            {
                if (Application.isPlaying)
                {
                    Titlebar("你不能在运行过程中改变它",Color.red);
                }
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.isDraw"), new GUIContent("绘制碰撞体"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isGlobal"), new GUIContent("是否为全局碰撞体"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.isOpen"), new GUIContent("是否打开"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.colliderType"), new GUIContent("Collider Type"), true);

                switch (controller.GetColliderType())
                {
                    case ColliderType.Sphere:
                        Titlebar("球体", Color.cyan);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.radius"), new GUIContent("┗━I 半径"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.positionOffset"), new GUIContent("┗━I 偏移坐标"), true);

                        break;
                    case ColliderType.Capsule:
                        Titlebar("胶囊体", Color.cyan);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.radius"), new GUIContent("┗━I 半径"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.length"), new GUIContent("┗━I 长度"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.positionOffset"), new GUIContent("┗━I 偏移坐标"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.staticDirection"), new GUIContent("┗━I 朝向"), true);
      
                        break;
                    case ColliderType.OBB:
                        Titlebar("立方体", Color.cyan);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.boxSize"), new GUIContent("┗━I 尺寸"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.positionOffset"), new GUIContent("┗━I 偏移坐标"), true);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.staticDirection"), new GUIContent("┗━I 朝向"), true);
                        break;
                    default:
                        break;
                }          
                collideFuncZh=(CollideFuncZh) EditorGUILayout.EnumPopup("┗━I 碰撞体功能", collideFuncZh);
                controller.editor.colliderRead.collideFunc = (CollideFunc)collideFuncZh;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.colliderChoice"), new GUIContent("┗━I 碰撞体种类"), true);

                // EditorGUILayout.PropertyField(serializedObject.FindProperty("editor.colliderRead.isConnectWithBody"), new GUIContent("Is Connect With Body"), true);
                if (!Application.isPlaying)
                {
                    controller.Refresh();
                    serializedObject.ApplyModifiedProperties();
                }
            }

            void Titlebar(string text, Color color)
            {
                GUILayout.Space(12);

                var backgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = color;

                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                GUILayout.Label(text);
                EditorGUILayout.EndHorizontal();

                GUI.backgroundColor = backgroundColor;

                GUILayout.Space(3);
            }
        }
    }
}
