using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCharacter : AbstractCharacter
{    public AbstractCharacter currentTarget;    public override void ReactToCharacter(AbstractCharacter whoReactTo)
    {
        Attack(whoReactTo);
    }}
