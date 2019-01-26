using UnityEngine;

namespace DefaultNamespace
{
    public static class CollisionUtils
    {
        public static int PushForce = 20;

        public static void HandlePlayerCollision(PlayerController p1, PlayerController p2)
        {
            if (p1.Rolling || p2.Rolling) return;

            p2.SetCollisionHandled(p1);
            var p1Rigid = p1.Rigidbody;
            var p2Rigid = p2.Rigidbody;

            // P1 -> P2
            Vector2 dir = (p2Rigid.position - p1Rigid.position).normalized;

            // Speed in P1 -> P2 direction
            float v1 = Vector2.Dot(p1Rigid.velocity, dir);
            // Speed in P2 -> P1 direction
            float v2 = Vector2.Dot(p2Rigid.velocity, -dir);

            PlayerController loserPlayer = v1 > v2 ? p2 : p1;
            PlayerController winnerPlayer = v1 > v2 ? p1 : p2;
            dir = v1 > v2 ? dir : -dir;

            int force = winnerPlayer.Leaping ? PushForce + 10 : PushForce;
            loserPlayer.Rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
        }
    }
}