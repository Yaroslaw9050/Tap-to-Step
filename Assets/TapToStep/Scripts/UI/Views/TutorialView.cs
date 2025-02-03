namespace UI.Views.Upgrades
{
    public class TutorialView : BaseView
    {
        private bool _isTutorialActive;
        
        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _isTutorialActive = true;

        }

        public override void HideView(float duration = 0.5f)
        {
            if(_isTutorialActive == false) return;
            base.HideView(duration);
            _isTutorialActive = false;
        }
    }
}