using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node
{
    public Vector3 position;
    public Vector2 uv;
    public int active;
    
    public Node(Vector3 position, Vector2 uv, float value, int threshold)
    {
        active = value >= threshold? 1 : 0;
        this.position = position;
        this.uv = uv;
    }
}

public class Square
{
    Node centerLeft, centerTop, centerRight, centerBot, topLeft, topRight, bottomRight, bottomLeft;

    public Square(Node centerLeft, Node centerTop, Node centerRight, 
        Node centerBot, Node topLeft, Node topRight, Node bottomRight, Node bottomLeft)
    {
        this.centerLeft = centerLeft;
        this.centerTop = centerTop;
        this.centerRight = centerRight;
        this.centerBot = centerBot;
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomRight = bottomRight;
        this.bottomLeft = bottomLeft;
    }

    // triangulate update the triangle arr and return new triangle index
    public int March(List<Vector3> vertexList, List<Vector2> uvList, int vertexCount)
    {
        int config = bottomLeft.active * 1 + bottomRight.active * 2 + topRight.active * 4 + topLeft.active * 8;

        switch (config)
        {
            case 0:
                // nothing created, nothing added
                return vertexCount;
            
            case 15:

                vertexList.Add(topLeft.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(bottomLeft.position);

                vertexList.Add(topLeft.position);
                vertexList.Add(topRight.position);
                vertexList.Add(bottomRight.position);
                
                uvList.Add(topLeft.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(topLeft.uv);
                uvList.Add(topRight.uv);
                uvList.Add(bottomRight.uv);

                return vertexCount + 6;
            
            case 1:
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(centerLeft.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                return vertexCount + 3;

            case 2:
                vertexList.Add(centerBot.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomRight.position);

                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomRight.uv);
                return vertexCount + 3;

            case 3:
                vertexList.Add(centerLeft.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomRight.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomRight.uv);
                return vertexCount + 6;

            case 4:
                vertexList.Add(centerTop.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerRight.position);

                uvList.Add(centerTop.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerRight.uv);
                return vertexCount + 3;

            case 5:
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerRight.position);

                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerRight.uv);
                return vertexCount + 12;

            case 6:
                vertexList.Add(centerTop.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(topRight.position);
                vertexList.Add(bottomRight.position);

                uvList.Add(centerTop.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(topRight.uv);
                uvList.Add(bottomRight.uv);
                return vertexCount + 6;

            case 7:
                vertexList.Add(centerLeft.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(bottomRight.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(topRight.position);
                vertexList.Add(bottomRight.position);

                uvList.Add(centerLeft.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(topRight.uv);
                uvList.Add(bottomRight.uv);
                return vertexCount + 9;

            case 8:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(centerLeft.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                return vertexCount + 3;

            case 9:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(topLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(centerBot.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(topLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                return vertexCount + 6;

            case 10:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(centerLeft.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(centerLeft.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(centerBot.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(centerBot.uv);
                return vertexCount + 12;

            case 11:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomRight.position);
                vertexList.Add(bottomLeft.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomRight.uv);
                uvList.Add(bottomLeft.uv);
                return vertexCount + 9;

            case 12:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerLeft.position);
                
                vertexList.Add(topLeft.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerRight.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(topLeft.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerRight.uv);
                return vertexCount + 6;

            case 13:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(bottomLeft.position);
                
                vertexList.Add(topLeft.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(topLeft.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerRight.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(bottomLeft.uv);
                uvList.Add(topLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(topLeft.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerRight.uv);
                return vertexCount + 9;

            case 14:
                vertexList.Add(topLeft.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerLeft.position);
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerBot.position);
                
                vertexList.Add(centerBot.position);
                vertexList.Add(topRight.position);
                vertexList.Add(bottomRight.position);

                uvList.Add(topLeft.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(topRight.uv);
                uvList.Add(bottomRight.uv);
                return vertexCount + 9;
        }

        // shouldn't actually reach
        return vertexCount;
    }
}

