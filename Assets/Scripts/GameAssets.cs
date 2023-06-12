using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
        public static GameAssets i;
    
        private void Awake(){
             i=this;
        }        
        
        public Sprite foodSpriteMiesko;
        public RuntimeAnimatorController foodAnimationController;
        public Sprite snakeBodySprite;
}
