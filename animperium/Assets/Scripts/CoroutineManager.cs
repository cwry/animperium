using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class CoroutineManager : MonoBehaviour {

    
    private Dictionary<string,List<Coroutine>> coroutinesByID;  //all coroutines saved as List with ID , Dictionary
    
    void Awake() {
        coroutinesByID = new Dictionary<string, List<Coroutine>>();
    }

    //starts and adds coroutine to the dictionary and creates entry if none is set
    public void Add (Coroutine coroutine, string ID){
        List<Coroutine> coroutineList;
        if (coroutinesByID.TryGetValue(ID, out coroutineList)){
            if(coroutineList == null){  //checks if entry exists
                List<Coroutine> newList = new List<Coroutine>();
                coroutinesByID.Add(ID, newList); //creates new entry for the ability
            }
        }
        coroutineList.Add(coroutine); //starts and adds coroutine
    }
    // Get CoroutineList by ID
    public List<Coroutine> GetCoroutinesByID(string ID){
        List<Coroutine> coroutineList;
        if(coroutinesByID.TryGetValue(ID, out coroutineList)){
            return coroutineList;
        }
        return null;
    }
    //kills coroutines by ID
    public void Kill(string ID){
        foreach(var entry in coroutinesByID){
            if (entry.Key == ID){
                entry.Value.ForEach(c => StopCoroutine(c));
                coroutinesByID.Remove(entry.Key);
            }
        }
    }
    //stops all coroutines
    public void Clear(){
        StopAllCoroutines();
        coroutinesByID.Clear();
    }
}
