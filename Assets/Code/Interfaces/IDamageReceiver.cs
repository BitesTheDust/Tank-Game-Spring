using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public interface IDamageReceiver
    {
        void TakeDamage(int amount);
    }
}
