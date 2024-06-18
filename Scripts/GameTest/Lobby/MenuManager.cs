using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private List<Menu> _menus;

    public void OpenMenu(string menuName)
    {
        foreach (var menu in _menus)
        {
            if (menu.menuName == menuName)
            { 
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
    private void Awake()
    {
        instance = this;
    }

}
