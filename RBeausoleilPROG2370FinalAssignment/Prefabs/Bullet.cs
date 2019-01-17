using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.Prefabs
{
    public class Bullet : GameObject
    {
        private BulletController bulletController;

        public override void OnCreated()
        {
            (AddComponent<ModelRenderer>() as ModelRenderer).Model = Game.Content.Load<Model>("tankammo");
            bulletController = AddComponent<BulletController>() as BulletController;
            AddComponent<CircleCollider2D>();
        }

        public void SetStartPosition(Vector3 position)
        {
            bulletController.StartPosition = position;
        }
    }
}