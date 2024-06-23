using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public Camera cam;

    //Spawning Dots
    public GameObject dot;
    public float time_between_spawns = 1f;
    private float dot_timer = 0.0f;

    //Score
    private int score = 0;
    public TMP_Text score_text;

    //Game Timer
    private float game_timer = 60f;
    public TMP_Text game_timer_text;

    //Restarting
    public GameObject restart_button;

    // Start is called before the first frame update
    void Start()
    {
        //Start Timer
        dot_timer = 0.5f;

        score_text.text = "Score: 0";

        restart_button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //***Game Timer***
        game_timer -= Time.deltaTime;
        if(game_timer <= 0)
        {
            game_timer = 0;
            game_timer_text.text = "Time: 0";
            restart_button.SetActive(true);
            return;
        }
        //game_timer_text.text = "Timer:" + game_timer.ToString();
        game_timer_text.text = $"Timer: {Mathf.Floor(game_timer).ToString()}";

        //***Clicking a Dot***
        if (Input.GetMouseButton(0))
        {
            Vector2 world_point = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(world_point, Vector2.zero);

            if(hit.collider != null && !hit.collider.gameObject.GetComponent<Dot>().audio_source.isPlaying)
            {
                hit.collider.gameObject.GetComponent<Dot>().audio_source.Play();

                hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                hit.collider.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                
                Destroy(hit.collider.gameObject, hit.collider.gameObject.GetComponent<Dot>().audio_source.clip.length);

                score += hit.collider.gameObject.GetComponent<Dot>().point_value;

                score_text.text = "Score: " + score.ToString();
            }
        }

        //***Spawnign Dots***//
        dot_timer -= Time.deltaTime;

        if(dot_timer <= 0)
        {
            dot_timer = time_between_spawns;

            SpawnDot();
        }

    }

    void SpawnDot()
    {
        GameObject new_dot = Instantiate(dot);

        int x_pos = Random.Range(0, cam.scaledPixelWidth);
        int y_pos = Random.Range(0, cam.scaledPixelHeight - 90);

        Vector3 spawn_point = new Vector3(x_pos, y_pos, 0);
        spawn_point = cam.ScreenToWorldPoint(spawn_point);
        spawn_point.z = 0;

        new_dot.transform.position = spawn_point;
    }

    public void RestartGame()
    {
        //Relaod the current scene, effectively restarting it.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
