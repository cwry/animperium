using UnityEngine;
using System.Collections;

public class GridManager
{
    //following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    public GameObject container;
	public GameObject hex;
	//next two variables can also be instantiated using unity editor
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;
    public bool isMainGrid = true;
    public GameObject[,] gridData;

    //Hexagon tile width and height in game world
    private float hexWidth;
	private float hexHeight;

    public GridManager(GameObject hex, int gridWidthInHexes, int gridHeightInHexes, bool isMainGrid){
        container = new GameObject("grid container " + (isMainGrid ? "main" : "sub"));
        MapGridContainer c = container.AddComponent<MapGridContainer>();
        c.isMainGrid = isMainGrid;
        c.width = gridWidthInHexes;
        c.height = gridHeightInHexes;
        this.hex = hex;
        this.gridWidthInHexes = gridWidthInHexes;
        this.gridHeightInHexes = gridHeightInHexes;
        this.isMainGrid = isMainGrid;

        setSizes();
        createGrid();
    }

    public GridManager(GameObject container){
        this.container = container;
        MapGridContainer c = container.GetComponent<MapGridContainer>();
        this.gridWidthInHexes = c.width;
        this.gridHeightInHexes = c.height;
        this.isMainGrid = c.isMainGrid;
        TileInfo[] tiles = container.GetComponentsInChildren<TileInfo>();
        gridData = new GameObject[c.width, c.height];
        foreach (TileInfo t in tiles){
            t.gridPosition = new Vec2i(t.initGridX, t.initGridY);
            t.grid = this;
            gridData[t.initGridX, t.initGridY] = t.gameObject;
        }
    }

    public bool isInBounds(int x, int y) {
        return x >= 0 && x < gridWidthInHexes && y >= 0 && y < gridHeightInHexes;
    }

    //Method to initialise Hexagon width and height
    void setSizes(){
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth =  hex.GetComponent<Renderer>().bounds.size.x;
		hexHeight = hex.GetComponent<Renderer>().bounds.size.z;

	}

	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
			gridHeightInHexes / 2f * hexHeight - hexHeight / 2);

		return initPos;
	}

	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos) 
	{
        //Position of the first hex tile
        //Vector3 initPos = calcInitPos();
        Vector3 initPos = Vector3.zero;
		//Every second row is offset by half of the tile width
        float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;

		float x =  initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
	}

    //Finally the method which initialises and positions all the tiles
    void createGrid()
    {
        //Game object which is the parent of all the hex tiles
        gridData = new GameObject[gridWidthInHexes, gridHeightInHexes];

        for (int y = 0; y < gridHeightInHexes; y++)
		{
			for (int x = 0; x < gridWidthInHexes; x++)
			{
				//GameObject assigned to Hex public variable is cloned
             
				GameObject hexClone = (GameObject)UnityEngine.Object.Instantiate(hex);
                hexClone.name = "Hex (" + x + "|" + y + ")"; 
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hexClone.transform.position = calcWorldCoord(gridPos);
                hexClone.transform.parent = container.transform;

                gridData[x, y] = hexClone;
                TileInfo info = hexClone.AddComponent<TileInfo>();
                info.grid = this;
                info.gridPosition = new Vec2i(x, y);
                info.initGridY = y;
                info.initGridX = x;
			}
		}
    }
}