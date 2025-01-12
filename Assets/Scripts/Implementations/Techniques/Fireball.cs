using System.Collections;
using BaseClasses;

namespace Implementations.Techniques
{
    public class Fireball : Technique
    {
        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
        }
        
        protected override void StartWrapper()
        {
            base.StartWrapper();
        }
        
        protected override IEnumerator PlayAnimation()
        {
            throw new System.NotImplementedException();
        }

        protected override void Effect(CharacterSheet cs)
        {
            throw new System.NotImplementedException();
        }
        
        public override string GetIconPath()
        {
            throw new System.NotImplementedException();
        }

        public override string GetPrefabPath()
        {
            throw new System.NotImplementedException();
        }
    }
}