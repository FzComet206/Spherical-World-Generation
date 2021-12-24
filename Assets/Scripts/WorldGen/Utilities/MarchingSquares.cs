using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public Vector2 uv;
    public int active;
    
    public Node(Vector3 position, Vector2 uv, float value, int threshold0, int threshold1)
    {
        active = (value >= threshold0 && value <= threshold1)? 1 : 0;
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
    public void March(List<Vector3> vertexList, List<Vector2> uvList, float cliffHeight)
    {
        int config = bottomLeft.active * 1 + bottomRight.active * 2 + topRight.active * 4 + topLeft.active * 8;

        switch (config)
        {
            case 0:
                // nothing created, nothing added
                break;
            
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
                break;
            
            case 1:
                vertexList.Add(bottomLeft.position);
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position);
                
                uvList.Add(bottomLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);

                vertexList.Add(centerBot.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                break;

            case 2:
                vertexList.Add(centerBot.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(bottomRight.position);
                
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(bottomRight.uv);

                vertexList.Add(centerBot.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                break;

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
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                break;

            case 4:
                vertexList.Add(centerTop.position);
                vertexList.Add(topRight.position);
                vertexList.Add(centerRight.position);

                uvList.Add(centerTop.uv);
                uvList.Add(topRight.uv);
                uvList.Add(centerRight.uv);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                break;

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
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                break;

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
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                break;

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
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                break;

            case 8:
                vertexList.Add(topLeft.position);
                vertexList.Add(centerTop.position);
                vertexList.Add(centerLeft.position);

                uvList.Add(topLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerLeft.uv);
                break;

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
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerTop.uv);
                break;

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
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerTop.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                break;

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
                
                vertexList.Add(centerTop.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                vertexList.Add(centerTop.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerRight.position);
                
                uvList.Add(centerTop.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerTop.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerTop.uv);
                
                break;

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
                
                vertexList.Add(centerRight.position);
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position);
                
                vertexList.Add(centerRight.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                
                uvList.Add(centerRight.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerLeft.uv);
                break;

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
                
                vertexList.Add(centerRight.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                vertexList.Add(centerRight.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerBot.position);
                
                uvList.Add(centerRight.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerRight.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                break;

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
                
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position * (1 - cliffHeight));
                vertexList.Add(centerLeft.position);
                vertexList.Add(centerBot.position);
                vertexList.Add(centerBot.position * (1 - cliffHeight));
                
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerLeft.uv);
                uvList.Add(centerBot.uv);
                uvList.Add(centerBot.uv);
                break;
        }
    }
}

