using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberynthScript : MonoBehaviour
{
    [SerializeField] private GameObject cubeObj;
    [SerializeField] private List<TileLaberinth> _objList = new List<TileLaberinth>();
    [SerializeField] private List<TileLaberinth> finalCandidatesList = new List<TileLaberinth>();
    private List<TileLaberinth> _currentlyVisitedList = new List<TileLaberinth>();
    private TileLaberinth _finalTileLaberinth;
    private void Start()
    {
        CreateMaze(99);
    }

    private void CreateMaze(int max)
    {
        GenerateGrid(max);
        ChooseFinalTarget();
        GeneratePath(max);
    }

    private void GenerateGrid(int max)
    {
        for (int x = 0; x < max; x++)
        {
            for (int y = 0;  y < max; y++)
            {
                GameObject go = Instantiate(cubeObj, transform.position + new Vector3(x, 0, y), Quaternion.identity);
                TileLaberinth newTileLaberinth = new TileLaberinth();
                newTileLaberinth.obj = go;
                newTileLaberinth.dir = Directions.NORTH;
                newTileLaberinth.xCoords = x;
                newTileLaberinth.yCoords = y;
                go.name = x + "/" + y;
                _objList.Add(newTileLaberinth);

                if(y == 0 || x == 0)
                {
                    if (!finalCandidatesList.Contains(newTileLaberinth))
                    {
                        finalCandidatesList.Add(newTileLaberinth);
                    }
                }
            }
        }
    }


    private void ChooseFinalTarget()
    {

        int rand = Random.Range(0, finalCandidatesList.Count - 1);

        TileLaberinth _newTile = finalCandidatesList[rand];

        _newTile.obj.name += "FINAL";

        finalCandidatesList[rand] = _newTile;

        _finalTileLaberinth = finalCandidatesList[rand];
    }
    private void GeneratePath(int max)
    {
        _objList[_objList.Count-1].obj.SetActive(false);
        TileLaberinth _currentTileLaberynth = _objList[_objList.Count - 1];

        _currentTileLaberynth.isVisitedBool = true;
        while (!isChangedEndCoords(_currentTileLaberynth.xCoords, _currentTileLaberynth.yCoords))
        {

            GoToThePath(_currentTileLaberynth.xCoords, _currentTileLaberynth.yCoords , max, ref _currentTileLaberynth);

            if(_currentTileLaberynth.obj == null)
            {
                _currentTileLaberynth = ChooseRandomPathCloseToTheExit(_finalTileLaberinth.xCoords, _finalTileLaberinth.yCoords);
            }
            else
            {

              
                    _currentTileLaberynth.isVisitedBool = true;
                    _currentTileLaberynth.obj.SetActive(false);
                    _currentlyVisitedList.Add(_currentTileLaberynth);
                
               
            }
        

        }


    }

    private bool isChangedEndCoords(int x, int y)
    {
        if (_finalTileLaberinth.xCoords == x && _finalTileLaberinth.yCoords == y) return true;
        return false;
    }

    private bool isUsedPathBool(int xCoords, int yCoords)
    {
        for (int i = 0; i < _currentlyVisitedList.Count; i++)
        {
            if (xCoords == _currentlyVisitedList[i].xCoords && yCoords == _currentlyVisitedList[i].yCoords)
                return true;
        }

        return false;
    }

   
    private TileLaberinth ChooseRandomPathCloseToTheExit(int finalXCoords, int finalYCoords)
    {
        float minDistance = 10000;
        TileLaberinth _currentTileLaberinth = new TileLaberinth();
        for (int i = 0; i < _currentlyVisitedList.Count; i++) 
        {
            float distance = Vector2.Distance(new Vector2(finalXCoords, finalYCoords), new Vector2(_currentlyVisitedList[i].xCoords, _currentlyVisitedList[i].yCoords) );
            if(distance < minDistance)
            {
                minDistance = distance;
                _currentTileLaberinth = _currentlyVisitedList[i];
            }
        }

        return _currentTileLaberinth;
    }
    private Directions GoToThePath(int x, int y, int max,  ref TileLaberinth getNeighBour)
    {
        while (true)
        {
            int random = Random.Range(1,5);

       //     Debug.Log("RAND " + random);
            switch (random)
            {
                case 1:
                    if (y > -1)
                    {
                        Vector2 coords = new Vector2(x, y - 1);
                        getNeighBour = getTileCoords(x, y - 1);


                        if (!getNeighBour.isVisitedBool && y > 1|| isChangedEndCoords((int)coords.x, (int)coords.y))
                                return Directions.NORTH;

                    }

                        break;

                case 2:
                    if(y < max + 1)
                    {
                        Vector2 coords = new Vector2(x, y + 1);


                        getNeighBour = getTileCoords(x, y + 1);

                        if (!getNeighBour.isVisitedBool && y < max - 2 || isChangedEndCoords((int)coords.x, (int)coords.y))
                            return Directions.SOUTH;

                    }

                    break;
                    
                case 3:
                    if(x > -1)
                    {
                        Vector2 coords = new Vector2(x - 1, y);
                        getNeighBour = getTileCoords(x - 1, y);


                        if (!getNeighBour.isVisitedBool && x > 1 || isChangedEndCoords((int)coords.x, (int)coords.y))
                            return Directions.EAST;

                    }

                    break;

                case 4:
                    if(x < max + 1)
                    {

                        Vector2 coords = new Vector2(x + 1, y);


                        getNeighBour = getTileCoords(x + 1, y);



                        if(!getNeighBour.isVisitedBool && x < max - 2 || isChangedEndCoords((int)coords.x, (int)coords.y))
                            return Directions.WEST;

                    }

                    break;
            }
        }
    }

    public TileLaberinth getTileCoords(int x, int y)
    {
        
            for (int i = 0; i < _objList.Count; i++)
            {
                if (_objList[i].xCoords == x && _objList[i].yCoords == y && _objList[i].obj.activeSelf)
                {
                return _objList[i];
                }
            }


        return new TileLaberinth();
    }
}
public struct TileLaberinth
{
    public GameObject obj;
    public Directions dir;
    public int xCoords;
    public int yCoords;
    public bool isVisitedBool;
}
public enum Directions
{
    NORTH,
    SOUTH,
    WEST,
    EAST
}