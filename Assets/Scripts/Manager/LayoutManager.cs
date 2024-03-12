using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    [Header("Layout Container Component")]
    [SerializeField] private GameObject mLayoutContainer;

    [Header("Prefab Component")]
    [SerializeField] private GameObject mHubPrefab;
    [SerializeField] private GameObject mFontainePrefab;
    [SerializeField] private GameObject mShopPrefab;
    [SerializeField] private List<GameObject> mLevelPrefab;

    [Header("Time to repeat instantiate")]
    [SerializeField] private int mWaitToRepeatLevel;

    //List of instanced level with appropriate wait time list
    private List<GameObject> mInstancedLevelsList;
    private List<int> mWaitInstanceLevelsList;

    //List of n+2 next room to generate
    private LinkedList<GameObject> mNextLevelList;

    //Last special room generate -> essential for control wher next special room to generate
    private GameObject mLastSpecialLayout;

    //Indicate current room instanced
    private GameObject mCurrentLayout;

    //Probability value to generate special room -> increment for any generate room
    private float mCurrentProbabilityToInstanceHub;
    private float mCurrentProbabilityToInstanceFontaine;
    private float mCurrentProbabilityToInstanceShop;

    private void Start()
    {
        //Initialize list
        mInstancedLevelsList = new List<GameObject>();
        mWaitInstanceLevelsList = new List<int>();
        mNextLevelList = new LinkedList<GameObject>();

        //Initilize currentlayout -> deafult(HUB)
        //Passing the child of layout container (is only one)
        mCurrentLayout = mLayoutContainer.transform.GetChild(0).gameObject;

        //Initialize probability of special layout
        mCurrentProbabilityToInstanceHub = 0;
        mCurrentProbabilityToInstanceFontaine = 0;
        mCurrentProbabilityToInstanceShop = 0;

        //Initialize lastSpecialLayout
        mLastSpecialLayout = null;

        //Generate first n+2 next level to generate
        for(int i = 0; i < 2; i++)
        {
            UpdateNextLevelToGenerate();
        }

        //Instantiate default level --> HUB
        //InstantiateLayout(mHubPrefab);
    }

    #region Layout
    private void InstantiateLayout()
    {
        //When new layout is created -> first delete current layout on scene, and after create new layout
        Destroy(mCurrentLayout);

        //Instance new layout in the scene
        GameObject layout = mNextLevelList.First.Value;
        mCurrentLayout = Instantiate(layout, mLayoutContainer.transform.position + Vector3.zero, Quaternion.identity, mLayoutContainer.transform);
    }

    private GameObject SelectRandomicLayout()
    {
        GameObject layout = CalculateProbabilityToInstaceSpecialLayout();

        if(layout == null)
        {
            layout = GenerateRandomicLevel();

            //Increment the percentage of probability to instantiate special layout
            IncrementProbabilityToGenerateSpecialLayout();

            //Update All time to wait
            UpdateTimeToRepeatLevel();

            //Update list to instantiate and time for wait to repeat level
            mInstancedLevelsList.Add(layout);
            mWaitInstanceLevelsList.Add(mWaitToRepeatLevel);
        }

        return layout;
    }

    private void UpdateNextLevelToGenerate()
    {
        //Select randomic next level
        GameObject layout = SelectRandomicLayout();

        //Update the list
        mNextLevelList.AddLast(layout);
    }

    #region SPECIAL_LAYOUT

    private float GenerateRandomicProbabilityToInstantiateHub()
    {
        float randomProbability = Random.Range(0, 2f);
        float probability = mCurrentProbabilityToInstanceHub * randomProbability;
        return probability;
    }

    private float GenerateRandomicProbabilityToInstantiateFontaine()
    {
        float randomProbability = Random.Range(0, 2f);
        float probability = mCurrentProbabilityToInstanceFontaine * randomProbability;
        return probability;
    }

    private float GenerateRandomicProbabilityToInstantiateShop()
    {
        float randomProbability = Random.Range(0, 3f);
        float probability = mCurrentProbabilityToInstanceShop * randomProbability;
        return probability;
    }

    private GameObject CalculateProbabilityToInstaceSpecialLayout()
    {
        float probabilityToGenerateHub = GenerateRandomicProbabilityToInstantiateHub();
        float probabilityToGenerateFontaine = GenerateRandomicProbabilityToInstantiateFontaine();
        float probabilityToGenerateShop = GenerateRandomicProbabilityToInstantiateShop();

        if (probabilityToGenerateShop > 4f && mShopPrefab != mLastSpecialLayout)
        {
            //Reset probability after instantiate special layout
            mCurrentProbabilityToInstanceHub = 0;
            mCurrentProbabilityToInstanceFontaine = 0;
            mCurrentProbabilityToInstanceShop = 0;

            //Update last special layout
            mLastSpecialLayout = mShopPrefab;

            //Instantiate shop because the probability is over 400%
            return mShopPrefab;
        }
        else if (probabilityToGenerateHub > 1f && mHubPrefab != mLastSpecialLayout)
        {
            //Reset probability after instantiate special layout
            mCurrentProbabilityToInstanceHub = 0;
            mCurrentProbabilityToInstanceFontaine = 0;
            mCurrentProbabilityToInstanceShop = 0;

            //Update last special layout
            mLastSpecialLayout = mHubPrefab;

            //Instantiate hub because the probability is over 100%
            return mHubPrefab;
        }
        else if (probabilityToGenerateFontaine > 1f && mFontainePrefab != mLastSpecialLayout)
        {
            //Reset probability after instantiate special layout
            mCurrentProbabilityToInstanceHub = 0;
            mCurrentProbabilityToInstanceFontaine = 0;
            mCurrentProbabilityToInstanceShop = 0;

            //Update last special layout
            mLastSpecialLayout = mFontainePrefab;

            //Instantiate hub because the probability is over 150%
            return mFontainePrefab;
        }

        return null;
    }

    private void IncrementProbabilityToGenerateSpecialLayout()
    {
        mCurrentProbabilityToInstanceHub += 0.1f;
        mCurrentProbabilityToInstanceFontaine += 0.1f;
        mCurrentProbabilityToInstanceShop += 0.2f;
    }

    #endregion

    #region LEVEL

    private GameObject GenerateRandomicLevel()
    {
        GameObject level = null;

        //This condition remove all posibility of loop: if number of elements contain in the list of level prefab is equals to number of elements contain
        //in the list of instantiate the program create loop in check method
        if (mInstancedLevelsList.Count == mLevelPrefab.Count)
        {
            level = mInstancedLevelsList[0];
            mInstancedLevelsList.RemoveAt(0);
            mWaitInstanceLevelsList.RemoveAt(0);
        }
        else
        {
            bool checkLevelNotInstantiate = false;

            while (!checkLevelNotInstantiate)
            {
                int indexRandomLevelGenerate = Random.Range(0, mLevelPrefab.Count);
                level = mLevelPrefab[indexRandomLevelGenerate];
                checkLevelNotInstantiate = CheckLevelInstantiate(level);
            }
        }

        return level;
    }

    private bool CheckLevelInstantiate(GameObject level)
    {
        //Check if level exist in the list
        bool checkLevel = mInstancedLevelsList.Contains(level);

        if (checkLevel)
        {
            //Initialize indexLevel for control if value is equals zero
            int indexLevel = mInstancedLevelsList.IndexOf(level);

            if (mWaitInstanceLevelsList[indexLevel] > 0)
            {
                return false;    //Retur true is equals to instance this level 
            }
        }
        return true;
    }

    private void UpdateTimeToRepeatLevel()
    {
        for (int i = 0; i < mWaitInstanceLevelsList.Count; i++)
        {
            if (mWaitInstanceLevelsList[i] > 0)
            {
                mWaitInstanceLevelsList[i]--;
            }
            else
            {
                mWaitInstanceLevelsList.RemoveAt(i);
            }
        }
    }

    #endregion

    #endregion

    #region CALL_BACK

    //Callback for GameManager -> GameManager use this method for create the level layot
    public void OnCreateNewLayout()
    {
        //Instance levele layout
        InstantiateLayout();

        //Remove first element in the next level list
        mNextLevelList.RemoveFirst();

        //Update next level list
        UpdateNextLevelToGenerate();
    }

    #endregion

    #region DEBUG

    private void PrintInstantiatedLevelsStatus()
    {
        Debug.Log("InstantiateLevelList");
        for(int i = 0; i < mInstancedLevelsList.Count; i++)
        {
            Debug.Log("Level " + i + ":" + mInstancedLevelsList[i]);
        }
        Debug.Log("Time to repeat instantiate");
        for (int i = 0; i < mInstancedLevelsList.Count; i++)
        {
            Debug.Log("Time " + i + ":" + mWaitInstanceLevelsList[i]);
        }
    }

    private void PrintNextdLevelsToInstantiateStatus()
    {
        Debug.Log("NextLevelsList");
        foreach(GameObject level in mNextLevelList)
        {
            Debug.Log("Level:" + level);
        }
    }

    public void TestingInstanceButton()
    {
        OnCreateNewLayout();
        PrintInstantiatedLevelsStatus();
        PrintNextdLevelsToInstantiateStatus();
    }

    public void PrintGenerateRandomicProbabilityToInstantiateHub()
    {
        float probabilityHub = GenerateRandomicProbabilityToInstantiateHub();
        Debug.Log("Current Probability Hub:" + mCurrentProbabilityToInstanceHub + ", Probability:" + probabilityHub);
        float probabilityFontaine = GenerateRandomicProbabilityToInstantiateFontaine();
        Debug.Log("Current Probability Fontaine:" + mCurrentProbabilityToInstanceFontaine + ", Probability:" + probabilityFontaine);
        float probabilityShop = GenerateRandomicProbabilityToInstantiateShop();
        Debug.Log("Current Probability Shop:" + mCurrentProbabilityToInstanceShop + ", Probability:" + probabilityShop);

        //Increment Probability
        IncrementProbabilityToGenerateSpecialLayout();
    }

    #endregion
}
