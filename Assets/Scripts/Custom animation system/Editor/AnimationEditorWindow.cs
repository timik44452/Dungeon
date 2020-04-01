using CustomAnimationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimationEditorWindow : EditorWindow
{
    private IAnimationReader animationReader;

    // in milliseconds
    private int timeout = 150;
    private bool is_recording;

    private DateTime lastSnapshot;
    private DateTime startSnapshot;

    private Rect timelineRect;
    private Rect toolbarRect;

    private Dictionary<string, AnimationCurve> curves = new Dictionary<string, AnimationCurve>();

    private void OnGUI()
    {
        if (animationReader != null && is_recording && animationReader.Connected && (DateTime.Now - lastSnapshot).TotalMilliseconds >= timeout)
        {
            var tempPoints = animationReader.Read(); 

            if (tempPoints.Count() > 0)
            {
                float time = Math.Max((float)(lastSnapshot - startSnapshot).TotalSeconds, 0);

                tempPoints.ForEach(point =>
                {
                    string key_position_x = $"{point.key}/localPosition.x";
                    string key_position_y = $"{point.key}/localPosition.y";
                    string key_position_z = $"{point.key}/localPosition.z";

                    //string key_rotation_x = $"{point.key}/localRotation.x";
                    //string key_rotation_y = $"{point.key}/localRotation.y";
                    //string key_rotation_z = $"{point.key}/localRotation.z";

                    //string key_scale_x = $"{point.key}/localScale.x";
                    //string key_scale_y = $"{point.key}/localScale.y";
                    //string key_scale_z = $"{point.key}/localScale.z";

                    if (!curves.ContainsKey(key_position_x)) curves.Add(key_position_x, new AnimationCurve());
                    if (!curves.ContainsKey(key_position_y)) curves.Add(key_position_y, new AnimationCurve());
                    if (!curves.ContainsKey(key_position_z)) curves.Add(key_position_z, new AnimationCurve());

                    //if (!curves.ContainsKey(key_rotation_x)) curves.Add(key_rotation_x, new AnimationCurve());
                    //if (!curves.ContainsKey(key_rotation_y)) curves.Add(key_rotation_y, new AnimationCurve());
                    //if (!curves.ContainsKey(key_rotation_z)) curves.Add(key_rotation_z, new AnimationCurve());

                    //if (!curves.ContainsKey(key_scale_x)) curves.Add(key_scale_x, new AnimationCurve());
                    //if (!curves.ContainsKey(key_scale_y)) curves.Add(key_scale_y, new AnimationCurve());
                    //if (!curves.ContainsKey(key_scale_z)) curves.Add(key_scale_z, new AnimationCurve());

                    curves[key_position_x].AddKey(time, point.localPosition.x);
                    curves[key_position_y].AddKey(time, point.localPosition.y);
                    curves[key_position_z].AddKey(time, point.localPosition.z);

                    //curves[key_rotation_x].AddKey(time, point.localRotation.x);
                    //curves[key_rotation_y].AddKey(time, point.localRotation.y);
                    //curves[key_rotation_z].AddKey(time, point.localRotation.z);

                    //curves[key_scale_x].AddKey(time, point.localScale.x);
                    //curves[key_scale_y].AddKey(time, point.localScale.y);
                    //curves[key_scale_z].AddKey(time, point.localScale.z);
                });
                
                //Frame frame = new Frame(time, tempPoints);
                
                //AnimationProfileEditor.animationProfile.Frames.Add(frame);
            }

            lastSnapshot = DateTime.Now;
        }

        CalculateLayout();

        DrawToolbar();

        //DrawTimeline(AnimationProfileEditor.animationProfile.Start, AnimationProfileEditor.animationProfile.End);

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

        Rect first_button = new Rect(toolbarRect.x, toolbarRect.y, toolbarRect.width, toolbarRect.width);
        Rect second_button = new Rect(toolbarRect.x, toolbarRect.y + toolbarRect.width, toolbarRect.width, toolbarRect.width);

        if (animationReader == null || !animationReader.Connected)
        {
            if (GUI.Button(first_button, "Connect"))
            {
                if (animationReader == null)
                    animationReader = new AnimationReader(3000);

                animationReader.Connect();
            }
        }
        else
        {
            if (GUI.Button(first_button, "Disconnect"))
            {
                animationReader.Disconnect();
            }
        }

        if (GUI.Button(second_button, is_recording ? "S" : "R"))
        {
            is_recording = !is_recording;
            startSnapshot = DateTime.Now;

            if (!is_recording && curves.Count > 0)
            {
                string path = EditorUtility.SaveFilePanelInProject("Save animation", "Motion.anim", "anim", "");

                if (path.Length > 0)
                {
                    SaveClip(path);
                }
            }

            curves.Clear();

        }
    }

    private void SaveClip(string path)
    {
        AnimationClip clip = new AnimationClip();

        foreach (var curve in curves)
        {
            string realtivePath = curve.Key.Split('/')[0];
            string property = curve.Key.Split('/')[1];

            clip.SetCurve($"{realtivePath}", typeof(Transform), property, curve.Value);
        }

        AssetDatabase.CreateAsset(clip, path);
    }
}
