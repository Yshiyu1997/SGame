using GameFramework;
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class DNAItemObject : ObjectBase
    {

        public static DNAItemObject Create(object target)
        {
            DNAItemObject dnaItemObject= ReferencePool.Acquire<DNAItemObject>();
            dnaItemObject.Initialize(target);
            return dnaItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            DNAItem dnaItem = (DNAItem)Target;
            if (dnaItem == null)
            {
                return;
            }

            Object.Destroy(dnaItem.gameObject);
        }
    }
}
