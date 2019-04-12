using UnityEngine;
using UnityEditor;
using System;

public class CustomShaderGUI : ShaderGUI
{
    MaterialEditor editor;
    MaterialProperty[] properties;
    Material target;

    enum Choice
    {
        NORMAL_ONLY, TEX_ONLY, BLINN_PHONG
    }

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        this.target = editor.target as Material;

        Choice choice = Choice.NORMAL_ONLY;
        if (target.IsKeywordEnabled("USE_NORMAL"))
            choice = Choice.NORMAL_ONLY;
        if (target.IsKeywordEnabled("USE_TEX"))
            choice = Choice.TEX_ONLY;
        if (target.IsKeywordEnabled("USE_SPECULAR"))
            choice = Choice.BLINN_PHONG;

        EditorGUI.BeginChangeCheck();
        choice = (Choice)EditorGUILayout.EnumPopup(
            new GUIContent("Shader Type"), choice
        );

        if (EditorGUI.EndChangeCheck())
        {
            if (choice == Choice.NORMAL_ONLY)
            {
                target.EnableKeyword("USE_NORMAL");
                target.DisableKeyword("USE_TEX");
                target.DisableKeyword("USE_SPECULAR");
            }
            else if (choice == Choice.TEX_ONLY)
            {
                target.DisableKeyword("USE_NORMAL");
                target.EnableKeyword("USE_TEX");
                target.DisableKeyword("USE_SPECULAR");
            }
            else
            {
                target.DisableKeyword("USE_NORMAL");
                target.DisableKeyword("USE_TEX");
                target.EnableKeyword("USE_SPECULAR");
            }
        }

        if (choice != Choice.NORMAL_ONLY)
        {
            MaterialProperty mainTex = FindProperty("_MainTex", properties);
            GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
            editor.TextureProperty(mainTex, mainTexLabel.text);

            if (choice == Choice.BLINN_PHONG)
            {
                MaterialProperty diffuse = FindProperty("_Diffuse", properties);
                GUIContent shininessLabe1 = new GUIContent(diffuse.displayName);
                editor.ColorProperty(diffuse, "Diffuse Color");

                MaterialProperty specular = FindProperty("_Specular", properties);
                GUIContent shininessLabe2 = new GUIContent(specular.displayName);
                editor.ColorProperty(specular, "Specular Color");

                MaterialProperty shininess = FindProperty("_Shininess", properties);
                GUIContent shininessLabe3 = new GUIContent(shininess.displayName);
                editor.FloatProperty(shininess, "Specular Factor");
            }
        }
    }
}
