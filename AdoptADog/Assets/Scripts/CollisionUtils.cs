using UnityEngine;

namespace DefaultNamespace
{
    public static class CollisionUtils
    {
        public static int PushForce = 20;

        public static void HandlePlayerCollision(PlayerController p1, PlayerController p2)
        {
            p2.SetCollisionHandled(p1);
            var p1Rigid = p1.GetComponent<Rigidbody2D>();
            var p2Rigid = p2.GetComponent<Rigidbody2D>();

            // P1 -> P2
            Vector2 dir = (p2Rigid.position - p1Rigid.position).normalized;

            // Speed in P1 -> P2 direction
            float v1 = Vector2.Dot(p1Rigid.velocity, dir);
            // Speed in P2 -> P1 direction
            float v2 = Vector2.Dot(p2Rigid.velocity, -dir);

            Rigidbody2D loserRigid = v1 > v2 ? p2Rigid : p1Rigid;
            dir = v1 > v2 ? dir : -dir;

            loserRigid.AddForce(dir * PushForce, ForceMode2D.Impulse);
        }
    }
}