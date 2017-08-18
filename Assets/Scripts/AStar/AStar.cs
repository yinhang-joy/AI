using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private const int mapWith = 15;
    private const int mapHeight = 15;

    private Point[,] map = new Point[mapWith, mapHeight];

    // Use this for initialization
    void Start()
    {
        InitMap();  //初始化地图
        Point start = map[2, 2];
        Point end = map[6, 2];
        FindPath(start, end);
        ShowPath(start, end);
    }


    private void ShowPath(Point start, Point end)
    {
        int z = -1;
        Point temp = end;
        while (true)
        {
            //Debug.Log(temp.X + "," + temp.Y);
            Color c = Color.gray;
            if (temp == start)
            {
                c = Color.green;
            }
            else if (temp == end)
            {
                c = Color.red;
            }
            CreateCube(temp.X, temp.Y,z, c);

            if (temp.Parent == null)
                break;
            temp = temp.Parent;
        }
        for (int x = 0; x < mapWith; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y].IsWall)
                {
                    CreateCube(x, y,z, Color.blue);
                }
            }
        }
    }

    private void CreateCube(int x, int y,int z, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = x+","+y;
        go.transform.position = new Vector3(x, y, z);
        go.GetComponent<Renderer>().material.color = color;
    }

    private void InitMap()
    {
        for (int x = 0; x < mapWith; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = new Point(x, y);
                CreateCube(x, y,0, Color.black);
            }
        }
        map[4, 1].IsWall = true;
        map[4, 2].IsWall = true;
        map[4, 3].IsWall = true;
        map[4, 4].IsWall = true;
        map[4, 5].IsWall = true;
        map[4, 6].IsWall = true;
    }
    /// <summary>
    /// 查找最优路径
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void FindPath(Point start, Point end)
    {
        List<Point> openList = new List<Point>();
        List<Point> closeList = new List<Point>();
        openList.Add(start);    //将开始位置添加进Open列表
        while (openList.Count > 0)//查找退出条件
        {
            Point point = FindMinFOfPoint(openList);//查找Open列表中最小的f值
            print(point.F + ";" + point.X + "," + point.Y);
            openList.Remove(point); closeList.Add(point);//不再考虑当前节点

            List<Point> surroundPoints = GetSurroundPoints(point);//得到当前节点的四周8个节点
            PointsFilter(surroundPoints, closeList);//将周围节点中已经添加进Close列表中的节点移除
            foreach (Point surroundPoint in surroundPoints)
            {
                if (openList.IndexOf(surroundPoint) > -1)//如果周围节点在open列表中
                {
                    float nowG = CalcG(surroundPoint, surroundPoint.Parent);//计算经过的Open列表中最小f值到周围节点的G值
                    if (nowG < surroundPoint.G)
                    {
                        print("123");
                        surroundPoint.UpdateParent(point, nowG);
                    }
                }
                else//周围节点不在Open列表中
                {
                    surroundPoint.Parent = point;//设置周围列表的父节点
                    CalcF(surroundPoint, end);//计算周围节点的F，G,H值
                    openList.Add(surroundPoint);//最后将周围节点添加进Open列表
                }
            }
            //判断一下
            if (openList.IndexOf(end) > -1)
            {
                break;
            }
        }

    }

    private void PointsFilter(List<Point> src, List<Point> closeList)
    {
        foreach (Point p in closeList)
        {
            if (src.IndexOf(p) > -1)
            {
                src.Remove(p);
            }
        }
    }

    private List<Point> GetSurroundPoints(Point point)
    {
        Point up = null, down = null, left = null, right = null;
        Point lu = null, ru = null, ld = null, rd = null;
        if (point.Y < mapHeight - 1)
        {
            up = map[point.X, point.Y + 1];
        }
        if (point.Y > 0)
        {
            down = map[point.X, point.Y - 1];
        }
        if (point.X > 0)
        {
            left = map[point.X - 1, point.Y];
        }
        if (point.X < mapWith - 1)
        {
            right = map[point.X + 1, point.Y];
        }
        if (up != null && left != null)
        {
            lu = map[point.X - 1, point.Y + 1];
        }
        if (up != null && right != null)
        {
            ru = map[point.X + 1, point.Y + 1];
        }
        if (down != null && left != null)
        {
            ld = map[point.X - 1, point.Y - 1];
        }
        if (down != null && right != null)
        {
            rd = map[point.X + 1, point.Y - 1];
        }
        List<Point> list = new List<Point>();
        if (down != null && down.IsWall == false)
        {
            list.Add(down);
        }
        if (up != null && up.IsWall == false)
        {
            list.Add(up);
        }
        if (left != null && left.IsWall == false)
        {
            list.Add(left);
        }
        if (right != null && right.IsWall == false)
        {
            list.Add(right);
        }
        if (lu != null && lu.IsWall == false && left.IsWall == false && up.IsWall == false)
        {
            list.Add(lu);
        }
        if (ld != null && ld.IsWall == false && left.IsWall == false && down.IsWall == false)
        {
            list.Add(ld);
        }
        if (ru != null && ru.IsWall == false && right.IsWall == false && up.IsWall == false)
        {
            list.Add(ru);
        }
        if (rd != null && rd.IsWall == false && right.IsWall == false && down.IsWall == false)
        {
            list.Add(rd);
        }
        return list;
    }

    private Point FindMinFOfPoint(List<Point> openList)
    {
        float f = float.MaxValue;
        Point temp = null;
        foreach (Point p in openList)
        {
            if (p.F < f)
            {
                temp = p;
                f = p.F;
            }
        }
        print("返回open列表中最小的f:"+temp.F);
        return temp;
    }

    private float CalcG(Point now, Point parent)
    {
        return Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(parent.X, parent.Y)) + parent.G;
    }

    private void CalcF(Point now, Point end)
    {
        //F = G + H
        float h = Mathf.Abs(end.X - now.X) + Mathf.Abs(end.Y - now.Y);
        float g = 0;
        if (now.Parent == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(now.Parent.X, now.Parent.Y)) + now.Parent.G;
        }
        float f = g + h;
        now.F = f;
        now.G = g;
        now.H = h;
    }
}
