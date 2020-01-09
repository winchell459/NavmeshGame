using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera Cam;
    public NavMeshAgent Guy;
    public Transform GuyPrefab;

    public Transform GuySpawnPoint;

    public Text LivesText;
    public int Lives = 3;

    public NavMeshSurface Surface;

    public string NextScene = "Scene_01";

    // Start is called before the first frame update
    void Start()
    {
        Surface.BuildNavMesh();
        if (LivesText)LivesText.text = Lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0)){
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit)){
                if (Guy.enabled == false) Guy.enabled = true;
                Guy.SetDestination(hit.point);
            }
        }
    }

    private int diamondCount = 0;
    public void AddDiamond(){
        diamondCount += 1;

        if (diamondCount >= 3) loadNextScene();
    }

    private void loadNextScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
    }

    public void KillPlayer(){
        Destroy(Guy.gameObject);

        Lives -= 1;
        LivesText.text = Lives.ToString();

        if(Lives > 0){
            Guy = Instantiate(GuyPrefab, GuySpawnPoint.position, Quaternion.identity).GetComponent<NavMeshAgent>();
        }else{
            //FindObjectOfType<LevelHandler>().GameOver();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

    }


}
