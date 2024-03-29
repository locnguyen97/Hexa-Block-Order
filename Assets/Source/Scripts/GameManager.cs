using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool canDrag = true;
    private int startIndex = 1;

    private int currentIndex;
    public bool isStartGame = false;
    [SerializeField] List<GameObject> particleVFXs;
    
    List<TouchPoint> listAllTouchPoint = new List<TouchPoint>();
    List<TouchPoint> listCollected = new List<TouchPoint>();
    private TouchPoint fistCollected;
    [SerializeField] private List<GameObject> listBg;
    [SerializeField] private List<Color> listLv1;
    [SerializeField] private List<Color> listLv2;
    [SerializeField] private List<Color> listLv3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        currentIndex = startIndex;
        
        RandomDataLevel();
        
    }

    void RandomDataLevel()
    {
        foreach (var bg in listBg)
        {
            bg.SetActive(false);
        }
        listBg[currentIndex-1].SetActive(true);
        listAllTouchPoint.Clear();
        listAllTouchPoint = FindObjectsOfType<TouchPoint>(false).ToList();
        if (currentIndex == 1)
        {
            foreach (var tp in listAllTouchPoint)
            {
                var x = listLv1[Random.Range(0, listLv1.Count)];
                tp.SetData(x);
            }
        }
        else if (currentIndex == 2)
        {
            foreach (var tp in listAllTouchPoint)
            {
                var x = listLv2[Random.Range(0, listLv2.Count)];
                tp.SetData(x);
            }
        }
        else
        {
            foreach (var tp in listAllTouchPoint)
            {
                var x = listLv3[Random.Range(0, listLv3.Count)];
                tp.SetData(x);
            }
        }
        canDrag = true;
        isStartGame = true;
        
    }

    void NextLevel()
    {
        currentIndex++;
        if (currentIndex > 3)
        {
            currentIndex = startIndex;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], transform.position, transform.rotation);
        Destroy(explosion, .75f);
        Invoke(nameof(RandomDataLevel),1.0f);
    }
    
    void Update()
    {
        if(!canDrag) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            listCollected.Clear();
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                var tp = targetObject.GetComponent<TouchPoint>();
                if (tp != null)
                {
                    fistCollected = tp;
                    tp.SetCollected();
                    listCollected.Add(tp);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                var tp = targetObject.GetComponent<TouchPoint>();
                if (tp != null)
                {
                    if (fistCollected == null)
                    {
                        fistCollected = tp;
                        tp.SetCollected();
                        listCollected.Add(tp);
                    }else if (tp.name == fistCollected.name)
                    {
                        tp.SetCollected();
                        listCollected.Add(tp);
                    }
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (listCollected.Count >0)
            {
                foreach (var tr in listCollected)
                {
                    tr.gameObject.SetActive(false);
                    listAllTouchPoint.Remove(tr);
                    GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], tr.transform.position, transform.rotation);
                    Destroy(explosion, .75f);
                    if (listAllTouchPoint.Count == 0)
                    {
                        Invoke(nameof(NextLevel),0.5f);
                    }
                }
            }
            fistCollected = null;
        }
    }
}