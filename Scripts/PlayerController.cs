using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject Path, Explosion, Cells, Confetti;
    public Transform Target;
    public Button shieldButton;
    public Material playerMat, shieldMat;
    public float speed;
    public bool shield = false;
    int curPos = 0;
    bool started = false;

    private void Start()
    {
        StartCoroutine(Wait(2));
    }

    void Update()
    {
        if(started) transform.position = Vector3.MoveTowards(transform.position, new Vector3(Path.GetComponent<GridBehavior>().path[curPos].transform.position.x, transform.position.y, Path.GetComponent<GridBehavior>().path[curPos].transform.position.z), Time.deltaTime * speed);
        if (curPos > 0)
        {
            if (transform.position == Path.GetComponent<GridBehavior>().path[curPos].transform.position) curPos--;
        }
        else
        {
            if (transform.position == new Vector3(Target.position.x, transform.position.y, Target.position.z))
            {
                Confetti.GetComponent<ParticleSystem>().Play();
                StartCoroutine(Restart());
            }
        }
    }

    public void Die()
    {
        Explosion.GetComponent<ParticleSystem>().Play();
        speed = 0;
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Restart());
        Cells.SetActive(true);
        for (int i = 0; i < Cells.transform.childCount; i++)
        {
            Cells.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void Shield()
    {
        StartCoroutine(ShieldEffect(2));
    }

    IEnumerator ShieldEffect(float time)
    {
        shield = true;
        GetComponent<MeshRenderer>().material = shieldMat;
        shieldButton.interactable = false;
        yield return new WaitForSeconds(time);
        GetComponent<MeshRenderer>().material = playerMat;
        shieldButton.interactable = true;
        shield = false;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Path.GetComponent<GridBehavior>().SetDistance();
        Path.GetComponent<GridBehavior>().SetPath();
        curPos = Path.GetComponent<GridBehavior>().path.Count - 1;
        started = true;
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
