using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(menu.gameObject);
            Destroy(hud);
            Destroy(deathMenu.gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

        Application.targetFrameRate = 60;
    }

    public void HideAll()
    {
        player.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        hud.SetActive(false);
        deathMenu.gameObject.SetActive(false);
    }

    public void ShowAll()
    {
        Debug.Log("showing");
        player.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        hud.SetActive(true);
        deathMenu.gameObject.SetActive(true);
    }

    public void OnPlayerDeath()
    {
        PlayerPrefs.DeleteAll();

        menu.animator.SetTrigger("hide");
        deathMenu.Show();
    }

    public void OnRestartButton()
    {
        deathMenu.Hide();
        menu.animator.ResetTrigger("hide");

        SceneManager.sceneLoaded += LoadState;
        SceneManager.LoadScene("Main");
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public FloatingTextManager floatingTextManager;
    public CharacterMenu menu;
    public GameObject hud;
    public DeathMenu deathMenu;
    public Slider hitpointBar;

    // Logic
    public int coins;
    public int experience;

    public OldWeapon Weapon { get { return player.weapon; } }

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // Is the player weapon max level?
        if (weaponPrices.Count <= Weapon.weaponLevel)
            return false;

        if (coins >= weaponPrices[Weapon.weaponLevel])
        {
            coins -= weaponPrices[Weapon.weaponLevel];
            Weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = Mathf.Max(0, (float)player.hitpoint / player.maxHitpoint);
        hitpointBar.value = ratio;
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int level = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[level];
            level++;

            if (level == xpTable.Count) // level is maxed
                break;
        }

        return level;

    }
    public int GetXpToLevel(int level)
    {
        int resultLevel = 0;
        int xp = 0;

        while (resultLevel < level)
        {
            xp += xpTable[resultLevel];
            resultLevel++;
        }

        return xp;
    }
    public void GrantXp(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        player.OnLevelUp();
    }

    // On scene loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        Vector3 spawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;

        player.transform.position = spawnPoint;
        player.IsAttacking = false;

        floatingTextManager.DeleteAll();
    }

    // Save state
    /*
     * INT preferedSkin
     * INT pesos
     * INT experience
     * INT weaponLevel
     */
    public void SaveState()
    {
        string s = "";

        s += menu.currentCharacterIndex + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += Weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        string[] data;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            data = new string[] { "0", "0", "0", "0" };
        }
        else
        {
            data = PlayerPrefs.GetString("SaveState").Split("|");
        }


        // Load player skin
        menu.currentCharacterIndex = int.Parse(data[0]);
        menu.OnSelectionChanged();

        // Coins
        coins = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);
        player.Respawn();
        player.SetLevel(GetCurrentLevel());
        OnHitpointChange();

        // Change weapon level
        Weapon.SetWeaponLevel(int.Parse(data[3]));
    }
}
