using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

namespace Core.Service.Authorization
{
    public class FirebaseAuthorization : IAuthorizationService
    {
        private FirebaseAuth _auth;

        public void Initialise()
        {
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async UniTask<string> SignUpAsync()
        {
            var data = SplitInHalf(SystemInfo.deviceUniqueIdentifier);
            try
            {
                var result = await _auth.CreateUserWithEmailAndPasswordAsync(data.Item2, data.Item1);
                return result.User.UserId;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SignUpAsync encountered an error: {ex}");
                return null;
            }
        }

        public async UniTask<(bool, string)> TrySignInAsync()
        {
            try
            {
                var data = SplitInHalf(SystemInfo.deviceUniqueIdentifier);
                var result = await _auth.SignInWithEmailAndPasswordAsync(data.Item2, data.Item1);
                return (true, result.User.UserId); 
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        private (string, string) SplitInHalf(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return ("", "");
            }

            var middleIndex = input.Length / 2;
            var firstHalf = input.Substring(0, middleIndex);
            var secondHalf = input.Substring(middleIndex);
            secondHalf += "@gmail.com";

            return (firstHalf, secondHalf);
        }
    }
}