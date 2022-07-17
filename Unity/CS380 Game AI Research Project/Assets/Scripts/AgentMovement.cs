using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Comparator for sorting open list

//This Class will move the player from the start to the end points on the grid
public class AgentMovement : MonoBehaviour
{
    [SerializeField] GameObject agent = null; //might not need?
    private MapPos start;
    private MapPos end;
    private MapPos curr;
    private List<MapPos> boardOld = new();
    private List<MapPos> board = new();

    private bool boardChanged = true;

    private bool keyPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        //agent.transform.position = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (boardChanged)
        {
            agent.transform.position = MapManager.Instance.GetTileWorldPos(start);
            curr = start;
            boardChanged = false;
        }
        else
        {
            if ((keyPressed == true && Input.GetKeyUp(KeyCode.D)) ||
                (keyPressed == true && Input.GetKeyUp(KeyCode.A)) ||
                (keyPressed == true && Input.GetKeyUp(KeyCode.W)) ||
                (keyPressed == true && Input.GetKeyUp(KeyCode.S)))
            {
                keyPressed = false;
            }

            //check for keyboard inputs
            if (Input.GetKeyDown(KeyCode.D) && keyPressed == false)
            {
                keyPressed = true;
                MapPos right = curr;
                right.x += 1;
                int index = findNodeInList(board, right); //this checks if valid position to move to
                if (index != -1)
                {
                    curr = right;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && keyPressed == false)
            {
                keyPressed = true;
                MapPos left = curr;
                left.x -= 1;
                int index = findNodeInList(board, left); //this checks if valid position to move to
                if (index != -1)
                {
                    curr = left;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W) && keyPressed == false)
            {
                keyPressed = true;
                MapPos up = curr;
                up.y += 1;
                int index = findNodeInList(board, up); //this checks if valid position to move to
                if (index != -1)
                {
                    curr = up;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && keyPressed == false)
            {
                keyPressed = true;
                MapPos down = curr;
                down.y -= 1;
                int index = findNodeInList(board, down); //this checks if valid position to move to
                if (index != -1)
                {
                    curr = down;
                }
            }

            agent.transform.position = MapManager.Instance.GetTileWorldPos(curr);

        }

    }



    public void FindEnds(ref MapData<bool> map)
    {
        board.Clear();
        //first to appear going front to back
        for (int i = 0; i < map.w * map.h; ++i)
        {
            MapPos gPos = new(i % map.w, i / map.w);
            if (map.GetPos(gPos.x, gPos.y) == true)
            {
                board.Add(gPos);
            }
        }

        if(board.Count > 0)
        {
            int size = board.Count - 1;
            if (size < 0) size = 0;
            start = board[0];
            end = board[size];

        }
        MapManager.Instance.WriteColor(start, Color.red);
        MapManager.Instance.WriteColor(end, Color.red);

        if (Enumerable.SequenceEqual(board, boardOld) == false)
        {
            boardChanged = true;
            boardOld = board.ToList<MapPos>();
        }


    }

    //true if node is in list, else false
    private int findNodeInList(List<MapPos> list, MapPos pos)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (pos.x == list[i].x && pos.y == list[i].y)
            {
                return i;
            }
        }

        return -1;
    }
}
