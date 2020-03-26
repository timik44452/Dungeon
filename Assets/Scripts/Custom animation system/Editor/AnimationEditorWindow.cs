using CustomAnimationSystem;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimationEditorWindow : EditorWindow
{
    private IAnimationReader animationReader;

    // in milliseconds
    private int timeout = 500;
    private bool is_recording;

    private DateTime lastSnapshot;
    private DateTime startSnapshot;

    private Rect timelineRect;
    private Rect toolbarRect;

    private void OnGUI()
    {
        if (AnimationProfileEditor.animationProfile == null)
        {
            return;
        }

        if (animationReader != null && is_recording && animationReader.Connected && (DateTime.Now - lastSnapshot).TotalMilliseconds >= timeout)
        {
            var tempPoints = animationReader.Read(); 

            if (tempPoints.Count() > 0)
            {
                Debug.Log(tempPoints.Count());
                Frame frame = new Frame((float)(lastSnapshot - startSnapshot).TotalSeconds, tempPoints);

                AnimationProfileEditor.animationProfile.Frames.Add(frame);
            }

            lastSnapshot = DateTime.Now;
        }

        CalculateLayout();

        DrawToolbar();

        DrawTimeline(AnimationProfileEditor.animationProfile.Start, AnimationProfileEditor.animationProfile.End);

        Repaint();
    }

    private void CalculateLayout()
    {
        float space = Mathf.Min(position.width, position.height) * 0.02F;

        float timeline_height = Mathf.Min(position.height * 0.05F, 30);
        float toolbar_height = position.height * 0.5F;
        float toolbar_width = Mathf.Min(position.width * 0.02F, 35);

        timelineRect = new Rect(0, position.height - timeline_height, position.width, timeline_height);
        toolbarRect = new Rect(space, (position.height - toolbar_height) * 0.5F, toolbar_width, toolbar_height);
    }

    private void DrawToolbar()
    {
        GUI.Box(toolbarRect, "");

        GUILayout.BeginArea(toolbarRect);

        if (animationReader == null || !animationReader.Connected)
        {
            if (GUILayout.Button("Connect"))
            {
                if (animationReader == null)
                    animationReader = new AnimationReader(3000);

                animationReader.Connect();
            }
        }
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                animationReader.Disconnect();
            }

            if (GUILayout.Button(is_recording ? "S" : "R"))
            {
                EditorUtility.SetDirty(AnimationProfileEditor.animationProfile);
                AssetDatabase.SaveAssets();

                is_recording = !is_recording;
                startSnapshot = DateTime.Now;
            }
        }

        GUILayout.EndArea();
    }

    private void DrawTimeline(float timeStart, float timeEnd)
    {
        GUI.Box(timelineRect, "");

        if (AnimationProfileEditor.animationProfile)
        {
            GUI.color = Color.red;

            float key_width = 5;
            float key_height = timelineRect.height * 0.33F;

            foreach (Frame frame in AnimationProfileEditor.animationProfile.Frames)
            {
                float localPosition = (frame.time - timeStart) / (timeEnd - timeStart);

                float key_x = timelineRect.x + localPosition * (timelineRect.width - key_width);
                float key_y = timelineRect.y;

                GUI.Box(new Rect(key_x, key_y, key_width, key_height), "");
            }
        }
    }
}
