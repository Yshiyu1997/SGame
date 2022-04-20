using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SpeciesGame;
using GameFramework.DataTable;


public enum Species_Enemy
{
    迅猛龙=20003,
    三角龙,
    腕龙,
}



public class Manager : MonoBehaviour
{

    public static Manager _instance = null;

    public bool isShowGameFormUI = false; // 是否显示了Game界面

    #region Variables
    const string ManagerHelp =
      "Disable creatures management.\n" +
      "Creatures A.I. still work, player inputs, camera behavior and GUI features are disabled.\n" +
      "Useful if you want to use a third party asset e.g. fps controller. " +
      "However, manager component still to be attached to the MainCam. ";
    [Header("JURASSIC PACK MANAGER")]
    [Tooltip(ManagerHelp)]
    public bool UseManager = true;
    [SerializeField] bool ShowGUI = true;
    [SerializeField] bool ShowFPS = true;
    public Texture2D helpscreen;
    public Texture2D icons;
    [SerializeField] bool InvertYAxis = false;
    [SerializeField] [Range(0.1f, 10.0f)] float sensivity = 2.5f;
    public AudioClip Wind;
    [Space(10)]
    [Header("GLOBAL CREATURES SETTINGS")]
    [Tooltip("Add your creatures prefabs here, this will make it spawnable during game.")]
    public List<GameObject> CollectionList;
    const string IKHelp = "Inverse Kinematics - Accurate feet placement on ground";
    [Tooltip(IKHelp)]
    public bool UseIK;
    [Tooltip("Creatures will be active even if they are no longer visible. (performance may be affected).")]
    public bool RealtimeGame;
    [Tooltip("Countdown to destroy the creature after his dead. Put 0 to cancels the countdown, the body will remain on the scene without disappearing.")]
    public int TimeAfterDead = 10000;
    const string RaycastHelp =
    "ENABLED : allow creatures to walk on all kind of collider. (more expensive).\n" + "\n" +
    "DISABLED : creatures can only walk on Terrain collider (faster).\n";
    [Tooltip(RaycastHelp)]
    public bool UseRaycast;
    [Tooltip("Layer used for water.")]
    public int waterLayer;
    [Tooltip("Unity terrain tree layer, the layer must be defined into tree model prefab")]
    public int treeLayer;
    [Tooltip("The maximium walkable slope before the creature start slipping.")]
    [Range(0.1f, 1.0f)] public float MaxSlope = 0.75f;
    [Tooltip("Water plane altitude")]
    public float WaterAlt = 55;
    [Tooltip("Blood particle for creatures")]
    public ParticleSystem blood;



    [Tooltip("相机旋转偏离物种的X角度")]
    public float rotateX = 0f;
    [Tooltip("相机旋转偏离物种的Y角度")]
    public float rotateY = 45f;
    [Tooltip("相机旋转偏离物种的Z角度")]
    public float rotateZ = 2.645978f;
    [Tooltip("相机离物种的高度")]
    public float upValue = 2.2f;

    [Tooltip("不同等级不同物种的最大存在数量")]
    private Dictionary<Species_Enemy, int> maxEnemyNum = new Dictionary<Species_Enemy, int>();

    public int[] playerSkinIndexArr;// 玩家皮肤下标数组

    [HideInInspector] public bool isFirstGame = true;// 是否是第一次游戏 调试用

    [HideInInspector] public int evolveLevel = 1;// 进化等级 大等级关卡

	[HideInInspector] public int level = 1;// 等级 进化线路上的每大关卡的小关卡

	[HideInInspector] public float DNAValue = 0;// DNA值

    [HideInInspector] public Vector3 playerPos = Vector3.zero; // 玩家的位置

    [HideInInspector] public GameObject playerObj = null;// 玩家

    [HideInInspector] public bool isHasAttack = false; // 是否已经攻击了

    [HideInInspector] public bool isCanMove = true;// 是否能够移动

    [HideInInspector] public bool isInvincible = false;// 是否处于无敌状态

    [HideInInspector] public bool isCanChangePos = true; // 摄像机是否能够跟随玩家变换

    [HideInInspector] public List<GameObject> creaturesList, playersList; //list of all creatures/players in game
    [HideInInspector] public int selected, CameraMode = 1, message = 0; //creature index, camera mode, game messages
                                                                        //Terrain datas
    [HideInInspector] public Terrain T = null;
    [HideInInspector] public TerrainData tdata = null;
    [HideInInspector] public Vector3 tpos = Vector3.zero;
    [HideInInspector] public float tres = 0;

    [HideInInspector] public int toolBarTab = -1, addCreatureTab = -2, count = 0; //toolbar tab
    bool browser = false; //creature browser
    Vector2 scroll1 = Vector2.zero, scroll2 = Vector2.zero; //Scroll position
    float vx, vy, vz = 25; //camera angle/zoom
    float timer, frame, fps; //fps counter
    Rigidbody body;
    AudioSource source;
    bool spawnAI, rndSkin, rndSize, rndSetting;
    int rndSizeSpan = 1;

    Vector3 beginPos = Vector3.zero; // 开始位置
    #endregion
    #region Start
    void Awake()
    {
        _instance = this;
        //Find all JP creatures/players prefab in scene
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in creatures)
        {
            if (!element.name.EndsWith("(Clone)")) creaturesList.Add(element.gameObject); //Add to list
            else Destroy(element.gameObject); //Delete unwanted ghost objects in hierarchy
        }
        foreach (GameObject element in players) { playersList.Add(element.gameObject); }//Add to list

        if (UseManager)
        {
            //Cursor.visible = false; Cursor.lockState=CursorLockMode.Locked;
            body = transform.root.GetComponent<Rigidbody>();
            source = transform.root.GetComponent<AudioSource>();
        }

        //Get terrain datas
        if (Terrain.activeTerrain)
        {
            T = Terrain.activeTerrain;
            tdata = T.terrainData;
            tpos = T.GetPosition();
            tres = tdata.heightmapResolution;
        }

        //Layers left-shift
        treeLayer = (1 << treeLayer);

       
        //Debug.Log("皮肤下标"+ playerSkinIndexArr[0]+ "皮肤下标" + playerSkinIndexArr[1] + "皮肤下标" + playerSkinIndexArr[2]);
    }

    void Start()
    {
        //GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //foreach (GameObject element in creatures)
        //{
        //    if (!element.name.EndsWith("(Clone)")) creaturesList.Add(element.gameObject); //Add to list
        //    else Destroy(element.gameObject); //Delete unwanted ghost objects in hierarchy
        //}
        //foreach (GameObject element in players) { playersList.Add(element.gameObject); }//Add to list
    }
  #endregion
  #region Camera behavior
  void Update()
	{
		if(!UseManager) return;

		//Fps counter
		if(ShowFPS) { frame += 1.0f; timer += Time.deltaTime; if(timer>1.0f) { fps = frame; timer = 0.0f; frame = 0.0f; } }

		//Lock/Unlock cursor
		if(Application.isEditor)
		{
			if(Input.GetKeyDown(KeyCode.Escape) && toolBarTab==-1) { Cursor.lockState=CursorLockMode.None; toolBarTab=1; }
			else if(Input.GetKeyDown(KeyCode.Escape) && toolBarTab!=-1) { Cursor.lockState=CursorLockMode.None; toolBarTab=-1; }
			else if(toolBarTab==-1) Cursor.lockState=CursorLockMode.None;
		}
		else
		{
			if(Cursor.lockState==CursorLockMode.None && Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState=CursorLockMode.None;
			else if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState=CursorLockMode.None;
		}

			//Creature select (Shortcut Key)
			if(Input.GetKeyDown(KeyCode.X)) { if(selected > 0) selected--; else selected=creaturesList.Count-1; }
			else if(Input.GetKeyDown(KeyCode.Y)) { if(selected < creaturesList.Count-1) selected++; else selected=0; }
			
			//Change View (Shortcut Key)
			if(Input.GetKeyDown(KeyCode.C))
			{ if(CameraMode==2) CameraMode=0; else CameraMode++; }

	}
    Creature creature = null;
    void LateUpdate()
	{
        //Debug.Log("选择的控龙" + selected + "总共多少控龙" + creaturesList.Count);
        if (!UseManager||!isCanChangePos) return;
       
		//If creature not found, switch to free camera mode
		if(creaturesList.Count==0) CameraMode=0;
		//else if(!creaturesList[selected] | !creaturesList[selected].activeInHierarchy) CameraMode=0;
		else if(creaturesList[creaturesList.IndexOf(playerObj)] !=null) creature=creaturesList[creaturesList.IndexOf(playerObj)].GetComponent<Creature>(); //Get creature script

		//Prevent camera from going into terrain    防止摄像机进入地形
		if(T && (T.SampleHeight(transform.root.position)+T.GetPosition().y)>transform.root.position.y-1.0f)
		{
			body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
			transform.root.position=new Vector3(transform.root.position.x, (T.SampleHeight(transform.root.position)+T.GetPosition().y)+1.0f, transform.root.position.z);
		}
        //Debug.Log("相机模式" + CameraMode);
        switch (CameraMode)
		{
		//Free
		case 0:
			if(source.clip==null) source.clip=Wind; else if(source.clip==Wind)
			{
				if(source.isPlaying) { source.volume=body.velocity.magnitude/128; source.pitch=source.volume; }
				else source.PlayOneShot(Wind);
			}

                Vector3 dir = Vector3.zero; float y = 0;
                if (Input.GetKey(KeyCode.LeftShift)) body.mass = 0.025f; else body.mass = 0.1f; body.drag = 1.0f;
                if (Cursor.lockState == CursorLockMode.Locked | Input.GetKey(KeyCode.Mouse2))
                {
                    vx += Input.GetAxis("Mouse X") * sensivity; //rotate cam X axe
                    vy = Mathf.Clamp(InvertYAxis ? vy + Input.GetAxis("Mouse Y") * sensivity : vy - Input.GetAxis("Mouse Y") * sensivity, -89.9f, 89.9f); //rotate cam Y axe
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(vx, Vector3.up) * Quaternion.AngleAxis(vy, Vector3.right), 0.1f);
                }

                if (Input.GetKey(KeyCode.Space)) y = 1; else if (Input.GetKey(KeyCode.LeftControl)) y = -1; else y = 0;
                dir = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), y, Input.GetAxis("Vertical")); //move
                body.AddForce(dir * (transform.root.position - (transform.root.position + dir)).magnitude);
                break;
		//Follow camera
		case 1:
                
                if (Cursor.lockState == CursorLockMode.Locked | Input.GetKey(KeyCode.Mouse2))
                {
                    //if (Input.GetKey(KeyCode.Mouse1))
                    //{
                    //    vx = creaturesList[selected].transform.eulerAngles.y; //lock camera to creature angle
                    //    if (creature.IsOnLevitation)
                    //    { vy = Mathf.Clamp(Mathf.Lerp(vy, creature.anm.GetFloat("Pitch") * 90, 0.01f), -45f, 90f); }//pitch flying creature with camera axe
                    //    else
                    //    { vy = Mathf.Clamp(InvertYAxis ? vy - Input.GetAxis("Mouse Y") * sensivity : vy + Input.GetAxis("Mouse Y") * sensivity, -90f, 90f); } //rotate cam Y axe
                    //}
                    //else if (!Input.GetKey(KeyCode.Mouse2) | Cursor.lockState != CursorLockMode.Locked)
                    //{
                    //    vx = vx + Input.GetAxis("Mouse X") * sensivity; //rotate cam X axe
                    //    vy = Mathf.Clamp(InvertYAxis ? vy - Input.GetAxis("Mouse Y") * sensivity : vy + Input.GetAxis("Mouse Y") * sensivity, -90f, 90f); //rotate cam Y axe
                    //}
                }
                else
                {
                    vx = rotateX;
                    vy = rotateY;
                    vz = rotateZ;
                }
                //vz = Mathf.Clamp(vz - Input.GetAxis("Mouse ScrollWheel") * 10, size, size * 32f); //zoom cam Z axe
                //Debug.Log("旋转的Y值"+vy+"旋转的X值"+vx+"旋转的Z值"+vz);
                if (isCanChangePos)
                {
                    body.mass = 1.0f;
                    body.drag = 10.0f;
                    float size = creature.withersSize;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(vy, vx, 0.0f), 0.1f);
                    //Vector3 pos=((creaturesList[selected].transform.root.position+Vector3.up*size*1.5f)-transform.root.position)-transform.forward*vz;
                    Vector3 pos = ((creaturesList[creaturesList.IndexOf(playerObj)].transform.position + Vector3.up * size * upValue) - transform.position) - transform.forward * vz;
                    body.AddForce(pos * 128f);
                    
                }
                break;
            // POV camera
            case 2:
                //size = creature.withersSize;
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((creaturesList[selected].transform.root.position + Vector3.up * size * 1.5f) - transform.root.position), 0.1f);
                break;
		default: CameraMode=0; break;
		}
	}
    /// <summary>
    ///  添加生物到场景中
    /// </summary>
    public void OnAddSpecies(GameObject speciesObj,bool isAi,Vector3 posValue,int skinIndex)
    {
        Creature script = speciesObj.GetComponent<Creature>();
        if (isAi) { speciesObj.transform.position = posValue; }
        else
        {
            
            if (isFirstGame)
            {
                beginPos= transform.position + transform.forward * 10;
                if (beginPos.y <= 58.49032f) beginPos.y = 58.49032f;
                speciesObj.transform.position = beginPos;
                playerPos = speciesObj.transform.position;
                CreateAi();
            }
            else
            {
				speciesObj.transform.position = posValue;
			}
            playerPos = speciesObj.transform.position;
            CameraMode = 1;
            playerObj = speciesObj;
            script.IsDead = false;
            //CamerFollow.instance.playerTarget = playerObj;
        }
        speciesObj.transform.rotation = Quaternion.identity;
       

        //if (!spawnAI) CameraMode = 1; script.UseAI = spawnAI;
        script.UseAI = isAi;
        script.SetMaterials(skinIndex,0);
        //if (rndSkin) { script.SetMaterials(Random.Range(0, 3), Random.Range(0, 16)); }
        //if (rndSize) { script.SetScale(0.5f + Random.Range((float)rndSizeSpan / -10, (float)rndSizeSpan / 10)); } else script.SetScale(0.05f);
        //if (rndSetting)
        //{
            script.Health = 100; script.Stamina = 100;
            script.Food = 100; script.Water = 100;
        //}

        //speciesObj.name = CollectionList[i].name;
        creaturesList.Add(speciesObj.gameObject);
        if (!isAi)
        {
            selected = creaturesList.IndexOf(speciesObj.gameObject); //add creature to creature list
            isCanChangePos = true;
        }
            
    }

   
    private int createAreaIndex = 0;// 敌人生成区域 （1,2,3,4） 分四个区域 均匀分布 
    [HideInInspector]
    public Vector3 enemyPos = Vector3.zero;
    private float enemyPosY = 58.54617f;// 敌人Y坐标
    private float limitDiatance = 5f; // 限制玩家周围生成敌人的距离大小
    public int distanceWithPlayer_Inside = 10;//敌人生物圈内圆半径
    public int distanceWithPlayer_Outside = 15;//敌人生物圈外圆半径
    public int maxEnemyLiveNum = 200;//当前场景存活的最大敌人数量

    public List<Vector3> enemyPosList = new List<Vector3>();// 敌人的生成位置


    /// <summary>
    ///  生成AI
    /// </summary>
    private void CreateAi()
    {
        // 在此获得存储的进化等级
        evolveLevel = GameUtil._instance.beginEvolveLevel;
        IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
        DRLevel drLevel = dtLevel.GetDataRow(evolveLevel);
        if (drLevel == null)
        {
            Debug.Log("Can not load Level '{0}' from data table."+evolveLevel.ToString());
            return;
        }
        maxEnemyNum.Clear();
        maxEnemyNum.Add((Species_Enemy)drLevel.Enemy_1, drLevel.EnemyNum_1);
        maxEnemyNum.Add((Species_Enemy)drLevel.Enemy_2, drLevel.EnemyNum_2);
        maxEnemyNum.Add((Species_Enemy)drLevel.Enemy_3, drLevel.EnemyNum_3);
        //生成对应的猎物
        // 迅猛龙
        for (int i = 0; i <= drLevel.EnemyNum_1; i++)
        {
            //GameEntry.Entity.ShowVelociraptor(new VelociraptorData(GameEntry.Entity.GenerateSerialId(), 10003, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
            DistributeEnemy_In(distanceWithPlayer_Inside);
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), drLevel.Enemy_1, enemyPos));
        }
        // 三角龙
        for (int i = 0; i <= drLevel.EnemyNum_2; i++)
        {
            //GameEntry.Entity.ShowTriceratops(new TriceratopsData(GameEntry.Entity.GenerateSerialId(), 10004, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
            DistributeEnemy_In(distanceWithPlayer_Inside);
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), drLevel.Enemy_2, enemyPos));
        }

        // 腕龙
        for (int i = 0; i <= drLevel.EnemyNum_3; i++)
        {
            //GameEntry.Entity.ShowBrachiosauru(new BrachiosauruData(GameEntry.Entity.GenerateSerialId(), 10005, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
            DistributeEnemy_In(distanceWithPlayer_Inside);
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), drLevel.Enemy_3, enemyPos));
        }
    }

    /// <summary>
    ///  根据typeID获得当前生物能够获得的最大
    /// </summary>
    /// <param name="typeId"></param>
    private int GetMaxNumByTypeId(int typeId)
    {
        foreach(var val in maxEnemyNum)
        {
            if ((int)val.Key == typeId)
            {
                return val.Value;
            }
        }

        return 0;
    }

    private int recordNum = 0;
    /*
    * 在场景中分配敌人  内圆
    */
    private void DistributeEnemy_In(int circleRadius)
    {
        createAreaIndex++;
        //playerPos = Manager._instance.playerPos;
        //Debug.Log("玩家的位置" + playerPos);
        Vector3 pos = Vector3.zero;
        Quaternion roa = Quaternion.identity;
        Vector2 point = Random.insideUnitCircle * circleRadius;
        //if (Vector2.Distance(point, Vector2.zero) < limitDiatance)
        //{
        //    // 在内圈  不生成
        //    return Vector3.zero;
        //}
        switch (createAreaIndex)
        {
            case 1:
                // 右上
                pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
                break;
            case 2:
                // 右下
                pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                break;
            case 3:
                // 左下
                pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                break;
            case 4:
                // 左上
                createAreaIndex = 0;// 还原
                pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
                break;
        }
        if (Vector3.Distance(playerPos, pos) < limitDiatance||JudgeIsRepetiPos(pos))
        {
            // 在内圈限制范围内 不生成
            return;
        }
        enemyPos = pos;
        recordNum++;
        //Debug.Log("生成位置X"+pos.x+"生成位置Y"+pos.y+"生成位置Z"+pos.z+"数量"+ recordNum);
        enemyPosList.Add(enemyPos);
    }

    /// <summary>
    ///  外部生成 跟随玩家的移动进行生成
    /// </summary>
    public void DistributeEnemy_Out()
    {
        createAreaIndex++;
        //playerPos = Manager._instance.playerPos;
        //Debug.Log("玩家的位置" + playerPos);
        Vector3 pos = Vector3.zero;
        Quaternion roa = Quaternion.identity;
        Vector2 point = Random.insideUnitCircle * distanceWithPlayer_Outside;
        playerPos = playerObj.transform.position;
        //if (Vector2.Distance(point, Vector2.zero) < limitDiatance)
        //{
        //    // 在内圈  不生成
        //    return Vector3.zero;
        //}
        switch (createAreaIndex)
        {
            case 1:
                // 右上
                pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
                break;
            case 2:
                // 右下
                pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                break;
            case 3:
                // 左下
                pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                break;
            case 4:
                // 左上
                createAreaIndex = 0;// 还原
                pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
                break;
        }
        if (Vector3.Distance(playerPos, pos) < distanceWithPlayer_Inside|| JudgeIsRepetiPos(pos))
        {
            // 在内圈限制范围内 不生成
            return;
        }
        enemyPos = pos;
        enemyPosList.Add(enemyPos);
    }
    /// <summary>
    ///  是否重复位置
    /// </summary>
    private bool JudgeIsRepetiPos(Vector3 pos)
    {
        if (enemyPosList.IndexOf(pos) == -1) return false;

        return true;
    }

    #endregion
    #region Draw GUI

    #endregion
}
