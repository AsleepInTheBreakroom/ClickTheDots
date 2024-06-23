using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Color[] colors;
    private SpriteRenderer sprite_renderer;

    //Sizes
    private int size = 0;

    //Points
    [HideInInspector] public int point_value;

    //Scale
    private Vector3 scale = Vector3.zero;

    //Life
    private float time_before_shrink = 0.0f;

    //Audio
    [HideInInspector] public AudioSource audio_source;

    void Start()
    {
        //Get audio source component.
        audio_source = GetComponent<AudioSource>();
        
        //Set Colors
        sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = colors[Random.Range(0, colors.Length - 1)];

        //Set Size
        size = Random.Range(0, 3);

        switch(size)
        {
            case 0: //Small
                scale = transform.localScale *= 0.7f;
                point_value = 20;
                time_before_shrink = 1f;
                audio_source.pitch = Random.Range(1.3f, 1.5f);
                break;

            case 1: //Medium
                scale = transform.localScale *= 1.4f;
                point_value = 10;
                time_before_shrink = 1.5f;
                audio_source.pitch = Random.Range(0.9f, 1.1f);
                break;

            case 2: //Large
            default:
                scale = transform.localScale *= 2.1f;
                point_value = 5;
                time_before_shrink = 2f;
                audio_source.pitch = Random.Range(0.5f, 0.7f);
                break;
        }

        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Grow/Shrink Animation
        transform.localScale = Vector3.MoveTowards(transform.localScale, scale, 3.5f * Time.deltaTime);

        //Shrinking
        time_before_shrink -= Time.deltaTime;
        if(time_before_shrink <= 0)
        {
            scale = Vector3.zero;
        }

        //Destroy once fully shrunk.
        if(transform.localScale == Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}
