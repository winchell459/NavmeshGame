using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int width = 18;
    public int height = 18;

    public GameObject wall;
    public GameObject player;
    public GameObject diamond;

    public int DiamondCount = 3;

    private bool playerSpawned = false;
    public NavMeshSurface NavMesh;

    private bool[,] blocks;// = new bool[(width + 2) / 2, (height + 2) / 2];

    private Node[,] nodesGrid;
    private List<Node> nodes = new List<Node>();
    private List<Node> diamonds = new List<Node>();
    private List<Node> walls = new List<Node>();
    private Node playerNode;

    public enum MazeTypes
    {
        Regular,
        Minimal,
        Paths
    }
    public MazeTypes MyType = MazeTypes.Regular;
    public bool IsAddObjectsLast = true;

    // Use this for initialization
    void Start()
    {
        blocks = new bool[(width + 2) / 2, (height + 2) / 2];
        nodesGrid = new Node[(width + 2) / 2, (height + 2) / 2];
        GenerateLevel();
        NavMesh.BuildNavMesh();
    }

    private int lastDiamond = 0;
    public int diamondDelay = 10;

    public int PlayerDelay = 10;
    // Create a grid based level
    void GenerateLevel()
    {
        // Loop over the grid
        for (int x = 0; x * 2 <= width; x += 1)
        {
            for (int y = 0; y * 2 <= height; y += 1)
            {

                blocks[x, y] = false;

                Node node = new Node(x, y);
                nodesGrid[x, y] = node;
                nodes.Add(node);

                // Should we place a wall?
                float chance = Random.value;
                if (MyType == MazeTypes.Minimal)
                {
                    if (x > 0 && y > 0)
                    {
                        if (blocks[x - 1, y] || blocks[x, y - 1]) chance = 0;
                    }
                    else if (x > 0)
                    {
                        if (blocks[x - 1, y]) chance = 0;
                    }
                    else if (y > 0)
                    {
                        if (blocks[x, y - 1]) chance = 0;
                    }
                }

                //place nodes based on chance
                if (chance > .7f)
                {
                    // Spawn a wall
                    Vector3 pos = new Vector3(x * 2 - width / 2f, 0.5f, y * 2 - height / 2f);
                    Instantiate(wall, pos, Quaternion.identity);
                    blocks[x, y] = true;
                    node.Contents = Node.Contains.Wall;
                    walls.Add(node);
                }
                else if (!IsAddObjectsLast && chance < 0.3f && DiamondCount > 0 && checkDiamond())
                {
                    addDiamond(node);
                }
                else if (!IsAddObjectsLast && !playerSpawned && checkPlayer()) // Should we spawn a player?
                {
                    // Spawn the player
                    addPlayer(node);
                }

                if (node.Contents != Node.Contains.Wall)
                {
                    if (x > 0)
                    {
                        if (nodesGrid[x - 1, y].addConnection(node))
                        {
                            node.addConnection(nodesGrid[x - 1, y]);
                        }
                    }
                    if (y > 0)
                    {
                        if (nodesGrid[x, y - 1].addConnection(node))
                        {
                            node.addConnection(nodesGrid[x, y - 1]);
                        }
                    }
                }
            }
        }

        findPaths();
        if (IsAddObjectsLast)
        {
            addObjectsLast();
        }
    }

    void addObjectsLast()
    {
        NodePath largestPath = NodePath.AllPaths[0];
        for (int i = 1; i < NodePath.AllPaths.Count; i += 1)
        {
            if (NodePath.AllPaths[i].myPath.Count > largestPath.myPath.Count)
            {
                largestPath = NodePath.AllPaths[i];
            }
        }


        while (!playerSpawned)
        {
            int playerNodeIndex = Random.Range(0, largestPath.myPath.Count - 1);
            addPlayer(largestPath.myPath[playerNodeIndex]);
        }

        while (DiamondCount > 0)
        {
            int nodeIndex = Random.Range(0, largestPath.myPath.Count - 1);
            if (largestPath.myPath[nodeIndex].Contents != Node.Contains.Player && largestPath.myPath[nodeIndex].Contents != Node.Contains.Diamond)
            {
                addDiamond(largestPath.myPath[nodeIndex]);
            }
        }

    }

    void findPaths()
    {
        foreach (Node node in nodes)
        {
            findPath(node);
        }
    }
    void findPath(Node node)
    {
        string nodeConnectionsString = "";
        if (node.Contents != Node.Contains.Wall)
        {
            NodePath pathOfNode = NodePath.GetPath(node);
            for (int i = 0; i < node.connectedNodes.Count; i += 1)
            {
                Node conNode = node.connectedNodes[i];
                nodeConnectionsString += "( " + conNode.x + ", " + conNode.y + ") ";
                pathOfNode.AddToPath(conNode);
            }

            if (node.connectedNodes.Count == 0) NodePath.GetPath(node);
        }

        Debug.Log(node.x + " " + node.y + " " + node.Contents.ToString() + nodeConnectionsString);
    }

    void addPlayer(Node node)
    {
        int x = node.x;
        int y = node.y;
        Vector3 pos = new Vector3(x * 2 - width / 2f, player.transform.position.y, y * 2 - height / 2f);
        //Instantiate(player, pos, Quaternion.identity);
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = pos;
        playerSpawned = true;
        //Debug.Log("Player pos set");

        node.Contents = Node.Contains.Player;
        playerNode = node;
    }

    void addDiamond(Node node)
    {
        int x = node.x;
        int y = node.y;
        Vector3 pos = new Vector3(x * 2 - width / 2f, 1.25f, y * 2 - height / 2f);
        Instantiate(diamond, pos, Quaternion.identity);
        DiamondCount -= 1;
        lastDiamond = 0;

        node.Contents = Node.Contains.Diamond;
        diamonds.Add(node);
    }

    bool checkDiamond()
    {
        lastDiamond += 1;
        if (lastDiamond > diamondDelay) return true;
        else return false;
    }

    bool checkPlayer()
    {
        PlayerDelay -= 1;
        if (PlayerDelay <= 0) return true;
        else return false;
    }

}