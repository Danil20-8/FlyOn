using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBattleGoal
{
    AIBehabiour GetAntiBehaviour();
    AIBehabiour GetBehaviour();
    bool Check();
}
