using UnityEngine;
using System.Collections;

public class AnimationTextureBehaviour : MonoBehaviour {

    Texture2D[] textures;
    float frameTime;
    float lastFrame = 0;
    int frame = 0;
    bool shaker;
    int step = 1;
    Material mat;
    public void SetTexture(Texture2D[] textures, float duration, bool shaker)
    {
        mat = GetComponent<Renderer>().material;
        this.textures = textures;
        frameTime = duration / textures.Length;
        this.shaker = shaker;
        mat.mainTexture = textures[0];
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(Time.time - lastFrame > frameTime)
        {
            frame += step;
            if (frame == textures.Length)
            {
                if (shaker)
                {
                    step *= -1;
                    frame += step;
                }
                else
                    frame = 0;
            }
            else if(shaker && frame == 0)
            {
                step *= -1;
                frame += step;
            }
            lastFrame = Time.time;
            mat.mainTexture = textures[frame];
        }
	}
}
