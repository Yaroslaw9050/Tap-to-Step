using Cysharp.Threading.Tasks;

namespace Core.Service.Authorization
{
    public interface IAuthorizationService
    {
        public void Initialise();

        public UniTask<string> SignUpAsync();

        public UniTask<string> SignInAsync();
    }
}