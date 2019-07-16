using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class OrderedSelection : Editor
{
    private static List<UnityEngine.Object> selectHold = new List<UnityEngine.Object>(); // used as a unfiltered holder for selected objects
    private static List<GameObject> gameObjectHold = new List<GameObject>(); // list for selected game objects
    public static GameObject[] gameObjects { get; private set; } = null; // array vertion for gameObjectHold

    static OrderedSelection()
    {
        Selection.selectionChanged += selectionChange;// adds ordered selection to unitys build in selectChange function when created
    }

    static private void selectionChange()
    {
        updateSelection();
    }

    // updates the lists
    static private void updateSelection()
    {
        //if there is no selected object it crats a new empty list
        if (Selection.gameObjects.Length == 0)
        {
            selectHold = new List<UnityEngine.Object>();
        }
        //
        else
        {
            // for eatch object in selection.objects if it is not in select hold it is added.
            foreach (UnityEngine.Object i in Selection.objects)
            {
                if (!selectHold.Contains(i))
                {
                    selectHold.Add(i);
                }
            }
            //

            //or eatch object in select hold if it is not in slection.objects it is removed (it was unselected)
            foreach (UnityEngine.Object j in selectHold)
            {
                bool found = false;
                for (int k = 0; k < Selection.objects.Length; k++)
                {
                    if (j.Equals(Selection.objects[k]))
                    {
                        found = true;
                    }
                }

                if (found == false)
                {
                    selectHold.Remove(j);
                    selectionChange();
                    break;
                }
            }
            //
        }
        updateGameObjects();
    }
    //

    // used for updating the list that only have gameobjects in it 
    static private void updateGameObjects()
    {
        gameObjectHold = new List<GameObject>();
        foreach(GameObject i in GetFiltered(typeof(GameObject)))
        {
            gameObjectHold.Add(i);
        }
        gameObjects = gameObjectHold.ToArray();
    }
    //

    // basic contains function for bote instensID based and object based 
    static public bool Contains(int instanceID)
    {
        try
        {
            foreach (UnityEngine.Object i in selectHold)
            {
                if (i.GetInstanceID() == instanceID)
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    static public bool Contains(UnityEngine.Object obj)
    {
        try
        {
            foreach (UnityEngine.Object i in selectHold)
            {
                if (i.Equals(obj))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    //

    // returns a object array containing only obj of the given type
    static public UnityEngine.Object[] GetFiltered(Type type)
    {
        List<UnityEngine.Object> hold = new List<UnityEngine.Object>();

        foreach(UnityEngine.Object i in selectHold)
        {
            if (i.GetType().Equals(type))
            {
                hold.Add(i);
            }
        }

        return hold.ToArray();
    }
    //

}
