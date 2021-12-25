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
    public int March(Vector3[] vertexArray, Vector2[] uvArray, int index, float cliffHeight)
    {
        int config = bottomLeft.active * 1 + bottomRight.active * 2 + topRight.active * 4 + topLeft.active * 8;

        switch (config)
        {
            case 0:
                // nothing created, nothing added
                return index;
            
            case 15:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = bottomRight.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = topLeft.position;
                vertexArray[index + 4] = topRight.position;
                vertexArray[index + 5] = bottomRight.position;
                
                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = bottomRight.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = topLeft.uv;
                uvArray[index + 4] = topRight.uv;
                uvArray[index + 5] = bottomRight.uv;
                return index + 6;
            
            case 1:
                vertexArray[index]= bottomLeft.position;
                vertexArray[index + 1]= centerLeft.position;
                vertexArray[index + 2]= centerBot.position;
                
                uvArray[index]= bottomLeft.uv;
                uvArray[index + 1]= centerLeft.uv;
                uvArray[index + 2]= centerBot.uv;

                vertexArray[index + 3]= centerBot.position;
                vertexArray[index + 4] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 5]= centerBot.position * (1 - cliffHeight);
                vertexArray[index + 6]= centerBot.position;
                vertexArray[index + 7]= centerLeft.position;
                vertexArray[index + 8]= centerLeft.position * (1 - cliffHeight);
                
                uvArray[index + 3]= centerBot.uv;
                uvArray[index + 4]= centerLeft.uv;
                uvArray[index + 5]= centerBot.uv;
                uvArray[index + 6]= centerBot.uv;
                uvArray[index + 7]= centerLeft.uv;
                uvArray[index + 8]= centerLeft.uv;
                return index + 9;

            case 2:
                vertexArray[index] = centerBot.position;
                vertexArray[index + 1] = centerRight.position;
                vertexArray[index + 2] = bottomRight.position;
                
                uvArray[index] = centerBot.uv;
                uvArray[index + 1] = centerRight.uv;
                uvArray[index + 2] = bottomRight.uv;

                vertexArray[index + 3] = centerBot.position;
                vertexArray[index + 4] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 5] = centerRight.position;
                vertexArray[index + 6] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 7] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 8] = centerRight.position;
                
                uvArray[index + 3] = centerBot.uv;
                uvArray[index + 4] = centerBot.uv;
                uvArray[index + 5] = centerRight.uv;
                uvArray[index + 6] = centerBot.uv;
                uvArray[index + 7] = centerRight.uv;
                uvArray[index + 8] = centerRight.uv;
                return index + 9;

            case 3:
                vertexArray[index] = centerLeft.position;
                vertexArray[index + 1] = bottomRight.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = centerLeft.position;
                vertexArray[index + 4] = centerRight.position;
                vertexArray[index + 5] = bottomRight.position;
                
                uvArray[index] = centerLeft.uv;
                uvArray[index + 1] = bottomRight.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = centerLeft.uv;
                uvArray[index + 4] = centerRight.uv;
                uvArray[index + 5] = bottomRight.uv;
                
                vertexArray[index + 6] = centerLeft.position;
                vertexArray[index + 7] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 8] = centerRight.position;
                vertexArray[index + 9] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 10] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerRight.position;
                
                uvArray[index + 6] = centerLeft.uv;
                uvArray[index + 7] = centerLeft.uv;
                uvArray[index + 8] = centerRight.uv;
                uvArray[index + 9] = centerLeft.uv;
                uvArray[index + 10] = centerRight.uv;
                uvArray[index + 11] = centerRight.uv;
                return index + 12;

            case 4:
                vertexArray[index] = centerTop.position;
                vertexArray[index + 1] = topRight.position;
                vertexArray[index + 2] = centerRight.position;

                uvArray[index] = centerTop.uv;
                uvArray[index + 1] = topRight.uv;
                uvArray[index + 2] = centerRight.uv;
                
                vertexArray[index + 3] = centerTop.position;
                vertexArray[index + 4] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 5] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = centerRight.position;
                vertexArray[index + 8] = centerRight.position * (1 - cliffHeight);
                
                uvArray[index + 3] = centerTop.uv;
                uvArray[index + 4] = centerRight.uv;
                uvArray[index + 5] = centerTop.uv;
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = centerRight.uv;
                uvArray[index + 8] = centerRight.uv;
                return index + 9;

            case 5:
                vertexArray[index] = centerLeft.position;
                vertexArray[index + 1] = centerBot.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = centerLeft.position;
                vertexArray[index + 4] = centerTop.position;
                vertexArray[index + 5] = centerBot.position;
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = centerRight.position;
                vertexArray[index + 8] = centerBot.position;
                vertexArray[index + 9] = centerTop.position;
                vertexArray[index + 10] = topRight.position;
                vertexArray[index + 11] = centerRight.position;
                
                uvArray[index] = centerLeft.uv;
                uvArray[index + 1] = centerBot.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = centerLeft.uv;
                uvArray[index + 4] = centerTop.uv;
                uvArray[index + 5] = centerBot.uv;
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = centerRight.uv;
                uvArray[index + 8] = centerBot.uv;
                uvArray[index + 9] = centerTop.uv;
                uvArray[index + 10] = topRight.uv;
                uvArray[index + 11] = centerRight.uv;
                
                vertexArray[index + 12] = centerLeft.position;
                vertexArray[index + 13] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 14] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 15] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 16] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 17] = centerTop.position;
                vertexArray[index + 18] = centerRight.position;
                vertexArray[index + 19] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 20] = centerBot.position;
                vertexArray[index + 21] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 22] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 23] = centerBot.position;
                
                uvArray[index + 12] = centerLeft.uv;
                uvArray[index + 13] = centerLeft.uv;
                uvArray[index + 14] = centerTop.uv;
                uvArray[index + 15] = centerLeft.uv;
                uvArray[index + 16] = centerTop.uv;
                uvArray[index + 17] = centerTop.uv;
                uvArray[index + 18] = centerRight.uv;
                uvArray[index + 19] = centerRight.uv;
                uvArray[index + 20] = centerBot.uv;
                uvArray[index + 21] = centerRight.uv;
                uvArray[index + 22] = centerBot.uv;
                uvArray[index + 23] = centerBot.uv;
                return index + 24;

            case 6:
                vertexArray[index] = centerTop.position;
                vertexArray[index + 1] = bottomRight.position;
                vertexArray[index + 2] = centerBot.position;
                vertexArray[index + 3] = centerTop.position;
                vertexArray[index + 4] = topRight.position;
                vertexArray[index + 5] = bottomRight.position;

                uvArray[index] = centerTop.uv;
                uvArray[index + 1] = bottomRight.uv;
                uvArray[index + 2] = centerBot.uv;
                uvArray[index + 3] = centerTop.uv;
                uvArray[index + 4] = topRight.uv;
                uvArray[index + 5] = bottomRight.uv;
                
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 8] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 9] = centerBot.position;
                vertexArray[index + 10] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerTop.position;
                
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = centerBot.uv;
                uvArray[index + 8] = centerTop.uv;
                uvArray[index + 9] = centerBot.uv;
                uvArray[index + 10] = centerBot.uv;
                uvArray[index + 11] = centerTop.uv;
                return index + 12;

            case 7:
                vertexArray[index] = centerLeft.position;
                vertexArray[index + 1] = bottomRight.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = centerLeft.position;
                vertexArray[index + 4] = centerTop.position;
                vertexArray[index + 5] = bottomRight.position;
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = topRight.position;
                vertexArray[index + 8] = bottomRight.position;

                uvArray[index] = centerLeft.uv;
                uvArray[index + 1] = bottomRight.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = centerLeft.uv;
                uvArray[index + 4] = centerTop.uv;
                uvArray[index + 5] = bottomRight.uv;
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = topRight.uv;
                uvArray[index + 8] = bottomRight.uv;
                
                vertexArray[index + 9] = centerLeft.position;
                vertexArray[index + 10] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerTop.position;
                vertexArray[index + 12] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 13] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 14] = centerTop.position;
                
                uvArray[index + 9] = centerLeft.uv;
                uvArray[index + 10] = centerLeft.uv;
                uvArray[index + 11] = centerTop.uv;
                uvArray[index + 12] = centerLeft.uv;
                uvArray[index + 13] = centerTop.uv;
                uvArray[index + 14] = centerTop.uv;
                return index + 15;

            case 8:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerTop.position;
                vertexArray[index + 2] = centerLeft.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerTop.uv;
                uvArray[index + 2] = centerLeft.uv;
                
                vertexArray[index + 3] = centerTop.position;
                vertexArray[index + 4] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 5] = centerLeft.position;
                vertexArray[index + 6] = centerLeft.position;
                vertexArray[index + 7] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 8] = centerLeft.position * (1 - cliffHeight);
                
                uvArray[index + 3] = centerTop.uv;
                uvArray[index + 4] = centerTop.uv;
                uvArray[index + 5] = centerLeft.uv;
                uvArray[index + 6] = centerLeft.uv;
                uvArray[index + 7] = centerTop.uv;
                uvArray[index + 8] = centerLeft.uv;
                return index + 9;

            case 9:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerBot.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = topLeft.position;
                vertexArray[index + 4] = centerTop.position;
                vertexArray[index + 5] = centerBot.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerBot.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = topLeft.uv;
                uvArray[index + 4] = centerTop.uv;
                uvArray[index + 5] = centerBot.uv;
                
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = centerBot.position *  (1 - cliffHeight);
                vertexArray[index + 8] = centerBot.position;
                vertexArray[index + 9] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 10] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerTop.position;
                
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = centerBot.uv;
                uvArray[index + 8] = centerBot.uv;
                uvArray[index + 9] = centerTop.uv;
                uvArray[index + 10] = centerBot.uv;
                uvArray[index + 11] = centerTop.uv;
                return index + 12;

            case 10:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerTop.position;
                vertexArray[index + 2] = centerLeft.position;
                vertexArray[index + 3] = centerTop.position;
                vertexArray[index + 4] = centerBot.position;
                vertexArray[index + 5] = centerLeft.position;
                vertexArray[index + 6] = centerTop.position;
                vertexArray[index + 7] = centerRight.position;
                vertexArray[index + 8] = centerBot.position;
                vertexArray[index + 9] = centerRight.position;
                vertexArray[index + 10] = bottomRight.position;
                vertexArray[index + 11] = centerBot.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerTop.uv;
                uvArray[index + 2] = centerLeft.uv;
                uvArray[index + 3] = centerTop.uv;
                uvArray[index + 4] = centerBot.uv;
                uvArray[index + 5] = centerLeft.uv;
                uvArray[index + 6] = centerTop.uv;
                uvArray[index + 7] = centerRight.uv;
                uvArray[index + 8] = centerBot.uv;
                uvArray[index + 9] = centerRight.uv;
                uvArray[index + 10] = bottomRight.uv;
                uvArray[index + 11] = centerBot.uv;
                
                vertexArray[index + 12] = centerLeft.position;
                vertexArray[index + 13] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 14] = centerLeft.position * (1 - cliffHeight);
                vertexArray[index + 15] = centerLeft.position;
                vertexArray[index + 16] = centerBot.position;
                vertexArray[index + 17] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 18] = centerTop.position;
                vertexArray[index + 19] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 20] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 21] = centerTop.position;
                vertexArray[index + 22] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 23] = centerRight.position;
                
                uvArray[index + 12] = centerLeft.uv;
                uvArray[index + 13] = centerBot.uv;
                uvArray[index + 14] = centerLeft.uv;
                uvArray[index + 15] = centerLeft.uv;
                uvArray[index + 16] = centerBot.uv;
                uvArray[index + 17] = centerBot.uv;
                uvArray[index + 18] = centerTop.uv;
                uvArray[index + 19] = centerTop.uv;
                uvArray[index + 20] = centerRight.uv;
                uvArray[index + 21] = centerTop.uv;
                uvArray[index + 22] = centerRight.uv;
                uvArray[index + 23] = centerRight.uv;
                return index + 24;

            case 11:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerTop.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = centerTop.position;
                vertexArray[index + 4] = centerRight.position;
                vertexArray[index + 5] = bottomLeft.position;
                vertexArray[index + 6] = centerRight.position;
                vertexArray[index + 7] = bottomRight.position;
                vertexArray[index + 8] = bottomLeft.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerTop.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = centerTop.uv;
                uvArray[index + 4] = centerRight.uv;
                uvArray[index + 5] = bottomLeft.uv;
                uvArray[index + 6] = centerRight.uv;
                uvArray[index + 7] = bottomRight.uv;
                uvArray[index + 8] = bottomLeft.uv;
                
                vertexArray[index + 9] = centerTop.position;
                vertexArray[index + 10] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerRight.position;
                vertexArray[index + 12] = centerTop.position * (1 - cliffHeight);
                vertexArray[index + 13] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 14] = centerRight.position;
                
                uvArray[index + 9] = centerTop.uv;
                uvArray[index + 10] = centerTop.uv;
                uvArray[index + 11] = centerRight.uv;
                uvArray[index + 12] = centerTop.uv;
                uvArray[index + 13] = centerRight.uv;
                uvArray[index + 14] = centerTop.uv;
                return index + 15;

            case 12:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerRight.position;
                vertexArray[index + 2] = centerLeft.position;
                vertexArray[index + 3] = topLeft.position;
                vertexArray[index + 4] = topRight.position;
                vertexArray[index + 5] = centerRight.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerRight.uv;
                uvArray[index + 2] = centerLeft.uv;
                uvArray[index + 3] = topLeft.uv;
                uvArray[index + 4] = topRight.uv;
                uvArray[index + 5] = centerRight.uv;
                
                vertexArray[index + 6] = centerRight.position;
                vertexArray[index + 7] = centerLeft.position *  (1 - cliffHeight);
                vertexArray[index + 8] = centerLeft.position;
                vertexArray[index + 9] = centerRight.position;
                vertexArray[index + 10] = centerRight.position *  (1 - cliffHeight);
                vertexArray[index + 11] = centerLeft.position *  (1 - cliffHeight);
                
                uvArray[index + 6] = centerRight.uv;
                uvArray[index + 7] = centerLeft.uv;
                uvArray[index + 8] = centerLeft.uv;
                uvArray[index + 9] = centerRight.uv;
                uvArray[index + 10] = centerRight.uv;
                uvArray[index + 11] = centerLeft.uv;
                return index + 12;

            case 13:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = centerBot.position;
                vertexArray[index + 2] = bottomLeft.position;
                vertexArray[index + 3] = topLeft.position;
                vertexArray[index + 4] = centerRight.position;
                vertexArray[index + 5] = centerBot.position;
                vertexArray[index + 6] = topLeft.position;
                vertexArray[index + 7] = topRight.position;
                vertexArray[index + 8] = centerRight.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = centerBot.uv;
                uvArray[index + 2] = bottomLeft.uv;
                uvArray[index + 3] = topLeft.uv;
                uvArray[index + 4] = centerRight.uv;
                uvArray[index + 5] = centerBot.uv;
                uvArray[index + 6] = topLeft.uv;
                uvArray[index + 7] = topRight.uv;
                uvArray[index + 8] = centerRight.uv;
                
                vertexArray[index + 9] = centerRight.position;
                vertexArray[index + 10] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 11] = centerBot.position;
                vertexArray[index + 12] = centerRight.position * (1 - cliffHeight);
                vertexArray[index + 13] = centerBot.position * (1 - cliffHeight);
                vertexArray[index + 14] = centerBot.position;
                
                uvArray[index + 9] = centerRight.uv;
                uvArray[index + 10] = centerRight.uv;
                uvArray[index + 11] = centerBot.uv;
                uvArray[index + 12] = centerRight.uv;
                uvArray[index + 13] = centerBot.uv;
                uvArray[index + 14] = centerBot.uv;
                return index + 15;

            case 14:
                vertexArray[index] = topLeft.position;
                vertexArray[index + 1] = topRight.position;
                vertexArray[index + 2] = centerLeft.position;
                vertexArray[index + 3] = centerLeft.position;
                vertexArray[index + 4] = topRight.position;
                vertexArray[index + 5] = centerBot.position;
                vertexArray[index + 6] = centerBot.position;
                vertexArray[index + 7] = topRight.position;
                vertexArray[index + 8] = bottomRight.position;

                uvArray[index] = topLeft.uv;
                uvArray[index + 1] = topRight.uv;
                uvArray[index + 2] = centerLeft.uv;
                uvArray[index + 3] = centerLeft.uv;
                uvArray[index + 4] = topRight.uv;
                uvArray[index + 5] = centerBot.uv;
                uvArray[index + 6] = centerBot.uv;
                uvArray[index + 7] = topRight.uv;
                uvArray[index + 8] = bottomRight.uv;
                
                vertexArray[index + 9] = centerLeft.position;
                vertexArray[index + 10] = centerBot.position *  (1 - cliffHeight);
                vertexArray[index + 11] = centerLeft.position *  (1 - cliffHeight);
                vertexArray[index + 12] = centerLeft.position;
                vertexArray[index + 13] = centerBot.position;
                vertexArray[index + 14] = centerBot.position *  (1 - cliffHeight);
                
                uvArray[index + 9] = centerLeft.uv;
                uvArray[index + 10] = centerBot.uv;
                uvArray[index + 11] = centerLeft.uv;
                uvArray[index + 12] = centerLeft.uv;
                uvArray[index + 13] = centerBot.uv;
                uvArray[index + 14] = centerBot.uv;
                return index + 15;
        }

        throw new UnityException("Marching Square configuration not right");
    }
}

