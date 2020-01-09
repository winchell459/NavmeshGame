using System.Collections;
using System.Collections.Generic;


public class Node 
{
    public int x, y;
    public enum Contains{
        Empty,
        Wall,
        Player,
        Diamond
    }
    public Contains Contents = Contains.Empty;
    public List<Node> connectedNodes = new List<Node>();

    public Node(int x, int y){
        this.x = x;
        this.y = y;
    }

    public bool addConnection(Node node){
        if(!connectedNodes.Contains(node) && Contents != Contains.Wall){
            connectedNodes.Add(node);
            return true;
        }else{
            return false;
        }
    }

}
