using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class AIGame : GameBase
    {
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }


        public override void OnEnterGame()
        {
            base.OnEnterGame();


        }
    }
}
