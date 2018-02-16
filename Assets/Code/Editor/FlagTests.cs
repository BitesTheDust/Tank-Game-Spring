using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TankGame.Systems;

namespace TankGame.Testing
{
    public class FlagTests
    {
        [Test]
        public void FlagTestsCreateEnemyAndPlayerMask()
        {
            int playerLayer = LayerMask.NameToLayer("Player"); // 8
            int enemyLayer = LayerMask.NameToLayer("Enemy"); // 9

            int mask = Flags.CreateMask(playerLayer, enemyLayer);
            int validmask = LayerMask.GetMask("Player", "Enemy");

            Assert.AreEqual(mask, validmask);
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator FlagTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
