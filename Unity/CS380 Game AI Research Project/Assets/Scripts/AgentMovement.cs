using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Comparator for sorting open list
class sorter : IComparer<MapPos>
{
    public int Compare(MapPos lhs, MapPos rhs)
    {
        if (lhs.cost == 0 || rhs.cost == 0)
        {
            return 0;
        }

        // CompareTo() method
        return lhs.cost.CompareTo(rhs.cost);

    }
}

//This Class will move the player from the start to the end points on the grid
public class AgentMovement : MonoBehaviour
{
    [SerializeField] GameObject agent = null; //might not need?
    private MapPos start;
    private MapPos end;
    private List<MapPos> board = new();
    private List<Vector3> optimalPath = new();

    private bool atDestination = false;

    //helpers for checking neighbors
    private int[] directionX = { 1, 0, -1, 0};
    private int[] directionY = { 0, -1, 0, 1};
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(start.x != end.x || start.y != end.y)
        {
            atDestination = false;
        }

        if (atDestination == false)
        {
            calcPath();
            StartCoroutine(MoveAlongPath());
        }

    }

    //simple function that slowly moves the player along the path
    IEnumerator MoveAlongPath()
    {
        for (int i = 0; i < optimalPath.Count; ++i)
        {
            yield return new WaitForSeconds(1);
            agent.transform.position = optimalPath[i];
            //now set player pos with this
        }
        atDestination = true;
    }

    public void FindEnds(ref MapData<bool> map)
    {
        //first to appear going front to back
        for (int i = 0; i < map.w * map.h; ++i)
        {
            MapPos gPos = new(i % map.w, i / map.w);
            if (map.GetPos(gPos.x, gPos.y) == true)
            {
                board.Add(gPos);
            }
        }

        start = board[0];
        end = board[board.Count];
    }

    public void calcPath()
    {   
        //set initial data
        start.cost = calcDistCost(start, end);
        MapManager.Instance.WriteColor(start, Color.green);
        MapManager.Instance.WriteColor(end, Color.red);

        List<MapPos> openList = new List<MapPos> { start };
        List<MapPos> closedList = new List<MapPos> { };
        sorter srt = new sorter();

        while(openList.Count > 0)
        {
            openList.Sort(srt);
            MapPos current = openList[0];
            openList.RemoveAt(0);

            if(current.x == end.x && current.y == end.y) //reached destination
            {
                //convert optimal path to world coordinates
                closedList.Insert(closedList.Count,current);
                closedList.Reverse();
                MapPos findParent = closedList[0];
                optimalPath.Add(MapManager.Instance.GetTileWorldPos(closedList[0]));
                foreach (var node in closedList)
                {
                    if (findParent.x == node.x && findParent.y == node.y)
                    {
                        findParent.x = node.parent_x;
                        findParent.y = node.parent_y;
                        optimalPath.Add(MapManager.Instance.GetTileWorldPos(node));
                    }
                }
            }

            //otherwise, we check neighbors
            for(int i = 0; i < 4; ++i)
            {
                MapPos neighbor = current;
                neighbor.x += directionX[i];
                neighbor.y += directionY[i];

                neighbor.parent_x = current.x;
                neighbor.parent_y = current.y;

                //cost will always be
                neighbor.given += 1.0f;
                float Hval = calcDistCost(neighbor, end);
                neighbor.cost = neighbor.given + Hval;

                int onOpen = findNodeInList(openList, neighbor);
                int onClose = findNodeInList(closedList, neighbor);
                int onIsland = findNodeInList(board, neighbor);

                if (onOpen == -1 && onClose == -1 && onIsland != -1) //add to openList
                {
                    openList.Add(neighbor);
                }
                else if (onOpen != -1 && neighbor.cost < openList[onOpen].cost) //open list replacement
                {
                    openList.RemoveAt(onOpen);
                    openList.Add(neighbor);
                }
                else if (onClose != -1 && neighbor.cost < closedList[onClose].cost) //closed list replacement
                {
                    closedList.RemoveAt(onClose);
                    closedList.Add(neighbor);
                }
            }

            //add parent to closed
            closedList.Add(current);
        }

    }

    private int calcDistCost(MapPos start, MapPos end)
    {
        return Mathf.Abs(end.y - start.y) + Mathf.Abs(end.x - start.x);
    }

    //true if node is in list, else false
    private int findNodeInList(List<MapPos> list, MapPos pos)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (pos.x == list[i].x && pos.y == list[i].x)
            {
                return i;
            }
        }

        return -1;
    }
}
