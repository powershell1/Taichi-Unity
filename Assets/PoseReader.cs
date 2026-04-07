using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Landmark { public float x, y, z; }

[System.Serializable]
public class FrameData { public List<Landmark> landmarks; }

[System.Serializable]
public class PoseAnimation 
{ 
    public int fps; 
    public List<FrameData> frames; 
}

public class PoseReader : MonoBehaviour
{
    public TextAsset jsonFile;
    public Transform[] targetPoints; // Array of 33 empty GameObjects
    
    private PoseAnimation animData;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        animData = JsonUtility.FromJson<PoseAnimation>(jsonFile.text);
    }

    void Update()
    {
        if (animData == null || animData.frames.Count == 0) return;

        timer += Time.deltaTime;
        float frameDuration = 1f / animData.fps;

        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame = (currentFrame + 1) % animData.frames.Count;
            UpdatePose(currentFrame);
        }
    }

    void UpdatePose(int frameIndex)
    {
        var frame = animData.frames[frameIndex];
        for (int i = 0; i < 33 && i < targetPoints.Length; i++)
        {
            if (targetPoints[i] != null)
            {
                // Coordinate Conversion (MediaPipe -> Unity Space)
                // MediaPipe is Right-Handed (Y-down), Unity is Left-Handed (Y-up)
                // You may need to tweak these multipliers based on your specific camera setup
                float x = frame.landmarks[i].x;
                float y = -frame.landmarks[i].y; 
                float z = frame.landmarks[i].z; 

                targetPoints[i].localPosition = new Vector3(x, y, z);
            }
        }
    }
}