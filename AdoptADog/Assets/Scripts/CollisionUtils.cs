using UnityEngine;

namespace DefaultNamespace
{
    public static class CollisionUtils
    {
        public static int PushForce = 20;

        public static void HandlePlayerCollision(Dog d1, Dog d2)
        {
            if (d1.Rolling || d2.Rolling) return;

            d2.SetCollisionHandled(d1);
            var p1Rigid = d1.Rigidbody;
            var p2Rigid = d2.Rigidbody;

            // P1 -> P2
            Vector2 dir = (p2Rigid.position - p1Rigid.position).normalized;

            // Speed in P1 -> P2 direction
            float v1 = Vector2.Dot(p1Rigid.velocity, dir);
            // Speed in P2 -> P1 direction
            float v2 = Vector2.Dot(p2Rigid.velocity, -dir);

            Dog loserDog = v1 > v2 ? d2 : d1;
            Dog winnerDog = v1 > v2 ? d1 : d2;
            dir = v1 > v2 ? dir : -dir;

            int force = winnerDog.Leaping ? PushForce + 10 : PushForce;
            loserDog.Rigidbody.AddForce(dir * force, ForceMode2D.Impulse);

            winnerDog.PushSomeone();
        }
    }
}