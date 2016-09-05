using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class World : MonoBehaviour {

    Subject<Damage> OnDamage;

    public IObservable<Damage> OnDamageAsObservable
    {
        get
        {
            return OnDamage ?? (OnDamage = new Subject<Damage>());
        }
    }

    public void SetDamageInfo(Damage damage)
    {
        if(OnDamage != null)
        {
            OnDamage.OnNext(damage);
        }
    }

 //   public GameObject[] WorldCells;

 //   public int CenterX;
 //   public int CenterY;

 //   public GameObject Hero;

 //   GameObject mHero;

 //   public Camera HeroCamera;

 //   Dictionary<string, GameObject> mCellMap = new Dictionary<string, GameObject>();

 //   // Use this for initialization
 //   void Start () {

 //      bool re = WorldCells[13].GetComponent<WorldCell>().CanBeConnect(WorldCells[9].GetComponent<WorldCell>(),1);

 //       Debug.Log(re);

 //       ChangeCenter(CenterX, CenterY);

 //       WorldCell cell = GetCell(CenterX, CenterY);

 //       mHero = Instantiate(Hero, new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;

 //       if(HeroCamera)
 //       {
 //           ABS0.SmoothFollow smoothFollow = HeroCamera.GetComponent<ABS0.SmoothFollow>() as ABS0.SmoothFollow;

 //           if(smoothFollow != null)
 //           {
 //               smoothFollow.target = mHero.transform;
 //           }
 //       }
 //   }
	
	//// Update is called once per frame
	//void Update () {
	
	//}

 //   void LateUpdate()
 //   {
 //       if(mHero)
 //       {
 //           int x = Mathf.FloorToInt(mHero.transform.position.x / 20.0f);
 //           int y = Mathf.FloorToInt(mHero.transform.position.z / 20.0f);
 //           if (x != CenterX || y != CenterY)
 //           {

 //               ChangeCenter(x, y);
 //           }
 //       }

 //   }

 //   public WorldCell GetCell(int centerX, int centerY)
 //   {
 //       GameObject cell;
 //       mCellMap.TryGetValue(centerX + "," + centerY, out cell);
 //       return cell.GetComponent<WorldCell>();
 //   }

 //   public void ChangeCenter(int centerX, int centerY)
 //   {
 //           CenterX = centerX;
 //           CenterY = centerY;

 //           Dictionary<string, GameObject> cellMap = new Dictionary<string, GameObject>();

 //           int sizeOfWoldCells = WorldCells.Length;

 //           for (int y = CenterY - 1; y <= CenterY + 1; y++)
 //           {
 //               for (int x = CenterX - 1; x <= CenterX + 1; x++)
 //               {
 //                   Vector3 position = new Vector3(x * 20, 0, y * 20);
 //                   string name = x + "," + y;
 //                   GameObject cell;
 //                   mCellMap.TryGetValue(name, out cell);
 //                   if(!cell)
 //                   {
 //                       bool result = true;
 //                       GameObject prefab = null;

 //                       for(int i = 0; i < 100; i++)
 //                       {
 //                           prefab = WorldCells[Random.Range(0, sizeOfWoldCells)];
 //                           WorldCell prefabCell = prefab.GetComponent<WorldCell>();

 //                           if ((x - 1) >= CenterX - 1)
 //                           {
 //                               GameObject pNode;
 //                               cellMap.TryGetValue((x - 1) + "," + y, out pNode);
 //                               if (pNode)
 //                               {
 //                                   result &= pNode.GetComponent<WorldCell>().CanBeConnect(prefabCell, 2);
 //                               }
 //                           }

 //                           if ((y - 1) >= CenterY - 1)
 //                           {
 //                               GameObject pNode;
 //                               cellMap.TryGetValue(x + "," + (y - 1), out pNode);
 //                               if (pNode)
 //                               {
 //                                   result &= pNode.GetComponent<WorldCell>().CanBeConnect(prefabCell, 1);
 //                               }
 //                           }

 //                           if(result)
 //                           {
 //                               break;
 //                           }
 //                       }

 //                       if(result && prefab != null)
 //                       {
 //                           cell = Instantiate(prefab, position, Quaternion.identity, transform) as GameObject;
 //                           cell.name = name;
 //                       }

 //                   }
 //                   cellMap.Add(cell.name, cell);
 //               }
 //           }

 //           var keys = new ArrayList(mCellMap.Keys);

 //           foreach(string key in keys)
 //           {
 //               if(!cellMap.ContainsKey(key))
 //               {
 //                   GameObject cell;
 //                   mCellMap.TryGetValue(key, out cell);
 //                   if(cell)
 //                   {
 //                       cell.transform.SetParent(null);
 //                       Destroy(cell);
 //                       mCellMap.Remove(key);
 //                   }
 //               }
 //           }

 //           mCellMap = cellMap;
 //   }
}
