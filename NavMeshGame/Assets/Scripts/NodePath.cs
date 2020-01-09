using System.Collections;
using System.Collections.Generic;


public class NodePath 
{
    public List<Node> myPath = new List<Node>();
    public int minX, minY, maxX, maxY;

    public static List<NodePath> AllPaths = new List<NodePath>();

    private void addNode(Node node){
        if (myPath.Count <= 0){
            minX = node.x;
            minY = node.y;
            maxX = node.x;
            maxY = node.y;
        }else{
            if (node.x < minX) minX = node.x;
            if (node.x > maxX) maxX = node.x;
            if (node.y < minY) minY = node.y;
            if (node.y > maxY) maxY = node.y;
        }
        myPath.Add(node);
    }

    public void AddToPath(Node node){
        
        if(!myPath.Contains(node)){
            List<NodePath> containedPaths = new List<NodePath>();
            for (int i = 0; i < AllPaths.Count; i += 1){
                if(AllPaths[i].myPath.Contains(node)){
                    containedPaths.Add(AllPaths[i]);
                }
            }
            if (containedPaths.Count == 0) addNode(node);
            else{
                foreach(NodePath containedPath in containedPaths){
                    MergePaths(this, containedPath);
                }
            }
        }
    }

    public static void MergePaths(NodePath path1, NodePath path2){
        AllPaths.Remove(path2);
        foreach(Node node in path2.myPath){
            if (!path1.myPath.Contains(node)) path1.addNode(node);
        }
    }

    public static NodePath GetPath(Node node){

        foreach(NodePath path in AllPaths){
            if (path.myPath.Contains(node)) return path;
        }
        NodePath newPath = new NodePath();
        newPath.addNode(node);
        AllPaths.Add(newPath);
        return newPath;
    }
}
