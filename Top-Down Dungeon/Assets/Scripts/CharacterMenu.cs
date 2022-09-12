using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, healthText, coinsText, upgradeCostText, xpText;

    // Logic
    public int currentCharacterIndex = 0;
    public Image currentCharacterSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Public fields
    public Animator animator;

    // Exit to Main Menu
    public void OnExitButton()
    {
        animator.SetTrigger("hide");
        animator.ResetTrigger("show");
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.SaveState();
    }

    // Character selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterIndex++;

            // If we went too far away
            if (currentCharacterIndex == GameManager.instance.playerSprites.Count)
                currentCharacterIndex = 0;
        }
        else
        {
            currentCharacterIndex--;

            if (currentCharacterIndex < 0)
                currentCharacterIndex = GameManager.instance.playerSprites.Count - 1;
        }
        OnSelectionChanged();
    }
    public void OnSelectionChanged()
    {
        currentCharacterSprite.sprite = GameManager.instance.playerSprites[currentCharacterIndex];
        GameManager.instance.player.SwapSprite(currentCharacterIndex);
        Debug.Log(currentCharacterIndex);
    }

    // Weapon upgrade
    public void OnUpgradeClick()
    {
        // GameManager updates player's weapon
        if (GameManager.instance.TryUpgradeWeapon())
            Update();
    }

    // Update the character information
    void Update()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.Weapon.weaponLevel];
        if (GameManager.instance.Weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.Weapon.weaponLevel].ToString();

        // Meta 
        healthText.text = GameManager.instance.player.hitpoint.ToString() + " / " + 
            GameManager.instance.player.maxHitpoint.ToString();
        coinsText.text = GameManager.instance.coins.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        // xp Bar
        int currentLevel = GameManager.instance.GetCurrentLevel();

        if (currentLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points"; // Display total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int previousLevelXp = GameManager.instance.GetXpToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXpToLevel(currentLevel);

            int difference = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;

            float completionRatio = (float)currentXpIntoLevel / difference;
            xpBar.localScale = new Vector3(completionRatio, 1);
            xpText.text = currentXpIntoLevel.ToString() + " / " + difference.ToString();
        }
    }
}
