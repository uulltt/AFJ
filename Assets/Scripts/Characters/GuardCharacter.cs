using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCharacter : AbstractCharacter
{    public AbstractCharacter currentTarget;    protected override void ReactToCharacter(AbstractCharacter whoReactTo)
    {
        if (whoReactTo == currentTarget)
            Attack(whoReactTo);
    }}
