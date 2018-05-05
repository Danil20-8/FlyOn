using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ThemePreserver: MonoBehaviour
{
    public PreserveType type;
}

public enum PreserveType
{
    This,
    All
}
