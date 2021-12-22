using System.Collections.Generic;

public class Node
{
    public int index;
    public int active;
    
    public Node(int index, float value, float threshold0, float threshold1)
    {
        active = value >= threshold0 && value <= threshold1 ? 1 : 0;
        this.index = index;
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
    public void Triangulate(List<int> triangleList)
    {
        int config = bottomLeft.active * 1 + bottomRight.active * 2 + topLeft.active * 4 + topRight.active * 8;

        switch (config)
        {
            case 1:
                triangleList.Add(centerLeft.index);
                triangleList.Add(centerBot.index);
                triangleList.Add(centerLeft.index);
                break;

            case 2:
                triangleList.Add(centerBot.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(bottomRight.index);
                break;

            case 3:
                triangleList.Add(centerLeft.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(centerLeft.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(bottomRight.index);
                break;

            case 4:
                triangleList.Add(centerTop.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerRight.index);
                break;

            case 5:
                triangleList.Add(centerLeft.index);
                triangleList.Add(centerBot.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(centerLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerRight.index);
                break;

            case 6:
                triangleList.Add(centerTop.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(topRight.index);
                triangleList.Add(bottomRight.index);
                break;

            case 7:
                triangleList.Add(centerLeft.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(centerLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(bottomRight.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(topRight.index);
                triangleList.Add(bottomRight.index);
                break;

            case 8:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(centerLeft.index);
                break;

            case 9:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerBot.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(topLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(centerBot.index);
                break;

            case 10:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(centerLeft.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(centerBot.index);
                triangleList.Add(centerLeft.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(centerRight.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(centerBot.index);
                break;

            case 11:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerTop.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(centerTop.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(centerRight.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(bottomLeft.index);
                break;

            case 12:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(centerLeft.index);

                triangleList.Add(topLeft.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerRight.index);
                break;

            case 13:
                triangleList.Add(topLeft.index);
                triangleList.Add(centerBot.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(topLeft.index);
                triangleList.Add(centerRight.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(topLeft.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerRight.index);
                break;

            case 14:
                triangleList.Add(topLeft.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerLeft.index);

                triangleList.Add(centerLeft.index);
                triangleList.Add(topRight.index);
                triangleList.Add(centerBot.index);

                triangleList.Add(centerBot.index);
                triangleList.Add(topRight.index);
                triangleList.Add(bottomRight.index);
                break;

            case 15:
                triangleList.Add(topLeft.index);
                triangleList.Add(bottomRight.index);
                triangleList.Add(bottomLeft.index);

                triangleList.Add(topLeft.index);
                triangleList.Add(topRight.index);
                triangleList.Add(bottomRight.index);
                break;
        }
    }
}

